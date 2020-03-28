using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using DataFusion.Services;
using DataFusion.Utils;
using DataFusion.Data;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using DataFusion.Model;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using DataFusion.Interfaces.Utils;
using DataFusion.Interfaces;

namespace DataFusion.ViewModel
{
    public class PluginEntryController : ViewModelBase
    {
        private IUnityContainer _unityContainer;
        private readonly TaskScheduler _scheduler;
        public PluginEntryController(IUnityContainer container)
        {
            //Messenger.Default.Register
            PluginEntries = new ObservableCollection<PluginEntrySg>();
            MineProtocalConfigInfos = new ObservableCollection<MineProtocalConfigInfo>();
            LoadPluginEntryVms = new ObservableCollection<PluginEntryViewModel>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve += CurrentDomain_TypeResolve;
            _unityContainer = container;
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            try
            {
                ScanPluginEntries();
                ReadMineProtocalConfigInfos();
            }
            catch (Exception ex)
            {
                LogD.Error("扫描插件错误:" + ex);
            }
        }

        private Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            LogD.Info($"类型{args.Name}动态生成失败...");
            return null;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
            var matchPattern = @"^\w*Plugin\z";
            if (Regex.IsMatch(args.Name, matchPattern))
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "Plugins";
                return Assembly.LoadFrom(PathUtils.Combine(path, name.Name + ".dll"));
            }
            LogD.Info($"程序集{args.Name}加载失败...");
            return null;
        }

        public ObservableCollection<MineProtocalConfigInfo> MineProtocalConfigInfos { get; set; }

        public ObservableCollection<PluginEntrySg> PluginEntries { get; private set; }

        public ObservableCollection<PluginEntryViewModel> LoadPluginEntryVms { get; set; }

        private void ScanPluginEntries()
        {
            var pluginsFolder = PathUtils.Combine(Constant.PluginsFolder);
            if (!Directory.Exists(pluginsFolder))
            {
                Directory.CreateDirectory(pluginsFolder);
            }
            var pluginDirs = Directory.GetDirectories(pluginsFolder);
            foreach (var dir in pluginDirs)
            {
                var pluginDllFile = Directory.GetFiles(dir, "*Plugin.dll").FirstOrDefault();
                if (pluginDllFile == null)
                    continue;
                var buildTime = GetLinkerTime(pluginDllFile);
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(pluginDllFile);
                var title = fvi.FileDescription;
                var product = fvi.ProductName;
                var productVersion = fvi.ProductVersion;
                var fileName = fvi.OriginalFilename;
                var company = fvi.CompanyName;
                var comments = fvi.Comments;
                var templateElement = GetFrameworkFromAssembly(Path.GetFileNameWithoutExtension(dir));

                var pluginEntrySg = new PluginEntrySg()
                {
                    Title = title,
                    Version = productVersion,
                    Description = comments,
                    AssemblyPath = pluginDllFile,
                    OriginalFilename = fileName,
                    Company = company,
                    ProductName = product,
                    BuildTime = buildTime,
                    TemplateElement = templateElement
                };
                PluginEntries.Add(pluginEntrySg);
            }
        }

        private void ReadMineProtocalConfigInfos()
        {
            var dataService = _unityContainer.Resolve<DataService>();
            var mineProtocalInfos = dataService.GetMineInfoModels();
            foreach (var item in mineProtocalInfos)
            {
                MineProtocalConfigInfos.Add(item);
                //var pluginEntrySg = PluginEntries.FirstOrDefault(p => p.Title == item.PluginTitle && p.Version == item.PluginVersion);
                //if (pluginEntrySg != null)
                //{
                //    AddAvaiablePluginEntry(pluginEntrySg, item);
                //}
            }
        }
        public async Task<IList<MenuViewModel>> LoadPluginEntiesAsync()
        {
            var menuItemList = new List<MenuViewModel>();
            var tasks = MineProtocalConfigInfos.Select(p => LoadPluginEntryAsync(p));
            var result= await Task.WhenAll(tasks);
            return result;
            //foreach (var item in MineProtocalConfigInfos)
            //{
            //    var subMenuItem = await LoadPluginEntryAsync(item);
            //    if (subMenuItem != null)
            //        menuItemList.Add(subMenuItem);

            //}
        }
        public async Task<MenuViewModel> LoadPluginEntryAsync(MineProtocalConfigInfo mineProtocalConfigInfo)
        {
            var pluginEntrySg = PluginEntries.FirstOrDefault(p => p.Title == mineProtocalConfigInfo.PluginTitle && p.Version == mineProtocalConfigInfo.PluginVersion);
            if (pluginEntrySg != null)
            {
                return await Task.Run(() => LoadPluginEntry(pluginEntrySg, mineProtocalConfigInfo)).ContinueWith(t =>
                {
                    var pluginEntry = t.Result;
                    pluginEntry.CreateView();
                    AddAvaiablePluginEntry(pluginEntrySg, mineProtocalConfigInfo);
                    return new MenuViewModel() { Header = mineProtocalConfigInfo.MineName, Screen = pluginEntry.View };
                }, _scheduler);
            }
            return null;
        }
        public PluginEntry LoadPluginEntry(PluginEntrySg pluginEntrySg, MineProtocalConfigInfo mineProtocalConfigInfo)
        {
            var pluginEntry = _unityContainer.Resolve<PluginEntry>();
            pluginEntry.Error += PluginEntry_Error;
            try
            {
                pluginEntry.Load(pluginEntrySg, mineProtocalConfigInfo);
            }
            catch (Exception ex)
            {
                DisposePlugin(pluginEntry);
            }
            return pluginEntry;
        }
        private void DisposePlugin(PluginEntry pluginEntry)
        {
            if (pluginEntry == null) return;
            pluginEntry.Error -= PluginEntry_Error;

            try
            {
                pluginEntry.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        private void PluginEntry_Error(object sender, PluginErrorEventArgs e)
        {
            var task = new Task(() => PluginErrorHandler(e));
            task.Start(_scheduler);
        }
        private void PluginErrorHandler(PluginErrorEventArgs args)
        {

        }
        public void AddAvaiablePluginEntry(PluginEntrySg pluginEntrySg, MineProtocalConfigInfo mineProtocalConfigInfo)
        {
            var pluginEntryVm = new PluginEntryViewModel(pluginEntrySg, mineProtocalConfigInfo);
            LoadPluginEntryVms.Add(pluginEntryVm);
        }

        private void ScanProtocals()
        {
            var protocalFolder = PathUtils.Combine(Constant.ProtocalFolder);
            if (!Directory.Exists(protocalFolder))
            {
                Directory.CreateDirectory(protocalFolder);
            }
            var protocalDir = Directory.GetDirectories(protocalFolder);
        }

        private FrameworkElement GetFrameworkFromAssembly(string assemblyName)
        {
            try
            {
                Uri uri = new Uri($"pack://application:,,,/{assemblyName};component/Resources/TemplateView.xaml", UriKind.Absolute);
                Stream stream = Application.GetResourceStream(uri).Stream;
                FrameworkElement obj = XamlReader.Load(stream) as FrameworkElement;
                return obj;
            }
            catch (Exception ex)
            {
                LogD.Error($"GetFrameworkFromAssembly:{ex.ToString()}");
            }
            return null;

        }
        private DateTime GetLinkerTime(string filePath, TimeZoneInfo target = null)
        {
            return File.GetLastWriteTime(filePath);
        }

    }
}
