using DataFusion.Data;
using DataFusion.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;
        private MessageService _messageService;
        private PluginEntryController _pluginEntryController;

        public MainViewModel(DataService dataService, PluginEntryController pluginEntryController, MessageService messageService)
        {
            _dataService = dataService;
            _messageService = messageService;
            _pluginEntryController = pluginEntryController;
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
            Messenger.Default.Register<MenuViewModel>(this, MessageToken.AddMenuItem, t =>
            {
                MenuInfoList.Add(t);
                //TODO:默认回到首页
            });
            Messenger.Default.Register<string>(this, MessageToken.RemoveItem, t =>
            {
                var menuItem = MenuInfoList.FirstOrDefault(p => p.Header == t);
                if(menuItem!=null)
                {
                    MenuInfoList.Remove(menuItem);
                }
            });
            MenuInfoList = _dataService.GetMainItemMenuViewModels();
            LoadPluginMenuItems();
        }

        private async void LoadPluginMenuItems()
        {
            var pluginMenuItem = await _pluginEntryController.LoadPluginEntiesAsync();
            foreach (var subMenuItem in pluginMenuItem)
            {
                MenuInfoList.Add(subMenuItem);
            }
        }

        public string SystemTitle => "数据融合";

        public RelayCommand LoadCommand => new RelayCommand(Load);
        //new Lazy<RelayCommand>(() =>
        //new RelayCommand(Load)).Value;

        private void Load()
        {

        }





        /// <summary>
        /// 切换例子的命令
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> SwitchCommand =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
            new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

        private MenuViewModel _currentMenuItem;

        private void SwitchDemo(SelectionChangedEventArgs e)
        {
            _messageService.Warnging("开始载入...");
            if (e.AddedItems.Count == 0) return;
            if ((e.Source as Selector).SelectedItem is MenuViewModel source)
            {
                if (Equals(_currentMenuItem, source)) return;
                _currentMenuItem = source;
                Messenger.Default.Send<MenuViewModel>(source, MessageToken.LoadShowContent);
            }
        }


        private ObservableCollection<MenuViewModel> _mainItemMenuViews = new ObservableCollection<MenuViewModel>();

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