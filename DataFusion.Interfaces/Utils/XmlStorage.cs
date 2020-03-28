using System.IO;
using System.Xml.Serialization;

namespace DataFusion.Interfaces.Utils
{
    public class XmlStorage<T>
        where T : class, new()
    {
        private T _t;
        private string _filePath;
        public XmlStorage(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            _filePath = fileName;
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, _t);
            }
        }
        public void Load()
        {
            if (File.Exists(_filePath))
            {
                var searlized = File.ReadAllText(_filePath);
                if (!string.IsNullOrWhiteSpace(searlized))
                {
                    using (StringReader sr = new StringReader(searlized))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        _t = (T)serializer.Deserialize(sr);
                    }
                }
            }
        }

        public T Storage
        {
            get
            {
                if (_t == null)
                {
                    _t = new T();
                }
                return _t;
            }
        }
    }
}
