using NanoCode.Database;
using NanoCode.Database.Interfaces;
using NanoCode.Database.SqlServer;

namespace NanoCode.Database.Examples
{
    public class AppConnections
    {
        public INanoDatabase DbConn { get; private set; }

        public AppConnections(SqlServerNanoCredentials dbConnCredentials)
        {
            this.DbConn = new SqlServerNanoDatabase(dbConnCredentials);
        }

        public AppConnections(string connectionString)
        {
            this.DbConn = new SqlServerNanoDatabase(new SqlServerNanoCredentials(connectionString));
        }
    }
}