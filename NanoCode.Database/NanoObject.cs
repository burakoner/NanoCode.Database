using Dapper;
using NanoCode.Database.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NanoCode.Database
{
    public abstract class NanoObject<TEntity>
    {
        #region Flags
        protected bool FlagForManualId = false;
        #endregion

        #region Attribute Properties
        public string GetTableName()
        {
            // Action
            var tableName = this.GetType().GetAttributeValue((NanoTableAttribute attr) => attr.TableName);
            if(string.IsNullOrEmpty(tableName)) tableName = this.GetType().Name;

            // Return
            return tableName;
        }
        public INanoCredentials GetDatabaseCredentials()
            => this.GetType().GetAttributeValue((NanoCredentialsAttribute attr) => attr.Credentials);

        public string GetPrimaryKeyColumnName()
        {
            var pi = this.GetPrimaryKeyPropertyInfo();
            return pi != null ? pi.Name : string.Empty;
        }

        public PropertyInfo GetPrimaryKeyPropertyInfo()
            => this.GetPrimaryKey().Info;
        public PrimaryKeyOptions GetPrimaryKeyOptions()
            => this.GetPrimaryKey().Options;
        public (PropertyInfo Info, PrimaryKeyOptions Options) GetPrimaryKey()
        {
            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // PrimaryKeyAttribute
                var attrs = pi.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    if (attr.GetType().Name != nameof(PrimaryKeyAttribute))
                        continue;

                    var pka = (PrimaryKeyAttribute)attr;
                    if (pka.IsPrimaryKey)
                        return (pi, pka.ToPrimaryKeyOptions());
                }

                // NanoTableColumnAttribute
                var options = this.GetColumnOptions(pi);
                if (options != null && options.IsPrimaryKey)
                    return (pi, options.ToPrimaryKeyOptions());
            }

            // Return
            return (null, null);
        }

        public string GetDatabaseColumnName(PropertyInfo pi)
        {
            // Şimdilik böyle kalması yeterli.
            // Property adı ile veritabanındaki sütunun adı farklılaştırıldığında geliştirme burada yapılabilir.
            return pi.Name;
        }

        private NanoTableColumnOptions GetColumnOptions(PropertyInfo pi)
        {
            // Find Correct Property and Set
            var attrs = pi.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (attr.GetType().Name != nameof(NanoTableColumnAttribute))
                    continue;

                return ((NanoTableColumnAttribute)attr).Options;
            }

            // Return
            return null;
        }

        public (INanoDatabase DbConn, bool Dispose) GetConnection(INanoDatabase db)
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
            var conn = GetConnection(db);

            // Check Point
            if (conn.DbConn == null)
                return;

            // Get Entity
            var data = conn.DbConn.GetConnection(true).QueryFirstOrDefault<TEntity>($"SELECT * FROM {this.GetTableName()} WHERE {this.GetPrimaryKeyColumnName()}='{primaryKeyValue}'");
            if (conn.Dispose) conn.DbConn.Dispose();

            // Mapping
            DataMapper(data);
        }
        #endregion

        private void DataMapper(TEntity data)
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

                // Find Correct Property and Set
                foreach (var dataProperty in dataProperties)
                {
                    if (nanoProperty.Name == dataProperty.Name)
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
                var columnInfo = this.GetColumnOptions(pi);
                if (columnInfo != null && columnInfo.DefaultValue != null)
                {
                    pi.SetValue(this, columnInfo.DefaultValue);
                }
            }
        }

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
            this.FlagForManualId = true;
        }

        public void SaveChanges(INanoDatabase db) => SaveChangesAsync(db).Wait();
        public async Task SaveChangesAsync(INanoDatabase db)
        {
            // Get Connection
            var conn = GetConnection(db);

            // Check Point
            if (conn.DbConn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForSaving();
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(PrimaryKeyAttribute)} attributes");

            // Execute
            var primaryKey = this.GetPrimaryKeyPropertyInfo();
            var pkOptions = this.GetPrimaryKeyOptions();
            if (!pkOptions.AutoIncrement && new Type[] { typeof(int), typeof(long) }.Contains(primaryKey.PropertyType))
            {
                object id = null;
                sql += conn.DbConn.Helper.ScopeIdentity(primaryKey.PropertyType);
                if (primaryKey.PropertyType == typeof(int)) id = (await conn.DbConn.GetConnection(true).QueryAsync<int>(sql, this)).Single();
                else if (primaryKey.PropertyType == typeof(long)) id = (await conn.DbConn.GetConnection(true).QueryAsync<long>(sql, this)).Single();
                if (conn.Dispose) conn.DbConn.Dispose();
            }
            else
            {
                await conn.DbConn.GetConnection(true).QueryAsync(sql, this);
                if (conn.Dispose) conn.DbConn.Dispose();
            }
        }

        public void Delete(INanoDatabase db)=>this.DeleteAsync(db).Wait();
        public async Task DeleteAsync(INanoDatabase db)
        {
            // Get Connection
            var conn = GetConnection(db);

            // Check Point
            if (conn.DbConn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForDelete();
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(PrimaryKeyAttribute)} attributes");

            // Execute
            await conn.DbConn.GetConnection(true).ExecuteAsync(sql, this);
            if (conn.Dispose) conn.DbConn.Dispose();
        }

        public string SqlCommandForSaving()
        {
            // Check Point
            if (string.IsNullOrEmpty(this.GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(this.GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var primaryKey = this.GetPrimaryKeyColumnName();
            var primaryKeyProperty = this.GetType().GetProperty(primaryKey);
            var primaryKeyType = primaryKeyProperty.PropertyType;
            var primaryKeyValue = primaryKeyProperty.GetValue(this);
            var primaryKeyOptions = this.GetPrimaryKeyOptions();

            if (primaryKeyOptions.AutoIncrement)
            {
                var isNull = primaryKeyValue == null;
                var isDefault = false;
                if (primaryKeyValue != null)
                {
                    isDefault =
                        EqualityComparer<int>.Default.Equals((int)primaryKeyValue, default) ||
                        EqualityComparer<long>.Default.Equals((long)primaryKeyValue, default) ||
                        EqualityComparer<string>.Default.Equals((string)primaryKeyValue, default) ||
                        EqualityComparer<Guid>.Default.Equals((Guid)primaryKeyValue, default);
                }

                // Insert
                if (isNull || isDefault) return this.SqlCommandForInsert();

                // Update
                else return this.SqlCommandForUpdate();
            }
            else
            {
                // Insert
                if (this.FlagForManualId) return this.SqlCommandForInsert();

                // Update
                else return this.SqlCommandForUpdate();
            }
        }

        private string SqlCommandForInsert()
        {
            // Check Point
            if (string.IsNullOrEmpty(this.GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(this.GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var primaryKey = this.GetPrimaryKeyColumnName();
            var primaryKeyOptions = this.GetPrimaryKeyOptions();

            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Check Point
                if (primaryKeyOptions.AutoIncrement && primaryKey == pi.Name)
                    continue;

                // Check Column Options
                var columnOptions = this.GetColumnOptions(pi);
                var ignore = columnOptions != null && (columnOptions.IsIgnored || columnOptions.IsIgnoredOnInsert);
                if (ignore) continue;

                // Add to lists
                columnNames.Add(this.GetDatabaseColumnName(pi));
                columnValues.Add($"@{pi.Name}");
            }

            // Return
            return $"INSERT INTO {this.GetTableName()} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", columnValues)});";
        }

        private string SqlCommandForUpdate()
        {
            // Check Point
            if (string.IsNullOrEmpty(this.GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(this.GetPrimaryKeyColumnName())) return string.Empty;

            // Action
            var columns = new List<string>();
            var primaryKey = this.GetPrimaryKeyColumnName();
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
                var columnOptions = this.GetColumnOptions(pi);
                var ignore = columnOptions != null && (columnOptions.IsIgnored || columnOptions.IsIgnoredOnUpdate);
                if (ignore) continue;

                // Add to lists
                columns.Add($"{this.GetDatabaseColumnName(pi)}=@{pi.Name}");
            }

            // Return
            return $"UPDATE {this.GetTableName()} SET {string.Join(", ", columns)} WHERE {primaryKeyColumn}={primaryKeyProperty};";
        }

        private string SqlCommandForDelete()
        {
            // Check Point
            if (string.IsNullOrEmpty(this.GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(this.GetPrimaryKeyColumnName())) return string.Empty;

            // Get Primary Key PropertyInfo
            var primaryKey = this.GetPrimaryKeyPropertyInfo();

            // Check Point
            if (primaryKey == null) return string.Empty;
            if (primaryKey.Name == "Item") return string.Empty;

            // Action
            var primaryKeyColumnName = GetDatabaseColumnName(primaryKey);
            var primaryKeyPropertyName = $"@{primaryKey.Name}";

            // Return
            return $"DELETE FROM {this.GetTableName()} WHERE {primaryKeyColumnName}={primaryKeyPropertyName};";
        }

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
            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"SELECT {dummy.GetTableName()}.* FROM {dummy.GetTableName()} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Get Entities
            var data = await conn.DbConn.GetConnection(true).QueryFirstOrDefaultAsync<TEntity>(sql);
            if (conn.Dispose) conn.DbConn.Dispose();

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
            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"SELECT {dummy.GetTableName()}.* FROM {dummy.GetTableName()} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Get Entities
            var data = (await conn.DbConn.GetConnection(true).QueryAsync<TEntity>(sql)).ToList();
            if (conn.Dispose) conn.DbConn.Dispose();

            // Return
            return data;
        }

        public static TEntity GetRowById(INanoDatabase db, object id)=> GetRowByIdAsync(db, id).Result;
        public static async Task<TEntity> GetRowByIdAsync(INanoDatabase db, object id)
        {
            // Check Point
            if (id == null) throw new Exception($"Primary Key value is null! Check {nameof(id)} parameter");

            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = dummy.GetPrimaryKeyPropertyInfo();
            if (id.GetType() != primaryKey.PropertyType) throw new Exception($"Primary Key type is invalid! Check {nameof(id)} parameter");

            // Build Sql Query
            var primaryKeyColumnName = dummy.GetPrimaryKeyColumnName();
            var sql = $"SELECT {dummy.GetTableName()}.* FROM {dummy.GetTableName()} WHERE {primaryKeyColumnName}=@ID";

            // Get Entity
            var data = await conn.DbConn.GetConnection(true).QueryFirstOrDefaultAsync<TEntity>(sql, new { ID = id });
            if (conn.Dispose) conn.DbConn.Dispose();

            // Return
            return data;
        }

        public static List<TEntity> GetRowsByIds(INanoDatabase db, List<object> ids, int useInClauseLimit = 100) => GetRowsByIdsAsync(db, ids, useInClauseLimit).Result;
        public static async Task<List<TEntity>> GetRowsByIdsAsync(INanoDatabase db, List<object> ids, int useInClauseLimit = 100)
        {
            // Check Point
            if (ids == null) throw new Exception($"Primary Key value is null! Check {nameof(ids)} parameter");

            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = dummy.GetPrimaryKeyPropertyInfo();

            // Check Point
            foreach (var id in ids)
                if (id.GetType() != primaryKey.PropertyType)
                    throw new Exception($"Primary Key type is invalid! Check {nameof(ids)} parameter");

            // Split ids into chunks
            var idChunks = ids.ChunkBy(useInClauseLimit);

            // Action
            var returnList = new List<TEntity>();
            var tableName = dummy.GetTableName();
            var primaryKeyColumnName = dummy.GetPrimaryKeyColumnName();
            foreach (var chunk in idChunks)
            {
                var sql = $"SELECT {tableName}.* FROM {tableName} WHERE {primaryKeyColumnName} IN @IDS";
                var data = (await conn.DbConn.GetConnection(true).QueryAsync<TEntity>(sql, new { IDS = chunk })).ToList();
                returnList.AddRange(data);
            }

            // Dispose Connection
            if (conn.Dispose) conn.DbConn.Dispose();

            // Return
            return returnList;
        }

        public static void DeleteRows(INanoDatabase db, string query)=> DeleteRowsAsync(db, query).Wait();
        public static async Task DeleteRowsAsync(INanoDatabase db, string query)
        {
            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = $"DELETE FROM {dummy.GetTableName()} ";
            if (!string.IsNullOrEmpty(query)) sql += query;

            // Execute
            await conn.DbConn.GetConnection(true).ExecuteAsync(sql);
            if (conn.Dispose) conn.DbConn.Dispose();
        }

        public static void DeleteRows(INanoDatabase db, List<NanoObject<TEntity>> entities, int useInClauseLimit = 100) => DeleteRowsAsync(db, entities, useInClauseLimit).Wait();
        public static async Task DeleteRowsAsync(INanoDatabase db, List<NanoObject<TEntity>> entities, int useInClauseLimit = 100)
        {
            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Split entities into chunks
            var entityChunks = entities.ChunkBy(useInClauseLimit);

            // Action
            var tableName = dummy.GetTableName();
            var primaryKeyProperty = dummy.GetPrimaryKeyPropertyInfo();
            var primaryKeyColumnName = dummy.GetPrimaryKeyColumnName();
            foreach (var chunk in entityChunks)
            {
                var sql = $"DELETE FROM {tableName} WHERE {primaryKeyColumnName} IN @IDS";
                await conn.DbConn.GetConnection(true).ExecuteAsync(sql, new { IDS = chunk.Select(x => x[primaryKeyProperty.Name]) });
            }

            // Dispose Connection
            if (conn.Dispose) conn.DbConn.Dispose();
        }

        public static void DeleteByIds(INanoDatabase db, List<object> ids, int useInClauseLimit = 100)=> DeleteByIdsAsync(db, ids, useInClauseLimit).Wait();
        public static async Task DeleteByIdsAsync(INanoDatabase db, List<object> ids, int useInClauseLimit = 100)
        {
            // Dummy Instance
            NanoObject<TEntity> dummy = default;

            // Get Connection
            var conn = dummy.GetConnection(db);

            // Check Point
            if (conn.DbConn == null) throw new Exception("Database connection is null or invalid!");

            // Get Primary Key Properties
            var primaryKey = dummy.GetPrimaryKeyPropertyInfo();

            // Check Point
            foreach (var id in ids)
                if (id.GetType() != primaryKey.PropertyType)
                    throw new Exception($"Primary Key type is invalid! Check {nameof(ids)} parameter");

            // Split ids into chunks
            var idChunks = ids.ChunkBy(useInClauseLimit);

            // Action
            var tableName = dummy.GetTableName();
            var primaryKeyColumnName = dummy.GetPrimaryKeyColumnName();
            foreach (var chunk in idChunks)
            {
                var sql = $"DELETE FROM {tableName} WHERE {primaryKeyColumnName} IN @IDS";
                await conn.DbConn.GetConnection(true).ExecuteAsync(sql, new { IDS = chunk });
            }

            // Dispose Connection
            if (conn.Dispose) conn.DbConn.Dispose();
        }
        #endregion

    }

}