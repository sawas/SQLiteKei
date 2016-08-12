using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class QueryBuilderException : Exception
    {
        public QueryBuilderException(string message) : base(message)
        {
        }
    }
}
