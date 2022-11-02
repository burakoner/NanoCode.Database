using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Nanocode.Database.Interfaces;

namespace Nanocode.Database
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
}
