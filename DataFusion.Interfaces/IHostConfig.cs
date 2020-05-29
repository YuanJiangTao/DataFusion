using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    public interface IHostConfig
    {
        SystemConfig SystemConfig { get; set; }

        MinePluginConfig MinePluginConfig { get; set; }
    }
}
