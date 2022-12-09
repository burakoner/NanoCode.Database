using Nanocode.Database.SqlServer;
using Newtonsoft.Json;
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
            List<Asset> assets = new List<Asset>();
            assets.Add(new Asset { ID = 1, NAME = "NAME-1", SYMBOL = "SYMBOL-1" });
            assets.Add(new Asset { ID = 2, NAME = "NAME-2", SYMBOL = "SYMBOL-2" });
            assets.Add(new Asset { ID = 3, NAME = "NAME-3", SYMBOL = "SYMBOL-3" });
            assets.Add(new Asset { ID = 4, NAME = "NAME-4", SYMBOL = "SYMBOL-4" });
            assets.Add(new Asset { ID = 5, NAME = "NAME-5", SYMBOL = "SYMBOL-5" });
            var json = JsonConvert.SerializeObject(assets);
            var assets2 = JsonConvert.DeserializeObject<List<Asset>>(json);



            /*
            NanoMapper.RegisterModels(Assembly.GetAssembly(typeof(Asset)));
            var conn = new AppConnections("Server=.; Database=NewCatalog; User Id=sa; Password=pa55w0Rd; MultipleActiveResultSets=true");

            var assets = Asset.GetRows(conn.DbConn);
            var asset = assets.FirstOrDefault();
            asset.NAME = asset.SYMBOL;
            asset.SaveChanges(conn.DbConn);
            */

            /*
            var creds = new PostgreSqlNanoCredentials("Host=host.docker.internal; Port=5432; User ID=postgres; Password=sa1234SA; Database=exc_app; Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0;");
            var db = new PostgreSqlNanoDatabase(creds);
            var sql = Asset.SqlBuilder(db).Append("SELECT * FROM").AppendTableName().Append("WHERE").AppendColumnName(x => x.STATUS).ToString(" ");
            */

            var p01 = Asset.Properties.TableName;
            var p02 = Asset.Properties.PrimaryColumnName;
            var p03 = Asset.Properties.ColumnName(x=>x.NAME);
            var p04 = Asset.Properties.ColumnName(x=>x.SYMBOL);

            //var creds = new SqlServerNanoCredentials("Server=localhost; Database=db; User Id=sa; Password=pa33w0Rd; MultipleActiveResultSets=true");
            //var conn = new SqlServerNanoDatabase(creds);
            //var creds = new PostgreSqlNanoCredentials("Host=localhost; Port=5432; User ID=postgres; Password=pa33w0Rd; Database=db; Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0;");
            //var conn = new PostgreSqlNanoDatabase(creds);


            /*
            var builder = Asset.CreateSqlBuilder(conn);
            var abc = builder.Query
                .Select("Id", "Name")
                .Where(p03, "Active")
                .Where(p04, "casdsa");
            var sqlString = builder.ToString();
            var sqlResult = builder.Compile();


            var builder2 = Asset.CreateSqlBuilder(conn);
            var user = builder2.Query
                .Where(Asset.Properties.PrimaryColumnName, 1)
                .Where(p03, "Active");
            var sqlResult2 = builder2.Compile();
            */






            Console.WriteLine("Hello, World!");
        }
    }
}