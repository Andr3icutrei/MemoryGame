using MemoryGame.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MemoryGame.ViewModel.UserAdd;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for UserAddWindiw.xaml
    /// </summary>
    public partial class UserAddWindow : Window
    {
        public UserAddWindow(SharedViewModel vm) 
        {
            InitializeComponent();
            DataContext = new UserAddViewModel(vm); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            var dataContext = this.DataContext as UserAddViewModel;

            if (senderButton != null && senderButton.Content == "Return to Login")
            {
                dataContext.SharedVM.BoundUser.IsAdded = false;
            }
            else
            { 
                dataContext.SharedVM.BoundUser.IsAdded = true; 
            }

            this.Close();
        }
    }
}