using SqlKata;
using System;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> Where(string column, string op, object value)
        {
            this.Query.Where(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNot(string column, string op, object value)
        {
            this.Query.WhereNot(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhere(string column, string op, object value)
        {
            this.Query.OrWhere(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNot(string column, string op, object value)
        {
            this.Query.OrWhereNot(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(string column, object value)
        {
            this.Query.Where(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNot(string column, object value)
        {
            this.Query.WhereNot(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhere(string column, object value)
        {
            this.Query.OrWhere(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNot(string column, object value)
        {
            this.Query.OrWhereNot(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(object constraints)
        {
            this.Query.Where(constraints);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(IEnumerable<KeyValuePair<string, object>> values)
        {
            this.Query.Where(values);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereRaw(string sql, params object[] bindings)
        {
            this.Query.WhereRaw(sql, bindings);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereRaw(string sql, params object[] bindings)
        {
            this.Query.OrWhereRaw(sql, bindings);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(Func<Query, Query> callback) // Func<Q, Q>
        {
            this.Query.Where(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNot(Func<Query, Query> callback) // Func<Q, Q>
        {
            this.Query.WhereNot(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhere(Func<Query, Query> callback) // Func<Q, Q>
        {
            this.Query.OrWhere(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNot(Func<Query, Query> callback) // Func<Q, Q>
        {
            this.Query.OrWhereNot(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereColumns(string first, string op, string second)
        {
            this.Query.WhereColumns(first, op, second);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereColumns(string first, string op, string second)
        {
            this.Query.OrWhereColumns(first, op, second);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNull(string column)
        {
            this.Query.WhereNull(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotNull(string column)
        {
            this.Query.WhereNotNull(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNull(string column)
        {
            this.Query.OrWhereNull(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotNull(string column)
        {
            this.Query.OrWhereNotNull(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereTrue(string column)
        {
            this.Query.WhereTrue(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereTrue(string column)
        {
            this.Query.OrWhereTrue(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereFalse(string column)
        {
            this.Query.WhereFalse(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereFalse(string column)
        {
            this.Query.OrWhereFalse(column);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereNotLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereNotLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereNotStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereNotStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereNotEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereNotEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereContains(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.WhereNotContains(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereContains(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrWhereNotContains(column, value, caseSensitive, escapeCharacter);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereBetween<T>(string column, T lower, T higher)
        {
            this.Query.WhereBetween<T>(column, lower, higher);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereBetween<T>(string column, T lower, T higher)
        {
            this.Query.OrWhereBetween<T>(column, lower, higher);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotBetween<T>(string column, T lower, T higher)
        {
            this.Query.WhereNotBetween<T>(column, lower, higher);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotBetween<T>(string column, T lower, T higher)
        {
            this.Query.OrWhereNotBetween<T>(column, lower, higher);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.WhereIn<T>(column, values);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.OrWhereIn<T>(column, values);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.WhereNotIn<T>(column, values);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.OrWhereNotIn<T>(column, values);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereIn(string column, Query query)
        {
            this.Query.WhereIn(column, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereIn(string column, Func<Query, Query> callback)
        {
            this.Query.WhereIn(column, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereIn(string column, Query query)
        {
            this.Query.OrWhereIn(column, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereIn(string column, Func<Query, Query> callback)
        {
            this.Query.OrWhereIn(column, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotIn(string column, Query query)
        {
            this.Query.WhereNotIn(column, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotIn(string column, Func<Query, Query> callback)
        {
            this.Query.WhereNotIn(column, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotIn(string column, Query query)
        {
            this.Query.OrWhereNotIn(column, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotIn(string column, Func<Query, Query> callback)
        {
            this.Query.OrWhereNotIn(column, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(string column, string op, Func<Query, Query> callback) // Func<Q, Q>
        {
            this.Query.Where(column, op, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> Where(string column, string op, Query query)
        {
            this.Query.Where(column, op, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereSub(Query query, object value)
        {
            this.Query.WhereSub(query, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereSub(Query query, string op, object value)
        {
            this.Query.WhereSub(query, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereSub(Query query, object value)
        {
            this.Query.OrWhereSub(query, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereSub(Query query, string op, object value)
        {
            this.Query.OrWhereSub(query, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhere(string column, string op, Query query)
        {
            this.Query.OrWhere(column, op, query);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhere(string column, string op, Func<Query, Query> callback)
        {
            this.Query.OrWhere(column, op, callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereExists(Query query)
        {
            this.Query.WhereExists(query);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereExists(Func<Query, Query> callback)
        {
            this.Query.WhereExists(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotExists(Query query)
        {
            this.Query.WhereNotExists(query);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotExists(Func<Query, Query> callback)
        {
            this.Query.WhereNotExists(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereExists(Query query)
        {
            this.Query.OrWhereExists(query);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereExists(Func<Query, Query> callback)
        {
            this.Query.OrWhereExists(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotExists(Query query)
        {
            this.Query.OrWhereNotExists(query);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotExists(Func<Query, Query> callback)
        {
            this.Query.OrWhereNotExists(callback);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereDatePart(string part, string column, string op, object value)
        {
            this.Query.WhereDatePart(part, column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotDatePart(string part, string column, string op, object value)
        {
            this.Query.WhereNotDatePart(part, column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereDatePart(string part, string column, string op, object value)
        {
            this.Query.OrWhereDatePart(part, column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotDatePart(string part, string column, string op, object value)
        {
            this.Query.OrWhereNotDatePart(part, column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereDate(string column, string op, object value)
        {
            this.Query.WhereDate(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotDate(string column, string op, object value)
        {
            this.Query.WhereNotDate(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereDate(string column, string op, object value)
        {
            this.Query.OrWhereDate(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotDate(string column, string op, object value)
        {
            this.Query.OrWhereNotDate(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereTime(string column, string op, object value)
        {
            this.Query.WhereTime(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotTime(string column, string op, object value)
        {
            this.Query.WhereNotTime(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereTime(string column, string op, object value)
        {
            this.Query.OrWhereTime(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotTime(string column, string op, object value)
        {
            this.Query.OrWhereNotTime(column, op, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereDatePart(string part, string column, object value)
        {
            this.Query.WhereDatePart(part, column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotDatePart(string part, string column, object value)
        {
            this.Query.WhereNotDatePart(part, column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereDatePart(string part, string column, object value)
        {
            this.Query.OrWhereDatePart(part, column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotDatePart(string part, string column, object value)
        {
            this.Query.OrWhereNotDatePart(part, column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereDate(string column, object value)
        {
            this.Query.WhereDate(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotDate(string column, object value)
        {
            this.Query.WhereNotDate(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereDate(string column, object value)
        {
            this.Query.OrWhereDate(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotDate(string column, object value)
        {
            this.Query.OrWhereNotDate(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereTime(string column, object value)
        {
            this.Query.WhereTime(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> WhereNotTime(string column, object value)
        {
            this.Query.WhereNotTime(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereTime(string column, object value)
        {
            this.Query.OrWhereTime(column, value);
            return this;
        }

        public NanoSqlBuilder<TEntity> OrWhereNotTime(string column, object value)
        {
            this.Query.OrWhereNotTime(column, value);
            return this;
        }
    }
}