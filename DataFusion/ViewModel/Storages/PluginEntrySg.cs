using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows;
using Newtonsoft.Json;

namespace DataFusion.ViewModel.Storages
{
    public class PluginEntrySg
    {

        public PluginEntrySg()
        {

        }
        public string Title { get; set; }

        public string Version { get; set; }
        public string Description { get; set; }

        public string AssemblyPath { get; set; }

        public string OriginalFilename { get; set; }

        public int Bits { get; set; }

        public string Parameters { get; set; }

        public bool IsEnable { get; set; }

        public string Company { get; set; }

        public string ProductName { get; set; }

        public bool IsDebug { get; set; }

        public DateTime BuildTime { get; set; }

        public bool Available { get; set; }

    }
}
