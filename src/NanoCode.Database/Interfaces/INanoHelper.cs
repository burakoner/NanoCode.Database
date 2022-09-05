using System;

namespace NanoCode.Database.Interfaces
{
    public interface INanoHelper
    {
        string Now();
        string Lower(string data, bool addQuotes = false);
        string Upper(string data, bool addQuotes = false);
        string ScopeIdentity(Type type);
    }
}