using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataFusion.Interfaces;
using DataFusion.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Data;
using System.Windows;
using DataFusion.Views;
using MahApps.Metro.Controls.Dialogs;
using DataFusion.ViewModel.Storages;

namespace DataFusion.ViewModel
{
    public class PluginEntryViewModel : ViewModelBase
    {
        public PluginEntryViewModel()
        {
        }
        public PluginEntryViewModel(PluginEntrySg pluginEntrySg)
        {
            PluginEntrySg = pluginEntrySg;
            LoadCommand = new Lazy<RelayCommand>(() => new RelayCommand(Load)).Value;
            UninstallCommand = new Lazy<RelayCommand>(() => new RelayCommand(UninstallPlugin)).Value;
        }


        private async void UninstallPlugin()
        {
            var result = await MetroDialog.StaticShowMessageAsync("提示", "正在卸载当前模块.卸载后无法恢复, 请确定!", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);

            if (result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Negative)
                return;
            try
            {
            }
            catch (Exception e)
            {

            }
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void Load()
        {
            var vm = await MetroDialog.ShowCustomDialog<PluginAddViewModel, PluginAddUserControl>("新增插件", action => new PluginAddViewModel(action));
            if (vm.DialogResult)
            {
                var mineConfigInfo = new MinePluginConfigModel()
                {
                    Id = Guid.NewGuid(),
                    MineCode = vm.MineCode,
                    MineName = vm.MineName,
                    CreatTime = DateTime.Now,
                    IsEnable = true,
                    PluginEntrySg = PluginEntrySg
                };
                Messenger.Default.Send<MinePluginConfigModel>(mineConfigInfo, MessageToken.LoadMinePlugin);
            }
        }
        private void Unload()
        {
            Messenger.Default.Send<PluginEntryViewModel>(this, MessageToken.UnloadMinePlugin);
        }

        public PluginEntrySg PluginEntrySg { get; }


        public string PluginTitle => PluginEntrySg.Title;
        public string PluginDescription => PluginEntrySg.Description;
        public string PluginVersion => PluginEntrySg.Version;
        public int Bits => PluginEntrySg.Bits;

        public bool IsDebug => PluginEntrySg.IsDebug;


        public ICommand LoadCommand { get; set; }
        public ICommand UninstallCommand { get; set; }




    }
}
