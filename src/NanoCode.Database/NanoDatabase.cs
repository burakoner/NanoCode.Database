using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NanoCode.Database.Interfaces;

namespace NanoCode.Database
{
    public abstract class NanoDatabase : INanoDatabase, IDisposable
    {
        public INanoCredentials Credentials { get; protected set; }
        public DatabaseEngine Engine => Credentials.Engine;
        public string ConnectionString => Credentials.ConnectionString;

        public abstract DbConnection Connection { get; protected set; }
        public abstract DbCommand Command { get; protected set; }
        public abstract DbTransaction Transaction { get; protected set; }
        public abstract INanoHelper Helper { get; protected set; }

        public ConnectionState ConnectionState => Connection.State;
        public int ConnectionTimeout => Connection.ConnectionTimeout;
        public int CommandTimeout
        {
            get { return Command.CommandTimeout; }
            set { Command.CommandTimeout = value; }
        }
        public string Database
        {
            get { return Connection.Database; }
            set { Connection.ChangeDatabase(value); }
        }
        public string ServerVersion => Connection.ServerVersion;
        public Stopwatch StopWatch { get; protected set; }
        public TimeSpan ExecutionTime { get; protected set; }

        public bool OpenConnection()
        {
            try
            {
                if (ConnectionState == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                else if (ConnectionState == ConnectionState.Broken)
                {
                    Connection.Close();
                    Connection.Open();
                }
            }
            catch { }

            // Return Dummy
            return false;
        }
        public bool CloseConnection()
        {
            try
            {
                if (ConnectionState == ConnectionState.Connecting || ConnectionState == ConnectionState.Executing || ConnectionState == ConnectionState.Fetching)
                    return false;

                else if (ConnectionState == ConnectionState.Broken || ConnectionState == ConnectionState.Open)
                {
                    Connection.Close();
                    Connection.Dispose();
                    return true;
                }

                else if (ConnectionState == ConnectionState.Closed)
                    return true;
            }
            catch { }

            // Return Dummy
            return false;
        }
        public IDbConnection GetConnection(bool openConnection = true)
        {
            if (openConnection)
                OpenConnection();

            // Return
            return Connection;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            // Close Connection
            CloseConnection();

            // Dispose Variables
            this.Credentials = null;
            this.StopWatch = null;

            // Dispose Db Variables
            if (this.Connection != null) this.Connection.Dispose();
            this.Connection = null;
            this.Command = null;
            this.Transaction = null;

            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~NanoDatabase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}