using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class ViewCreateQueryBuilder : QueryBuilderBase
    {
        private string viewName;

        private bool isIfNotExists;

        private string asStatement;

        public ViewCreateQueryBuilder(string viewName)
        {
            this.viewName = viewName;
        }

        public ViewCreateQueryBuilder IfNotExists(bool value)
        {
            isIfNotExists = value;
            return this;
        }

        public ViewCreateQueryBuilder As(string asStatement)
        {
            this.asStatement = asStatement;
            return this;
        }

        public override string Build()
        {
            if (isIfNotExists)
                return string.Format("CREATE VIEW IF NOT EXISTS {0} AS\n{1}", viewName, asStatement);

            return string.Format("CREATE VIEW {0} AS\n{1}", viewName, asStatement);
        }
    }
}
