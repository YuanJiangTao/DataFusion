using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataFusion.Interfaces
{
    [Serializable]
    public class MineProtocalConfigInfo
    {
        public MineProtocalConfigInfo()
        {

        }
        [JsonIgnore]
        public string Key { get => $"{MineName}:{MineCode}"; }

        public string MineName { get; set; }

        public string MineCode { get; set; }

        public DateTime CreatTime { get; set; }

        public bool IsEnableSafetyMonitorProtocal { get; set; }

        public int SafetyMonitorRunState { get; set; }

        public bool IsEnableEpipemonitorProtocal { get; set; }

        public int EpipemonitorRunState { get; set; }

        public bool IsEnableVideoMonitorProtocal { get; set; }

        public int VideoMonitorRunState { get; set; }

        public int State { get; set; }

        public string PluginTitle { get; set; }

        public string PluginVersion { get; set; }

    }
}
