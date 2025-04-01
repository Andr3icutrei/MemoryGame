using MemoryGame.Model;
using MemoryGame.ViewModel.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.ViewModel.UserAdd
{
    public class UserAddViewModel
    {
        private UserLoginViewModel userLoginVM;

        public ICommand ButtonAddUserClick;
        public ICommand ButtonReturnToLoginClick;
        public ICommand ButtonLeftArrowClick;
        public ICommand ButtonRightArrowClick;
        public UserAddViewModel() 
        {
            ButtonAddUserClick = new RelayCommand(Execute_ButtonAddUserClick);
            ButtonReturnToLoginClick = new RelayCommand(Execute_ButtonReturnToLoginClick);
            ButtonLeftArrowClick = new RelayCommand(Execute_ButtonLeftArrowClick);
            ButtonRightArrowClick = new RelayCommand(Execute_ButtonRightArrowClick);

            userLoginVM = new UserLoginViewModel();
        }

        private void Execute_ButtonAddUserClick()
        {

        }

        private void Execute_ButtonReturnToLoginClick()
        {

        }

        private void Execute_ButtonLeftArrowClick()
        {

        }

        private void Execute_ButtonRightArrowClick()
        {

        }
    }
}
