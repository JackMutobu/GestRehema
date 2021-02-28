using GestRehema.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestRehema.Converters
{
    public class StringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentString = value as string;
            var paramString = parameter as string;
            if(!string.IsNullOrEmpty(currentString) && !string.IsNullOrEmpty(paramString))
                return currentString.Equals(parameter).ToVisibility();
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
