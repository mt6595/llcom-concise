using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace llcom.Pages
{
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private static bool loaded = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded)
                return;
            loaded = true;
            this.DataContext = Tools.Global.setting;
            aboutScrollViewer.ScrollToTop();
            versionTextBlock.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void OpenSourceButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://llcom.papapoi.com/");
        }
    }
}
