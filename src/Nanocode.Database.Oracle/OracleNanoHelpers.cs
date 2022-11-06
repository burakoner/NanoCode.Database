using Nanocode.Database.Interfaces;
using System;

namespace Nanocode.Database.Oracle
{
    public class OracleNanoHelpers : INanoHelper
    {
        public string QuoteLeft { get; } = "\"";

        public string QuoteRight { get; } = "\"";

        public string Quote(string data)
        {
            return $"{QuoteLeft}{data}{QuoteRight}";
        }

        public string ScopeIdentity(Type type, string columnName, bool isNumeric)
        {
            return string.Empty;
        }
    }

}