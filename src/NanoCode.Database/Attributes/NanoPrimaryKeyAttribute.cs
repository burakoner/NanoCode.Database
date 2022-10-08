using NanoCode.Database.Options;
using System;

namespace NanoCode.Database
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NanoPrimaryKeyAttribute : Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        public NanoPrimaryKeyAttribute(bool value = true, bool autoIncrement = true)
        {
            IsPrimaryKey = value;
            AutoIncrement = autoIncrement;
        }

        internal NanoPrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new NanoPrimaryKeyOptions
            {
                IsPrimaryKey = this.IsPrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }
    }
}
