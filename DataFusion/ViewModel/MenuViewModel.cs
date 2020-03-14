using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DataFusion.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace DataFusion.ViewModel
{
   public class MenuViewModel:ViewModelBase
    {
        public MenuViewModel()
        {

        }
        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set => Set(ref _imageName, value);
        }
        private string _header;
        public string Header
        {
            get => _header;
            set => Set(ref _header, value);
        }
        private FrameworkElement _screen;
        public FrameworkElement Screen
        {
            get => _screen;
            set => Set(ref _screen, value);
        }
    }
}
