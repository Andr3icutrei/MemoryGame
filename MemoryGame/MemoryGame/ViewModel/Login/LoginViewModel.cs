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

namespace MemoryGame.ViewModel.Login
{
    public class LoginViewModel 
    {
        private SharedViewModel sharedVM;
        public SharedViewModel SharedVM => sharedVM;
        public ICommand ButtonLeftArrowClick { get; }
        public ICommand ButtonRightArrowClick { get; }
        public ICommand ButtonNewUserClick { get; }
        public ICommand ButtonDeleteUserClick { get; }
        public ICommand ButtonPlayClick { get; }
        public ICommand ButtonCancelClick { get; }

        public const string LoginImagesPath = "../../LoginImages/";

        public ObservableCollection<User> ListboxItems { get; }

        public LoginViewModel()
        {
            ButtonLeftArrowClick = new RelayCommand(Execute_LeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_RightArrowClick);
            ButtonNewUserClick = new RelayCommand(Execute_NewUserClick);
            ButtonDeleteUserClick = new RelayCommand(Execute_DeleteUserClick);
            ButtonPlayClick = new RelayCommand(Execute_PlayClick);
            ButtonCancelClick = new RelayCommand(Execute_CancelClick);

            ListboxItems = new ObservableCollection<User>();
        }

        private void Execute_LeftArrowClick()
        {

        }

        private void Execute_RightArrowClick()
        {

        }

        private void Execute_NewUserClick()
        {
            UserAddWindow w = new UserAddWindow(sharedVM);
            w.Show();
        }

        private void Execute_DeleteUserClick()
        {

        }

        private void Execute_PlayClick()
        {

        }

        private void Execute_CancelClick()
        {

        }
    }
}
