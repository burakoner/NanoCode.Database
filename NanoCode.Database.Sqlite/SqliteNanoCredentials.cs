using NanoCode.Database.Interfaces;
using Newtonsoft.Json;

namespace NanoCode.Database.Sqlite
{
    public class SqliteNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static SqliteNanoCredentials DefaultCredentials { get; private set; }

        [JsonProperty("engine")]
        public DatabaseEngine Engine { get { return DatabaseEngine.Sqlite; } }

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

        [JsonProperty("source")]
        public string DataSource { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; } = 3;

        [JsonProperty("utf8")]
        public bool UseUTF16Encoding { get; set; } = true;

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                return
                    $"Data Source={this.DataSource}; " +
                    $"Version={this.Version}; " +
                    $"UseUTF16Encoding={this.UseUTF16Encoding.ToString().ToLowerInvariant()}; " +
                    $"Password={this.Password}; ";
            }
        }

        public SqliteNanoCredentials(string datasource, int version, string password)
        {
            DataSource = datasource;
            Version = version;
            Password = password;
        }

        public static void SetDefaultCredentials(SqliteNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }
    }
}
