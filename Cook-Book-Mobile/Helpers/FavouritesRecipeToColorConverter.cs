using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cook_Book_Mobile.Helpers
{
    public class FavouritesRecipeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                {
                    return Color.FromRgb(255, 217, 0);
                }
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
