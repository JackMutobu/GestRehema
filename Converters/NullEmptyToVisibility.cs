using GestRehema.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GestRehema.Converters
{
    public class NullEmptyToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return (!string.IsNullOrEmpty(s)).ToVisibility();
            else
                return (value is not null).ToVisibility();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
