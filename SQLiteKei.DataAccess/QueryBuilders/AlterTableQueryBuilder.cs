using System;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Data;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// QueryBuilder to generate a statement to alter a table.
    /// </summary>
    public class AlterTableQueryBuilder : QueryBuilderBase
    {
        private string tableName;

        private ColumnData column;

        private AlterType alterType;

        public AlterTableQueryBuilder(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        /// Configures the builder to use the specified column definition for an ALTER TABLE ADD COLUMN statement.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="isPrimary">if set to <c>true</c> [is primary].</param>
        /// <param name="isNotNull">if set to <c>true</c> [is not null].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="ColumnDefinitionException">Invalid column name.</exception>
        public AlterTableQueryBuilder AddColumn(string columnName, string dataType, bool isNotNull = true, object defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ColumnDefinitionException("Invalid column name.");
            }

            column = new ColumnData
            {
                ColumnName = columnName,
                DataType = dataType,
                IsNotNull = isNotNull,
                DefaultValue = defaultValue
            };

            alterType = AlterType.AddColumn;
            return this;
        }

        public override string Build()
        {
            switch(alterType)
            {
                case AlterType.AddColumn:
                    return string.Format("ALTER TABLE '{0}'\nADD COLUMN {1};", tableName, column);
                default:
                    throw new QueryBuilderException("Could not build ALTER TABLE query. The altering type has not been defined.");
            }
        }
    }
}
