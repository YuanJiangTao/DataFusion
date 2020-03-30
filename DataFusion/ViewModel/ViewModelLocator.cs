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
            container.RegisterType<MainViewModel>();
            container.RegisterType<PluginStateDisplayViewModel>();
            container.RegisterType<NoUserContentViewModel>();
            container.RegisterType<SystemConfigViewModel>();
            _container = container;



        }


        public MainViewModel Main => _container.Resolve<MainViewModel>();

        public PluginStateDisplayViewModel PluginStateDisplayView => _container.Resolve<PluginStateDisplayViewModel>();

        public NoUserContentViewModel NoUserContentViewModel => _container.Resolve<NoUserContentViewModel>();

        public SystemConfigViewModel SystemConfigViewModel => _container.Resolve<SystemConfigViewModel>();


        public static void Cleanup()
        {

        }
    }
}