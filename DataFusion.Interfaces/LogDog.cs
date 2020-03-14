using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    public class LogDog : ILogDog
    {
        private ILog _log;

        public LogDog(ILog log)
        {
            _log = log;
        }

        public void Info(string msg)
        {
            _log.Info(msg);
        }
        public void Debug(string msg)
        {
            _log.Debug(msg);
        }
        public void Warn(string msg)
        {
            _log.Warn(msg);
        }
        public void Error(string msg)
        {
            _log.Error(msg);
        }
        public void Fatal(string msg)
        {
            _log.Fatal(msg);
        }


        public void Info(string msg, Exception ex)
        {
            _log.Info(msg, ex);
        }
        public void Debug(string msg, Exception ex)
        {
            _log.Debug(msg, ex);
        }
        public void Warn(string msg, Exception ex)
        {
            _log.Warn(msg, ex);
        }
        public void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }
        public void Fatal(string msg, Exception ex)
        {
            _log.Fatal(msg, ex);
        }

        public void Dispose()
        {
            var appenders = _log.Logger.Repository.GetAppenders();
            _log.Logger.Repository.Shutdown();
            _log = null;
        }
    }
}
