﻿using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;

namespace DataFusion.PluginHosting
{
    public static class PluginCreator
    {
        public static object CreatePlugin(string assemblyName, Type type, IPluginHost host)
        {
            var assembly = Assembly.Load(assemblyName);
            if (type == null) throw new InvalidOperationException("Could not find type " + type.Name + " in assembly " + assemblyName);
            var pluginType = assembly.GetTypes().Where(o => type.IsAssignableFrom(o)).FirstOrDefault();
            if (pluginType == null)
                throw new Exception($"the AssignableFrom of {type.Name} is null...");

            SetupWpfApplication(assembly);
            var hostConstructor = pluginType.GetConstructor(new[] { typeof(IPluginHost) });
            if (hostConstructor != null)
            {
                return hostConstructor.Invoke(new object[] { host });
            }

            var defaultConstructor = pluginType.GetConstructor(new Type[0]);
            if (defaultConstructor == null)
            {
                var message = String.Format("Cannot create an instance of {0}. Either a public default constructor, or a public constructor taking IWpfHost must be defined", pluginType.Name);
                throw new InvalidOperationException(message);
            }

            return defaultConstructor.Invoke(null);

        }
        private static void SetupWpfApplication(Assembly assembly)
        {
            var application = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            Application.ResourceAssembly = assembly;
        }
    }
}
