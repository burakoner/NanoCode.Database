using SqlKata;
using System;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> Combine(string operation, bool all, Query query)
        {
            this.Query.Combine(operation, all, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> CombineRaw(string sql, params object[] bindings)
        {
            this.Query.CombineRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Union(Query query, bool all = false)
        {
            this.Query.Union(query, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> UnionAll(Query query)
        {
            this.Query.UnionAll(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> Union(Func<Query, Query> callback, bool all = false)
        {
            this.Query.Union(callback, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> UnionAll(Func<Query, Query> callback)
        {
            this.Query.UnionAll(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> UnionRaw(string sql, params object[] bindings)
        {
            this.Query.UnionRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Except(Query query, bool all = false)
        {
            this.Query.Except(query, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> ExceptAll(Query query)
        {
            this.Query.ExceptAll(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> Except(Func<Query, Query> callback, bool all = false)
        {
            this.Query.Except(callback, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> ExceptAll(Func<Query, Query> callback)
        {
            this.Query.ExceptAll(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> ExceptRaw(string sql, params object[] bindings)
        {
            this.Query.ExceptRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Intersect(Query query, bool all)
        {
            this.Query.Intersect(query, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> IntersectAll(Query query)
        {
            this.Query.IntersectAll(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> Intersect(Func<Query, Query> callback, bool all = false)
        {
            this.Query.Intersect(callback, all);
            return this;
        }
        public NanoSqlBuilder<TEntity> IntersectAll(Func<Query, Query> callback)
        {
            this.Query.IntersectAll(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> IntersectRaw(string sql, params object[] bindings)
        {
            this.Query.IntersectRaw(sql, bindings);
            return this;
        }
    }
}
