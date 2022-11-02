using Nanocode.Database.Interfaces;
using SqlKata;
using SqlKata.Compilers;
using System;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        /* Private Fields */
        private INanoDatabase _db;
        private Compiler _compiler;
        private Query _query;

        /* Public Properties */
        public Query Query
        {
            get { return this._query; }
            set { this._query = value; }
        }

        public NanoSqlBuilder(INanoDatabase db)
        {
            this.Construct(db);
        }

        private void Construct(INanoDatabase db)
        {
            this._db = db;
            switch (_db.Engine)
            {
                case DatabaseEngine.MySql:
                    this._compiler = new MySqlCompiler();
                    break;
                case DatabaseEngine.Oracle:
                    this._compiler = new OracleCompiler();
                    break;
                case DatabaseEngine.PostgreSql:
                    this._compiler = new PostgresCompiler();
                    break;
                case DatabaseEngine.Sqlite:
                    this._compiler = new SqliteCompiler();
                    break;
                case DatabaseEngine.SqlServer:
                    this._compiler = new SqlServerCompiler();
                    break;
            }

            if (this._compiler == null)
                throw new Exception("Database Engine is missing or unsupported");

            var tableName = NanoObject<TEntity>.Properties.TableName;
            this.Query = new Query(tableName);
        }

        public NanoSqlBuilder<TEntity> Reset()
        {
            this.Construct(_db);
            return this;
        }

        public SqlResult Compile()
        {
            return this._compiler.Compile(this.Query);
        }

        public override string ToString()
        {
            return this._compiler.Compile(this.Query).ToString();
        }

    }
}
