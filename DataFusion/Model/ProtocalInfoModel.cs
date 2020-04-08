using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;


namespace DataFusion.Model
{
    public class ProtocalInfoModel : ObservableObject
    {
        public ProtocalInfoModel()
        {

        }

        private string _protocalName;

        public string ProtocalName
        {
            get => _protocalName;
            set
            {
                _protocalName = value;
                RaisePropertyChanged();
            }
        }

        private string _protocalVersion;

        public string ProtocalVersion
        {
            get => _protocalVersion;
            set
            {
                _protocalVersion = value;
                RaisePropertyChanged();
            }
        }
        
        
    }
}
