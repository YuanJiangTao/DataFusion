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
using DataFusion.Data;
using Newtonsoft.Json;
using DataFusion.Interfaces;
using StackExchange.Redis;
using System.IO;
using DataFusion.Views;

namespace DataFusion.Services
{
    public class DataService
    {


        private string _macAddress = "";


        private RedisHelper _redis;

        public DataService()
        {

            _redis = new RedisHelper(0);
            _macAddress = SystemInfoUtil.GetMacAddress();
        }




        /// <summary>
        /// 从缓存中获取当前所有实例的运行信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientRunInfo> GetClientRunInfos()
        {
            try
            {
                var str = _redis.StringGet(MessageToken.AllClientInfo);
                if (!string.IsNullOrEmpty(str))
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ClientRunInfo>>(str);
                }
            }
            catch (Exception ex)
            {
                LogD.Error($"GetClientRunInfos:{ex.ToString()}");
            }
            return null;
        }

        public ClientRunInfo GetClientRunInfo()
        {
            try
            {
                var clientList = GetClientRunInfos();
                if (clientList != null)
                {
                    return clientList.FirstOrDefault(p => p.MacAddress == _macAddress);
                }
            }
            catch (Exception ex)
            {
                LogD.Error($"GetClientRunInfo:{ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<MineProtocalConfigInfo> GetMineInfoModels()
        {
            var mineKeys = _redis.SetMembers<string>(_macAddress);
            foreach (var key in mineKeys)
            {
                if (_redis.KeyExists(key))
                {
                    yield return _redis.StringGet<MineProtocalConfigInfo>(key);
                }
            }
        }










        /// <summary>
        /// 从缓存中获取当前系统的配置信息
        /// </summary>
        /// <returns></returns>
        public SystemConfigSg GetSystemConfigModel()
        {
            try
            {
                var systemConfigStr = _redis.StringGet(_macAddress);
                if (!string.IsNullOrEmpty(systemConfigStr))
                    return JsonConvert.DeserializeObject<SystemConfigSg>(systemConfigStr);
            }
            catch (Exception ex)
            {
                LogD.Error($"GetSystemConfigModel:{ex.ToString()}");
            }
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
                    Screen=new PluginStateDisplayView(),
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
