using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace NanoCode.Database.Interfaces
{
    public interface INanoDatabase
    {
        INanoCredentials Credentials { get; }
        DatabaseEngine Engine { get; }
        string ConnectionString { get; }

        DbConnection Connection { get; }
        DbCommand Command { get; }
        DbTransaction Transaction { get; }
        INanoHelper Helper { get; }

        ConnectionState ConnectionState { get; }
        int ConnectionTimeout { get; }
        int CommandTimeout { get; set; }
        string Database { get; set; }
        string ServerVersion { get; }
        Stopwatch StopWatch { get; }
        TimeSpan ExecutionTime { get; }

        bool OpenConnection();
        bool CloseConnection();
        void Dispose();
        IDbConnection GetConnection(bool openConnection = true);
    }
}