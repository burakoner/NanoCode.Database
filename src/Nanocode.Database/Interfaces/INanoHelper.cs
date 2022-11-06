using System;

namespace Nanocode.Database.Interfaces
{
    public interface INanoHelper
    {
        string QuoteLeft { get; }
        string QuoteRight { get; }
        string Quote(string data);
        string ScopeIdentity(Type type, string columnName, bool isNumeric);
    }
}