using Nanocode.Database.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Nanocode.Database.SqlServer
{
    public class SqlServerNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static SqlServerNanoCredentials DefaultCredentials { get; private set; }

        public DatabaseEngine Engine { get { return DatabaseEngine.SqlServer; } }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Database { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool TrustedConnection { get; set; }

        public bool MultipleActiveResultSets { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                var _port = this.Port > 0 ? "," + this.Port.ToString() : "";
                return this.TrustedConnection
                    ?
                        $"Server={this.Host}{_port}; " +
                        $"Database={this.Database}; " +
                        $"Trusted_Connection=True; " +
                        $"MultipleActiveResultSets={this.MultipleActiveResultSets.ToString().ToLowerInvariant()}"
                    :
                        $"Server={this.Host}{_port}; " +
                        $"Database={this.Database}; " +
                        $"User Id={this.Username}; " +
                        $"Password={this.Password}; " +
                        $"MultipleActiveResultSets={this.MultipleActiveResultSets.ToString().ToLowerInvariant()}";
            }
        }

        public SqlServerNanoCredentials()
        {
        }

        public SqlServerNanoCredentials(string connectionString)
        {
            var conn = ParseConnectionString(connectionString);

            Host = conn.Host;
            Port = conn.Port;
            Database = conn.Database;
            Username = conn.Username;
            Password = conn.Password;
            TrustedConnection = conn.TrustedConnection;
            MultipleActiveResultSets = conn.MultipleActiveResultSets;
        }

        public SqlServerNanoCredentials(string host, int port, string database, string username, string password, bool trustedConnection = false, bool multipleActiveResultSets = true)
        {
            Host = host;
            Port = port;
            Database = database;
            Username = username;
            Password = password;
            TrustedConnection = trustedConnection;
            MultipleActiveResultSets = multipleActiveResultSets;
        }

        public static void SetDefaultCredentials(SqlServerNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }

        public static SqlServerNanoCredentials ParseConnectionString(string connectionString)
        {
            var dict = connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            // Parse
            var host = "";
            var port = 1433;
            var database = "";
            var username = "";
            var password = "";
            var mars = false;
            var trusted = false;
            foreach (var kvp in dict)
            {
                if (kvp.Key.Equals("server", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("host", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("datasource", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("data source", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("addr", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("address", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("network address", StringComparison.InvariantCultureIgnoreCase))
                {
                    var server = kvp.Value.Split(',');
                    host = server[0].Trim();
                    if (server.Length >= 2) port = int.Parse(server[1].Trim());
                }

                else if (kvp.Key.Equals("database", StringComparison.InvariantCultureIgnoreCase) ||
                     kvp.Key.Equals("initial catalog", StringComparison.InvariantCultureIgnoreCase))
                {
                    database = kvp.Value;
                }

                else if (kvp.Key.Equals("uid", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("user", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("userid", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("user id", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("username", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("user name", StringComparison.InvariantCultureIgnoreCase))
                {
                    username = kvp.Value;
                }

                else if (kvp.Key.Equals("pwd", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("password", StringComparison.InvariantCultureIgnoreCase))
                {
                    password = kvp.Value;
                }

                else if (kvp.Key.Equals("multipleactiveresultsets", StringComparison.InvariantCultureIgnoreCase))
                {
                    mars = Convert.ToBoolean(kvp.Value);
                }

                else if (kvp.Key.Equals("trustedconnection", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("trusted_connection", StringComparison.InvariantCultureIgnoreCase))
                {
                    trusted = Convert.ToBoolean(kvp.Value);
                }
            }

            // Return
            return new SqlServerNanoCredentials
            {
                Host = host,
                Port = port,
                Database = database,
                Username = username,
                Password = password,
                TrustedConnection = trusted,
                MultipleActiveResultSets = mars,
            };
        }
    }
}
