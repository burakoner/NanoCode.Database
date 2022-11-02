using SqlKata;
using System;
using System.Collections.Generic;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> Having(string column, string op, object value)
        {
            this.Query.Having(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNot(string column, string op, object value)
        {
            this.Query.HavingNot(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHaving(string column, string op, object value)
        {
            this.Query.OrHaving(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNot(string column, string op, object value)
        {
            this.Query.OrHavingNot(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(string column, object value)
        {
            this.Query.Having(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNot(string column, object value)
        {
            this.Query.HavingNot(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHaving(string column, object value)
        {
            this.Query.OrHaving(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNot(string column, object value)
        {
            this.Query.OrHavingNot(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(object constraints)
        {
            this.Query.Having(constraints);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(IEnumerable<KeyValuePair<string, object>> values)
        {
            this.Query.Having(values);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingRaw(string sql, params object[] bindings)
        {
            this.Query.HavingRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingRaw(string sql, params object[] bindings)
        {
            this.Query.OrHavingRaw(sql, bindings);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(Func<Query, Query> callback)
        {
            this.Query.Having(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNot(Func<Query, Query> callback)
        {
            this.Query.HavingNot(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHaving(Func<Query, Query> callback)
        {
            this.Query.OrHaving(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNot(Func<Query, Query> callback)
        {
            this.Query.OrHavingNot(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingColumns(string first, string op, string second)
        {
            this.Query.HavingColumns(first, op, second);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingColumns(string first, string op, string second)
        {
            this.Query.OrHavingColumns(first, op, second);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNull(string column)
        {
            this.Query.HavingNull(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotNull(string column)
        {
            this.Query.HavingNotNull(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNull(string column)
        {
            this.Query.OrHavingNull(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotNull(string column)
        {
            this.Query.OrHavingNotNull(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingTrue(string column)
        {
            this.Query.HavingTrue(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingTrue(string column)
        {
            this.Query.OrHavingTrue(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingFalse(string column)
        {
            this.Query.HavingFalse(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingFalse(string column)
        {
            this.Query.OrHavingFalse(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingNotLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingNotLike(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingNotStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingNotStarts(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingNotEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingEnds(column, value, caseSensitive, escapeCharacter);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingNotEnds(column, value, caseSensitive, escapeCharacter = null);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingContains(column, value, caseSensitive, escapeCharacter = null);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.HavingNotContains(column, value, caseSensitive, escapeCharacter = null);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingContains(column, value, caseSensitive, escapeCharacter = null);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null)
        {
            this.Query.OrHavingNotContains(column, value, caseSensitive, escapeCharacter = null);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingBetween<T>(string column, T lower, T higher)
        {
            this.Query.HavingBetween<T>(column, lower, higher);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingBetween<T>(string column, T lower, T higher)
        {
            this.Query.OrHavingBetween<T>(column, lower, higher);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotBetween<T>(string column, T lower, T higher)
        {
            this.Query.HavingNotBetween<T>(column, lower, higher);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotBetween<T>(string column, T lower, T higher)
        {
            this.Query.OrHavingNotBetween<T>(column, lower, higher);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.HavingIn<T>(column, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.OrHavingIn<T>(column, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.HavingNotIn<T>(column, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotIn<T>(string column, IEnumerable<T> values)
        {
            this.Query.OrHavingNotIn<T>(column, values);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingIn(string column, Query query)
        {
            this.Query.HavingIn(column, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingIn(string column, Func<Query, Query> callback)
        {
            this.Query.HavingIn(column, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingIn(string column, Query query)
        {
            this.Query.OrHavingIn(column, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingIn(string column, Func<Query, Query> callback)
        {
            this.Query.OrHavingIn(column, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotIn(string column, Query query)
        {
            this.Query.HavingNotIn(column, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotIn(string column, Func<Query, Query> callback)
        {
            this.Query.HavingNotIn(column, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotIn(string column, Query query)
        {
            this.Query.OrHavingNotIn(column, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotIn(string column, Func<Query, Query> callback)
        {
            this.Query.OrHavingNotIn(column, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(string column, string op, Func<Query, Query> callback)
        {
            this.Query.Having(column, op, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> Having(string column, string op, Query query)
        {
            this.Query.Having(column, op, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHaving(string column, string op, Query query)
        {
            this.Query.OrHaving(column, op, query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHaving(string column, string op, Func<Query, Query> callback)
        {
            this.Query.OrHaving(column, op, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingExists(Query query)
        {
            this.Query.HavingExists(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingExists(Func<Query, Query> callback)
        {
            this.Query.HavingExists(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotExists(Query query)
        {
            this.Query.HavingNotExists(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotExists(Func<Query, Query> callback)
        {
            this.Query.HavingNotExists(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingExists(Query query)
        {
            this.Query.OrHavingExists(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingExists(Func<Query, Query> callback)
        {
            this.Query.OrHavingExists(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotExists(Query query)
        {
            this.Query.OrHavingNotExists(query);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotExists(Func<Query, Query> callback)
        {
            this.Query.OrHavingNotExists(callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingDatePart(string part, string column, string op, object value)
        {
            this.Query.HavingDatePart(part, column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotDatePart(string part, string column, string op, object value)
        {
            this.Query.HavingNotDatePart(part, column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingDatePart(string part, string column, string op, object value)
        {
            this.Query.OrHavingDatePart(part, column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotDatePart(string part, string column, string op, object value)
        {
            this.Query.OrHavingNotDatePart(part, column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingDate(string column, string op, object value)
        {
            this.Query.HavingDate(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotDate(string column, string op, object value)
        {
            this.Query.HavingNotDate(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingDate(string column, string op, object value)
        {
            this.Query.OrHavingDate(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotDate(string column, string op, object value)
        {
            this.Query.OrHavingNotDate(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingTime(string column, string op, object value)
        {
            this.Query.HavingTime(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotTime(string column, string op, object value)
        {
            this.Query.HavingNotTime(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingTime(string column, string op, object value)
        {
            this.Query.OrHavingTime(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotTime(string column, string op, object value)
        {
            this.Query.OrHavingNotTime(column, op, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingDatePart(string part, string column, object value)
        {
            this.Query.HavingDatePart(part, column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotDatePart(string part, string column, object value)
        {
            this.Query.HavingNotDatePart(part, column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingDatePart(string part, string column, object value)
        {
            this.Query.OrHavingDatePart(part, column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotDatePart(string part, string column, object value)
        {
            this.Query.OrHavingNotDatePart(part, column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingDate(string column, object value)
        {
            this.Query.HavingDate(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotDate(string column, object value)
        {
            this.Query.HavingNotDate(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingDate(string column, object value)
        {
            this.Query.OrHavingDate(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotDate(string column, object value)
        {
            this.Query.OrHavingNotDate(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingTime(string column, object value)
        {
            this.Query.HavingTime(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> HavingNotTime(string column, object value)
        {
            this.Query.HavingNotTime(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingTime(string column, object value)
        {
            this.Query.OrHavingTime(column, value);
            return this;
        }
        public NanoSqlBuilder<TEntity> OrHavingNotTime(string column, object value)
        {
            this.Query.OrHavingNotTime(column, value);
            return this;
        }
    }
}
