using CommonServiceLocator;
using DataFusion.Data;
using DataFusion.Interfaces;
using DataFusion.Interfaces.Utils;
using DataFusion.Services;
using DataFusion.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //_log.Info($"DataFusion:{e.ExceptionObject.ToString()}");
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //_log.Info($"无法加载:{args.Name}");
            return null;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfoHelper.SetDateTimeFormat();
            var locator = (ViewModelLocator)this.Resources["Locator"];
            container = new UnityContainer();
            locator.IniContainer(container);
            ConfigureContainer();
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var collar =container.Resolve <ILogDogCollar>(Constant.ClietnName);
            _log = collar.GetLogger();
            LogD.Initializer(_log);
            _log.Info($"Hello {Constant.ClietnName}.");
        }

        private void ConfigureContainer()
        {
            container.RegisterType<ILogDogCollar, LogDogCollar>(Constant.ClietnName, Singleton());
            var logDogCollar = container.Resolve<ILogDogCollar>(Constant.ClietnName);
            logDogCollar.Setup(Constant.ClietnName, Constant.ClietnName);
            container.RegisterType<DataService>(Singleton());
            container.RegisterType<PluginEntryController>(Singleton());
        }
        private static ContainerControlledLifetimeManager Singleton()
        {
            return new ContainerControlledLifetimeManager();
        }
    }
}
