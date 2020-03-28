using System.IO;
using System.Reflection;

namespace DataFusion.Utils
{
    class ProductUtil
    {
        public static string GetAssemblyTitle(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != "")
                    return titleAttribute.Title;
            }
            return Path.GetFileNameWithoutExtension(assembly.CodeBase);
        }

        public static string GetAssemblyVersion(Assembly assembly)
        {
            return assembly.GetName().Version.ToString();
        }

        public static string GetAssemblyDescription(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
                return "";
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }

        public static string GetAssemblyProduct(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
                return "";
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        public static string GetAssemblyCopyright(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
                return "";
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        public static string GetAssemblyCompany(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
                return "";
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle => GetAssemblyTitle(Assembly.GetEntryAssembly());

        public static string AssemblyVersion => GetAssemblyVersion(Assembly.GetEntryAssembly());

        public static string AssemblyDescription => GetAssemblyDescription(Assembly.GetEntryAssembly());

        public static string AssemblyProduct => GetAssemblyProduct(Assembly.GetEntryAssembly());

        public static string AssemblyCopyright => GetAssemblyCopyright(Assembly.GetEntryAssembly());

        public static string AssemblyCompany => GetAssemblyCompany(Assembly.GetEntryAssembly());

        #endregion
    }
}
