using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace DataFusion.Model
{
    public class SubMenuItem:ViewModelBase
    {
        public SubMenuItem()
        {

        }
        public SubMenuItem(string name, FrameworkElement screen = null)
        {

        }
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private FrameworkElement _screen;
        public FrameworkElement Screen
        {
            get => _screen;
            set => Set(ref _screen, value);
        }
    }
}
