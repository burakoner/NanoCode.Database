﻿using System;

namespace Nanocode.Database
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NanoTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public NanoTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
