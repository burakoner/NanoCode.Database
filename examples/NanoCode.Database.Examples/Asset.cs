using Nanocode.Database;
using Nanocode.Database.Interfaces;

namespace Nanocode.Database.Examples
{
    [NanoTable("EXC_ASSETS")]
    public class Asset : NanoObject<Asset>
    {
        /* Constructors */
        public Asset() { }
        public Asset(INanoDatabase db, int id) : base(db, id) { }

        /* Columns */
        [NanoPrimaryKey]
        public int ID { get; set; }
        public AssetType TYPE { get; set; }
        public AssetStatus STATUS { get; set; }
        public string NAME { get; set; }
        [NanoTableColumn(ColumnName = "SYM")]
        public string SYMBOL { get; set; }
    }

    public enum AssetStatus : byte
    {
        Passive = 0,
        Active = 1,
    }

    public enum AssetType : byte
    {
        Crypto = 1,
        Fiat = 2,
    }
}
