using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    public class SystemConfig : MarshalByRefObject
    {
        public string RedisServer { get; set; }

        public string RedisPwd { get; set; }
        public override string ToString()
        {
            return $"RedisServe:{RedisServer}\tRedisPwd:{RedisPwd}";
        }
    }
}
