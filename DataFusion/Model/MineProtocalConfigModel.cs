using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace DataFusion.Model
{
    public class MineProtocalConfigModel : ObservableObject
    {
        public string Key { get; private set; }

        public string MineName { get; set; }

        public string MineCode { get; set; }

        public DateTime CreatTime { get; set; }

        public PluginType PluginType { get; set; }

        public bool IsEnableSafetyMonitorProtocal { get; set; }

        public int SafetyMonitorRunState { get; set; }

        public bool IsEnableEpipemonitorProtocal { get; set; }

        public int EpipemonitorRunState { get; set; }

        public bool IsEnableVideoMonitorProtocal { get; set; }

        public int VideoMonitorRunState { get; set; }

        public int State { get; set; }
    }

    public enum PluginType
    {
        FilePlugin = 0x01,
    }
}
