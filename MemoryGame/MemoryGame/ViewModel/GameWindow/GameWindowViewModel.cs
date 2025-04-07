using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Model;
using MemoryGame.Services;
using MemoryGame.View;
using MemoryGame.ViewModel.SelectBoardDimensions;
using MemoryGame.ViewModel.GameCellControl;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Windows.Threading;


namespace MemoryGame.ViewModel.GameWindow
{
    public enum CategoryType : UInt16
    {
        Invalid = 0,
        League = 4,
        RockAlbum = 16,
        Beer = 64
    }
    public class GameWindowViewModel : INotifyPropertyChanged
    {
        private IWindowService windowService { get; set; }
        private SelectBoardDimensionsWindow selectBoardDimensionsWindow { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public BoardDimensions Dimensions { get; set; }
        public ICommand NewGameCommand { get; }
        public ICommand CustomGameCommand { get; }
        public ICommand ChosenLeagueCommand { get; }
        public ICommand ChosenRockCommand { get; }
        public ICommand ChosenBeerCommand { get; }
        public ICommand FlipCommand { get; }
        public ICommand StandardGameCommand { get; }

        private CategoryType chosenCategoryType;
        public CategoryType ChosenCategoryType
        {
            get => chosenCategoryType;
            set
            {
                chosenCategoryType = value;
                OnPropertyChanged(nameof(ChosenCategoryType));
            }
        }
        private UInt16 cardMatches { get; set; }

        private GameCellControlViewModel firstSelectedCell { get; set; }
        private GameCellControlViewModel secondSelectedCell { get; set; }
        public ObservableCollection<ImageSource> ChosenImages { get; set; }
        private ObservableCollection<ObservableCollection<GameCellControlViewModel>> gameBoardCells;
        public ObservableCollection<ObservableCollection<GameCellControlViewModel>> GameBoardCells
        {
            get => gameBoardCells;
            set
            {
                gameBoardCells = value;
                OnPropertyChanged(nameof(GameBoardCells));
            }
        }
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
        private DispatcherTimer gameTimer;
        private int gameSecondsPassed;
        private bool isCardClickBusy;

        private bool isChosenGameTimeReadOnly;
        public bool IsChosenGameTimeReadOnly
        {
            get => isChosenGameTimeReadOnly;
            set
            {
                isChosenGameTimeReadOnly = value;
                OnPropertyChanged(nameof(IsChosenGameTimeReadOnly));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameWindowViewModel()
        {
            ChosenGameTime = String.Empty;
            IsChosenGameTimeReadOnly = false;

            NewGameCommand = new RelayCommand(StartNewGame, CanExecute_NewGame);
            CustomGameCommand = new RelayCommand(ButtonCustomGameClick);
            ChosenLeagueCommand = new RelayCommand(ButtonChosenLeagueCategory);
            ChosenRockCommand = new RelayCommand(ButtonChosenRockCategory);
            ChosenBeerCommand = new RelayCommand(ButtonChosenBeerCategory);
            StandardGameCommand = new RelayCommand(StartStandardGame, CanExecute_NewGame);

            ChosenCategoryType = CategoryType.Invalid;
        }

        private void StartStandardGame()
        {
            Dimensions.Rows = 4.ToString();
            Dimensions.Columns = 4.ToString();
            StartNewGame();
        }

        private void InitializeGameTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void ResetRound()
        {
            GameBoardCells = null;
            firstSelectedCell = null;
            secondSelectedCell = null;
            cardMatches = 0;
            isCardClickBusy = false;
            isChosenGameTimeReadOnly = false;
            gameTimer.Stop();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ChosenGameTime) == 0)
            {
                MessageBox.Show("You lost!");
                ResetRound();
                return;
            }
            ChosenGameTime = Convert.ToString(Convert.ToInt32(ChosenGameTime) - 1);
        }

        private void InitializeGameChosenTime()
        {
            IsChosenGameTimeReadOnly = true;
        }

        private void InitializeGameBoardCells()
        {
            GameBoardCells = new ObservableCollection<ObservableCollection<GameCellControlViewModel>>();
            ChosenImages = GameImagesLoadService.LoadImages(chosenCategoryType, Dimensions);

            List<int> duplicated = Enumerable.Range(0, ChosenImages.Count).ToList();
            List<int> combined = Enumerable.Range(0, ChosenImages.Count).ToList();

            combined.AddRange(duplicated);
            Random random = new Random();

            for (int i = combined.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                var temp = combined[i];
                combined[i] = combined[j];
                combined[j] = temp;
            }

            int k = 0;
            for (UInt16 i = 0; i < int.Parse(Dimensions.Rows); i++)
            {
                ObservableCollection<GameCellControlViewModel> rowToReturn = new ObservableCollection<GameCellControlViewModel>();

                for (UInt16 j = 0; j < int.Parse(Dimensions.Columns); j++)
                {
                    GameCellControlViewModel cell = new GameCellControlViewModel(i, j, combined[k], ChosenImages[combined[k]],
                        new RelayCommand<GameCellControlViewModel>(FlipCardClick));
                    rowToReturn.Add(cell);
                    k++;
                }
                GameBoardCells.Add(rowToReturn);
            }
        }

        private void InitializeGame()
        {
            InitializeGameChosenTime();
            InitializeGameBoardCells();
            InitializeGameTimer();

            firstSelectedCell = null;
            secondSelectedCell = null;
            cardMatches = 0;
            isCardClickBusy = false;
        }

        private void StartNewGame() // when the second window is closed this is called
        {
            if (!CanExecute_NewGame())
            {
                MessageBox.Show("Select a category from File item or choose the desired time!");
                return;
            }

            InitializeGame();
        }

        private void FlipCard(GameCellControlViewModel cellVM)
        {
            cellVM.IsCardFaceDown = !cellVM.IsCardFaceDown;
            cellVM.IsCardFaceUp = !cellVM.IsCardFaceUp;
        }

        private void SelectCell(GameCellControlViewModel selectedCell, GameCellControlViewModel cellVM)
        {
            selectedCell.IsSelected = true;
        }

        private void RemoveCell(GameCellControlViewModel selectedCell)
        {
            selectedCell.IsCardFaceDown = selectedCell.IsCardFaceUp = false;
            selectedCell.IsMatched = true;
        }

        public void FlipCardToFaceDown(GameCellControlViewModel selectedCell)
        {
            selectedCell.IsCardFaceDown = true;
            selectedCell.IsCardFaceUp = false;
        }

        private async void FlipCardClick(object parameter)
        {
            if (isCardClickBusy)
                return;

            GameCellControlViewModel cellVM = parameter as GameCellControlViewModel;
            if (cellVM == null)
            {
                return;
            }

            FlipCard(cellVM);

            if (firstSelectedCell == null)
            {
                firstSelectedCell = cellVM;
                SelectCell(firstSelectedCell, cellVM);
                return;
            }

            if (secondSelectedCell == null)
            {
                secondSelectedCell = cellVM;
                SelectCell(secondSelectedCell, cellVM);
            }

            if (firstSelectedCell != null && secondSelectedCell != null &&
                firstSelectedCell.IsSelected && secondSelectedCell.IsSelected)
            {
                isCardClickBusy = true;
                await Task.Delay(2000);
                isCardClickBusy = false;
                if (firstSelectedCell.Cell.ImageIndex == secondSelectedCell.Cell.ImageIndex)
                {
                    RemoveCell(firstSelectedCell);
                    RemoveCell(secondSelectedCell);

                    cardMatches++;
                    if (cardMatches == ChosenImages.Count)
                    {
                        MessageBox.Show("You won!");
                        ResetRound();
                    }
                }
                else
                {
                    FlipCardToFaceDown(firstSelectedCell);
                    FlipCardToFaceDown(secondSelectedCell);
                }
                firstSelectedCell = null;
                secondSelectedCell = null;
            }
        }

        private void ButtonCustomGameClick()
        {
            Dimensions = new BoardDimensions();
            windowService = new WindowService();

            windowService.ShowWindow<SelectBoardDimensionsViewModel>(
                new object[] { Dimensions }, // pass the dimensions param
                StartNewGame // this is the supervisor callback
            );
        }

        private bool CanExecute_NewGame()
        {
            int result;
            return int.TryParse(ChosenGameTime, out result) && ChosenGameTime != String.Empty && ChosenGameTime != "0" && ChosenCategoryType != CategoryType.Invalid;
        }

        private void ButtonChosenRockCategory()
        {
            ChosenCategoryType = CategoryType.RockAlbum;
        }

        private void ButtonChosenLeagueCategory()
        {
            ChosenCategoryType = CategoryType.League;
        }
        private void ButtonChosenBeerCategory()
        {
            ChosenCategoryType = CategoryType.Beer;
        }
    }
}
