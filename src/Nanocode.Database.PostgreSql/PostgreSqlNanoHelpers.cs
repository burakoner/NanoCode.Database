using Nanocode.Database.Interfaces;
using System;

namespace Nanocode.Database.PostgreSql
{
    public class PostgreSqlNanoHelpers : INanoHelper
    {
        public string QuoteLeft { get; } = "\"";

        public string QuoteRight { get; } = "\"";

        public string Quote(string data)
        {
            return $"{QuoteLeft}{data}{QuoteRight}";
        }

        public string ScopeIdentity(Type type)
        {
            return string.Empty;
        }
    }

}