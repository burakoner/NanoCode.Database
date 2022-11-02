﻿using Nanocode.Database.Interfaces;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;

namespace Nanocode.Database.PostgreSql
{
    public class PostgreSqlNanoCredentials : INanoCredentials
    {
        [JsonIgnore]
        public static PostgreSqlNanoCredentials DefaultCredentials { get; private set; }

        public DatabaseEngine Engine { get { return DatabaseEngine.PostgreSql; } }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Database { get; set; }

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
                    $"Database={this.Database}; " +
                    $"User Id={this.Username}; " +
                    $"Password={this.Password}; ";
            }
        }

        public PostgreSqlNanoCredentials()
        {
        }

        public PostgreSqlNanoCredentials(string connectionString)
        {
            var conn = ParseConnectionString(connectionString);

            this.Host = conn.Host;
            this.Port = conn.Port;
            this.Database = conn.Database;
            this.Username = conn.Username;
            this.Password = conn.Password;
        }

        public PostgreSqlNanoCredentials(string host, int port, string database, string username, string password)
        {
            Host = host;
            Port = port;
            Database = database;
            Username = username;
            Password = password;
        }

        public static void SetDefaultCredentials(PostgreSqlNanoCredentials credentials)
        {
            DefaultCredentials = credentials;
        }

        public static PostgreSqlNanoCredentials ParseConnectionString(string connectionString)
        {
            var dict = connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            // Parse
            var host = "";
            var port = 5432;
            var database = "";
            var username = "";
            var password = "";
            //var pooling = false;
            //var protocol = 0;
            //var ssl = false;
            //var contimeout = 0;
            //var cmdtimeout = 0;
            //var minimumPoolSize = 0;
            //var maximumPoolSize = 0;
            //var connectionLifetime = 0;
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
                    host = kvp.Value;
                }

                else if (kvp.Key.Equals("port", StringComparison.InvariantCultureIgnoreCase))
                {
                    port = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("database", StringComparison.InvariantCultureIgnoreCase) ||
                     kvp.Key.Equals("initial catalog", StringComparison.InvariantCultureIgnoreCase))
                {
                    database = kvp.Value;
                }

                else if (kvp.Key.Equals("uid", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("user", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("user id", StringComparison.InvariantCultureIgnoreCase) ||
                    kvp.Key.Equals("userid", StringComparison.InvariantCultureIgnoreCase) ||
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

                /*
                else if (kvp.Key.Equals("pooling", StringComparison.InvariantCultureIgnoreCase))
                {
                    pooling = Convert.ToBoolean(kvp.Value);
                }

                else if (kvp.Key.Equals("protocol", StringComparison.InvariantCultureIgnoreCase))
                {
                    protocol = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("ssl", StringComparison.InvariantCultureIgnoreCase))
                {
                    ssl = Convert.ToBoolean(kvp.Value);
                }

                else if (kvp.Key.Equals("timeout", StringComparison.InvariantCultureIgnoreCase))
                {
                    contimeout = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("commandtimeout", StringComparison.InvariantCultureIgnoreCase))
                {
                    cmdtimeout = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("MinimumPoolSize", StringComparison.InvariantCultureIgnoreCase))
                {
                    minimumPoolSize = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("MaximumPoolSize", StringComparison.InvariantCultureIgnoreCase))
                {
                    maximumPoolSize = Convert.ToInt32(kvp.Value);
                }

                else if (kvp.Key.Equals("ConnectionLifetime", StringComparison.InvariantCultureIgnoreCase))
                {
                    connectionLifetime = Convert.ToInt32(kvp.Value);
                }
                */
            }

            // Return
            return new PostgreSqlNanoCredentials
            {
                Host = host,
                Port = port,
                Database = database,
                Username = username,
                Password = password,
            };
        }
    }
}
