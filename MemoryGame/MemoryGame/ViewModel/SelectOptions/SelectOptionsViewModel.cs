using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Model;
using MemoryGame.Services;
using MemoryGame.View;

namespace MemoryGame.ViewModel.SelectOptions
{
    public class SelectOptionsViewModel :INotifyPropertyChanged
    {
        private SelectBoardDimensionsWindow selectBoardDimensionsWindow { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public BoardDimensions Dimensions { get; set; }
        public ICommand NewGameCommand { get; }
        public ICommand CustomGameCommand { get; }
        private string chosenGameTime;
        public string ChosenGameTime
        {
            get => chosenGameTime;
            set
            {
                chosenGameTime = value;
                OnPropertyChanged(nameof(ChosenGameTime));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

        public SelectOptionsViewModel()
        {
            ChosenGameTime = String.Empty;
            NewGameCommand = new RelayCommand(StartNewGame, CanExecute_NewGame);
            CustomGameCommand = new RelayCommand(ButtonCustomGameClick);
        }


        private void StartNewGame()
        {

        }

        private void ButtonCustomGameClick()
        {
            Dimensions = new BoardDimensions();
            selectBoardDimensionsWindow = new SelectBoardDimensionsWindow(Dimensions);
            selectBoardDimensionsWindow.Show();
        }

        private bool CanExecute_NewGame()
        {
            return ChosenGameTime != String.Empty;
        }
    }
}
