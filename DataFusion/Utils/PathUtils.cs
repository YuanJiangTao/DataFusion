using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Utils
{
    public class PathUtils
    {
        public static string Combine(params string[] pathes)
        {
            var p = new[] { AppDomain.CurrentDomain.BaseDirectory };
            return Path.Combine(p.Union(pathes).ToArray());
        }
    }
}
