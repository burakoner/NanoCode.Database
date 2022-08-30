using NanoCode.Database.Interfaces;
using Newtonsoft.Json;

namespace NanoCode.Database.MySql
{
    public class MySqlNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static MySqlNanoCredentials DefaultCredentials { get; private set; }

        [JsonProperty("engine")]
        public DatabaseEngine Engine { get { return DatabaseEngine.MySql; } }

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

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                return
                    $"Server={this.Host}; " +
                    $"Port={this.Port}; " +
                    $"Database={this.Database}; " +
                    $"Uid={this.Username}; " +
                    $"Pwd={this.Password}; ";
            }
        }

        public MySqlNanoCredentials()
        {
        }

        public MySqlNanoCredentials(string host, int port, string catalog, string username, string password)
        {
            Host = host;
            Port = port;
            Database = catalog;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(MySqlNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }
    }
}
