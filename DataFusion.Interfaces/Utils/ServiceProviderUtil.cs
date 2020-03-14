using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces.Utils
{
    public static class ServiceProviderUtil
    {
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

    }
}
