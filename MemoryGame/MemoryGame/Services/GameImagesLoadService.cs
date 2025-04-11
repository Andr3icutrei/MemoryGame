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
using MemoryGame.Model;
using MemoryGame.ViewModel.GameWindow;

namespace MemoryGame.Services
{
    public static class GameImagesLoadService
    {
        #region Methods
        public static ObservableCollection<ImageSource> LoadImages(CategoryType type,BoardDimensions dim) 
        {
            ObservableCollection<ImageSource> Images = new ObservableCollection<ImageSource>();
            string pathToImages = String.Empty;
            string pathCategory = String.Empty;
            int necessaryImages = int.Parse(dim.Rows) * int.Parse(dim.Columns) / 2;
            
            switch (type)
            {
                case CategoryType.Invalid:
                    return new ObservableCollection<ImageSource>();
                case CategoryType.Beer:
                    pathToImages = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "GameImages", "beri");
                    pathCategory = "beri";
                    break;
                case CategoryType.League:
                    pathToImages = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "GameImages", "lol");
                    pathCategory = "league";
                    break;
                case CategoryType.RockAlbum:
                    pathToImages = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "GameImages", "roackereli");
                    pathCategory = "roackereli";
                    break;
            }
            Debug.Print(pathToImages);
            Images = new ObservableCollection<ImageSource>();
            if (Directory.Exists(pathToImages))
            {
                string[] files = Directory.GetFiles(pathToImages, "*.jpg");
                int index = 0;
                foreach (string file in files)
                {
                    if (index >= necessaryImages)
                        break;
                    BitmapImage image = new BitmapImage();
                    try
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = new Uri("file://" + file, UriKind.Absolute); 
                        image.EndInit();
                        Images.Add(image);
                        index++;
                    }
                    catch (IOException ioEx)
                    {
                        Debug.Print($"Error loading image: {file}, {ioEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"Unexpected error: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.Print("Images not loaded");
            }
            return Images;
        }
        #endregion
    }
}
