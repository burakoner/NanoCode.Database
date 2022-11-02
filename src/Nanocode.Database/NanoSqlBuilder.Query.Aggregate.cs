namespace Nanocode.Database
{
    public partial class NanoSqlBuilder<TEntity>
    {
        public NanoSqlBuilder<TEntity> AsAggregate(string type, string[] columns = null)
        {
            this.Query.AsAggregate(type, columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsCount(string[] columns = null)
        {
            this.Query.AsCount(columns);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsAvg(string column)
        {
            this.Query.AsAvg(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsAverage(string column)
        {
            this.Query.AsAverage(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsSum(string column)
        {
            this.Query.AsSum(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsMax(string column)
        {
            this.Query.AsMax(column);
            return this;
        }
        public NanoSqlBuilder<TEntity> AsMin(string column)
        {
            this.Query.AsMin(column);
            return this;
        }
    }
}
