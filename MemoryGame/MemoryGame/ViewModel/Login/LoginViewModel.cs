using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using MemoryGame.View;
using MemoryGame.ViewModel.Shared;
using System.Diagnostics;
using System.Windows.Media;
using MemoryGame.Services;
using MemoryGame.ViewModel.GameWindow;

namespace MemoryGame.ViewModel.Login
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        UserAddWindow w;
        public SharedViewModel SharedVM { get; set; }
        public ICommand ButtonLeftArrowClick { get; }
        public ICommand ButtonRightArrowClick { get; }
        public ICommand ButtonNewUserClick { get; }
        public ICommand ButtonDeleteUserClick { get; }
        public ICommand ButtonPlayClick { get; }
        public ICommand ButtonCancelClick { get; }
        
        private User selectedUser;
        public User SelectedUser 
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        private int selectedUserIndex=0;
        public int SelectedUserIndex
        {
            get => selectedUserIndex;
            set
            {
                selectedUserIndex = value;
                if (selectedUserIndex == -1)
                    selectedUserIndex = 0;
                UserImage = LoginImagesLoadService.Images[ListboxUserItems[selectedUserIndex].ImageIndex];
                OnPropertyChanged(nameof(SelectedUserIndex));
            }
        }

        public ObservableCollection<User> ListboxUserItems { get; set; }

        private ImageSource userImage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ImageSource UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                OnPropertyChanged(nameof(UserImage));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginViewModel(SharedViewModel vm)
        {
            SharedVM = vm;

            ButtonLeftArrowClick = new RelayCommand(Execute_LeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_RightArrowClick);
            ButtonNewUserClick = new RelayCommand(Execute_NewUserClick);
            ButtonDeleteUserClick = new RelayCommand(Execute_DeleteUserClick,CanExecute_DeleteUserClick);
            ButtonPlayClick = new RelayCommand(Execute_PlayClick,CanExecute_PlayClick);
            ButtonCancelClick = new RelayCommand(Execute_CancelClick);

            ListboxUserItems = new ObservableCollection<User>();
        }

        private bool CanExecute_DeleteUserClick()
        {
            return SelectedUser != null;
        }

        private bool CanExecute_PlayClick()
        {
            return CanExecute_DeleteUserClick();
        }

        private void Execute_LeftArrowClick()
        {

        }

        private void Execute_RightArrowClick()
        {

        }

        private void Execute_NewUserClick()
        {
            w = new UserAddWindow(SharedVM);
            w.Closing += AddUser;
            w.Show();
        }

        private void AddUser(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SharedVM.BoundUser.IsAdded)
            {
                User newUser = new User(SharedVM.BoundUser);
                ListboxUserItems.Add(newUser);
                UserImage = LoginImagesLoadService.Images[SharedVM.BoundUser.ImageIndex];
            }
        }

        private void Execute_DeleteUserClick()
        {
            int index = SelectedUserIndex;
            ListboxUserItems.RemoveAt(index);
        }

        private void Execute_PlayClick()
        {
            View.GameWindow gameWindow = new View.GameWindow(SelectedUser);
            gameWindow.Show();
        }

        private void Execute_CancelClick()
        {

        }
    }
}
