using Nanocode.Database;
using Nanocode.Database.Interfaces;
using Nanocode.Database.SqlServer;

namespace Nanocode.Database.Examples
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