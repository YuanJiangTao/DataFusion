using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Model
{
    public class ClientRunInfo
    {
        public string MacAddress { get; set; }

        public List<string> IpAddressList { get; set; }
        /// <summary>
        /// 0-停止；1-运行
        /// </summary>
        public int State { get; set; }
    }
}
