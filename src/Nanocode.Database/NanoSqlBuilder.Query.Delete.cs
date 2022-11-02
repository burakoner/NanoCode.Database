using Nanocode.Database.Interfaces;
using SqlKata;
using SqlKata.Compilers;
using System;

namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> AsDelete()
        {
            this.Query.AsDelete();
            return this;
        }
    }
}
