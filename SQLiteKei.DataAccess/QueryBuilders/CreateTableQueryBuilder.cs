using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Data;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// QueryBuilder to generate a statement to create a table.
    /// </summary>
    public class CreateTableQueryBuilder
    {
        private string table; 

        private List<ColumnData> Columns { get; set; }

        private List<ForeignKeyData> ForeignKeys { get; set; }

        private bool primaryKeyAdded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTableQueryBuilder"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public CreateTableQueryBuilder(string table)
        {
            this.table = table;
            Columns = new List<ColumnData>();
            ForeignKeys = new List<ForeignKeyData>();
        }

        /// <summary>
        /// Adds a new column to the query builder
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dataType">Type of the column.</param>
        /// <param name="isPrimary">Defines if the column is the primary key.</param>
        /// <param name="isNotNull">Defines if the column is marked NotNull</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="ColumnDefinitionException">
        /// Invalid column name.
        /// or
        /// Multiple primary keys defined.
        /// </exception>
        public CreateTableQueryBuilder AddColumn(string columnName, string dataType, bool isPrimary = false, bool isNotNull = true, object defaultValue = null)
        {
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new ColumnDefinitionException("Invalid column name.");
            }

            CheckIfColumnAlreadyAdded(columnName);

            if (isPrimary)
            {
                if (primaryKeyAdded)
                    throw new ColumnDefinitionException("Multiple primary keys defined.");

                primaryKeyAdded = true;
            }

            Columns.Add(new ColumnData
            {
                ColumnName = columnName,
                DataType = dataType,
                IsPrimary = isPrimary,
                IsNotNull = isNotNull,
                DefaultValue = defaultValue
            });

            return this;
        }

        private void CheckIfColumnAlreadyAdded(string columnName)
        {
            var result = Columns.Find(c => c.ColumnName.Equals(columnName));

            if(result != null)
            {
                var exceptionMessage =
                    string.Format("The column with the name '{0}' was defined more than once.", columnName);
                throw new ColumnDefinitionException(exceptionMessage);
            }
        }

        /// <summary>
        /// Adds the foreign key.
        /// </summary>
        /// <param name="localColumn">The local column.</param>
        /// <param name="referencedTable">The referenced table.</param>
        /// <param name="referencedColumn">The referenced column.</param>
        /// <returns></returns>
        public CreateTableQueryBuilder AddForeignKey(string localColumn, string referencedTable, string referencedColumn)
        {
            CheckIfForeignKeyAlreadyAdded(localColumn);

            ForeignKeys.Add(new ForeignKeyData
            {
                LocalColumn = localColumn,
                ReferencedTable = referencedTable,
                ReferencedColumn = referencedColumn
            });

            return this;
        }

        private void CheckIfForeignKeyAlreadyAdded(string columnName)
        {
            var result = ForeignKeys.Find(c => c.LocalColumn.Equals(columnName));

            if (result != null)
            {
                var exceptionMessage =
                    string.Format("The foreign key on '{0}' was defined more than once.", columnName);
                throw new CreateQueryBuilderException(exceptionMessage);
            }
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
            {
                throw new CreateQueryBuilderException("No valid table name provided.");
            }

            if(!Columns.Any())
            {
                throw new ColumnDefinitionException("No columns defined.");
            }

            string columns = string.Join(",\n", Columns);
            
            if(ForeignKeys.Any())
            {
                string foreignKeys = string.Join(",\n", ForeignKeys);
                return string.Format("CREATE TABLE '{0}'\n(\n{1},\n{2}\n);", table, columns, foreignKeys);
            }

            return string.Format("CREATE TABLE '{0}'\n(\n{1}\n);", table, columns);
        }
    }
}
