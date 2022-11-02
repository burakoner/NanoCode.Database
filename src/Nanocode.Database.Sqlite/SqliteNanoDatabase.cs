using Nanocode.Database.Interfaces;
using System.Data.Common;
using System.Data.SQLite;

namespace Nanocode.Database.Sqlite
{
    public class SqliteNanoDatabase : NanoDatabase
    {
        public override DbConnection Connection { get; protected set; }
        public override DbCommand Command { get; protected set; }
        public override DbTransaction Transaction { get; protected set; }
        public override INanoHelper Helper { get; protected set; }

        #region Constructors
        public SqliteNanoDatabase() : this(SqliteNanoCredentials.DefaultCredentials)
        {
        }

        public SqliteNanoDatabase(SqliteNanoCredentials credentials)
        {
            // Connection Parameters
            this.Credentials = credentials;

            // SqlServer Variables
            this.Connection = new SQLiteConnection(ConnectionString);
            this.Command = new SQLiteCommand();

            // Helpers
            Helper = new SqliteNanoHelpers();
        }
        #endregion
    }
}