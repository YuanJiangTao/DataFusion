using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    [Serializable]
    public class PluginStartupInfo
    {
        public PluginStartupInfo()
        {

        }
        public string AssemblyName { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string AssemblyPath { get; set; }
        public string OriginalFilename { get; set; }
        public int Bits { get; set; } = 32;
        public string Parameters { get; set; }
        public bool IsEnable { get; set; }
        public string Company { get; set; }
        public string ProductName { get; set; }
        public bool IsDebug { get; set; }
    }
}
