using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFusion.Model;
using DataFusion.Services;
using GalaSoft.MvvmLight;
using Unity;
using StackExchange.Redis;
using System.Collections.ObjectModel;
using DataFusion.Interfaces;
using Newtonsoft.Json;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Data;

namespace DataFusion.ViewModel
{
    public class MineProtocalEnableConfigViewModel : ViewModelBase
    {
        public MinePluginConfigModel MinePluginConfigModel { get; }
        private readonly DataService _service;
        public MineProtocalEnableConfigViewModel(MinePluginConfigModel minePluginConfigModel, DataService dataService)
        {
            ProtocalEnableConfigs = new ObservableCollection<ProtocalEnableConfig>();
            ProtocalEnableConfigs.CollectionChanged += ProtocalEnableConfigs_CollectionChanged;
            MinePluginConfigModel = minePluginConfigModel;
            _service = dataService;
            _service.Subscribe(minePluginConfigModel.Id.ToString(), UpdateState);
            IsRunning = ProtocalEnableConfigs.Any(p => p.IsEnable);

        }

        private void ProtocalEnableConfigs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        public ObservableCollection<ProtocalEnableConfig> ProtocalEnableConfigs { get; set; }


        public string MineName
        {
            get => MinePluginConfigModel.MineName;
        }

        public Guid Id
        {
            get => MinePluginConfigModel.Id;
        }
        private void UpdateState(RedisChannel channel, RedisValue value)
        {
            try
            {
                if (!channel.IsNullOrEmpty)
                {
                    if (channel == Id.ToString())
                    {
                        var protocalConfig = JsonConvert.DeserializeObject<ProtocalEnableConfig>(value);
                        if (protocalConfig != null)
                        {
                            var isExist = ProtocalEnableConfigs.Any<ProtocalEnableConfig>(p => p.ProtocalName == protocalConfig.ProtocalName);
                            if (isExist)
                            {
                                for (var i = 0; i < ProtocalEnableConfigs.Count; i++)
                                {
                                    if (ProtocalEnableConfigs[i].ProtocalName == protocalConfig.ProtocalName)
                                    {
                                        ProtocalEnableConfigs[i].IsEnable = protocalConfig.IsEnable;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ProtocalEnableConfigs.Add(protocalConfig);
                            }
                        }
                    }
                }
                IsRunning = ProtocalEnableConfigs.Any(p => p.IsEnable);
                Messenger.Default.Send<string>("", MessageToken.ProtocalStateChanged);
            }
            catch
            {

            }

        }
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                RaisePropertyChanged();
            }
        }

        public override void Cleanup()
        {
            try
            {
                base.Cleanup();
                _service.Unsubscribe(MinePluginConfigModel.Id.ToString());
                ProtocalEnableConfigs.CollectionChanged -= ProtocalEnableConfigs_CollectionChanged;
            }
            catch
            {
            }
        }



    }
}
