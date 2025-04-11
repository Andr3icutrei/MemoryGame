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
        #region Commands
        public ICommand ButtonNewUserClick { get; }
        public ICommand ButtonDeleteUserClick { get; }
        public ICommand ButtonPlayClick { get; }
        public ICommand ButtonCancelClick { get; }
        #endregion

        #region Private fields

        private User selectedUser;
        private int selectedUserIndex = 0;
        private ImageSource userImage;

        #endregion

        #region Public properties

        public User SelectedUser 
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public int SelectedUserIndex
        {
            get => selectedUserIndex;
            set
            {
                selectedUserIndex = value;
                if (selectedUserIndex == -1)
                {
                    selectedUserIndex = 0;
                    UserImage = LoginImagesLoadService.Images[0];
                    return;
                }
                UserImage = LoginImagesLoadService.Images[ListboxUserItems[selectedUserIndex].ImageIndex];
                OnPropertyChanged(nameof(SelectedUserIndex));
            }
        }
        public View.GameWindow GameWindow { get; set; }
        public ImageSource UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                OnPropertyChanged(nameof(UserImage));
            }
        }
        public ObservableCollection<User> ListboxUserItems { get; set; }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
    
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginViewModel(SharedViewModel vm)
        {
            SharedVM = vm;

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
                newUser.GamesPlayed = 0;
                newUser.GamesWon = 0;
                SelectedUser = newUser;
                ListboxUserItems.Add(newUser);
                UserImage = LoginImagesLoadService.Images[SharedVM.BoundUser.ImageIndex];
            }
        }

        private void Execute_DeleteUserClick()
        {
            int index = SelectedUserIndex;
            JsonSerializerService.DeleteUser(SelectedUser.Username);
            ListboxUserItems.RemoveAt(index);
        }

        private void Execute_PlayClick()
        {
            GameWindow = new View.GameWindow(SelectedUser,ListboxUserItems);
            GameWindow.Show();
        }

        private void Execute_CancelClick()
        {
            SelectedUser = null;
            SelectedUserIndex = -1;
        }
    }
}
