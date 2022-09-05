using MySql.Data.MySqlClient;
using NanoCode.Database.Interfaces;
using System.Data.Common;

namespace NanoCode.Database.MySql
{
    public class MySqlNanoDatabase : NanoDatabase
    {
        public override DbConnection Connection { get; protected set; }
        public override DbCommand Command { get; protected set; }
        public override DbTransaction Transaction { get; protected set; }
        public override INanoHelper Helper { get; protected set; }

        #region Constructors
        public MySqlNanoDatabase() : this(MySqlNanoCredentials.DefaultCredentials)
        {
        }

        public MySqlNanoDatabase(MySqlNanoCredentials credentials)
        {
            // Connection Parameters
            this.Credentials = credentials;

            // SqlServer Variables
            this.Connection = new MySqlConnection(ConnectionString);
            this.Command = new MySqlCommand();

            // Helpers
            Helper = new MySqlNanoHelpers();
        }
        #endregion
    }
}