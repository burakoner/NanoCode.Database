using Nanocode.Database.Interfaces;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> AsUpdate(object data)
        {
            this.Query.AsUpdate(data);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsUpdate(IEnumerable<string> columns, IEnumerable<object> values)
        {
            this.Query.AsUpdate(columns, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsUpdate(IEnumerable<KeyValuePair<string, object>> values)
        {
            this.Query.AsUpdate(values);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsIncrement(string column, int value = 1)
        {
            this.Query.AsIncrement(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsDecrement(string column, int value = 1)
        {
            this.Query.AsDecrement(column, value);
            return this;
        }
    }
}
