using MemoryGame.View;
using System.Configuration;
using System.Data;
using System.Windows;
using MemoryGame.Services;
using MemoryGame.Model;
using MemoryGame.ViewModel.Login;
using System.Diagnostics;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            List<User> users = JsonSerializerService.LoadUsers();

            Application.Current.Dispatcher.Invoke(() =>
            {
                LoginWindow w = new LoginWindow();
                if (w.DataContext is LoginViewModel vm)
                {
                    foreach (var user in users)
                    {
                        vm.ListboxUserItems.Add(user);
                    }
                    if(vm.ListboxUserItems.Count > 0)
                        vm.UserImage = LoginImagesLoadService.Images[vm.ListboxUserItems[0].ImageIndex];
                }
                w.Show();
            });
        }


        public void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoginWindow loginWindow = sender as LoginWindow;
            if (loginWindow != null)
            {
                SaveWindowSettings(loginWindow);
            }
        }
        public static void SaveWindowSettings(LoginWindow lw)
        {
            JsonSerializerService.SaveUsers(lw);
        }
    }
}
