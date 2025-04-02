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
        private SharedViewModel sharedVM;
        public SharedViewModel SharedVM => sharedVM;

        public ICommand ButtonAddUserClick { get; }
        public ICommand ButtonReturnToLoginClick { get; }
        public ICommand ButtonLeftArrowClick { get; }
        public ICommand ButtonRightArrowClick { get; }

        private UInt16 imageIndex;

        private static readonly string LoginImagesPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "LoginImages");
        private ImageSource currentImage;

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

        public ObservableCollection<ImageSource> Images { get; set; }

        private void InitializeImages()
        {
            imageIndex = 0;
            Images = new ObservableCollection<ImageSource>();
            LoadImages(LoginImagesPath);
            CurrentImage = Images[0];
        }
        public UserAddViewModel()
        {
            ButtonAddUserClick = new RelayCommand(Execute_ButtonAddUserClick);
            ButtonReturnToLoginClick = new RelayCommand(Execute_ButtonReturnToLoginClick);
            ButtonLeftArrowClick = new RelayCommand(Execute_ButtonLeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_ButtonRightArrowClick);

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

        public UserAddViewModel(SharedViewModel vm) : this()
        {
            sharedVM = vm;
        }

        private void Execute_ButtonAddUserClick()
        {

        }

        private void Execute_ButtonReturnToLoginClick()
        {

        }

        private void Execute_ButtonLeftArrowClick()
        {
            if (imageIndex > 0)
            {
                imageIndex--;
                CurrentImage = Images[imageIndex % Images.Count];
            }
        }

        private void Execute_ButtonRightArrowClick()
        {
            if (imageIndex < Images.Count)
            {
                imageIndex++;
                CurrentImage = Images[imageIndex % Images.Count];
            }
        }
    }
}
