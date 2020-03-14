using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion
{
    public static class LogD
    {
        private static ILogDog _logDog;
        public static void Initializer(ILogDog log)
        {
            _logDog = log;
        }

        public static void Info(string msg)
        {
            _logDog.Info(msg);
        }
        public static void Debug(string msg)
        {
            _logDog.Debug(msg);
        }
        public static void Warn(string msg)
        {
            _logDog.Warn(msg);
        }
        public static void Fatal(string msg)
        {
            _logDog.Fatal(msg);
        }
        public static void Error(string msg)
        {
            _logDog.Error(msg);
        }
    }
}
