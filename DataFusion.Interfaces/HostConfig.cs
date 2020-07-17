using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataFusion.Interfaces
{
    public class HostConfig : MarshalByRefObject, IHostConfig
    {
        public SystemConfig SystemConfig { get; set; }

        public MinePluginConfig MinePluginConfig { get; set; }
    }
    public class ProtocalEnableConfig : INotifyPropertyChanged
    {
        private string _protocalName;

        public string ProtocalName
        {
            get => _protocalName;
            set
            {
                if (Equals(_protocalName, value)) return;
                _protocalName = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (Equals(_isEnable, value)) return;
                _isEnable = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
