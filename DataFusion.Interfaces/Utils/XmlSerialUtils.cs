using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataFusion.Interfaces.Utils
{
    public class XmlSerialUtils
    {
        public static string XmlSerial<T>(T obj, string encoding)
        {
            var ret = string.Empty;
            try
            {
                if (ReferenceEquals(obj, null))
                    return ret;
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "    ";
                settings.NewLineChars = "\r\n";
                settings.Encoding = Encoding.GetEncoding(encoding);
                StringBuilder sb = new StringBuilder();
                using (XmlWriter xmlWriter = XmlWriter.Create(sb, settings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    serializer.Serialize(xmlWriter, obj, namespaces);
                    xmlWriter.Close();
                }
                ret = sb.ToString();
            }
            catch (Exception)
            {

            }
            return ret;
        }
        public static T XmlDeserial<T>(string content) where T : class
        {
            T obj = default(T);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader sr = new StringReader(content))
                {
                    obj = serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                obj = null;
            }
            return obj;
        }
    }
}
