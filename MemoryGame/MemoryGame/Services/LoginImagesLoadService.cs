using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MemoryGame.Services
{
    public static class LoginImagesLoadService
    {
        private static readonly string _loginImagesPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "LoginImages");
        
        public static ObservableCollection<ImageSource> Images { get; set; }
        private static void LoadImages()
        {
            Images = new ObservableCollection<ImageSource>();
            if (Directory.Exists(_loginImagesPath))
            {
                string[] files = Directory.GetFiles(_loginImagesPath, "*.jpg");
                foreach (string file in files)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri("pack://application:,,,/LoginImages/" + Path.GetFileName(file), UriKind.Absolute);
                    image.EndInit();
                    Images.Add(image);
                }
            }
            else
            {
                Debug.Print("Images not loaded");
            }
        }
        static LoginImagesLoadService()
        {
            LoadImages();
        }
    }
}
