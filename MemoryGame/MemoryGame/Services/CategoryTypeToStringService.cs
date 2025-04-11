using MemoryGame.ViewModel.GameWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Services
{
    public static class CategoryTypeToStringService
    {
        public static string CategoryTypeToString(CategoryType type)
        {
            string typeToReturn = string.Empty;
            switch (type)
            {
                case CategoryType.Invalid:
                    typeToReturn = "Invalid";
                    break;
                case CategoryType.League:
                    typeToReturn = "League";
                    break;
                case CategoryType.RockAlbum:
                    typeToReturn = "Rock album";
                    break;
                case CategoryType.Beer:
                    typeToReturn = "Beer";
                    break;
            }
            return typeToReturn;
        }
    }
}
