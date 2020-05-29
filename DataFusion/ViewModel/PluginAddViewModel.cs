using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataFusion.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DataFusion.ViewModel
{
    [MetadataType(typeof(PluginAddViewModel))]
    public class PluginAddViewModel : ViewModelBase, IDataErrorInfo
    {

        public PluginAddViewModel(Action<object> closeHandler)
        {
            CloseCommand = new RelayCommand(() =>
            {
                DialogResult = true;
                closeHandler(this);
            }, CanSave);

            CancelCommand = new RelayCommand(() =>
            {
                DialogResult = false;
                closeHandler(this);
            });
        }

        private string _mineName { get; set; }
        [Required(ErrorMessage = "煤矿名称不能为空")]
        public string MineName
        {
            get => _mineName;
            set
            {
                _mineName = value;
                RaisePropertyChanged();
            }
        }
        private string _mineCode;
        [Required(ErrorMessage = "煤矿编号不能为空")]
        public string MineCode
        {
            get => _mineCode;
            set
            {
                _mineCode = value;
                RaisePropertyChanged();
            }
        }

        private bool CanSave()
        {
            return dataErrors.Count != 0;
        }

        public ICommand CloseCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public bool DialogResult { get; set; } = false;

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
