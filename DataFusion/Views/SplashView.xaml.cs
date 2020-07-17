using DataFusion.Utils;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataFusion.Views
{
    /// <summary>
    /// SplashView.xaml 的交互逻辑
    /// </summary>
    public partial class SplashView 
    {
        //MahApps.Brushes.Accent4
        public SplashView()
        {
            InitializeComponent();
            this.DataContext = this;
            //var accent = (Color)TryFindResource("MahApps.Brushes.Accent4");
            //SplashColor = new SolidColorBrush(accent);
            TxbSplash.Text = ProductUtil.AssemblyTitle + ProductUtil.AssemblyProduct;
            Storyboard animation = this.TryFindResource("Storyboard1") as Storyboard;
            animation?.Begin();
        }
        //public Brush SplashColor { get; set; }
        internal async Task ShowMessage(string loadMessage, Action action = null)
        {
            TxbLoadMessage.Text = loadMessage;
            if (action == null)
                await Task.Delay(100);
            else
                await Task.Run(action);
        }
    }
}
