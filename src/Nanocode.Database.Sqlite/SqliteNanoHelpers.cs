using Nanocode.Database.Interfaces;
using System;

namespace Nanocode.Database.Sqlite
{
    public class SqliteNanoHelpers : INanoHelper
    {
        public string QuoteLeft { get; } = "\"";

        public string QuoteRight { get; } = "\"";

        public string Quote(string data)
        {
            return $"{QuoteLeft}{data}{QuoteRight}";
        }

        public string ScopeIdentity(Type type, string columnName, bool isNumeric)
        {
            if (type == typeof(int)) return " SELECT CAST(SCOPE_IDENTITY() AS INT) ";
            if (type == typeof(long)) return " SELECT CAST(SCOPE_IDENTITY() AS BIGINT) ";
            if (type == typeof(string)) return " SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(255)) ";

            return string.Empty;
        }
    }

}