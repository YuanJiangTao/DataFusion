using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataFusion.Interfaces
{
    public class MinePluginConfig : MarshalByRefObject
    {
        public MinePluginConfig()
        {

        }
        public Guid Id { get; set; }
        public string Key { get; set; }

        public string MineName { get; set; }

        public string MineCode { get; set; }

        public DateTime CreatTime { get; set; }


    }
}
