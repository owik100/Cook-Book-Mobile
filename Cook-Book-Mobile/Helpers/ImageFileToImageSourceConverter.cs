using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Cook_Book_Mobile.Helpers
{
    public class ImageFileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource output;
            var path = (string)value;

            if (path.Contains("/data/user/"))
            {          
                output = ImageSource.FromFile(path);
            }
            else
            {
                output = ImageSource.FromResource(path);
            }

            
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
