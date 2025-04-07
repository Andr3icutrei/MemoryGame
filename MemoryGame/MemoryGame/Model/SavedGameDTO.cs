using MemoryGame.ViewModel.GameCellControl;
using MemoryGame.ViewModel.GameWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class SavedGameDTO
    {
        public string Username { get; set; }
        public ushort ImageIndex { get; set; }
        public bool IsAdded { get; set; }

        public CategoryType ChosenCategoryType { get; set; }
        public BoardDimensions Dimensions { get; set; }
        public ObservableCollection<ObservableCollection<GameCellControlViewModel>> GameBoardCells { get; set; }
        public string ChosenGameTime { get; set; }
    }

}
