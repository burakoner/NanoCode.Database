using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using NanoCode.Database.Interfaces;

namespace NanoCode.Database
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NanoCredentialsAttribute : Attribute
    {
        public INanoCredentials Credentials { get; set; }
        public NanoCredentialsAttribute(INanoCredentials credentials)
        {
            this.Credentials = credentials;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NanoTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public NanoTableAttribute()
        {
        }
        public NanoTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PrimaryKeyAttribute : Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }
        public PrimaryKeyAttribute(bool value = true, bool autoIncrement = true)
        {
            IsPrimaryKey = value;
            AutoIncrement = autoIncrement;
        }

        public PrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new PrimaryKeyOptions
            {
                IsPrimaryKey = this.IsPrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }
    }

    public class PrimaryKeyOptions
    {
        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NanoTableColumnAttribute : Attribute
    {
        public object DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }
        public bool IsIgnored { get; set; }
        public bool IsIgnoredOnInsert { get; set; }
        public bool IsIgnoredOnUpdate { get; set; }

        public NanoTableColumnOptions Options 
        {
            get {
                return new NanoTableColumnOptions
                {
                    DefaultValue = this.DefaultValue,
                    IsPrimaryKey = this.IsPrimaryKey,
                    AutoIncrement = this.AutoIncrement,
                    IsIgnored = this.IsIgnored,
                    IsIgnoredOnInsert = this.IsIgnoredOnInsert,
                    IsIgnoredOnUpdate = this.IsIgnoredOnUpdate,
                };
            }
        }

        public NanoTableColumnAttribute()
        {
        }

        public PrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new PrimaryKeyOptions
            {
                IsPrimaryKey = this.IsPrimaryKey,
                AutoIncrement = this.AutoIncrement,
            };
        }
    }

    public class NanoTableColumnOptions
    {
        /// <summary>
        /// Boş ise Property Adı sütun adı olarak iletilir.
        /// </summary>
        // public string ColumnName { get; set; }

        /// <summary>
        /// Column Description
        /// </summary>
        // public string Description { get; set; }
        public object DefaultValue { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// Insert ve Update işlemlerinde ignore edilen kolon gönderilmez
        /// </summary>
        public bool IsIgnored { get; set; }

        /// <summary>
        /// Insert işlemlerinde ignore edilen kolon gönderilmez
        /// </summary>
        public bool IsIgnoredOnInsert { get; set; }

        /// <summary>
        /// Update işlemlerinde ignore edilen kolon gönderilmez
        /// </summary>
        public bool IsIgnoredOnUpdate { get; set; }

        // public int MaxLength { get; set; }
        // public DateTimeStoreMethod DateTimeStoreMethod { get; set; }

        public NanoTableColumnOptions()
        {
        }


        public PrimaryKeyOptions ToPrimaryKeyOptions()
        {
            return new PrimaryKeyOptions
            {
                IsPrimaryKey = this.IsPrimaryKey,
                AutoIncrement = this.AutoIncrement,
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
