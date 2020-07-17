/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DataFusion"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using DataFusion.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Unity;
using MahApps.Metro.Controls.Dialogs;
using Unity.Lifetime;
using Unity.Injection;

namespace DataFusion.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //SimpleIoc.Default.Register<DataService>();
            //SimpleIoc.Default.Register<MessageService>();
            //SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<PluginEditViewModel>();
        }
        private IUnityContainer _container;
        public void IniContainer(IUnityContainer container)
        {
            _container = container;
            container.RegisterType<MainViewModel>();
            container.RegisterType<PluginStateDisplayViewModel>();
            container.RegisterType<SystemConfigViewModel>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<MetroDialog>()));
            container.RegisterType<PluginManagerViewModel>();
            container.RegisterType<PluginCatalogEntryViewModel>();
            Main = _container.Resolve<MainViewModel>();
            container.RegisterInstance(Main, new ContainerControlledLifetimeManager());
            var dialog = new MetroDialog(Main);
            container.RegisterInstance(dialog, new ContainerControlledLifetimeManager());
        }


        public MainViewModel Main { get; private set; }

        public PluginStateDisplayViewModel PluginStateDisplayViewModel => _container.Resolve<PluginStateDisplayViewModel>();


        public SystemConfigViewModel SystemConfigViewModel => _container.Resolve<SystemConfigViewModel>();


        public PluginManagerViewModel PluginManagerViewModel => _container.Resolve<PluginManagerViewModel>();

        public PluginCatalogEntryViewModel PluginCatalogEntryViewModel => _container.Resolve<PluginCatalogEntryViewModel>();

    }
}