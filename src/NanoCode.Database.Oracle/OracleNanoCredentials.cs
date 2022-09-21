using NanoCode.Database.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;

namespace NanoCode.Database.Oracle
{
    public class OracleNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static OracleNanoCredentials DefaultCredentials { get; private set; }

        public DatabaseEngine Engine { get { return DatabaseEngine.Oracle; } }

        public string Host { get; set; }

        public int Port { get; set; }

        // public string Catalog { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ServiceName { get; set; }

        public string DataSource { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                // Action
                return
                    (string.IsNullOrEmpty(DataSource) ? $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={this.Host})(PORT={this.Port}))(CONNECT_DATA=(SERVICE_NAME={this.ServiceName}))); " : $"Data Source={this.DataSource}; ") +
                    $"User Id={this.Username}; " +
                    $"Password={this.Password}; ";
            }
        }

        public OracleNanoCredentials()
        {
        }

        public OracleNanoCredentials(string connectionString)
        {
            var conn = ParseConnectionString(connectionString);

            this.DataSource = conn.DataSource;
            this.Username = conn.Username;
            this.Password = conn.Password;
        }

        public OracleNanoCredentials(string host, int port, string service, string username, string password)
        {
            Host = host;
            Port = port;
            ServiceName = service;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(OracleNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }

        public static OracleNanoCredentials ParseConnectionString(string connectionString)
        {
            var dict = connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            // Parse
            var datasource = "";
            var username = "";
            var password = "";
            foreach (var kvp in dict)
            {
                if (kvp.Key.Equals("datasource", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("data source", StringComparison.InvariantCultureIgnoreCase))
                {
                    datasource = kvp.Value;
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
            return new OracleNanoCredentials
            {
                DataSource = datasource,
                Username = username,
                Password = password,
            };
        }
    }
}