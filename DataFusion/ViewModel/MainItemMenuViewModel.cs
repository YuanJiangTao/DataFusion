using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DataFusion.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Data;
using System.Windows.Controls.Primitives;

namespace DataFusion.ViewModel
{
    public class MainItemMenuViewModel : ViewModelBase
    {
        public MainItemMenuViewModel()
        {
            _subItems = new ObservableCollection<SubMenuItem>();

        }

        /// <summary>
        /// 切换例子的命令
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> SwitchCommand =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
            new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

        private SubMenuItem _currentSubItem;

        private void SwitchDemo(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if ((e.Source as Selector).SelectedItem is SubMenuItem source)
            {
                if (Equals(_currentSubItem, source)) return;
                _currentSubItem = source;
                Messenger.Default.Send<SubMenuItem>(source, MessageToken.LoadShowContent);
            }
        }

        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set=>Set(ref _imageName,value);
        }

        private string _header;
        public string Header
        {
            get => _header;
            set => Set(ref _header, value);
        }
        //private PackIconKind _icon;

        //public PackIconKind Icon
        //{
        //    get => _icon;
        //    set => Set(ref _icon, value);
        //}
        private ObservableCollection<SubMenuItem> _subItems;
        public ObservableCollection<SubMenuItem> SubItems
        {
            get => _subItems;
            set => Set(ref _subItems, value);
        }
    }
}
