using NanoCode.Database.Interfaces;
using System;

namespace NanoCode.Database.Sqlite
{
    public class SqliteNanoHelpers : INanoHelper
    {
        public string Now()
        {
            return " GETDATE() ";
        }

        public string Lower(string data, bool addQuotes = false)
        {
            return " LOWER(" + (addQuotes ? "'" : "") + data + (addQuotes ? "'" : "") + ") ";
        }

        public string Upper(string data, bool addQuotes = false)
        {
            return " UPPER(" + (addQuotes ? "'" : "") + data + (addQuotes ? "'" : "") + ") ";
        }

        public string ScopeIdentity(Type type)
        {
            if (type == typeof(int)) return " SELECT CAST(SCOPE_IDENTITY() AS INT) ";
            if (type == typeof(long)) return " SELECT CAST(SCOPE_IDENTITY() AS BIGINT) ";
            if (type == typeof(string)) return " SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(255)) ";

            return string.Empty;
        }
    }

}