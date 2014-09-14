using System;
using System.Globalization;
using System.Windows.Data;
using TVGenius.Model;

namespace TVGenius.Common.Resource
{
    class MarkAttributeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return  MarkAttribute.Get(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
