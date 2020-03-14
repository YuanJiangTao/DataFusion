using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Services;
using DataFusion.Data;
using System.Windows;
using DataFusion.Model;
using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
            Messenger.Default.Register<MenuViewModel>(this, MessageToken.LoadShowContent, t =>
             {
                 if (t != null)
                 {
                     if (MainContent is IDisposable dispose)
                     {
                         dispose.Dispose();
                     }
                     ContentTitle = t.Header;
                     MainContent = t.Screen;
                 }
             });
            MenuInfoList = _dataService.GetMainItemMenuViewModels();
        }

        public string SystemTitle  => "数据融合";

        /// <summary>
        /// 切换例子的命令
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> SwitchCommand =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
            new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

        private MenuViewModel _currentMenuItem;

        private void SwitchDemo(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if ((e.Source as Selector).SelectedItem is MenuViewModel source)
            {
                if (Equals(_currentMenuItem, source)) return;
                _currentMenuItem = source;
                Messenger.Default.Send<MenuViewModel>(source, MessageToken.LoadShowContent);
            }
        }


        private ObservableCollection<MenuViewModel> _mainItemMenuViews;

        public ObservableCollection<MenuViewModel> MenuInfoList
        {
            get => _mainItemMenuViews;
            set => Set(ref _mainItemMenuViews, value);
        }
        private string _contentTitle;
        public string ContentTitle
        {
            get => _contentTitle;
            set => Set(ref _contentTitle, value);
        }

        private object _mainContent;
        public object MainContent
        {
            get => _mainContent;
            set => Set(ref _mainContent, value);
        }
    }
}