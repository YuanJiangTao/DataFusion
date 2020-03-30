using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataFusion.Resources.Converter
{
    public class ProtocalRunStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            var state = int.Parse(value.ToString());
            switch (state)
            {
                case 0:
                    result = "暂停";
                    break;
                case 1:
                    result = "运行";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
