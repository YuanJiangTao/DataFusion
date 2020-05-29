using DataFusion.Interfaces;
using DataFusion.ViewModel.Storages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Model
{
   public class MinePluginConfigModel
    {
        public MinePluginConfigModel()
        {

        }
        public Guid Id { get; set; }
        [JsonIgnore]
        public string Key { get => $"{MineName}:{MineCode}"; }

        public string MineName { get; set; }

        public string MineCode { get; set; }

        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 用来标明该煤矿插件是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;

        public PluginEntrySg PluginEntrySg { get; set; }

        public MinePluginConfig ToMinePluginConfig()
        {
            return new MinePluginConfig()
            {
                Id = Id,
                Key = Key,
                MineName = MineName,
                MineCode = MineCode
            };
        }
    }
}
