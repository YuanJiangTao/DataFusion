using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Data
{
    public class MessageToken
    {
        /*-------------------- Messages --------------------*/

        public static readonly string LoadShowContent = nameof(LoadShowContent);

        public static readonly string AddMenuItem = nameof(AddMenuItem);
        public static readonly string RemoveItem = nameof(RemoveItem);

        public static readonly string DeleteProtocal = nameof(DeleteProtocal);
        public static readonly string AddProtocal = nameof(AddProtocal);

        public static readonly string ShowMessageInfo = nameof(ShowMessageInfo);




        public static readonly string DeleteMinePlugin = nameof(DeleteMinePlugin);
        public static readonly string LoadMinePlugin = nameof(LoadMinePlugin);
        public static readonly string UnloadMinePlugin = nameof(UnloadMinePlugin);








        /*-------------------- RedisKey --------------------*/

        public static readonly string AllClientInfo = nameof(AllClientInfo);
    }
}
