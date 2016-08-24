using log4net;

using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Enums;
using SQLiteKei.Util;

using System.Collections.Generic;
using System;

namespace SQLiteKei.ViewModels.QueryEditorWindow
{
    internal class QueryTemplateGenerator
    {
        private readonly ILog logger = LogHelper.GetLogger();

        public static IEnumerable<string> GetAvailableTemplates()
        {
            return new List<string>
            {
                "ALTER TABLE ADD COLUMN",
                "CREATE INDEX",
                "CREATE TABLE",
                "CREATE TRIGGER",
                "CREATE VIEW",
                "DROP INDEX",
                "DROP TABLE",
                "DROP TRIGGER",
                "DROP VIEW",
                "INSERT",
                "SELECT",
                "SELECT ALL"
            };
        }

        public string GetTemplateFor(string templateName)
        {
            if (templateName.Equals("ALTER TABLE ADD COLUMN"))
                return GetAlterTableAddColumn();
            if (templateName.Equals("CREATE INDEX"))
                return CreateIndex();
            if (templateName.Equals("CREATE TABLE"))
                return CreateTable();
            if (templateName.Equals("CREATE TRIGGER"))
                return CreateTrigger();
            if (templateName.Equals("CREATE VIEW"))
                return CreateView();
            if (templateName.Equals("DROP INDEX"))
                return DropIndex();
            if (templateName.Equals("DROP TABLE"))
                return DropTable();
            if (templateName.Equals("DROP TRIGGER"))
                return DropTrigger();
            if (templateName.Equals("DROP VIEW"))
                return DropView();
            if (templateName.Equals("INSERT"))
                return Insert();
            if (templateName.Equals("SELECT"))
                return Select();
            if (templateName.Equals("SELECT ALL"))
                return SelectAll();

            logger.Error("Could not load template " + templateName + ". Unknown template name.");
            return string.Empty;
        }

        private string SelectAll()
        {
            return QueryBuilder.Select()
                .From("TableName")
                .Build();
        }

        private string Select()
        {
            return QueryBuilder.Select()
                .From("TableName")
                .AddSelect("ColumnA", "Alias")
                .AddSelect("ColumnB")
                .Distinct()
                .Limit(10, 50)
                .OrderBy("ColumnB", true)
                .Where("ColumnB")
                .IsGreaterThanOrEqual("42")
                .And("ColumnA")
                .Contains("Statement")
                .Build();
        }

        private string Insert()
        {
            return QueryBuilder.InsertInto("TableName")
                .Values(new[] { "ValueA", "3", "Abc" })
                .Build();
        }

        private string DropView()
        {
            return QueryBuilder.DropView("ViewName")
               .IfExists()
               .Build();
        }

        private string DropTrigger()
        {
            return QueryBuilder.DropTrigger("TriggerName")
               .IfExists()
               .Build();
        }

        private string DropTable()
        {
            return QueryBuilder.DropTable("TableName")
               .IfExists()
               .Cascade()
               .Build();
        }

        private string DropIndex()
        {
            return QueryBuilder.DropIndex("IndexName")
                .IfExists()
                .Build();
        }

        private string CreateView()
        {
            return QueryBuilder.CreateView("ViewName")
                .IfNotExists()
                .As("SelectStatement")
                .Build();
        }

        private string CreateTrigger()
        {
            return QueryBuilder.CreateTrigger("TriggerName")
                .IfNotExists()
                .After()
                .Insert()
                .On("TargetTableName")
                .ForEachRow()
                .When("Expression")
                .Do("TriggerActions")                
                .Build();
        }

        private string CreateTable()
        {
            return QueryBuilder.CreateTable("TableName")
                 .AddColumn("ColumnA", "Integer", true, true)
                 .AddColumn("ColumnB", "Integer")
                 .AddColumn("ColumnC", "Varchar(255)", false, false, "DefaultValue")
                 .AddForeignKey("ColumnB", "OtherTable", "OtherTableColumn")
                 .Build();
        }

        private string CreateIndex()
        {
            return QueryBuilder.CreateIndex("IndexName")
                .Unique()
                .IfNotExists()
                .On("TableName")
                .IndexColumn("ColumnName", OrderType.Descending)
                .Where("Statement")
                .Build();
        }

        private static string GetAlterTableAddColumn()
        {
            return QueryBuilder.AlterTable("TableName")
                .AddColumn("ColumnName", "varchar(255)", true, "defaultValue")
                .Build();
        }
    }
}
