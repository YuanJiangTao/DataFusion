using DataFusion.Data;
using DataFusion.Interfaces;
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
using Unity;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;
        private PluginEntryController _pluginEntryController;
        private ILogDog _log;


        public MainViewModel(IUnityContainer container, DataService dataService, PluginEntryController pluginEntryController)
        {
            _log = container.Resolve<ILogDog>(Constant.ClietnName);
            Dialog = DialogCoordinator.Instance;
            _dataService = dataService;
            _pluginEntryController = pluginEntryController;
            OptionsMenuItems = new ObservableCollection<HamburgerMenuIconItem>();
            LoadCommand = new Lazy<RelayCommand>(() => new RelayCommand(Load)).Value;
            ToastCommand = new Lazy<RelayCommand>(() => new RelayCommand(Toast)).Value;
            Messenger.Default.Register<ToastErrorMsg>(this, toastErrorMsg =>
            {
                this.ToastText = toastErrorMsg.ToString();
                _log.Error(toastErrorMsg.ErrorMessage, toastErrorMsg.Exception);
            });
        }

        private void Toast()
        {
            FlyoutToastIsOpen = !FlyoutToastIsOpen;
        }


        public string SystemTitle => "数据融合";

        public ICommand LoadCommand { get; set; }

        public ICommand ToastCommand { get; }


        public IDialogCoordinator Dialog { get; }

        private async void Load()
        {
            _pluginEntryController.MenuItems.Add(new HamburgerMenuIconItemWrapper()
            {
                Label = "首页",
                Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.PowerPlug },
                Tag = new Views.PluginStateDisplayView()
            });
            OptionsMenuItems.Add(new HamburgerMenuIconItem()
            {
                Label = "系统设置",
                Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.SettingsHelper },
                Tag = new Views.SystemConfigView()
            });
            OptionsMenuItems.Add(new HamburgerMenuIconItem()
            {
                Label = "插件管理",
                Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.SettingsHelper },
                Tag = new Views.MinePluginManagerView()
            });





            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
            await Task.Delay(100);
        }
        private HamburgerMenuIconItemWrapper _selectedMenuItem;

        public HamburgerMenuIconItemWrapper SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                _selectedMenuItem = value;
                RaisePropertyChanged();
            }
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
                SelectedMenuItem = MenuItems.First();
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


        public ObservableCollection<HamburgerMenuIconItemWrapper> MenuItems
        {
            get => _pluginEntryController.MenuItems;
        }

        public ObservableCollection<HamburgerMenuIconItem> OptionsMenuItems { get; set; }



        private bool _flyoutToastIsOpen;
        private string _toastText;

        public bool FlyoutToastIsOpen
        {
            get => _flyoutToastIsOpen;
            set
            {
                _flyoutToastIsOpen = value;
                RaisePropertyChanged();
            }
        }
        public string ToastText
        {
            get => _toastText;
            set
            {
                _toastText = value;
                RaisePropertyChanged();
                FlyoutToastIsOpen = true;
            }
        }

        public override void Cleanup()
        {
            try
            {
                base.Cleanup();
            }
            catch (Exception)
            {

            }
        }
    }
}