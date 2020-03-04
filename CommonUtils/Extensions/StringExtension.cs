using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Extensions
{
    public static class StringExtension
    {
        public static string Repeat(this string @this, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(@this);
            }
            return sb.ToString();
        }
    }
}
