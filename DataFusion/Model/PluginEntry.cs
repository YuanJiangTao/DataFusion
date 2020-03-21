using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows;

namespace DataFusion.Model
{
    public class PluginEntry : ObservableObject
    {

        public PluginEntry()
        {

        }
        private string _title;
        public string Title { get => _title; set { Set(ref _title, value); } }

        private string _version;
        public string Version { get => _version; set { Set(ref _version, value); } }

        private string _description;

        public string Description { get => _description; set { Set(ref _description, value); } }

        private string _assemblyPath;
        public string AssemblyPath { get => _assemblyPath; set { Set(ref _assemblyPath, value); } }

        private string _originalFilename;
        public string OriginalFilename { get => _originalFilename; set { Set(ref _originalFilename, value); } }

        private int _bits = 32;
        public int Bits { get => _bits; set { Set(ref _bits, value); } }

        private string _parameters;
        public string Parameters { get => _parameters; set { Set(ref _parameters, value); } }

        private bool _isEnable;

        public bool IsEnable { get=> _isEnable; set { Set(ref _isEnable, value); } }

        private string _company;
        public string Company { get=>_company; set { Set(ref _company, value); } }

        private string _productName;
        public string ProductName { get => _productName; set { Set(ref _productName, value); } }

        private bool _isDebug;
        public bool IsDebug { get=> _isDebug; set { Set(ref _isDebug, value); } }

        private DateTime _buildTime;
        public DateTime BuildTime { get=>_buildTime; set { Set(ref _buildTime, value); } }

        private bool _available;
        public bool Available { get=>_available; set { Set(ref _available, value); } }


        /// <summary>
        /// 插件模板
        /// </summary>
        private FrameworkElement _templateElement;
        public FrameworkElement TemplateElement { get => _templateElement; set { Set(ref _templateElement, value); } }

    }
}
