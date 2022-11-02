using Dapper;
using Nanocode.Database.Interfaces;
using Nanocode.Database.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nanocode.Database
{
    public class NanoObject<TEntity>
    {
        #region Flags
        private bool _flagForManualId = false;
        #endregion

        #region Attribute Properties
        internal static string GetTableName()
        {
            // Action
            var tableName = typeof(TEntity).GetAttributeValue((NanoTableAttribute attr) => attr.TableName);
            if (string.IsNullOrEmpty(tableName)) tableName = typeof(TEntity).Name;

            // Return
            return tableName;
        }
        internal static INanoCredentials GetDatabaseCredentials()
            => typeof(TEntity).GetAttributeValue((NanoCredentialsAttribute attr) => attr.Credentials);

        internal static (PropertyInfo Info, NanoPrimaryKeyOptions Options) GetPrimaryKey()
        {
            foreach (var pi in typeof(TEntity).GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // PrimaryKeyAttribute
                var attrs = pi.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    if (attr.GetType().Name != nameof(NanoPrimaryKeyAttribute))
                        continue;

                    var pka = (NanoPrimaryKeyAttribute)attr;
                    if (pka.PrimaryKey)
                        return (pi, pka.ToPrimaryKeyOptions());
                }

                // NanoTableColumnAttribute
                var options = GetColumnOptions(pi);
                if (options != null && options.IsPrimaryKey)
                    return (pi, options.ToPrimaryKeyOptions());
            }

            // Return
            return (null, null);
        }
        internal static PropertyInfo GetPrimaryKeyPropertyInfo()
            => GetPrimaryKey().Info;
        internal static NanoPrimaryKeyOptions GetPrimaryKeyOptions()
            => GetPrimaryKey().Options;
        internal static string GetPrimaryKeyColumnName()
            => GetDatabaseColumnName(GetPrimaryKeyPropertyInfo());

        internal static NanoTableColumnOptions GetColumnOptions(PropertyInfo pi)
        {
            // Find Correct Property and Set
            var attrs = pi.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (attr.GetType().Name != nameof(NanoTableColumnAttribute))
                    continue;

                return ((NanoTableColumnAttribute)attr).ToTableColumnOptions();
            }

            // Return
            return null;
        }
        internal static string GetDatabaseColumnName(PropertyInfo pi)
        {
            if (pi == null) return string.Empty;

            var columnOptions = GetColumnOptions(pi);
            if (columnOptions == null) return pi.Name;
            if (string.IsNullOrWhiteSpace(columnOptions.ColumnName)) return pi.Name;

            return columnOptions.ColumnName;
        }

        internal static (INanoDatabase Conn, bool Dispose) GetConnection(INanoDatabase db)
        {
            // NanoDatabase Connection
            INanoDatabase conn = null;
            var disposeDbConn = false;

            // Check Point
            if (db != null) conn = db;
            /** /
            else
            {
                var dbOptions = GetDatabaseCredentials();
                if (dbOptions != null)
                {
                    conn = new NanoDatabase(dbOptions);
                    disposeDbConn = true;
                }
                else if (INanoCredentials.DefaultCredentials != null)
                {
                    conn = new NanoDatabase(INanoCredentials.DefaultCredentials);
                    disposeDbConn = true;
                }
            }
            */

            // Return
            return (conn, disposeDbConn);
        }
        #endregion

        #region Instance As An Array
        public object this[string propertyName]
        {
            get { return GetType().GetProperty(propertyName).GetValue(this); }
            set { GetType().GetProperty(propertyName).SetValue(this, value); }
        }
        #endregion

        #region Constructor
        public NanoObject()
        {
            SetDefaultValues();
        }

        public NanoObject(INanoDatabase db, object primaryKeyValue)
        {
            // Check Point
            if (primaryKeyValue == null) return;

            // Check Point
            if (string.IsNullOrEmpty(GetTableName()) || string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return;

            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null)
                return;

            // Get Entity
            var data = Conn.GetConnection(true).QueryFirstOrDefault<TEntity>($"SELECT * FROM {db.Helper.Quote(GetTableName())} WHERE {db.Helper.Quote(GetPrimaryKeyColumnName())}='{primaryKeyValue}'");
            if (Dispose) Conn.Dispose();

            // Mapping
            EntityMapper(data);
        }
        #endregion

        #region Private Methods
        private void EntityMapper(TEntity data)
        {
            // Check Point
            if (data == null) return;

            // Action
            var dataProperties = data.GetType().GetProperties();
            var nanoProperties = this.GetType().GetProperties();
            foreach (var nanoProperty in nanoProperties)
            {
                // Check Point
                if (nanoProperty.Name == "Item")
                    continue;

                // Column Options
                var columnOptions = GetColumnOptions(nanoProperty);

                // Find Correct Property and Set
                foreach (var dataProperty in dataProperties)
                {
                    if (nanoProperty.Name == dataProperty.Name || (columnOptions != null && columnOptions.ColumnName == dataProperty.Name))
                    {
                        // Set Value
                        nanoProperty.SetValue(this, dataProperty.GetValue(data));

                        // Break
                        break;
                    }
                }
            }
        }

        private void SetDefaultValues()
        {
            // Action
            var properties = this.GetType().GetProperties();
            foreach (var pi in properties)
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Get Column Info
                var columnInfo = GetColumnOptions(pi);
                if (columnInfo != null && columnInfo.DefaultValue != null)
                {
                    pi.SetValue(this, columnInfo.DefaultValue);
                }
            }
        }
        #endregion

        #region Public Methods
        public void SetIdentity(object value)
        {
            // Primary Key
            var pi = GetPrimaryKeyPropertyInfo();

            // Check Point
            if (pi == null) return;
            if (pi.PropertyType != value.GetType()) return;

            // Set Value
            pi.SetValue(this, value);

            // Flag
            this._flagForManualId = true;
        }

        public void SaveChanges(INanoDatabase db) => SaveChangesAsync(db).Wait();
        public async Task SaveChangesAsync(INanoDatabase db)
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForSaving(db);
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(NanoPrimaryKeyAttribute)} attributes");

            // Execute
            var primaryKey = GetPrimaryKeyPropertyInfo();
            var pkOptions = GetPrimaryKeyOptions();
            if (!pkOptions.AutoIncrement && new Type[] { typeof(int), typeof(long) }.Contains(primaryKey.PropertyType))
            {
                object id = null;
                sql += Conn.Helper.ScopeIdentity(primaryKey.PropertyType);
                if (primaryKey.PropertyType == typeof(int)) id = (await Conn.GetConnection(true).QueryAsync<int>(sql, this)).Single();
                else if (primaryKey.PropertyType == typeof(long)) id = (await Conn.GetConnection(true).QueryAsync<long>(sql, this)).Single();
                if (Dispose) Conn.Dispose();
            }
            else
            {
                await Conn.GetConnection(true).QueryAsync(sql, this);
                if (Dispose) Conn.Dispose();
            }
        }

        public void Delete(INanoDatabase db) => this.DeleteAsync(db).Wait();
        public async Task DeleteAsync(INanoDatabase db)
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForDelete(db);
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(NanoPrimaryKeyAttribute)} attributes");

            // Execute
            await Conn.GetConnection(true).ExecuteAsync(sql, this);
            if (Dispose) Conn.Dispose();
        }
        #endregion

        #region Sql Commands
        public string SqlCommandForSaving(INanoDatabase db)
        {
            // Check Point
            if (string.IsNullOrEmpty(GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var primaryKey = GetPrimaryKeyColumnName();
            var primaryKeyProperty = this.GetType().GetProperty(primaryKey);
            var primaryKeyType = primaryKeyProperty.PropertyType;
            var primaryKeyValue = primaryKeyProperty.GetValue(this);
            var primaryKeyOptions = GetPrimaryKeyOptions();

            if (primaryKeyOptions.AutoIncrement)
            {
                var isNull = primaryKeyValue == null;
                var isDefault = false;
                if (primaryKeyValue != null)
                {
                    if (primaryKeyValue is int) isDefault = EqualityComparer<int>.Default.Equals((int)primaryKeyValue, default);
                    if (primaryKeyValue is int?) isDefault = EqualityComparer<int?>.Default.Equals((int?)primaryKeyValue, default);
                    if (primaryKeyValue is long) isDefault = EqualityComparer<long>.Default.Equals((long)primaryKeyValue, default);
                    if (primaryKeyValue is long?) isDefault = EqualityComparer<long?>.Default.Equals((long?)primaryKeyValue, default);
                    if (primaryKeyValue is Guid) isDefault = EqualityComparer<Guid>.Default.Equals((Guid)primaryKeyValue, default);
                    if (primaryKeyValue is Guid?) isDefault = EqualityComparer<Guid?>.Default.Equals((Guid?)primaryKeyValue, default);
                    if (primaryKeyValue is string) isDefault = EqualityComparer<string>.Default.Equals((string)primaryKeyValue, default);
                }

                // Insert
                if (isNull || isDefault) return this.SqlCommandForInsert(db);

                // Update
                else return this.SqlCommandForUpdate(db);
            }
            else
            {
                // Insert
                if (this._flagForManualId) return this.SqlCommandForInsert(db);

                // Update
                else return this.SqlCommandForUpdate(db);
            }
        }

        private string SqlCommandForInsert(INanoDatabase db)
        {
            // Check Point
            if (string.IsNullOrEmpty(GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var primaryKey = GetPrimaryKeyColumnName();
            var primaryKeyOptions = GetPrimaryKeyOptions();

            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Check Point
                if (primaryKeyOptions.AutoIncrement && primaryKey == pi.Name)
                    continue;

                // Check Column Options
                var columnOptions = GetColumnOptions(pi);
                var ignore = columnOptions != null && columnOptions.IgnoreOnInsert;
                if (ignore) continue;

                // Add to lists
                columnNames.Add(db.Helper.Quote(GetDatabaseColumnName(pi)));
                columnValues.Add($"@{pi.Name}");
            }

            // Return
            return $"INSERT INTO {db.Helper.Quote(GetTableName())} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", columnValues)});";
        }

        private string SqlCommandForUpdate(INanoDatabase db)
        {
            // Check Point
            if (string.IsNullOrEmpty(GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var columns = new List<string>();
            var primaryKey = GetPrimaryKeyColumnName();
            var primaryKeyColumn = string.Empty;
            var primaryKeyProperty = string.Empty;
            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Check Point
                if (primaryKey == pi.Name)
                {
                    primaryKeyColumn = GetDatabaseColumnName(pi);
                    primaryKeyProperty = $"@{pi.Name}";
                    continue;
                }

                // Check Column Options
                var columnOptions = GetColumnOptions(pi);
                var ignore = columnOptions != null && columnOptions.IgnoreOnUpdate;
                if (ignore) continue;

                // Add to lists
                columns.Add($"{db.Helper.Quote(GetDatabaseColumnName(pi))}=@{pi.Name}");
            }

            // Return
            return $"UPDATE {db.Helper.Quote(GetTableName())} SET {string.Join(", ", columns)} WHERE {db.Helper.Quote(primaryKeyColumn)}={primaryKeyProperty};";
        }

        private string SqlCommandForDelete(INanoDatabase db)
        {
            // Check Point
            if (string.IsNullOrEmpty(GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return string.Empty;

            // Get Primary Key PropertyInfo
            var primaryKey = GetPrimaryKeyPropertyInfo();

            // Check Point
            if (primaryKey == null) return string.Empty;
            if (primaryKey.Name == "Item") return string.Empty;

            // Action
            var primaryKeyColumnName = GetDatabaseColumnName(primaryKey);
            var primaryKeyPropertyName = $"@{primaryKey.Name}";

            // Return
            return $"DELETE FROM {db.Helper.Quote(GetTableName())} WHERE {db.Helper.Quote(primaryKeyColumnName)}={primaryKeyPropertyName};";
        }
        #endregion

        #region Json Methods
        public void JsonImport(string jsonObject)
        {
            var json = JsonConvert.DeserializeObject(jsonObject);

            var jsonProperties = json.GetType().GetProperties();
            var pocoProperties = GetType().GetProperties();
            foreach (var pocoProperty in pocoProperties)
            {
                // Check Point
                if (pocoProperty.Name == "Item")
                    continue;

                // Find Correct Property and Set
                foreach (var jsonProperty in jsonProperties)
                {
                    if (pocoProperty.Name == jsonProperty.Name)
                    {
                        // Set Value
                        pocoProperty.SetValue(this, jsonProperty.GetValue(json));

                        // Break
                        break;
                    }
                }
            }
        }

        public string JsonExport()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion

        #region Static Properties
        public static NanoProperties<TEntity> Properties { get { return new NanoProperties<TEntity>(); } }
        public static NanoSqlBuilder<TEntity> CreateSqlBuilder(INanoDatabase db) => new NanoSqlBuilder<TEntity>(db);
        #endregion

#if FALSE
        #region Static Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query">"SELECT TABLENAME.* FROM TABLENAME "'den sonra eklenebilecek her türlü sorgu (Where, Order By, Join vs). Örnek: WHERE ID>100 AND ID<1000 ORDER BY ID</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static TEntity GetRow(INanoDatabase db, string query = "") => GetRowAsync(db, query).Result;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query">"SELECT TABLENAME.* FROM TABLENAME "'den sonra eklenebilecek her türlü sorgu (Where, Order By, Join vs). Örnek: WHERE ID>100 AND ID<1000 ORDER BY ID</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<TEntity> GetRowAsync(INanoDatabase db, string query = "")
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"SELECT {db.Helper.Quote(GetTableName())}.* FROM {db.Helper.Quote(GetTableName())} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Get Entities
            var data = await Conn.GetConnection(true).QueryFirstOrDefaultAsync<TEntity>(sql);
            if (Dispose) Conn.Dispose();

            // Return
            return data;
        }

        /// <summary>
        /// Birden fazla kayıt döndürür
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query">"SELECT TABLENAME.* FROM TABLENAME "'den sonra eklenebilecek her türlü sorgu (Where, Order By, Join vs). Örnek: WHERE ID>100 AND ID<1000 ORDER BY ID</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<TEntity> GetRows(INanoDatabase db, string query = "") => GetRowsAsync(db, query).Result;
        /// <summary>
        /// Birden fazla kayıt döndürür
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query">"SELECT TABLENAME.* FROM TABLENAME "'den sonra eklenebilecek her türlü sorgu (Where, Order By, Join vs). Örnek: WHERE ID>100 AND ID<1000 ORDER BY ID</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<TEntity>> GetRowsAsync(INanoDatabase db, string query = "")
        {
            // Get Connection
            var (DbConn, Dispose) = GetConnection(db);

            // Check Point
            if (DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"SELECT {db.Helper.Quote(GetTableName())}.* FROM {db.Helper.Quote(GetTableName())} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Get Entities
            var data = (await DbConn.GetConnection(true).QueryAsync<TEntity>(sql)).ToList();
            if (Dispose) DbConn.Dispose();

            // Return
            return data;
        }

        public static TEntity GetRowById(INanoDatabase db, object id) => GetRowByIdAsync(db, id).Result;
        public static async Task<TEntity> GetRowByIdAsync(INanoDatabase db, object id)
        {
            // Check Point
            if (id == null) throw new Exception($"Primary Key value is null! Check {nameof(id)} parameter");

            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = GetPrimaryKeyPropertyInfo();
            if (id.GetType() != primaryKey.PropertyType) throw new Exception($"Primary Key type is invalid! Check {nameof(id)} parameter");

            // Build Sql Query
            var primaryKeyColumnName = GetPrimaryKeyColumnName();
            var sql = $"SELECT {db.Helper.Quote(GetTableName())}.* FROM {db.Helper.Quote(GetTableName())} WHERE {db.Helper.Quote(primaryKeyColumnName)}=@ID";

            // Get Entity
            var data = await Conn.GetConnection(true).QueryFirstOrDefaultAsync<TEntity>(sql, new { ID = id });
            if (Dispose) Conn.Dispose();

            // Return
            return data;
        }

        public static List<TEntity> GetRowsByIds(INanoDatabase db, List<object> ids, int useInClauseLimit = 100) => GetRowsByIdsAsync(db, ids, useInClauseLimit).Result;
        public static async Task<List<TEntity>> GetRowsByIdsAsync(INanoDatabase db, List<object> ids, int useInClauseLimit = 100)
        {
            // Check Point
            if (ids == null) throw new Exception($"Primary Key value is null! Check {nameof(ids)} parameter");

            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = GetPrimaryKeyPropertyInfo();

            // Check Point
            foreach (var id in ids)
                if (id.GetType() != primaryKey.PropertyType)
                    throw new Exception($"Primary Key type is invalid! Check {nameof(ids)} parameter");

            // Split ids into chunks
            var idChunks = ids.ChunkBy(useInClauseLimit);

            // Action
            var returnList = new List<TEntity>();
            var tableName = GetTableName();
            var primaryKeyColumnName = GetPrimaryKeyColumnName();
            foreach (var chunk in idChunks)
            {
                var sql = $"SELECT {db.Helper.Quote(tableName)}.* FROM {db.Helper.Quote(tableName)} WHERE {db.Helper.Quote(primaryKeyColumnName)} IN @IDS";
                var data = (await Conn.GetConnection(true).QueryAsync<TEntity>(sql, new { IDS = chunk })).ToList();
                returnList.AddRange(data);
            }

            // Dispose Connection
            if (Dispose) Conn.Dispose();

            // Return
            return returnList;
        }

        public static void DeleteRows(INanoDatabase db, string query) => DeleteRowsAsync(db, query).Wait();
        public static async Task DeleteRowsAsync(INanoDatabase db, string query)
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"DELETE FROM {db.Helper.Quote(GetTableName())} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Execute
            await Conn.GetConnection(true).ExecuteAsync(sql);
            if (Dispose) Conn.Dispose();
        }

        public static void DeleteRows(INanoDatabase db, List<NanoObject<TEntity>> entities, int useInClauseLimit = 100) => DeleteRowsAsync(db, entities, useInClauseLimit).Wait();
        public static async Task DeleteRowsAsync(INanoDatabase db, List<NanoObject<TEntity>> entities, int useInClauseLimit = 100)
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Split entities into chunks
            var entityChunks = entities.ChunkBy(useInClauseLimit);

            // Action
            var tableName = GetTableName();
            var primaryKeyProperty = GetPrimaryKeyPropertyInfo();
            var primaryKeyColumnName = GetPrimaryKeyColumnName();
            foreach (var chunk in entityChunks)
            {
                var sql = $"DELETE FROM {db.Helper.Quote(tableName)} WHERE {db.Helper.Quote(primaryKeyColumnName)} IN @IDS";
                await Conn.GetConnection(true).ExecuteAsync(sql, new { IDS = chunk.Select(x => x[primaryKeyProperty.Name]) });
            }

            // Dispose Connection
            if (Dispose) Conn.Dispose();
        }

        public static void DeleteByIds(INanoDatabase db, List<object> ids, int useInClauseLimit = 100) => DeleteByIdsAsync(db, ids, useInClauseLimit).Wait();
        public static async Task DeleteByIdsAsync(INanoDatabase db, List<object> ids, int useInClauseLimit = 100)
        {
            // Get Connection
            var (Conn, Dispose) = GetConnection(db);

            // Check Point
            if (Conn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = GetPrimaryKeyPropertyInfo();

            // Check Point
            foreach (var id in ids)
                if (id.GetType() != primaryKey.PropertyType)
                    throw new Exception($"Primary Key type is invalid! Check {nameof(ids)} parameter");

            // Split ids into chunks
            var idChunks = ids.ChunkBy(useInClauseLimit);

            // Action
            var tableName = GetTableName();
            var primaryKeyColumnName = GetPrimaryKeyColumnName();
            foreach (var chunk in idChunks)
            {
                var sql = $"DELETE FROM {db.Helper.Quote(tableName)} WHERE {db.Helper.Quote(primaryKeyColumnName)} IN @IDS";
                await Conn.GetConnection(true).ExecuteAsync(sql, new { IDS = chunk });
            }

            // Dispose Connection
            if (Dispose) Conn.Dispose();
        }
        #endregion
#endif
    }
}