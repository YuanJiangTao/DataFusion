using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataFusion.Model
{
    public class MineInfoModel
    {
        public MineInfoModel(string key, MineProtocalConfigModel configModel)
        {
            this.Key = key;
            this.ConfigModel = configModel;
        }


        [JsonIgnore]
        public string Name
        {
            get
            {
                var list = Key.Split(':');
                if (list != null && list.Length == 2)
                    return list[0];
                else
                    return string.Empty;
            }
        }
        [JsonIgnore]
        public string Code
        {
            get
            {
                var list = Key.Split(':');
                if (list != null && list.Length == 2)
                    return list[1];
                else
                    return string.Empty;
            }
        }
        public string Key { get; private set; }

        public MineProtocalConfigModel ConfigModel { get; set; }
    }
}
