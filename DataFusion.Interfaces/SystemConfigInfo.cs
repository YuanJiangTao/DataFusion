using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    [Serializable]
    public class SystemConfigInfo
    {
        public SystemConfigInfo()
        {

        }
        public string RedisServer { get; set; }
        public string RedisPwd { get; set; }
    }
}
