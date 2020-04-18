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
            ImageSource output = ImageSource.FromResource("");

            if (value!= null)
            {
                var path = (string)value;

                if (path.Contains("load default"))
                {
                    output = ImageSource.FromResource("Cook_Book_Mobile.Images.foodtemplate.png");
                }
                else
                {
                    output = ImageSource.FromFile(path);      
                }
            }
          
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
