using Nanocode.Database.Interfaces;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> Select(params string[] columns)
        {
            this.Query.Select(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> Select(IEnumerable<string> columns)
        {
            this.Query.Select(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectRaw(string sql, params object[] bindings)
        {
            this.Query.SelectRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Select(Query query, string alias)
        {
            this.Query.Select(query, alias);
            return this;
        }
        public NanoSqlBuilder<TEntity> Select(Func<Query, Query> callback, string alias)
        {
            this.Query.Select(callback, alias);
            return this;
        }
        /*
        public NanoSqlBuilder<TEntity> SelectAggregate(string aggregate, string column, Query filter = null)
        {
            this.Query.SelectAggregate(aggregate, column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectAggregate(string aggregate, string column, Func<Query, Query> filter)
        {
            this.Query.SelectAggregate(aggregate, column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectSum(string column, Func<Query, Query> filter = null)
        {
            this.Query.SelectSum(column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectCount(string column, Func<Query, Query> filter = null)
        {
            this.Query.SelectCount(column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectAvg(string column, Func<Query, Query> filter = null)
        {
            this.Query.SelectAvg(column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectMin(string column, Func<Query, Query> filter = null)
        {
            this.Query.SelectMin(column, filter);
            return this;
        }
        public NanoSqlBuilder<TEntity> SelectMax(string column, Func<Query, Query> filter = null)
        {
            this.Query.SelectMax(column, filter);
            return this;
        }
        */
    }
}
