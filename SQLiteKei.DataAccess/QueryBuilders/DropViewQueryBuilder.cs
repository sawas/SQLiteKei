using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class DropViewQueryBuilder : QueryBuilderBase
    {
        private string viewName;

        private bool ifExists;

        public DropViewQueryBuilder(string viewName)
        {
            this.viewName = viewName;
        }

        public DropViewQueryBuilder IfExists(bool value = true)
        {
            ifExists = value;
            return this;
        }

        public override string Build()
        {
            if (ifExists)
                return string.Format("DROP VIEW IF EXISTS {0}", viewName);

            return string.Format("DROP VIEW {0}", viewName);
        }
    }
}
