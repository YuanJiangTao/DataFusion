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

namespace DataFusion.Views
{
    /// <summary>
    /// NoUserContentView.xaml 的交互逻辑
    /// </summary>
    public partial class NoUserContentView 
    {
        public NoUserContentView()
        {
            InitializeComponent();
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            this.PopupConfig.IsOpen = true;
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSetting_Click(object sender, RoutedEventArgs e)
        {
            //this.PopupSetting.IsOpen = true;
        }
    }
}
