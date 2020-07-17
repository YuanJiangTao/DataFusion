using CommonServiceLocator;
using DataFusion.Data;
using DataFusion.Interfaces;
using DataFusion.Interfaces.Utils;
using DataFusion.Services;
using DataFusion.ViewModel;
using DataFusion.Views;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace DataFusion
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private ILogDog _log;
        private IUnityContainer container;

        private SplashView _splashView;
        private MainWindow _mainWindow;

        private static Mutex AppMutex;
        public App()
        {
            
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var errorWindow = container.Resolve<ErrorWindow>();
            errorWindow.Init((Exception)e.ExceptionObject);
            errorWindow.ShowDialog();
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            _log?.Info($"无法加载:{args.Name}");
            return null;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            AppMutex = new Mutex(true, Constant.ClietnName, out var createdNew);
            if (!createdNew)
            {
                var current = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        Win32Utils.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Shutdown();
            }
            else
            {
                CultureInfoHelper.SetDateTimeFormat();
                var locator = (ViewModelLocator)this.Resources["Locator"];
                container = new UnityContainer();
                ConfigureContainer();
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                _splashView = container.Resolve<SplashView>();
                _splashView.Show();
                await _splashView.ShowMessage("正在初始化容器");
                locator.IniContainer(container);
                await container.Resolve<PluginEntryController>().IniMinePlugins();
                base.OnStartup(e);
            }
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _splashView.ShowMessage("正在初始插件");
            _mainWindow = container.Resolve<MainWindow>();
            _mainWindow.InitializeComponent();
            _log.Info($"Hello {Constant.ClietnName}.");
            Current.MainWindow = _mainWindow;
            _mainWindow.Show();
            _splashView.Close();
        }

        private void ConfigureContainer()
        {
            container.RegisterType<ILogDogCollar, LogDogCollar>(Constant.ClietnName, Singleton());
            var logDogCollar = container.Resolve<ILogDogCollar>(Constant.ClietnName);
            logDogCollar.Setup(Constant.ClietnName, Constant.ClietnName);
            _log = logDogCollar.GetLogger();
            LogD.Initializer(_log);
            container.RegisterInstance<ILogDog>(Constant.ClietnName, _log, Singleton());
            container.RegisterType<DataService>(Singleton());
            container.RegisterType<PluginEntryController>(Singleton());
        }
        private static ContainerControlledLifetimeManager Singleton()
        {
            return new ContainerControlledLifetimeManager();
        }
    }
}
