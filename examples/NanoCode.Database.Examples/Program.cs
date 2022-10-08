using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoCode.Database.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NanoMapper.RegisterModels(Assembly.GetAssembly(typeof(Asset)));
            var conn = new AppConnections("Server=.; Database=NewCatalog; User Id=sa; Password=pa55w0Rd; MultipleActiveResultSets=true");

            var assets = Asset.GetRows(conn.DbConn);
            var asset = assets.FirstOrDefault();
            asset.NAME = asset.SYMBOL;
            asset.SaveChanges(conn.DbConn);


            Console.WriteLine("Hello, World!");
        }
    }
}