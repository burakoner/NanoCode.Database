using SqlKata;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {

        public NanoSqlBuilder<TEntity> AsInsert(object data, bool returnId = false)
        {
            this.Query.AsInsert(data, returnId);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsInsert(IEnumerable<string> columns, IEnumerable<object> values)
        {
            this.Query.AsInsert(columns, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsInsert(IEnumerable<KeyValuePair<string, object>> values, bool returnId = false)
        {
            this.Query.AsInsert(values, returnId);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsInsert(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> rowsValues)
        {
            this.Query.AsInsert(columns, rowsValues);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsInsert(IEnumerable<string> columns, Query query)
        {
            this.Query.AsInsert(columns, query);
            return this;
        }
    }
}
