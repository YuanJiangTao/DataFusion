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
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using DataFusion.ViewModel.Storages;

namespace DataFusion.ViewModel
{
    public class PluginEntryController : ViewModelBase
    {
        private IUnityContainer _unityContainer;
        private readonly TaskScheduler _scheduler;
        public PluginEntryController(IUnityContainer container)
        {
            MenuItems = new ObservableCollection<HamburgerMenuIconItem>();
            //MenuItems.Add(new HamburgerMenuIconItem()
            //{
            //    Label = "插件状态",
            //    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Home },
            //    Tag = new Views.PluginStateDisplayView()
            //});

            Messenger.Default.Register<PluginEntryViewModel>(this, MessageToken.UnloadMinePlugin, UnloadMinePlguin);
            Messenger.Default.Register<MinePluginConfigModel>(this, MessageToken.LoadMinePlugin, LoadMinePlugin);
            Messenger.Default.Register<MinePluginConfigInfoViewModel>(this, MessageToken.DeleteMinePlugin, DeleteMinePlugin);


            PluginEntries = new ObservableCollection<PluginEntrySg>();
            MineProtocalConfigInfos = new ObservableCollection<MinePluginConfigModel>();
            LoadPluginEntryVms = new ObservableCollection<PluginEntryViewModel>();
            ProtocalInfoModels = new ObservableCollection<ProtocalInfoModel>();
            MinePluginConfigInfoViewModels = new ObservableCollection<MinePluginConfigInfoViewModel>();

            _unityContainer = container;
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            try
            {
#if DEBUG
                Test();
#endif

                ScanPluginEntries();

            }
            catch (Exception ex)
            {
                LogD.Error("扫描插件错误:" + ex);
            }
        }


        /// <summary>
        /// 删除已经定义的煤矿插件信息，同时要卸载还在运行的插件
        /// </summary>
        /// <param name="minePluginConfigInfoVm"></param>
        private void DeleteMinePlugin(MinePluginConfigInfoViewModel minePluginConfigInfoVm)
        {

        }


        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="pluginEntryViewModel"></param>
        private void UnloadMinePlguin(PluginEntryViewModel pluginEntryViewModel)
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
        /// <summary>
        /// 添加煤矿插件信息，同时运行插件
        /// </summary>
        /// <param name="mineProtocalConfigInfo"></param>
        private async void LoadMinePlugin(MinePluginConfigModel mineProtocalConfigInfo)
        {
            var menuItem = await LoadPluginEntryAsync(mineProtocalConfigInfo);
            if (menuItem != null)
            {
                MenuItems.Add(menuItem);
            }
        }


        private void Test()
        {
            MinePluginConfigModel configInfo = new MinePluginConfigModel()
            {
                MineName = "车集矿",
                MineCode = "0123456789",
                IsEnable = true
            };
            MineProtocalConfigInfos.Add(configInfo);
        }



        public ObservableCollection<HamburgerMenuIconItem> MenuItems { get; set; }

        public ObservableCollection<MinePluginConfigModel> MineProtocalConfigInfos { get; private set; }

        public ObservableCollection<PluginEntrySg> PluginEntries { get; private set; }

        public ObservableCollection<PluginEntryViewModel> LoadPluginEntryVms { get; set; }

        public ObservableCollection<MinePluginConfigInfoViewModel> MinePluginConfigInfoViewModels { get; set; }


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
                };
                PluginEntries.Add(pluginEntrySg);
                AddAvaiablePluginEntry(pluginEntrySg);
            }
        }


        private void AddAvaiablePluginEntry(PluginEntrySg o)
        {
            var m = new PluginEntryViewModel(o);
            LoadPluginEntryVms.Add(m);
        }


      
        public async Task<HamburgerMenuIconItem> LoadPluginEntryAsync(MinePluginConfigModel mineProtocalConfigInfo)
        {
            var pluginEntrySg = PluginEntries.FirstOrDefault(p => p.Title == mineProtocalConfigInfo.PluginEntrySg.Title && p.Version == mineProtocalConfigInfo.PluginEntrySg.Version);
            if (pluginEntrySg != null)
            {
                return await Task.Run(() => LoadPluginEntry(pluginEntrySg, mineProtocalConfigInfo)).ContinueWith(t =>
                {
                    var pluginEntry = t.Result;
                    pluginEntry.CreateView();
                    //AddAvaiablePluginEntry(pluginEntrySg, mineProtocalConfigInfo);
                    return new HamburgerMenuIconItem()
                    {
                        Label = mineProtocalConfigInfo.MineName,
                        Tag = pluginEntry.View,
                        Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Home }
                    };
                }, _scheduler);
            }
            return null;
        }
        public PluginEntry LoadPluginEntry(PluginEntrySg pluginEntrySg, MinePluginConfigModel mineProtocalConfigInfo)
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

        private DateTime GetLinkerTime(string filePath, TimeZoneInfo target = null)
        {
            return File.GetLastWriteTime(filePath);
        }

        public override void Cleanup()
        {
            try
            {
                base.Cleanup();

            }
            catch (Exception)
            {

            }
        }


        #region --弃用--


        public ObservableCollection<ProtocalInfoModel> ProtocalInfoModels { get; set; }


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
        #endregion


    }
}
