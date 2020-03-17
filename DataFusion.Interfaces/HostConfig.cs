using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    public class HostConfig : MarshalByRefObject, IHostConfig
    {
        public RedisConfig RedisConfig { get; set; }

        public ProtocalConfig ProtocalConfig { get; set; }
    }


    public class RedisConfig : MarshalByRefObject
    {
        public string ServerIp { get; set; }

        public int ServerPort { get; set; }

        public override string ToString() => $"ServerIp:{ServerIp}\tServerPort:{ServerPort}";
    }

    public class ProtocalConfig : MarshalByRefObject
    {
        public string MineName { get; set; }

        public string MineCode { get; set; }

        public DateTime BuildTime { get; set; }

        public PluginType PluginType { get; set; }

        public bool IsEnableSafetyMonitorProtocal { get; set; }

        public bool IsEnableEpipemonitorProtocal { get; set; }

        public bool IsEnableVideoMonitorProtocal { get; set; }

        public override string ToString()
        {
            return $"IsEnableSafetyMonitorProtocal:{IsEnableSafetyMonitorProtocal}\t" +
                $"IsEnableEpipemonitorProtocal:{IsEnableEpipemonitorProtocal}\t" +
                $"IsEnableVideoMonitorProtocal:{IsEnableVideoMonitorProtocal}";
        }
    }
    public enum PluginType
    {
        FileUc=0x01,
        ApiUc=0x02
    }

}
