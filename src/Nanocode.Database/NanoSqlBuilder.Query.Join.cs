using Nanocode.Database.Interfaces;
using SqlKata;
using SqlKata.Compilers;
using System;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> Join(string table, string first, string second, string op = "=", string type = "inner join")
        {
            this.Query.Join(table, first, second, op, type);
            return this;
        }
        public NanoSqlBuilder<TEntity> Join(string table, Func<Join, Join> callback, string type = "inner join")
        {
            this.Query.Join(table, callback, type);
            return this;
        }
        public NanoSqlBuilder<TEntity> Join(Query query, Func<Join, Join> onCallback, string type = "inner join")
        {
            this.Query.Join(query, onCallback, type);
            return this;
        }
        public NanoSqlBuilder<TEntity> LeftJoin(string table, string first, string second, string op = "=")
        {
            this.Query.LeftJoin(table, first, second, op);
            return this;
        }
        public NanoSqlBuilder<TEntity> LeftJoin(string table, Func<Join, Join> callback)
        {
            this.Query.LeftJoin(table, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> LeftJoin(Query query, Func<Join, Join> onCallback)
        {
            this.Query.LeftJoin(query, onCallback);
            return this;
        }
        public NanoSqlBuilder<TEntity> RightJoin(string table, string first, string second, string op = "=")
        {
            this.Query.RightJoin(table, first, second, op);
            return this;
        }
        public NanoSqlBuilder<TEntity> RightJoin(string table, Func<Join, Join> callback)
        {
            this.Query.RightJoin(table, callback);
            return this;
        }
        public NanoSqlBuilder<TEntity> RightJoin(Query query, Func<Join, Join> onCallback)
        {
            this.Query.RightJoin(query, onCallback);
            return this;
        }
        public NanoSqlBuilder<TEntity> CrossJoin(string table)
        {
            this.Query.CrossJoin(table);
            return this;
        }
    }
}
