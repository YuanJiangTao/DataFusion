using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
namespace DataFusion.Model
{
    public class SystemConfigModel : ObservableObject
    {
        public SystemConfigModel()
        {

        }
        private string _redisServer;

        public string RedisServer
        {
            get => _redisServer;
            set => Set(ref _redisServer, value);
        }
        private string _redisPwd;
        public string RedisPwd
        {
            get => _redisPwd;
            set => Set(ref _redisPwd, value);
        }
    }
}
