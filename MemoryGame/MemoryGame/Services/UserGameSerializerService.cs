using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MemoryGame.Model;
using MemoryGame.ViewModel.GameCellControl;
using MemoryGame.ViewModel.GameWindow;

namespace MemoryGame.Services
{

    public static class UserGameSerializerService
    {
        #region Private fields
        private const string _filepathUsers = "users.json";
        #endregion
        #region Methods
        public static SavedGameDTO LoadGame(string username)
        {
            string json = File.ReadAllText(_filepathUsers);
            var users = JsonSerializer.Deserialize<List<SavedGameDTO>>(json);

            foreach(SavedGameDTO u in users)
            {
                if(u.Username == username)
                    return u;
            }
            return new SavedGameDTO();
        }

        public static void SaveGame(User user,GameWindowViewModel vm)
        {
            var dto = new SavedGameDTO
            {
                Username = user.Username,
                ImageIndex = user.ImageIndex,
                IsAdded = user.IsAdded,
                GamesPlayed = user.GamesPlayed,
                GamesWon = user.GamesWon,
                ChosenCategoryType = (CategoryType)vm.ChosenCategoryType,
                Dimensions = vm.Dimensions, 
                GameBoardCells = vm.GameBoardCells,
                ChosenGameTime = vm.ChosenGameTime,
            };

            var dtoList = new List<SavedGameDTO> { dto };
            var json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filepathUsers, json);
        }
        #endregion
    }
}
