using System.Windows;

namespace GestRehema.Extensions
{
    public static class VisibilityExtensions
    {
        public static Visibility ToVisibility(this bool value)
        {
            return value switch
            {
                true => Visibility.Visible,
                _ => Visibility.Collapsed
            };
        }

        public static Visibility ToVisibility(this string value)
        {
            return string.IsNullOrEmpty(value) switch
            {
                true => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        public static Visibility ToVisibility(this object value)
        {
            return (value is null) switch
            {
                true => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }
    }
}
