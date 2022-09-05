using NanoCode.Database.Interfaces;
using System;

namespace NanoCode.Database.PostgreSql
{
    public class PostgreSqlNanoHelpers : INanoHelper
    {
        public string Now()
        {
            return " NOW() ";
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
            throw new NotImplementedException();
        }
    }

}