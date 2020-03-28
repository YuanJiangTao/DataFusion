using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
namespace DataFusion.Model
{
    [Serializable]
    public class SystemConfigSg:ObservableObject
    {
        public SystemConfigSg()
        {


        }
        private string _redisServer = "127.0.0.1";
        private string _redisPwd = "gl";

        public string RedisServer
        {
            get => _redisServer;
            set
            {
                Set(ref _redisServer, value);
            }
        }
        public string RedisPwd
        {
            get => _redisPwd;
            set
            {
                Set(ref _redisPwd, value);
            }
        }
    }
}
