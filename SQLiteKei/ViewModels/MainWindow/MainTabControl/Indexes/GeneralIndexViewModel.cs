using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Indexes
{
    public class GeneralIndexViewModel
    {
        private string indexName;
        public string IndexName { get; set; }

        public GeneralIndexViewModel(string indexName)
        {
            this.indexName = indexName;
        }
    }
}
