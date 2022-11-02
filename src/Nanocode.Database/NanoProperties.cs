using Nanocode.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Nanocode.Database
{
    public class NanoProperties<TEntity>
    {
        internal NanoProperties()
        {
        }

        public string TableName => NanoObject<TEntity>.GetTableName();
        public string PrimaryColumnName => NanoObject<TEntity>.GetPrimaryKeyColumnName();
        public string ColumnName<TColumn>(Expression<Func<TEntity, TColumn>> property)
        {
            // Get PropertyInfo
            var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            // Return
            return NanoObject<TEntity>.GetDatabaseColumnName(propertyInfo);
        }
    }
}
