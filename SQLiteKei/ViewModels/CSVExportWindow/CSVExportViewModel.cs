using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace SQLiteKei.ViewModels.CSVExportWindow
{
    public class CSVExportViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private readonly string tableName;

        public string DescriptionLabelText { get; set; }

        public bool IsFirstRowColumnTitles { get; set; }

        public string SelectedSeparator { get; set; }

        public List<string> AvailableSeparators { get; set; }

        public string SelectedEnclosure { get; set; }

        public List<string> AvailableEnclosures { get; set; }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public CSVExportViewModel(string tableName)
        {
            this.tableName = tableName;
            DescriptionLabelText = LocalisationHelper.GetString("CSVExportWindow_CSVExportFor", tableName);

            AvailableSeparators = new List<string> { ",", ";", "|", "Tab" };
            AvailableEnclosures = new List<string> { "\"", "None" };

            SelectedSeparator = ";";
            SelectedEnclosure = "None";

            exportCommand = new DelegateCommand(Export);
        }

        private void Export()
        {
            StatusInfo = string.Empty;

            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                var stringBuilder = new StringBuilder();
                
                if (IsFirstRowColumnTitles)
                {
                    var columns = tableHandler.GetColumns(tableName);
                    string combinedColumnHeaders = GetCombinedColumnHeadersFor(columns);

                    stringBuilder.Append(combinedColumnHeaders + Environment.NewLine);
                }

                var rows = tableHandler.GetRows(tableName);

                foreach (DataRow row in rows)
                {
                    string combinedValues = GetCombinedRowValuesFor(row);

                    stringBuilder.Append(combinedValues + Environment.NewLine);
                }
                ExportCSV(stringBuilder.ToString());
            }
        }

        private string GetCombinedColumnHeadersFor(List<Column> columns)
        {
            IEnumerable<string> columnHeaders;

            if (SelectedEnclosure.Equals("None"))
                columnHeaders = columns.Select(x => x.Name);
            else
                columnHeaders = columns.Select(x => "\"" + x.Name + "\"");

            string combinedColumnHeaders;

            if (SelectedSeparator.Equals("Tab"))
                return combinedColumnHeaders = string.Join("\t", columnHeaders);
            else
                return combinedColumnHeaders = string.Join(SelectedSeparator, columnHeaders);
        }

        private string GetCombinedRowValuesFor(DataRow row)
        {
            IEnumerable<string> values;

            if (SelectedEnclosure.Equals("None"))
                values = row.ItemArray.Select(x => x.ToString());
            else
                values = row.ItemArray.Select(x => "\"" + x + "\"");

            string combinedValues;

            if (SelectedSeparator.Equals("Tab"))
                return combinedValues = string.Join("\t", values);
            else
                return combinedValues = string.Join(SelectedSeparator, values);
        }

        private void ExportCSV(string csv)
        {
            using (var fileDialog = new SaveFileDialog())
            {
                fileDialog.Filter = "CSV Files(*.csv)|*.csv";
                fileDialog.FileName = tableName + ".csv";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(fileDialog.FileName, csv);
                        logger.Info("Exported table to CSV file " + fileDialog.FileName);
                        StatusInfo = LocalisationHelper.GetString("CSVExportWindow_ExportSuccess");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Could not export table to CSV file " + fileDialog.FileName, ex);
                        var errorMessage = LocalisationHelper.GetString("MessageBox_TableCSVExportFailed", ex.Message);

                        System.Windows.MessageBox.Show(errorMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private DelegateCommand exportCommand;
        public DelegateCommand ExportCommand { get { return exportCommand; } }
    }
}
