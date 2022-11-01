using System;

namespace Nanocode.Database.Interfaces
{
    public interface INanoHelper
    {
        string Now();
        string QuoteLeft { get; }
        string QuoteRight { get; }
        string Quote(string data);
        string Lower(string data, bool addQuotes = false);
        string Upper(string data, bool addQuotes = false);
        string ScopeIdentity(Type type);
    }
}