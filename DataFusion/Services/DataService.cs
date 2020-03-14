using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFusion.ViewModel;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using DataFusion.Model;
using DataFusion.UserControls;
using DataFusion.Interfaces.Utils;

namespace DataFusion.Services
{
    public class DataService
    {




        private RedisHelper _redis;

        public DataService()
        {
            _redis = new RedisHelper(0);
        }






        public SystemConfigModel GetSystemConfigModel()
        {
            return null;
        }

        public ObservableCollection<MenuViewModel> GetMainItemMenuViewModels()
        {
            return new ObservableCollection<MenuViewModel>()
            {
                new MenuViewModel()
                {
                    Header="插件管理",
                    ImageName=$"../Resources/Img/menu.png",
                    Screen=new PluginStateDisplay(),
                }

            };
        }

        public ObservableCollection<MainItemMenuViewModel> GetExpanderMainItemMenuViewModels()
        {
            return new ObservableCollection<MainItemMenuViewModel>()
            {
                new MainItemMenuViewModel()
                {
                    Header="插件管理",
                    Icon=PackIconKind.Menu,
                    ImageName=$"../Resources/Img/menu.png",
                    SubItems=new ObservableCollection<SubMenuItem>()
                    {
                        new SubMenuItem()
                        {
                            Name="插件状态展示",
                            Screen=new PluginStateDisplay()
                        },
                        new SubMenuItem()
                        {
                            Name="插件编辑",
                            Screen=null
                        }
                    }
                }

            };
        }


    }
}
