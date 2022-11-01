using Nanocode.Database.PostgreSql;
using Nanocode.Database.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nanocode.Database.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            NanoMapper.RegisterModels(Assembly.GetAssembly(typeof(Asset)));
            var conn = new AppConnections("Server=.; Database=NewCatalog; User Id=sa; Password=pa55w0Rd; MultipleActiveResultSets=true");

            var assets = Asset.GetRows(conn.DbConn);
            var asset = assets.FirstOrDefault();
            asset.NAME = asset.SYMBOL;
            asset.SaveChanges(conn.DbConn);
            */

            var creds = new PostgreSqlNanoCredentials("Host=host.docker.internal; Port=5432; User ID=postgres; Password=sa1234SA; Database=exc_app; Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0;");
            var db = new PostgreSqlNanoDatabase(creds);
            var sql = Asset.SqlBuilder(db).Append("SELECT * FROM").AppendTableName().Append("WHERE").AppendColumnName(x => x.STATUS).ToString(" ");


            Console.WriteLine("Hello, World!");
        }
    }
}