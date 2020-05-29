using DataFusion.Data;
using DataFusion.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;
        private PluginEntryController _pluginEntryController;


        public MainViewModel(DataService dataService, PluginEntryController pluginEntryController)
        {
            Dialog = DialogCoordinator.Instance;
            _dataService = dataService;
            _pluginEntryController = pluginEntryController;
            MenuItems = new ObservableCollection<HamburgerMenuItem>();
            OptionsMenuItems = new ObservableCollection<HamburgerMenuIconItem>();
            LoadCommand = new Lazy<RelayCommand>(() => new RelayCommand(Load)).Value;
        }



        public string SystemTitle => "数据融合";

        public ICommand LoadCommand { get; set; }

        public IDialogCoordinator Dialog { get; }

        private async void Load()
        {
            OptionsMenuItems.Add(new HamburgerMenuIconItem()
            {
                Label = "插件管理",
                Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.SettingsHelper },
                Tag = new Views.PluginManagerView()
            });
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
            await Task.Delay(100);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTimeNow = DateTime.Now;
        }

        private DispatcherTimer _dispatcherTimer;

        private DateTime _dateTimeNow;
        public DateTime DateTimeNow
        {
            get => _dateTimeNow;

            set
            {
                _dateTimeNow = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand<CancelEventArgs> CloseCommand => new Lazy<RelayCommand<CancelEventArgs>>(() =>
        new RelayCommand<CancelEventArgs>(async e =>
        {
            try
            {
                e.Cancel = true;
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "确定",
                    NegativeButtonText = "取消",
                    AnimateShow = true,
                    AnimateHide = false
                };
                var result = await Dialog.ShowMessageAsync(this, "提示",
                                             "要退出程序吗?",
                                             MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    e.Cancel = false;
                    Application.Current.Shutdown();
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        })).Value;


        public ObservableCollection<HamburgerMenuItem> MenuItems { get; set; }

        public ObservableCollection<HamburgerMenuIconItem> OptionsMenuItems { get; set; }


    }
}