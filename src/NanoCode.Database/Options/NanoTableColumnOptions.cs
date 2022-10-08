using System;
using System.Collections.Generic;
using System.Text;

namespace NanoCode.Database.Options
{
    internal class NanoTableColumnOptions
    {
        public string ColumnName { get; set; }
        public object DefaultValue { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        public bool IgnoreOnInsert { get; set; }
        public bool IgnoreOnUpdate { get; set; }

        // public int MaxLength { get; set; }
        // public DateTimeStoreMethod DateTimeStoreMethod { get; set; }

        public NanoTableColumnOptions()
        {
        }

        internal NanoPrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new NanoPrimaryKeyOptions
            {
                PrimaryKey = this.IsPrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }
    }

}
