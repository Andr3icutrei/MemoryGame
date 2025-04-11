using MemoryGame.Model;
using MemoryGame.Services;
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

        private ImageSource _currentImage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ImageSource CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
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
            CurrentImage = LoginImagesLoadService.Images[0];
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

        private void Execute_ButtonLeftArrowClick()
        {
            if (SharedVM.BoundUser.ImageIndex > 0)
            {
                SharedVM.BoundUser.ImageIndex--;
                CurrentImage = LoginImagesLoadService.Images[SharedVM.BoundUser.ImageIndex % LoginImagesLoadService.Images.Count];
            }
        }

        private void Execute_ButtonRightArrowClick()
        {
            if (SharedVM.BoundUser.ImageIndex < LoginImagesLoadService.Images.Count-1)
            {
                SharedVM.BoundUser.ImageIndex++;
                CurrentImage = LoginImagesLoadService.Images[SharedVM.BoundUser.ImageIndex % LoginImagesLoadService.Images.Count];
            }
        }
    }
}
