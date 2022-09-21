using NanoCode.Database.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace NanoCode.Database.Sqlite
{
    public class SqliteNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static SqliteNanoCredentials DefaultCredentials { get; private set; }

        public DatabaseEngine Engine { get { return DatabaseEngine.Sqlite; } }

        public string Password { get; set; }

        public string DataSource { get; set; }

        public int Version { get; set; } = 3;

        public bool UseUTF8Encoding { get; set; }
        public bool UseUTF16Encoding { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                return
                    $"Data Source={this.DataSource}; " +
                    $"Version={this.Version}; " +
                    (UseUTF8Encoding ? $"UseUTF8Encoding={this.UseUTF8Encoding.ToString().ToLowerInvariant()}; " : "") +
                    (UseUTF16Encoding ? $"UseUTF16Encoding={this.UseUTF16Encoding.ToString().ToLowerInvariant()}; " : "") +
                    (string.IsNullOrEmpty(Password) ? "" : $"Password={this.Password}; ");
            }
        }

        public SqliteNanoCredentials()
        {
        }

        public SqliteNanoCredentials(string connectionString)
        {
            var conn = ParseConnectionString(connectionString);

            DataSource = conn.DataSource;
            Version = conn.Version;
            Password = conn.Password;
            UseUTF8Encoding = conn.UseUTF8Encoding;
            UseUTF16Encoding = conn.UseUTF16Encoding;
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


        public static SqliteNanoCredentials ParseConnectionString(string connectionString)
        {
            var dict = connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            // Parse
            var datasource = "";
            var version = 3;
            var password = "";
            var utf8 = false;
            var utf16 = false;
            foreach (var kvp in dict)
            {
                if (kvp.Key.Equals("datasource", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("data source", StringComparison.InvariantCultureIgnoreCase))
                {
                    datasource = kvp.Value;
                }

                else if (kvp.Key.Equals("version", StringComparison.InvariantCultureIgnoreCase))
                {
                    version = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("pwd", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("password", StringComparison.InvariantCultureIgnoreCase))
                {
                    password = kvp.Value;
                }

                else if (kvp.Key.Equals("utf8encoding", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("utf8-encoding", StringComparison.InvariantCultureIgnoreCase))
                {
                    utf8 = Convert.ToBoolean(kvp.Value);
                }

                else if (kvp.Key.Equals("utf16encoding", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("utf16-encoding", StringComparison.InvariantCultureIgnoreCase))
                {
                    utf16 = Convert.ToBoolean(kvp.Value);
                }
            }

            // Return
            return new SqliteNanoCredentials
            {
                DataSource = datasource,
                Version = version,
                Password = password,
                UseUTF8Encoding = utf8,
                UseUTF16Encoding = utf16,
            };
        }
    }
}
