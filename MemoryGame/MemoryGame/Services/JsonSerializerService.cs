using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryGame.Services
{
    public static class JsonSerializerService
    {
        private const string filepath = "users.json";
        public static void SaveUsers(LoginWindow window)
        {
            if (window.DataContext is LoginViewModel loginViewModel)
            {
                List<User> users = loginViewModel.ListboxUserItems.ToList();
                string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filepath, json);
            }
        }
        public static List<User> LoadUsers()
        {
            if (!File.Exists(filepath))
                return new List<User>();

            string json = File.ReadAllText(filepath);
            return JsonSerializer.Deserialize<List<User>>(json);
        }
    }
}