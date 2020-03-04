using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace DataFusion.Model
{
    public class MainItemMenu : ViewModelBase
    {
        public MainItemMenu()
        {
            _subItems = new ObservableCollection<SubMenuItem>();
        }
        public MainItemMenu(string header, List<SubMenuItem> subItems, PackIconKind icon)
        {
            _header = header;
            _subItems = new ObservableCollection<SubMenuItem>(subItems);
        }


        /// <summary>
        /// 切换例子的命令
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> SwitchCommand =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
            new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

        private void SwitchDemo(SelectionChangedEventArgs e)
        {

        }

        private string _header;
        public string Header
        {
            get => _header;
            set => Set(ref _header, value);
        }
        private PackIconKind _icon;

        public PackIconKind Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }
        private ObservableCollection<SubMenuItem> _subItems;
        public ObservableCollection<SubMenuItem> SubItems
        {
            get => _subItems;
            set => Set(ref _subItems, value);
        }
        private FrameworkElement _screen;
        public FrameworkElement Screen
        {
            get => _screen;
            set => Set(ref _screen, value);
        }
    }
}
