using MemoryGame.ViewModel.Login;
using MemoryGame.ViewModel.Shared;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemoryGame.Services;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var sharedViewModel = new SharedViewModel();

            DataContext = new LoginViewModel(sharedViewModel);
            this.Closing += ((System.Windows.Application.Current as App)).LoginWindow_Closing;
        }
        public void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window activeWindow = sender as Window;
            if (activeWindow != null)
            {
                JsonSerializerService.SaveUsers(this);
            }
        }

        public void Window_Hide(object sender, ExecutedRoutedEventArgs e)
        {
            this.Hide();    
        }
    }
}