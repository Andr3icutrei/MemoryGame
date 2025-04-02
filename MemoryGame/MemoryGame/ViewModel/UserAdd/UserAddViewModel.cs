using MemoryGame.Model;
using MemoryGame.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryGame.ViewModel.UserAdd
{
    public class UserAddViewModel : INotifyPropertyChanged
    {
        public SharedViewModel SharedVM { get; set; }

        public ICommand ButtonLeftArrowClick { get; }
        public ICommand ButtonRightArrowClick { get; }

        private static readonly string LoginImagesPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "LoginImages");
        private ImageSource currentImage;
        public ObservableCollection<ImageSource> Images { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ImageSource CurrentImage
        {
            get => currentImage;
            set
            {
                if (currentImage != value)
                {
                    currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeImages()
        {
            SharedVM.BoundUser.ImageIndex = 0;
            Images = new ObservableCollection<ImageSource>();
            LoadImages(LoginImagesPath);
            CurrentImage = Images[0];
        }
        public UserAddViewModel()
        {
            ButtonLeftArrowClick = new RelayCommand(Execute_ButtonLeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_ButtonRightArrowClick);
        }

        public UserAddViewModel(SharedViewModel vm) : this()
        {
            SharedVM = vm;

            InitializeImages();
        }

        private void LoadImages(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.jpg");
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
            Debug.Print(Images[0].ToString());
        }

        private void Execute_ButtonLeftArrowClick()
        {
            if (SharedVM.BoundUser.ImageIndex > 0)
            {
                SharedVM.BoundUser.ImageIndex--;
                CurrentImage = Images[SharedVM.BoundUser.ImageIndex % Images.Count];
            }
        }

        private void Execute_ButtonRightArrowClick()
        {
            if (SharedVM.BoundUser.ImageIndex < Images.Count)
            {
                SharedVM.BoundUser.ImageIndex++;
                CurrentImage = Images[SharedVM.BoundUser.ImageIndex % Images.Count];
            }
        }
    }
}
