using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using DataFusion.Model;
using DataFusion.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using DataFusion.Services;

namespace DataFusion.ViewModel
{
    [MetadataType(typeof(ProtocalAddWindowViewModel))]
    public class ProtocalAddWindowViewModel : ViewModelBase, IDataErrorInfo
    {

        private PluginEntryController _controller;

        private MineProtocalConfigInfo _configInfo;
        private MessageService _message;
        public ProtocalAddWindowViewModel(PluginEntryController controller, MessageService message)
        {
            _controller = controller;
            _configInfo = new MineProtocalConfigInfo();
            _message = message;
            _selectedProtocalInfoModels = new ObservableCollection<ProtocalInfoModel>();
        }


        public string WindowTitle => "新增";



        public ObservableCollection<ProtocalInfoModel> ProtocalInfoModels => _controller.ProtocalInfoModels;

        public ObservableCollection<PluginEntrySg> PluginEntries => _controller.PluginEntries;
        [Required(ErrorMessage = "煤矿名称不能为空")]
        public string MineName
        {
            get => _configInfo.MineName;
            set
            {
                _configInfo.MineName = value;
                RaisePropertyChanged();
            }
        }
        [Required(ErrorMessage = "煤矿编号不能为空")]
        public string MineCode
        {
            get => _configInfo.MineCode;
            set
            {
                _configInfo.MineCode = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnableSafetyMonitorProtocal
        {
            get => _configInfo.IsEnableSafetyMonitorProtocal;
            set
            {
                _configInfo.IsEnableSafetyMonitorProtocal = value;
                RaisePropertyChanged();
            }
        }
        public bool IsEnableEpipemonitorProtocal
        {
            get => _configInfo.IsEnableEpipemonitorProtocal;
            set
            {
                _configInfo.IsEnableEpipemonitorProtocal = value;
                RaisePropertyChanged();
            }
        }
        public bool IsEnableVideoMonitorProtocal
        {
            get => _configInfo.IsEnableVideoMonitorProtocal;
            set
            {
                _configInfo.IsEnableVideoMonitorProtocal = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<ProtocalInfoModel> _selectedProtocalInfoModels;
        public ObservableCollection<ProtocalInfoModel> SelectedProtocalInfoModels
        {
            get => _selectedProtocalInfoModels;
            set
            {
                _selectedProtocalInfoModels = value;
                RaisePropertyChanged();
            }
        }
        private PluginEntrySg _selectedPluginEntrySg;
        public RelayCommand<SelectionChangedEventArgs> SelectedCommand => new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
        new RelayCommand<SelectionChangedEventArgs>(p =>
  {
      if (p.Source is ListView)
      {
          var selectedItem = (p.Source as ListView).SelectedItem;
          if (selectedItem != null)
          {
              _selectedPluginEntrySg = selectedItem as PluginEntrySg;
          }
      }})).Value;


        public RelayCommand SaveCommand => new Lazy<RelayCommand>(() =>
          new RelayCommand(() =>
          {
              if (_selectedPluginEntrySg == null)
              {
                  _message.Warnging("必须选择插件模板");
                  return;
              }
              _configInfo.PluginTitle = _selectedPluginEntrySg.Title;
              _configInfo.PluginVersion = _selectedPluginEntrySg.Version;

          })).Value;

        public RelayCommand<SelectionChangedEventArgs> TransferCommand => new Lazy<RelayCommand<SelectionChangedEventArgs>>(
            () => new RelayCommand<SelectionChangedEventArgs>(
                e =>
              {
                  var transfer = e.OriginalSource as HandyControl.Controls.Transfer;
                  if (transfer != null && transfer.SelectedItems != null)
                  {
                      var selectedItems = transfer.SelectedItems.Cast<ProtocalInfoModel>().ToList();
                      foreach (var item in selectedItems)
                      {
                          if (item.ProtocalName == "")
                          {

                          }
                      }
                  }

              })).Value;



        #region --验证
        public string this[string columnName]
        {
            get
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), context, res);
                if (res.Count > 0)
                {
                    AddDic(dataErrors, context.MemberName);
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                }
                RemoveDic(dataErrors, context.MemberName);
                return null;
            }

        }
        private Dictionary<string, string> dataErrors = new Dictionary<string, string>();
        public string Error => null;

        #region 附属方法

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void RemoveDic(Dictionary<String, String> dics, String dicKey)
        {
            dics.Remove(dicKey);
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void AddDic(Dictionary<String, String> dics, String dicKey)
        {
            if (!dics.ContainsKey(dicKey)) dics.Add(dicKey, "");
        }
        #endregion

        #endregion 
    }
}
