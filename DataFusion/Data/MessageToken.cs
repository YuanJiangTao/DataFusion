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

        public static readonly string UnloadEntry = nameof(UnloadEntry);
        public static readonly string LoadEntry = nameof(LoadEntry);
        public static readonly string AddMenuItem = nameof(AddMenuItem);
        public static readonly string RemoveItem = nameof(RemoveItem);

        public static readonly string DeleteProtocal = nameof(DeleteProtocal);
        public static readonly string AddProtocal = nameof(AddProtocal);






        /*-------------------- RedisKey --------------------*/

        public static readonly string AllClientInfo = nameof(AllClientInfo);
    }
}
