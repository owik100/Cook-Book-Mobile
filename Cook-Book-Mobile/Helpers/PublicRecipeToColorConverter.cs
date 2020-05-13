using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cook_Book_Mobile.Helpers
{
    public class PublicRecipeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                {
                    return Color.Green;
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
