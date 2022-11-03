using System;
using System.Threading.Tasks;

namespace Nanocode.Database.Interfaces
{
    public interface INanoObject
    {
        object this[string propertyName] { get; set; }

        void SetIdentity(object value);

        string JsonExport();
        void JsonImport(string jsonObject);

        void SaveChanges(INanoDatabase db);
        Task SaveChangesAsync(INanoDatabase db);

        void Delete(INanoDatabase db);
        Task DeleteAsync(INanoDatabase db);
    }
}