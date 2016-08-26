using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            VersionLabel.Content = GetVersionString();
        }

        private object GetVersionString()
        {
            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            return string.Format("Version {0}", assemblyVersion);
        }

        private void CheckoutOnGithub(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
