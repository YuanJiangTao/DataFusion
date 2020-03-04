using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Extensions
{
    public static class ObjectExtension
    {
        public static T Clone<T>(this T @this) where T : class
        {
            var json = JsonConvert.SerializeObject(@this);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
