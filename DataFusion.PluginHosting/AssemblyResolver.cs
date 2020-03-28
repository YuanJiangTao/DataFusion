using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataFusion.Interfaces;
using System.Reflection;

namespace DataFusion.PluginHosting
{
   public class AssemblyResolver
    {
        private string _thisAssemblyName;
        private string _interfacesAssemblyName;
        public void Setup()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            _thisAssemblyName = GetType().Assembly.GetName().Name;
            _interfacesAssemblyName = typeof(IPlugin).Assembly.GetName().Name;

        }
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            Console.WriteLine($"加载{name}");
            if (name.Name == _thisAssemblyName) return GetType().Assembly;
            if (name.Name == _interfacesAssemblyName) return typeof(IPlugin).Assembly;

            var dll = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), name.Name + ".dll");

            if (File.Exists(dll))
            {
                Console.WriteLine("加载dll:" + dll);
                return Assembly.LoadFile(dll);
            }

            Console.WriteLine($"无法加载{name}");
            return null;
        }
    }
}
