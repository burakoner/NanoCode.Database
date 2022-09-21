using NanoCode.Database.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace NanoCode.Database.MySql
{
    public class MySqlNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static MySqlNanoCredentials DefaultCredentials { get; private set; }

        public DatabaseEngine Engine { get { return DatabaseEngine.MySql; } }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Catalog { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                return
                    $"Server={this.Host}; " +
                    $"Port={this.Port}; " +
                    $"Database={this.Catalog}; " +
                    $"Uid={this.Username}; " +
                    $"Pwd={this.Password}; ";
            }
        }

        public MySqlNanoCredentials()
        {
        }

        public MySqlNanoCredentials(string connectionString)
        {
            var conn = ParseConnectionString(connectionString);

            this.Host = conn.Host;
            this.Port = conn.Port;
            this.Catalog = conn.Catalog;
            this.Username = conn.Username;
            this.Password = conn.Password;
        }

        public MySqlNanoCredentials(string host, int port, string catalog, string username, string password)
        {
            Host = host;
            Port = port;
            Catalog = catalog;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(MySqlNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }

        public static MySqlNanoCredentials ParseConnectionString(string connectionString)
        {
            var dict = connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            // Parse
            var host = "";
            var port = 3306;
            var catalog = "";
            var username = "";
            var password = "";
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
                    catalog = kvp.Value;
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
            }

            // Return
            return new MySqlNanoCredentials
            {
                Host = host,
                Port = port,
                Catalog = catalog,
                Username = username,
                Password = password,
            };
        }
    }
}
