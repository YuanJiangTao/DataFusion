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

        public void IniContainer(IUnityContainer container)
        {
            container.RegisterType<PluginEntryController>();
            container.RegisterType<DataService>();

            container.RegisterType<MainViewModel>();
            container.RegisterType<PluginStateDisplayViewModel>();

            Main = container.Resolve<MainViewModel>();

            PluginStateDisplayView = container.Resolve<PluginStateDisplayViewModel>();
        }
        public MainViewModel Main { get; private set; }

        public PluginStateDisplayViewModel PluginStateDisplayView { get; private set; }


        public static void Cleanup()
        {

        }
    }
}