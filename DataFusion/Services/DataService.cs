using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFusion.ViewModel;
using System.Collections.ObjectModel;
using DataFusion.Model;
using DataFusion.UserControls;
using DataFusion.Interfaces.Utils;
using DataFusion.Data;
using Newtonsoft.Json;
using DataFusion.Interfaces;
using StackExchange.Redis;
using System.IO;
using DataFusion.Views;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DataFusion.Services
{
    public class DataService
    {
        private readonly string _dataRedisKey = "";
        private RedisHelper _redis;
        public DataService()
        {
            _redis = new RedisHelper(0);
            _dataRedisKey = SystemInfoUtil.GetMacAddress() ?? Constant.ClietnName;
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
                    return clientList.FirstOrDefault(p => p.MacAddress == _dataRedisKey);
                }
            }
            catch (Exception ex)
            {
                LogD.Error($"GetClientRunInfo:{ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<MinePluginConfigModel> GetMineInfoModels()
        {
            var mineKeys = _redis.SetMembers<string>(_dataRedisKey);
            foreach (var key in mineKeys)
            {
                if (_redis.KeyExists(key))
                {
                    yield return _redis.StringGet<MinePluginConfigModel>(key);
                }
            }
        }

        public bool HandleMinePluginConfigModel(MinePluginConfigModel model)
        {
            return _redis.HashSet<MinePluginConfigModel>(_dataRedisKey, model.Id.ToString(), model);
        }

        public bool DeleteMinePluginConfigModel(MinePluginConfigModel model)
        {
            _redis.KeyDelete(model.Id.ToString());
            return _redis.HashDelete(_dataRedisKey, model.Id.ToString());
        }

        public IEnumerable<HashEntry> GetHashEntries()
        {
            return _redis.HashGetAll(_dataRedisKey);
        }

        public IEnumerable<MinePluginConfigModel> GetMinePluginConfigModels()
        {
            return _redis.HashGetAll<MinePluginConfigModel>(_dataRedisKey);
        }

        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            _redis.Subscribe(subChannel, handler);
        }
        public void Unsubscribe(string channel)
        {
            _redis.Unsubscribe(channel);
        }










        /// <summary>
        /// 从缓存中获取当前系统的配置信息
        /// </summary>
        /// <returns></returns>
        public SystemConfigSg GetSystemConfigModel()
        {
            try
            {
                var systemConfigStr = _redis.StringGet(_dataRedisKey);
                if (!string.IsNullOrEmpty(systemConfigStr))
                    return JsonConvert.DeserializeObject<SystemConfigSg>(systemConfigStr);
            }
            catch (Exception ex)
            {
                LogD.Error($"GetSystemConfigModel:{ex.ToString()}");
            }
            return null;
        }
    }
}
