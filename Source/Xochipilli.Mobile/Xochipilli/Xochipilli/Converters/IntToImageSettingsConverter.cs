using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xochipilli.Converters
{
    public class IntToImageSettingsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int32 val = System.Convert.ToInt32(parameter);

            ImageSource result = null;

            ImageSource pointer = Device.OnPlatform(
            iOS: ImageSource.FromFile("pointer.png"),
            Android: ImageSource.FromFile("pointer.png"),
            WinPhone: ImageSource.FromFile("pointer.png"));     

            switch (val)
            {
                case 1:
                    result = pointer;
                    break;

                default:
                    result = pointer;
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
