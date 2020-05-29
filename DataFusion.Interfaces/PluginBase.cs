using DataFusion.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DataFusion.Interfaces
{
    public abstract class PluginBase : MarshalByRefObject, IPlugin
    {
        protected ILogDogCollar LogDogCollar;
        protected ILogDog LogDogRoot;
        protected IPluginHost PluginHost;
        protected IHostConfig HostConfig;
        public string BaseDirectory;
        private Thread _loadThread;
        public PluginBase(IPluginHost host)
        {
            CultureInfoHelper.SetDateTimeFormat();
            PluginHost = host;
            HostConfig = PluginHost.GetService<IHostConfig>();

            LogDogCollar = new LogDogCollar();
            LogDogRoot = RegisterLogDog(Title);
            LogDogRoot.Info(HostConfig.ToString());
            BaseDirectory = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
        }
        protected ILogDog RegisterLogDog(string logName, string level = "ALL")
        {
            LogDogCollar.Setup(Title, logName, level);
            return LogDogCollar.GetLogger();
        }

        public virtual void Dispose()
        {
            try
            {
                _loadThread.Join(3000);
                _loadThread.Abort();
                _loadThread.DisableComObjectEagerCleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public virtual object GetService(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(GetType())) return this;
            return null;
        }

        public void Load()
        {
            Console.WriteLine("loaded..");
            try
            {
                _loadThread = new Thread(() =>
                {
                    Onload();
                });
                _loadThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"插件{Title}加载失败!!!");
                Console.WriteLine(e);

#if DEBUG
                Console.WriteLine("请输入回车退出.");
                Console.ReadLine();
#endif
                throw;
            }
        }
        public override object InitializeLifetimeService()
        {
            return null;
        }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract FrameworkElement CreateControl();
        protected abstract void Onload();
    }
}
