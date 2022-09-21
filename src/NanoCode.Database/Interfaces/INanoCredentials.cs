using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace NanoCode.Database.Interfaces
{
    public interface INanoCredentials
    {
        public DatabaseEngine Engine { get; }
        /*
        public string Host { get; set; }
        public int Port { get; set; }
        public string Catalog { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        */
        public string ConnectionString { get; }
    }
}