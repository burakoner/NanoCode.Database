using Dapper;
using Nanocode.Database.Interfaces;
using Nanocode.Database.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nanocode.Database
{
    public abstract class NanoObject<TEntity> : INanoObject
    {
        #region Flags
        private bool? _existOnDatabase = null;
        #endregion

        #region Attribute Properties
        public static string GetTableName()
        {
            // Action
            var tableName = typeof(TEntity).GetAttributeValue((NanoTableAttribute attr) => attr.TableName);
            if (string.IsNullOrEmpty(tableName)) tableName = typeof(TEntity).Name;

            // Return
            return tableName;
        }
        public static string GetDatabaseColumnName(PropertyInfo pi)
        {
            if (pi == null) return string.Empty;

            var columnOptions = GetColumnOptions(pi);
            if (columnOptions == null) return pi.Name;
            if (string.IsNullOrWhiteSpace(columnOptions.ColumnName)) return pi.Name;

            return columnOptions.ColumnName;
        }
        public static string GetPrimaryKeyColumnName() => GetDatabaseColumnName(GetPrimaryKeyPropertyInfo());

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
        internal static PropertyInfo GetPrimaryKeyPropertyInfo() => GetPrimaryKey().Info;
        internal static NanoPrimaryKeyOptions GetPrimaryKeyOptions() => GetPrimaryKey().Options;
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
            this.SetDefaultValues();
        }

        public NanoObject(INanoDatabase db, object primaryKeyValue)
        {
            // Check Point
            if (primaryKeyValue == null) return;

            // Check Point
            if (string.IsNullOrEmpty(GetTableName()) || string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return;

            // Get Connection
            var (conn, dispose) = GetConnection(db);

            // Check Point
            if (conn == null)
                return;

            // Get Entity
            var data = conn.GetConnection(true).QueryFirstOrDefault<TEntity>($"SELECT * FROM {db.Helper.Quote(GetTableName())} WHERE {db.Helper.Quote(GetPrimaryKeyColumnName())}='{primaryKeyValue}'");
            if (dispose) conn.Dispose();

            // Mapping
            EntityMapper(data);
            _existOnDatabase = true;
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
        public void SaveChanges(INanoDatabase db) => SaveChangesAsync(db).Wait();
        public async Task SaveChangesAsync(INanoDatabase db)
        {
            // Get Connection
            var (conn, dispose) = GetConnection(db);

            // Check Point
            if (conn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForSave(db);
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(NanoPrimaryKeyAttribute)} attributes");

            // Scope Identity
            var pkOptions = GetPrimaryKeyOptions();

            // Execute
            var wait = (!pkOptions.AutoIncrement && !dispose);
            await conn.GetConnection(true).QueryAsync(sql, this).ConfigureAwait(wait);

            // Scope Identity
            if (pkOptions.AutoIncrement)
            {
                var primaryKey = GetPrimaryKeyPropertyInfo();
                var primaryKeyColumn = GetPrimaryKeyColumnName();
                var primaryKeyIsNumeric =
                    primaryKey.PropertyType == typeof(byte) ||
                    primaryKey.PropertyType == typeof(short) ||
                    primaryKey.PropertyType == typeof(int) ||
                    primaryKey.PropertyType == typeof(long);
                var primaryKeyIsUnsigned =
                    primaryKey.PropertyType == typeof(ushort) ||
                    primaryKey.PropertyType == typeof(uint) ||
                    primaryKey.PropertyType == typeof(ulong);
                var primaryKeyIsFloatingNumber =
                    primaryKey.PropertyType == typeof(float) ||
                    primaryKey.PropertyType == typeof(double) ||
                    primaryKey.PropertyType == typeof(decimal);
                var primaryKeyIsString =
                    primaryKey.PropertyType == typeof(Guid) ||
                    primaryKey.PropertyType == typeof(string);
                var scopeIdentitySql = conn.Helper.ScopeIdentity(primaryKey.PropertyType, primaryKeyColumn, 
                    primaryKeyIsNumeric || primaryKeyIsUnsigned || primaryKeyIsFloatingNumber || primaryKeyIsString);
                if (!string.IsNullOrEmpty(scopeIdentitySql))
                {
                    if (primaryKeyIsNumeric)
                    {
                        var id = (await conn.GetConnection(true).QueryAsync<long>(scopeIdentitySql, this)).Single();
                        primaryKey.SetValue(this, id);
                    }
                    else if (primaryKeyIsUnsigned)
                    {
                        var id = (await conn.GetConnection(true).QueryAsync<ulong>(scopeIdentitySql, this)).Single();
                        primaryKey.SetValue(this, id);
                    }
                    else if (primaryKeyIsFloatingNumber)
                    {
                        var id = (await conn.GetConnection(true).QueryAsync<decimal>(scopeIdentitySql, this)).Single();
                        primaryKey.SetValue(this, id);
                    }
                    else if (primaryKeyIsString)
                    {
                        var id = (await conn.GetConnection(true).QueryAsync<string>(scopeIdentitySql, this)).Single();
                        primaryKey.SetValue(this, id);
                    }
                }
            }

            // Set Flag
            this._existOnDatabase = true;

            // Dispose
            if (dispose) conn.Dispose();
        }

        public void Delete(INanoDatabase db) => this.DeleteAsync(db).Wait();
        public async Task DeleteAsync(INanoDatabase db)
        {
            // Get Connection
            var (conn, dispose) = GetConnection(db);

            // Check Point
            if (conn == null)
                throw new Exception("Database connection is null or invalid!");

            // Get Sql Query
            var sql = this.SqlCommandForDelete(db);
            if (string.IsNullOrEmpty(sql))
                throw new Exception($"Sql command is invalid. Check {nameof(NanoTableAttribute)} {nameof(NanoTableAttribute.TableName)} and {nameof(NanoPrimaryKeyAttribute)} attributes");

            // Execute
            await conn.GetConnection(true).ExecuteAsync(sql, this).ConfigureAwait(dispose);
            if (dispose) conn.Dispose();
        }

        public void SetDatabaseStatus(bool exists)
        {
            this._existOnDatabase = exists;
        }
        #endregion

        #region Sql Commands
        private string SqlCommandForSave(INanoDatabase db)
        {
            // Check Point
            if (string.IsNullOrEmpty(GetTableName())) return string.Empty;
            if (string.IsNullOrEmpty(GetPrimaryKeyColumnName())) return string.Empty;

            // Check Point
            if (_existOnDatabase.HasValue)
                return _existOnDatabase.Value
                    ? this.SqlCommandForUpdate(db)
                    : this.SqlCommandForInsert(db);

            // Action
            var primaryKey = GetPrimaryKeyColumnName();
            var primaryKeyProperty = GetPrimaryKeyPropertyInfo();
            var primaryKeyType = primaryKeyProperty.PropertyType;
            var primaryKeyValue = primaryKeyProperty.GetValue(this);
            var primaryKeyOptions = GetPrimaryKeyOptions();

            var isNull = primaryKeyValue == null;
            var isDefault = false;
            if (primaryKeyValue != null)
            {
                if (primaryKeyValue is int) isDefault = EqualityComparer<int>.Default.Equals((int)primaryKeyValue, default);
                else if (primaryKeyValue is int?) isDefault = EqualityComparer<int?>.Default.Equals((int?)primaryKeyValue, default);
                else if (primaryKeyValue is long) isDefault = EqualityComparer<long>.Default.Equals((long)primaryKeyValue, default);
                else if (primaryKeyValue is long?) isDefault = EqualityComparer<long?>.Default.Equals((long?)primaryKeyValue, default);
                else if (primaryKeyValue is Guid) isDefault = EqualityComparer<Guid>.Default.Equals((Guid)primaryKeyValue, default);
                else if (primaryKeyValue is Guid?) isDefault = EqualityComparer<Guid?>.Default.Equals((Guid?)primaryKeyValue, default);
                else if (primaryKeyValue is string) isDefault = EqualityComparer<string>.Default.Equals((string)primaryKeyValue, default);
            }

            if (primaryKeyOptions.AutoIncrement)
            {
                return isNull || isDefault
                    ? this.SqlCommandForInsert(db)
                    : this.SqlCommandForUpdate(db);
            }
            else
            {
                return isNull || isDefault
                   ? this.SqlCommandForInsert(db)
                   : this.SqlCommandForUpdate(db);
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
            var primaryKeyColumn = GetPrimaryKeyColumnName();
            var primaryKeyOptions = GetPrimaryKeyOptions();

            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Check Column Options
                var columnOptions = GetColumnOptions(pi);
                var ignore = columnOptions != null && columnOptions.IgnoreOnInsert;
                if (ignore) continue;

                // Check Point
                if (primaryKeyOptions.AutoIncrement)
                {
                    if (columnOptions != null && columnOptions.ColumnName == primaryKeyColumn)
                        continue;
                    if (columnOptions == null && pi.Name == primaryKeyColumn)
                        continue;
                }

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
            var primaryKey = GetPrimaryKey();
            var primaryKeyColumn = string.Empty;
            var primaryKeyProperty = string.Empty;
            foreach (var pi in this.GetType().GetProperties())
            {
                // Check Point
                if (pi.Name == "Item")
                    continue;

                // Check Point
                if (primaryKey.Info.Name == pi.Name)
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

        #region Static Properties & Methods
        public static NanoProperties<TEntity> Properties { get { return new NanoProperties<TEntity>(); } }
        public static NanoSqlBuilder<TEntity> CreateSqlBuilder(INanoDatabase db) => new NanoSqlBuilder<TEntity>(db);

        public static List<TEntity> GetAllRows(INanoDatabase db) => GetAllRowsAsync(db).Result;
        public static async Task<List<TEntity>> GetAllRowsAsync(INanoDatabase db)
        {
            // Get Connection
            var (conn, dispose) = GetConnection(db);

            // Check Point
            if (conn == null) throw new Exception("Database connection is null or invalid!");

            // Build Sql Query
            var sql = CreateSqlBuilder(db).Select("*").ToString();

            // Get Entities
            var data = (await conn.GetConnection(true).QueryAsync<TEntity>(sql)).ToList();
            if (dispose) conn.Dispose();

            // Return
            return data;
        }
        #endregion
    }
}