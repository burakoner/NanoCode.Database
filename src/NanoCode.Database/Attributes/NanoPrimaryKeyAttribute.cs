using NanoCode.Database.Options;
using System;

namespace NanoCode.Database
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NanoPrimaryKeyAttribute : Attribute
    {
        public bool PrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        public NanoPrimaryKeyAttribute(bool primaryKey = true, bool autoIncrement = true)
        {
            PrimaryKey = primaryKey;
            AutoIncrement = autoIncrement;
        }

        internal NanoPrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new NanoPrimaryKeyOptions
            {
                PrimaryKey = this.PrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }
    }
}
