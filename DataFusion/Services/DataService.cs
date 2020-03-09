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

namespace DataFusion.Services
{
    public class DataService
    {
        public ObservableCollection<MainItemMenuViewModel> GetMainItemMenuViewModels()
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
