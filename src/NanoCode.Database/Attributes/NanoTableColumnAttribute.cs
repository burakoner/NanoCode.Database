using NanoCode.Database.Options;
using System;

namespace NanoCode.Database
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NanoTableColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public object DefaultValue { get; set; }

        public bool PrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        public bool IgnoreOnInsert { get; set; }
        public bool IgnoreOnUpdate { get; set; }

        public NanoTableColumnAttribute()
        {
        }

        public NanoTableColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        internal NanoPrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new NanoPrimaryKeyOptions
            {
                PrimaryKey = this.PrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }

        internal NanoTableColumnOptions ToTableColumnOptions()
        {
            return new NanoTableColumnOptions
            {
                ColumnName = this.ColumnName,
                DefaultValue = this.DefaultValue,

                IsPrimaryKey = this.PrimaryKey,
                AutoIncrement = this.AutoIncrement,

                IgnoreOnInsert = this.IgnoreOnInsert,
                IgnoreOnUpdate = this.IgnoreOnUpdate,
            };
        }

    }

    /*
    public enum DateTimeStoreMethod
    {
        AsDateTime,
        AsEpochSeconds,
        AsEpochMilliSeconds,
    }
    */

}
