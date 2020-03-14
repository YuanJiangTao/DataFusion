using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces.Utils
{
    public static class CultureInfoHelper
    {
        public static void SetDateTimeFormat()
        {
            var cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
            if (cultureInfo == null) return;
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
