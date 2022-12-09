using System;
using System.Threading.Tasks;

namespace Nanocode.Database.Interfaces
{
    public interface INanoObject
    {
        object this[string propertyName] { get; set; }

        string JsonExport();
        void JsonImport(string jsonObject);

        void SaveChanges(INanoDatabase db);
        Task SaveChangesAsync(INanoDatabase db);

        void Delete(INanoDatabase db);
        Task DeleteAsync(INanoDatabase db);

        void SetDatabaseStatus(bool exists);
    }
}