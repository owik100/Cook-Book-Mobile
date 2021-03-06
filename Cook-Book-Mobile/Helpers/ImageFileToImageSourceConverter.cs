﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cook_Book_Mobile.Helpers
{
    public class ImageFileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource output = ImageSource.FromResource("");
            try
            {
                if (value != null)
                {
                    var path = (string)value;

                    if (path.Contains(ImageConstants.LoadDefaultImage))
                    {
                        output = ImageSource.FromResource(ImageConstants.DefaultImagePath);
                    }
                    else
                    {
                        output = ImageSource.FromFile(path);
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
            }
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
