using NanoCode.Database.Interfaces;
using Newtonsoft.Json;
using System.Drawing;

namespace NanoCode.Database.PostgreSql
{
    public class PostgreSqlNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static PostgreSqlNanoCredentials DefaultCredentials { get; private set; }

        [JsonProperty("engine")]
        public DatabaseEngine Engine { get { return DatabaseEngine.PostgreSql; } }

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

        [JsonProperty("pooling")]
        public bool Pooling { get; set; } = true;

        [JsonProperty("minpoolsize")]
        public int MinimumPoolSize { get; set; } = 0;

        [JsonProperty("maxpoolsize")]
        public int MaximumPoolSize { get; set; } = 100;

        [JsonProperty("lifetime")]
        public int ConnectionLifetime { get; set; } = 0;

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                // Action
                return
                    $"Host={this.Host}; " +
                    $"Port={this.Port}; " +
                    $"Database={this.Database}; " +
                    $"User Id={this.Username}; " +
                    $"Password={this.Password}; " +
                    $"Pooling={this.Pooling.ToString().ToLowerInvariant()}; " +
                    $"Min Pool Size={this.MinimumPoolSize}; " +
                    $"Max Pool Size={this.MaximumPoolSize}; " +
                    $"Connection Lifetime={this.ConnectionLifetime}; ";
            }
        }

        public PostgreSqlNanoCredentials()
        {
        }

        public PostgreSqlNanoCredentials(string host, int port, string catalog, string username, string password)
        {
            Host = host;
            Port = port;
            Database = catalog;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(PostgreSqlNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }
    }
}
