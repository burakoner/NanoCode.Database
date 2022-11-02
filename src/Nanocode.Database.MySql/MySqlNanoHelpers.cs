using Nanocode.Database.Interfaces;
using System;

namespace Nanocode.Database.MySql
{
    public class MySqlNanoHelpers : INanoHelper
    {
        public string QuoteLeft { get; } = "`";

        public string QuoteRight { get; } = "`";

        public string Quote(string data)
        {
            return $"{QuoteLeft}{data}{QuoteRight}";
        }

        public string ScopeIdentity(Type type)
        {
            return " SELECT LAST_INSERT_ID() ";
        }

    }

}