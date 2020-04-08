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
using HcMessageBox = HandyControl.Controls.MessageBox;
using HandyControl.Data;
using System.Windows;
using HandyControl.Controls;
using DataFusion.Views;
using HandyControl.Tools.Extension;

namespace DataFusion.ViewModel
{
    public class PluginEntryViewModel : ViewModelBase
    {
        private MineProtocalConfigInfo _configInfo;
        private PluginEntrySg _entrySg;

        public PluginEntryViewModel()
        {
        }
        private bool _isExample;
        public PluginEntryViewModel(bool isExample):this()
        {
            _isExample = isExample;
        }
        public PluginEntryViewModel(PluginEntrySg pluginEntrySg, MineProtocalConfigInfo mineProtocalConfigInfo,bool isExample):this(isExample)
        {
            _entrySg = pluginEntrySg;
            _configInfo = mineProtocalConfigInfo;
            UnloadCommand = new Lazy<RelayCommand>(() => new RelayCommand(Unload)).Value;
            LoadCommand = new Lazy<RelayCommand>(() => new RelayCommand(Load)).Value;
            DeleteCommand = new Lazy<RelayCommand>(() => new RelayCommand(Delete)).Value;
        }


        private async void Delete()
        {
            var dialogResult = HcMessageBox.Show(new MessageBoxInfo()
            {
                Message = "你确定要删除吗?",
                Caption = "提示",
                Button = MessageBoxButton.OKCancel,
                IconBrushKey = ResourceToken.AccentBrush,
                IconKey = ResourceToken.AskGeometry,
                StyleKey = "MessageBoxCustom"
            });
            if (dialogResult == MessageBoxResult.Cancel)
                return;
            else if(dialogResult== MessageBoxResult.OK)
            {
                var pwdResult = await Dialog.Show<PasswordDiaglogView>()
                .Initialize<PasswordDiaglogViewModel>(vm => vm.Message = "请输入密码")
                .GetResultAsync<string>();
                if (!string.IsNullOrEmpty(pwdResult))
                {
                    Messenger.Default.Send<PluginEntryViewModel>(this, MessageToken.DeleteProtocal);
                }
            }
        }


        private void Load()
        {
            Messenger.Default.Send<PluginEntryViewModel>(this, MessageToken.LoadEntry);
        }
        private void Unload()
        {
            Messenger.Default.Send<PluginEntryViewModel>(this, MessageToken.UnloadEntry);
        }

        public MineProtocalConfigInfo MineProtocalConfigInfo => _configInfo;
        public PluginEntrySg PluginEntrySg => _entrySg;

        public string MineName => _configInfo.MineName;
        public string MineCode => _configInfo.MineCode;

        public string PluginTitle => _entrySg.Title;
        public string PluginDescription => _entrySg.Description;
        public string PluginVersion => _entrySg.Version;
        public DateTime CreateTime => _configInfo.CreatTime;
        public int Bits => _entrySg.Bits;

        public bool IsDebug => _entrySg.IsDebug;

        public bool IsExample
        {
            get => _isExample;
            set
            {
                _isExample = value;
                RaisePropertyChanged();
            }
        }


        public bool IsEnable
        {
            get => _configInfo.IsEnable;
            set
            {
                _configInfo.IsEnable = value;
                RaisePropertyChanged();
                if(_configInfo.IsEnable)
                {
                    //启用
                    Load();
                }
                else
                {
                    //卸载
                    Unload();
                }
            }
        }


        public ICommand DeleteCommand { get; set; }

        public ICommand LoadCommand { get; set; }
        public ICommand UnloadCommand { get; set; }
        public ICommand UninstallCommand { get; set; }
        public ICommand InstallCommand { get; set; }



        


    }
}
