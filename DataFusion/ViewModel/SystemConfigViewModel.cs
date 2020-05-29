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
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace DataFusion.ViewModel
{
    public class SystemConfigViewModel : ViewModelBase
    {
        private XmlStorage<SystemConfigSg> _systemConfigSgStorage;
        private MetroDialog _dialog;
        public SystemConfigViewModel(MetroDialog dialog)
        {
            var configPath = PathUtils.Combine(Constant.ConfigFolder, Constant.SystemConfig);
            _systemConfigSgStorage = new XmlStorage<SystemConfigSg>(configPath);
            _systemConfigSgStorage.Load();
            SystemConfigModel = _systemConfigSgStorage.Storage;
            SaveCommand = new RelayCommand(Save);
            _dialog = dialog;
        }
        public SystemConfigSg SystemConfigModel { get; }

        public string RedisServer
        {
            get => SystemConfigModel.RedisServer;
            set
            {
                SystemConfigModel.RedisServer = value;
                RaisePropertyChanged();
            }
        }
        public string RedisPwd
        {
            get => SystemConfigModel.RedisPwd;
            set
            {
                SystemConfigModel.RedisPwd = value;
                RaisePropertyChanged();
            }
        }

        private async void Save()
        {
            if (SystemConfigModel == null || !SystemConfigModel.IsValid)
            {
                return;
            }
            _systemConfigSgStorage.Save();
            await _dialog.ShowTipsAsync("保存成功!");
        }

        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);

        }

        public ICommand SaveCommand { get; set; }

    }
}
