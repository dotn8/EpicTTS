using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EpicTTS.Converters
{
    public class DictionaryConverter : Dictionary<object, object>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            if (TryGetValue(value, out result))
                return result;
            throw new Exception("Unknown key " + value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.First(kvp => kvp.Value == value).Key;
        }
    }
}