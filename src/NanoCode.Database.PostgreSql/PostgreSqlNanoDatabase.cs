using NanoCode.Database.Interfaces;
using System.Data.Common;
using Npgsql;

namespace NanoCode.Database.PostgreSql
{
    public class PostgreSqlNanoDatabase : NanoDatabase
    {
        public override DbConnection Connection { get; protected set; }
        public override DbCommand Command { get; protected set; }
        public override DbTransaction Transaction { get; protected set; }
        public override INanoHelper Helper { get; protected set; }

        #region Constructors
        public PostgreSqlNanoDatabase() : this(PostgreSqlNanoCredentials.DefaultCredentials)
        {
        }

        public PostgreSqlNanoDatabase(PostgreSqlNanoCredentials credentials)
        {
            // Connection Parameters
            this.Credentials = credentials;

            // SqlServer Variables
            this.Connection = new NpgsqlConnection(ConnectionString);
            this.Command = new NpgsqlCommand();

            // Helpers
            Helper = new PostgreSqlNanoHelpers();
        }
        #endregion

    }
}