using NanoCode.Database.Interfaces;
using Newtonsoft.Json;

namespace NanoCode.Database.SqlServer
{
    public class SqlServerNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static SqlServerNanoCredentials DefaultCredentials { get; private set; }

        [JsonProperty("engine")]
        public DatabaseEngine Engine { get { return DatabaseEngine.SqlServer; } }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("pass")]
        public string Password { get; set; }

        [JsonProperty("mars")]
        public bool MultipleActiveResultSets { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                // Arrange
                var _conn = "";

                // Action
                var _port = this.Port > 0 ? "," + this.Port.ToString() : "";
                _conn = 
                    $"Server={this.Host}{_port}; " +
                    $"Database={this.Database}; " +
                    $"User Id={this.Username}; " +
                    $"Password={this.Password}; " +
                    $"MultipleActiveResultSets={this.MultipleActiveResultSets.ToString().ToLowerInvariant()}";

                // Return
                return _conn;
            }
        }

        public SqlServerNanoCredentials()
        {
        }

        public SqlServerNanoCredentials(string host, int port, string catalog, string username, string password, bool mars = true)
        {
            Host = host;
            Port = port;
            Database = catalog;
            Username = username;
            Password = password;
            MultipleActiveResultSets = mars;
        }

        public static void SetDefaultCredentials(SqlServerNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }
    }
}
