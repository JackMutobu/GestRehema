using GestRehema.Extensions;
using GestRehema.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GestRehema.Converters
{
    public class SaleArticleDeliveryToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var saleArticle = value as SaleArticleItem;

            return (saleArticle!.DeliveredQuantity < saleArticle.Quantity).ToVisibility();
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
