using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.GameCellControl;
using MemoryGame.ViewModel.GameWindow;
using MemoryGame.ViewModel.Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryGame.Services
{
    public static class JsonSerializerService
    {
        #region Private field
        private const string filepathUsers = "users.json";
        #endregion
        #region Methods
        public static void SaveUsers(LoginWindow window)
        {
            if (window.DataContext is LoginViewModel loginViewModel)
            {
                List<User> users = loginViewModel.ListboxUserItems.ToList();

                List<SavedGameDTO> existingUsers = new List<SavedGameDTO>();
                if (File.Exists(filepathUsers))
                {
                    string existingJson = File.ReadAllText(filepathUsers);
                    existingUsers = JsonSerializer.Deserialize<List<SavedGameDTO>>(existingJson) ?? new List<SavedGameDTO>();
                }

                Dictionary<string, SavedGameDTO> userDict = existingUsers.ToDictionary(u => u.Username, u => u);

                foreach (var user in users)
                {
                    var savedGameDTO = new SavedGameDTO
                    {
                        Username = user.Username,
                        ImageIndex = user.ImageIndex,
                        IsAdded = true,
                        GamesPlayed = user.GamesPlayed,
                        GamesWon = user.GamesWon,
                        Dimensions = new BoardDimensions(),
                        ChosenGameTime = "0",
                        GameBoardCells = new ObservableCollection<ObservableCollection<GameCellControlViewModel>>(),
                        ChosenCategoryType = CategoryType.Invalid
                    };

                    if (!userDict.ContainsKey(user.Username)) // user non existent
                    {
                        
                        userDict.Add(user.Username, savedGameDTO);
                    }
                    else // existent user 
                    {
                        var existing = userDict[user.Username];
                        userDict[user.Username] = savedGameDTO;
                    }
                }

                string json = JsonSerializer.Serialize(userDict.Values.ToList(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filepathUsers, json);
            }
        }
        public static List<User> LoadUsers()
        {
            if (!File.Exists(filepathUsers))
                return new List<User>();

            string json = File.ReadAllText(filepathUsers);
            return JsonSerializer.Deserialize<List<User>>(json);
        }

        public static void DeleteUser(string usernameToDelete)
        {
            if (!File.Exists(filepathUsers))
                return;

            string json = File.ReadAllText(filepathUsers);
            List<SavedGameDTO> existingUsers = JsonSerializer.Deserialize<List<SavedGameDTO>>(json) ?? new List<SavedGameDTO>();

            existingUsers = existingUsers.Where(u => u.Username != usernameToDelete).ToList();

            string updatedJson = JsonSerializer.Serialize(existingUsers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filepathUsers, updatedJson);
        }
        #endregion
    }
}