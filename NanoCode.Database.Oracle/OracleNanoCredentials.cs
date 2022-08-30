using NanoCode.Database.Interfaces;
using Newtonsoft.Json;

namespace NanoCode.Database.Oracle
{
    public class OracleNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static OracleNanoCredentials DefaultCredentials { get; private set; }

        [JsonProperty("engine")]
        public DatabaseEngine Engine { get { return DatabaseEngine.Oracle; } }

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

        [JsonProperty("service")]
        public string ServiceName { get; set; }

        [JsonProperty("source")]
        public string DataSource { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                // Action
                return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={this.Host})(PORT={this.Port}))(CONNECT_DATA=(SERVICE_NAME={this.ServiceName})));" +
                    $"User Id={this.Username}; " +
                    $"Password={this.Password};";
            }
        }

        public OracleNanoCredentials()
        {
        }

        public OracleNanoCredentials(string host, int port, string catalog, string username, string password)
        {
            Host = host;
            Port = port;
            Database = catalog;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(OracleNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }
    }
}
