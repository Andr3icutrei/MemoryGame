using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MemoryGame.Services
{
    public class StringToImageSourceService : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            return null;  // Return a default image or null if path is empty
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();  // Not needed for one-way binding
        }
    }
}
