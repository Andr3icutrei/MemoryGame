using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Model;
using System.ComponentModel;
using MemoryGame.View;
using MemoryGame.ViewModel.Shared;
using System.Diagnostics;

namespace MemoryGame.ViewModel.Login
{
    public class LoginViewModel 
    {
        UserAddWindow w;
        public SharedViewModel SharedVM { get; set; }
        public ICommand ButtonLeftArrowClick { get; }
        public ICommand ButtonRightArrowClick { get; }
        public ICommand ButtonNewUserClick { get; }
        public ICommand ButtonDeleteUserClick { get; }
        public ICommand ButtonPlayClick { get; }
        public ICommand ButtonCancelClick { get; }

        public const string LoginImagesPath = "../../LoginImages/";

        public ObservableCollection<String> ListboxItems { get; }

        public LoginViewModel(SharedViewModel vm)
        {
            SharedVM = vm;

            ButtonLeftArrowClick = new RelayCommand(Execute_LeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_RightArrowClick);
            ButtonNewUserClick = new RelayCommand(Execute_NewUserClick);
            ButtonDeleteUserClick = new RelayCommand(Execute_DeleteUserClick);
            ButtonPlayClick = new RelayCommand(Execute_PlayClick);
            ButtonCancelClick = new RelayCommand(Execute_CancelClick);

            ListboxItems = new ObservableCollection<String>();
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
            ListboxItems.Add(SharedVM.BoundUser.Username);
        }

        private void Execute_DeleteUserClick()
        {

        }

        private void Execute_PlayClick()
        {
            Debug.Print(SharedVM.BoundUser.Username);
        }

        private void Execute_CancelClick()
        {

        }
    }
}
