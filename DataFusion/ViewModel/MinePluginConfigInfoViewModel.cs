using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using DataFusion.Interfaces;
using System.Runtime.InteropServices;
using DataFusion.Model;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Data;

namespace DataFusion.ViewModel
{
    public class MinePluginConfigInfoViewModel : ViewModelBase
    {
        public MinePluginConfigInfoViewModel(MinePluginConfigModel configInfo)
        {
            MinePluginConfigModel = configInfo;
            DeleteCommand = new Lazy<RelayCommand>(() => new RelayCommand(Delete)).Value;
        }

        public ICommand DeleteCommand { get; set; }

        public MinePluginConfigModel MinePluginConfigModel { get; set; }





        public string MineName
        {
            get => MinePluginConfigModel.MineName;
            set
            {
                MinePluginConfigModel.MineName = value;
                RaisePropertyChanged();
            }
        }
        public string MineCode
        {
            get => MinePluginConfigModel.MineCode;
            set
            {
                MinePluginConfigModel.MineCode = value;
                RaisePropertyChanged();
            }
        }
        public string PluginTitle
        {
            get => MinePluginConfigModel.Title;
            set
            {
                MinePluginConfigModel.Title = value;
                RaisePropertyChanged();
            }
        }
        public string PluginVersion
        {
            get => MinePluginConfigModel.Version;
            set
            {
                MinePluginConfigModel.Version = value;
                RaisePropertyChanged();
            }
        }
        public DateTime CreateTime
        {
            get => MinePluginConfigModel.CreatTime;
            set
            {
                MinePluginConfigModel.CreatTime = value;
                RaisePropertyChanged();
            }
        }


        public bool IsEnable
        {
            get => MinePluginConfigModel.IsEnable;
            set
            {
                MinePluginConfigModel.IsEnable = value;
                if (MinePluginConfigModel.IsEnable)
                {
                    LoadMinePlugin();
                }
                else
                {
                    UnloadMinePlugin();
                }
            }
        }
        private void LoadMinePlugin()
        {
            //TODO:加载煤矿插件
            Messenger.Default.Send<MinePluginConfigModel>(MinePluginConfigModel, MessageToken.ReloadMinePlugin);

        }
        private void UnloadMinePlugin()
        {
            //TODO:卸载煤矿插件
            Messenger.Default.Send<MinePluginConfigModel>(MinePluginConfigModel, MessageToken.UnloadMinePlugin);
        }



        private async void Delete()
        {
            var result = await MetroDialog.StaticShowMessageAsync("提示", "确定要删除该插件吗?", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);

            if (result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Negative)
                return;
            Messenger.Default.Send<MinePluginConfigInfoViewModel>(this, MessageToken.DeleteMinePlugin);

        }
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
