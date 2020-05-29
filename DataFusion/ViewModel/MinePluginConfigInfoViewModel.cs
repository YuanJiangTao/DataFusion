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

        public MinePluginConfigModel MinePluginConfigModel { get; }


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

        }
        private void UnloadMinePlugin()
        {
            //TODO:卸载煤矿插件
        }



        private void Delete()
        {
            //TODO:删除煤矿插件

        }
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
