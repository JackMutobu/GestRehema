using GestRehema.Entities;
using GestRehema.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestRehema.Converters
{
    public class ArticleToBooleanVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentArticle = value as Article;
            var paramString = parameter as string;
            if (currentArticle != null && !string.IsNullOrEmpty(paramString))
                return (currentArticle.Id == int.Parse(paramString)).ToVisibility();
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
