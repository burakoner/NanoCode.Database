using SqlKata;
using System;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> NewQuery()
        {
            this.Query.NewQuery();
            return this;
        }
        public NanoSqlBuilder<TEntity> Clone()
        {
            this.Query.Clone();
            return this;
        }
        public NanoSqlBuilder<TEntity> As(string alias)
        {
            this.Query.As(alias);
            return this;
        }
        public NanoSqlBuilder<TEntity> Comment(string comment)
        {
            this.Query.Comment(comment);
            return this;
        }
        public NanoSqlBuilder<TEntity> For(string engine, Func<Query, Query> fn)
        {
            this.Query.For(engine, fn);
            return this;
        }
        public NanoSqlBuilder<TEntity> With(Query query)
        {
            this.Query.With(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> With(Func<Query, Query> fn)
        {
            this.Query.With(fn);
            return this;
        }
        public NanoSqlBuilder<TEntity> With(string alias, Query query)
        {
            this.Query.With(alias, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> With(string alias, Func<Query, Query> fn)
        {
            this.Query.With(alias, fn);
            return this;
        }
        public NanoSqlBuilder<TEntity> With(string alias, IEnumerable<string> columns, IEnumerable<IEnumerable<object>> valuesCollection)
        {
            this.Query.With(alias, columns, valuesCollection);
            return this;
        }
        public NanoSqlBuilder<TEntity> WithRaw(string alias, string sql, params object[] bindings)
        {
            this.Query.WithRaw(alias, sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Limit(int value)
        {
            this.Query.Limit(value);
            return this;
        }
        public NanoSqlBuilder<TEntity> Offset(long value)
        {
            this.Query.Offset(value);
            return this;
        }
        public NanoSqlBuilder<TEntity> Offset(int value)
        {
            this.Query.Offset(value);
            return this;
        }
        public NanoSqlBuilder<TEntity> Take(int limit)
        {
            this.Query.Take(limit);
            return this;
        }
        public NanoSqlBuilder<TEntity> Skip(int offset)
        {
            this.Query.Skip(offset);
            return this;
        }
        public NanoSqlBuilder<TEntity> ForPage(int page, int perPage = 10)
        {
            this.Query.ForPage(page, perPage);
            return this;
        }
        public NanoSqlBuilder<TEntity> Distinct()
        {
            this.Query.Distinct();
            return this;
        }
        public NanoSqlBuilder<TEntity> When(bool condition, Func<Query, Query> whenTrue, Func<Query, Query> whenFalse = null)
        {
            this.Query.When(condition, whenTrue, whenFalse);
            return this;
        }
        public NanoSqlBuilder<TEntity> WhenNot(bool condition, Func<Query, Query> callback)
        {
            this.Query.WhenNot(condition, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrderBy(params string[] columns)
        {
            this.Query.OrderBy(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrderByDesc(params string[] columns)
        {
            this.Query.OrderByDesc(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrderByRaw(string expression, params object[] bindings)
        {
            this.Query.OrderByRaw(expression, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrderByRandom(string seed)
        {
            this.Query.OrderByRandom(seed);
            return this;
        }
        public NanoSqlBuilder<TEntity> GroupBy(params string[] columns)
        {
            this.Query.GroupBy(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> GroupByRaw(string expression, params object[] bindings)
        {
            this.Query.GroupByRaw(expression, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Include(string relationName, Query query, string foreignKey = null, string localKey = "Id", bool isMany = false)
        {
            this.Query.Include(relationName, query, foreignKey, localKey, isMany);
            return this;
        }
        public NanoSqlBuilder<TEntity> IncludeMany(string relationName, Query query, string foreignKey = null, string localKey = "Id")
        {
            this.Query.IncludeMany(relationName, query, foreignKey, localKey);
            return this;
        }
        public NanoSqlBuilder<TEntity> Define(string variable, object value)
        {
            this.Query.Define(variable, value);
            return this;
        }

    }
}
