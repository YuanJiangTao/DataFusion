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
using DataFusion.ViewModel;
using MahApps.Metro.Controls.Dialogs;
namespace DataFusion
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, MahApps.Metro.Controls.HamburgerMenuItemInvokedEventArgs args)
        {
            HamburgerMenuControl.Content = args.InvokedItem;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.SystemConfigFlyout.IsOpen = !this.SystemConfigFlyout.IsOpen;
        }

        private void BtnPluginEntrySettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
