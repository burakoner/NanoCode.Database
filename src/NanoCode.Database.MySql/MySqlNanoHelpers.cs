using NanoCode.Database.Interfaces;
using System;

namespace NanoCode.Database.MySql
{
    public class MySqlNanoHelpers : INanoHelper
    {
        public string QuoteLeft { get; } = "`";

        public string QuoteRight { get; } = "`";

        public string Quote(string data)
        {
            return $"{QuoteLeft}{data}{QuoteRight}";
        }

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
            return " SELECT LAST_INSERT_ID() ";
        }

    }

}