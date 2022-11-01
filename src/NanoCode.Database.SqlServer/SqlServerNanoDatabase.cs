using Nanocode.Database.Interfaces;
using System.Data.Common;
using System.Data.SqlClient;

namespace Nanocode.Database.SqlServer
{
    public class SqlServerNanoDatabase : NanoDatabase
    {
        public override DbConnection Connection { get; protected set; }
        public override DbCommand Command { get; protected set; }
        public override DbTransaction Transaction { get; protected set; }
        public override INanoHelper Helper { get; protected set; }

        #region Constructors
        public SqlServerNanoDatabase() : this(SqlServerNanoCredentials.DefaultCredentials)
        {
        }

        public SqlServerNanoDatabase(SqlServerNanoCredentials credentials)
        {
            // Connection Parameters
            this.Credentials = credentials;

            // SqlServer Variables
            this.Connection = new SqlConnection(ConnectionString);
            this.Command = new SqlCommand();

            // Helpers
            Helper = new SqlServerNanoHelpers();
        }
        #endregion
    }
}