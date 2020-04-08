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

        /// <summary>
        /// 1-正在运行；0-暂停
        /// </summary>
        public int SafetyMonitorRunState { get; set; }

        public bool IsEnableEpipemonitorProtocal { get; set; }

        public int EpipemonitorRunState { get; set; }

        public bool IsEnableVideoMonitorProtocal { get; set; }

        public int VideoMonitorRunState { get; set; }

        public int State { get; set; }

        public string PluginTitle { get; set; }

        public string PluginVersion { get; set; }

        /// <summary>
        /// 用来标明该煤矿插件是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;

    }
}
