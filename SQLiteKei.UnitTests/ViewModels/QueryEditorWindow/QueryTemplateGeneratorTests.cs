using NUnit.Framework;
using SQLiteKei.ViewModels.QueryEditorWindow;
using System.Linq;

namespace SQLiteKei.UnitTests.ViewModels.QueryEditorWindow
{
    [TestFixture]
    public class QueryTemplateGeneratorTests
    {
        private static readonly string[] supportedTemplateNames =
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

        [Test]
        public void GetAvailableTemplates_DoesNotReturnEmptyEnumerable()
        {
            var result = QueryTemplateGenerator.GetAvailableTemplates();
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void GetAvailableTemplates_DoesNotReturnNull()
        {
            var result = QueryTemplateGenerator.GetAvailableTemplates();
            Assert.NotNull(result);
        }

        [Test]
        public void GetAvailableTemplates_ReturnsAllSupportedTemplates()
        {
            var availableTemplates = QueryTemplateGenerator.GetAvailableTemplates();

            foreach (var template in availableTemplates)
            {
                Assert.IsTrue(supportedTemplateNames.Contains(template));
            }
        }

        [Test]
        [TestCaseSource("supportedTemplateNames")]
        public void GetTemplateFor_WithValidTemplateName_ReturnsTemplateString(string templateName)
        {
            var template = QueryTemplateGenerator.GetTemplateFor(templateName);
            var result = string.IsNullOrEmpty(template);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetTemplateFor_WithEmptyTemplateName_ReturnsEmptyString()
        {
            var result = QueryTemplateGenerator.GetTemplateFor(string.Empty);
            Assert.IsTrue(result.Equals(string.Empty));
        }

        [Test]
        public void GetTemplateFor_WithNulledTemplateName_ReturnsEmptyString()
        {
            var result = QueryTemplateGenerator.GetTemplateFor(null);
            Assert.IsTrue(result.Equals(string.Empty));
        }

        [Test]
        public void GetTemplateFor_WithUnsupportedTemplateName_ReturnsEmptyString()
        {
            var result = QueryTemplateGenerator.GetTemplateFor("UNSUPPORTED TEMPLATE NAME");
            Assert.IsTrue(result.Equals(string.Empty));
        }
    }
}
