using CommonServiceLocator;
using DataFusion.Interfaces;
using DataFusion.Interfaces.Utils;
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
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfoHelper.SetDateTimeFormat();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IUnityContainer, UnityContainer>();
            ConfigureContainer();
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var collar = SimpleIoc.Default.GetInstance<ILogDogCollar>();
            var _log = collar.GetLogger();
            LogD.Initializer(_log);
            _log.Info("Hello DataFusion.");
        }
        private void ConfigureContainer()
        {
            SimpleIoc.Default.Register<ILogDogCollar, LogDogCollar>();
            var logDogCollar = SimpleIoc.Default.GetInstance<ILogDogCollar>();
            logDogCollar.Setup("DataFusion", "DataFusion");
        }
        private static ContainerControlledLifetimeManager Singleton()
        {
            return new ContainerControlledLifetimeManager();
        }
    }
}
