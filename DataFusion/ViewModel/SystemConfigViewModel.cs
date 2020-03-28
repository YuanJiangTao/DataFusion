using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using DataFusion.Services;
using DataFusion.Model;
using DataFusion.Interfaces.Utils;
using System.IO;
using DataFusion.Data;
using System.Runtime.CompilerServices;

namespace DataFusion.ViewModel
{
    public class SystemConfigViewModel : ViewModelBase
    {
        private SystemConfigSg _systemConfigModel;

        private XmlStorage<SystemConfigSg> _systemConfigSgStorage;
        public SystemConfigViewModel()
        {
            var configPath = PathUtils.Combine(Constant.ConfigFolder, Constant.SystemConfig);
            _systemConfigSgStorage = new XmlStorage<SystemConfigSg>(configPath);
            _systemConfigSgStorage.Load();
            _systemConfigModel = _systemConfigSgStorage.Storage;

        }

        



        public SystemConfigSg SystemConfigModel { get => _systemConfigModel; }

        public string RedisServer
        {
            get => _systemConfigModel.RedisServer;
            set
            {
                _systemConfigModel.RedisServer = value;
                RaisePropertyChanged();
            }
        }
        public string RedisPwd
        {
            get => _systemConfigModel.RedisPwd;
            set
            {
                _systemConfigModel.RedisPwd = value;
                RaisePropertyChanged();
            }
        }
        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            _systemConfigSgStorage.Save();
        }

    }
}
