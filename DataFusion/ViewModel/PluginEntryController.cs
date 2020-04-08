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
            Messenger.Default.Register<PluginEntryViewModel>(this, MessageToken.UnloadEntry, Unload);
            Messenger.Default.Register<PluginEntryViewModel>(this, MessageToken.LoadEntry, Load);
            Messenger.Default.Register<PluginEntryViewModel>(this, MessageToken.DeleteProtocal, Delete);
            Messenger.Default.Register<MineProtocalConfigInfo>(this, MessageToken.AddProtocal, Load);
            

            PluginEntries = new ObservableCollection<PluginEntrySg>();
            MineProtocalConfigInfos = new ObservableCollection<MineProtocalConfigInfo>();
            LoadPluginEntryVms = new ObservableCollection<PluginEntryViewModel>();
            ProtocalInfoModels = new ObservableCollection<ProtocalInfoModel>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve += CurrentDomain_TypeResolve;
            _unityContainer = container;
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            try
            {
#if DEBUG
                Test();
#endif

                ReadMineProtocalInfos();
                ScanPluginEntries();
                ReadMineProtocalConfigInfos();

            }
            catch (Exception ex)
            {
                LogD.Error("扫描插件错误:" + ex);
            }
        }

        private void Delete(PluginEntryViewModel model)
        {
            Unload(model);
            var protocalConfigInfo = MineProtocalConfigInfos.FirstOrDefault(p => p.MineName == model.MineName && p.MineCode == model.MineCode);
            if (protocalConfigInfo != null)
                MineProtocalConfigInfos.Remove(protocalConfigInfo);
        }

        private void Load(PluginEntryViewModel pluginEditViewModel)
        {
            try
            {
                if (pluginEditViewModel != null && pluginEditViewModel.MineProtocalConfigInfo != null)
                {
                    Load(pluginEditViewModel.MineProtocalConfigInfo);
                }
            }
            catch (Exception ex)
            {
                LogD.Error("加载插件错误:" + ex);
            }

        }

        private void Unload(PluginEntryViewModel pluginEntryViewModel)
        {
            try
            {
                //TODO:卸载插件
                //Messenger.Default.Send<string>()
            }
            catch (Exception ex)
            {
                LogD.Error("卸载插件错误:" + ex);
            }
        }
        private async void Load(MineProtocalConfigInfo mineProtocalConfigInfo)
        {
            var menuItem = await LoadPluginEntryAsync(mineProtocalConfigInfo);
            Messenger.Default.Send<MenuViewModel>(menuItem, MessageToken.AddMenuItem);
        }


        private void Test()
        {
            MineProtocalConfigInfo configInfo = new MineProtocalConfigInfo()
            {
                MineName = "车集矿",
                MineCode = "0123456789",
                IsEnableEpipemonitorProtocal = false,
                IsEnableSafetyMonitorProtocal = false,
                IsEnableVideoMonitorProtocal = false,
                EpipemonitorRunState = 1,
                SafetyMonitorRunState = 1,
                VideoMonitorRunState = 1,
                State = 1
            };
            MineProtocalConfigInfos.Add(configInfo);
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

        public ObservableCollection<MineProtocalConfigInfo> MineProtocalConfigInfos { get; private set; }

        public ObservableCollection<PluginEntrySg> PluginEntries { get; private set; }

        public ObservableCollection<PluginEntryViewModel> LoadPluginEntryVms { get; set; }

        public ObservableCollection<ProtocalInfoModel> ProtocalInfoModels { get; set; }

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
        private void ReadMineProtocalInfos()
        {
            var protocalsFolder = PathUtils.Combine(Constant.ProtocalFolder);
            if (!Directory.Exists(protocalsFolder))
            {
                Directory.CreateDirectory(protocalsFolder);
            }
            var prtocalDirs = Directory.GetDirectories(protocalsFolder);
            foreach (var dir in prtocalDirs)
            {
                var protocalDllFile = Directory.GetFiles(dir, "*Protocal.dll").FirstOrDefault();
                if (protocalDllFile == null)
                    continue;
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(protocalDllFile);

                ProtocalInfoModels.Add(new ProtocalInfoModel()
                {
                    ProtocalName = fvi.ProductName,
                    ProtocalVersion = fvi.FileVersion
                });
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
            var result = await Task.WhenAll(tasks);
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
            var pluginEntryVm = new PluginEntryViewModel(pluginEntrySg, mineProtocalConfigInfo, false);
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
