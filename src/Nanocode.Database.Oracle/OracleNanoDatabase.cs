using Nanocode.Database.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Nanocode.Database.Oracle
{
    public class OracleNanoDatabase : NanoDatabase
    {
        public override DbConnection Connection { get; protected set; }
        public override DbCommand Command { get; protected set; }
        public override DbTransaction Transaction { get; protected set; }
        public override INanoHelper Helper { get; protected set; }

        #region Constructors
        public OracleNanoDatabase() : this(OracleNanoCredentials.DefaultCredentials)
        {
        }

        public OracleNanoDatabase(OracleNanoCredentials credentials)
        {
            // Connection Parameters
            this.Credentials = credentials;

            // SqlServer Variables
            this.Connection = new OracleConnection(ConnectionString);
            this.Command = new OracleCommand();

            // Helpers
            Helper = new OracleNanoHelpers();
        }
        #endregion
    }
}