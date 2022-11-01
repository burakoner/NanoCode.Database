using Nanocode.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Nanocode.Database
{
    public class NanoSqlBuilder<TEntity>
    {
        private readonly INanoDatabase _db;
        private readonly List<string> _list;

        internal NanoSqlBuilder(INanoDatabase db)
        {
            this._db = db;
            this._list = new List<string>();
        }

        public NanoSqlBuilder<TEntity> Append(string query)
        {
            // Action
            this._list.Add(query);

            // Return
            return this;
        }

        public NanoSqlBuilder<TEntity> AppendTableName()
        {
            // Get TableName
            var tableName = NanoObject<TEntity>.GetTableName();

            // Append
            this._list.Add(_db.Helper.Quote(tableName));

            // Return
            return this;
        }

        public NanoSqlBuilder<TEntity> AppendColumnName<TColumn>(Expression<Func<TEntity, TColumn>> property)
        {
            // Get PropertyInfo
            var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }
            var columnName = NanoObject<TEntity>.GetDatabaseColumnName(propertyInfo);

            // Append
            this._list.Add(_db.Helper.Quote(columnName));

            // Return
            return this;
        }

        public NanoSqlBuilder<TEntity> AppendPrimaryColumnName()
        {
            // Get ColumnName
            var columnName = NanoObject<TEntity>.GetPrimaryKeyColumnName();

            // Append
            this._list.Add(_db.Helper.Quote(columnName));

            // Return
            return this;
        }

        public NanoSqlBuilder<TEntity> Clear()
        {
            // Action
            this._list.Clear();

            // Return
            return this;
        }

        public override string ToString()
        {
            // Action
            var sb = new StringBuilder();
            foreach (var item in this._list)
                sb.Append(item);

            // Action
            return sb.ToString();
        }

        public string ToString(string delimiter)
        {
            // Return
            return string.Join(delimiter, this._list);
        }
    }
}
