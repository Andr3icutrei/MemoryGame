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
using MemoryGame.ViewModel.StatisticsWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Documents;

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
        public event PropertyChangedEventHandler? PropertyChanged;
        #region Commands
        public ICommand CustomGameCommand { get; }
        public ICommand ChosenLeagueCommand { get; }
        public ICommand ChosenRockCommand { get; }
        public ICommand ChosenBeerCommand { get; }
        public ICommand FlipCommand { get; }
        public ICommand StandardGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand ShowStatsCommand { get; }
        public ICommand HelpCommand { get; }
        #endregion

        #region Catergories
        private CategoryType chosenCategoryType;
        public CategoryType ChosenCategoryType
        {
            get => chosenCategoryType;
            set
            {
                chosenCategoryType = value;
                OnPropertyChanged(nameof(ChosenCategoryType));
                ChosenCategoryToString = "Category: " + CategoryTypeToStringService.CategoryTypeToString(ChosenCategoryType);
            }
        }
        #endregion

        private IWindowService windowService { get; set; }

        #region Private fields
        private string chosenCategoryToString;
        private SelectBoardDimensionsWindow selectBoardDimensionsWindow { get; set; }
        private Int16 cardMatches { get; set; }
        private GameCellControlViewModel firstSelectedCell { get; set; }
        private GameCellControlViewModel secondSelectedCell { get; set; }
        private ObservableCollection<ObservableCollection<GameCellControlViewModel>> gameBoardCells;
        private DispatcherTimer gameTimer;
        private bool isCardClickBusy;
        private bool isChosenGameTimeReadOnly;
        private string chosenGameTime;
        private User GameUser { get; set; }
        private string currentUsername;
        private ObservableCollection<User> allUsers;
        #endregion

        #region Public properties
        public string CurrentUsername
        {
            get => currentUsername;
            set
            {
                currentUsername = value;
                OnPropertyChanged(nameof(CurrentUsername));
            }
        }
        public string ChosenCategoryToString
        {
            get => chosenCategoryToString;
            set
            {
                chosenCategoryToString = value;
                OnPropertyChanged(nameof(ChosenCategoryToString));
            }
        }
        public BoardDimensions Dimensions { get; set; }
        public ObservableCollection<ImageSource> ChosenImages { get; set; }
        public ObservableCollection<ObservableCollection<GameCellControlViewModel>> GameBoardCells
        {
            get => gameBoardCells;
            set
            {
                gameBoardCells = value;
                OnPropertyChanged(nameof(GameBoardCells));
            }
        }
        public string ChosenGameTime
        {
            get => chosenGameTime;
            set
            {
                chosenGameTime = value;
                OnPropertyChanged(nameof(ChosenGameTime));
            }
        }

        public bool IsChosenGameTimeReadOnly
        {
            get => isChosenGameTimeReadOnly;
            set
            {
                isChosenGameTimeReadOnly = value;
                OnPropertyChanged(nameof(IsChosenGameTimeReadOnly));
            }
        }
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructor
        public GameWindowViewModel(User u,ObservableCollection<User> users)
        {
            ChosenGameTime = String.Empty;
            IsChosenGameTimeReadOnly = false;

            ChosenCategoryToString = "Category: ";

            CustomGameCommand = new RelayCommand(ButtonCustomGameClick, CanExecute_NewGame);
            ChosenLeagueCommand = new RelayCommand(ButtonChosenLeagueCategory);
            ChosenRockCommand = new RelayCommand(ButtonChosenRockCategory);
            ChosenBeerCommand = new RelayCommand(ButtonChosenBeerCategory);
            StandardGameCommand = new RelayCommand(StartStandardGame, CanExecute_NewGame);
            SaveGameCommand = new RelayCommand(SaveCurrentGame, CanExecute_SaveCurrentGame);
            LoadGameCommand = new RelayCommand(LoadCurrentGame, CanExecute_LoadCurrentGame);
            ShowStatsCommand = new RelayCommand(ShowStatsWindow);
            HelpCommand = new RelayCommand(HelpClick);

            GameUser = u;
            CurrentUsername = "User: "+ GameUser.Username;
            allUsers = users;

            ChosenCategoryType = CategoryType.Invalid;
        }
        #endregion

        #region Command Implementations
        private void HelpClick()
        {
            string email = "andrei.arustei@student.unitbv.com";
            string message = "10LF331\nInformatica aplicata\nArustei Andrei";

            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add("Click here to send an email to: ");

            Hyperlink hyperlink = new Hyperlink(new Run(email))
            {
                NavigateUri = new Uri($"mailto:{email}")
            };

            hyperlink.RequestNavigate += (sender, args) =>
            {
                try
                {

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = args.Uri.ToString(),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            textBlock.Inlines.Add(hyperlink);
            textBlock.Inlines.Add("\n" + message);

            Window customMessageBox = new Window
            {
                Title = "Email Link",
                Width = 400,
                Height = 120,
                Content = textBlock
            };

            customMessageBox.ShowDialog();
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
                        GameUser.GamesWon++;
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

        private void ShowStatsWindow()
        {
            MemoryGame.View.StatisticsWindow w = new MemoryGame.View.StatisticsWindow(allUsers);
            w.Show();
        }

        private void SaveCurrentGame()
        {
            UserGameSerializerService.SaveGame(GameUser,this);
        }

        private void LoadCurrentGame()
        {
            SavedGameDTO savedGameDTO = UserGameSerializerService.LoadGame(GameUser.Username);

            if (savedGameDTO.GamesPlayed == 0)
            { 
                MessageBox.Show("You have not saved a game before!");
                return; 
            }

            GameUser.Username = savedGameDTO.Username;
            GameUser.IsAdded = savedGameDTO.IsAdded;
            GameUser.ImageIndex = savedGameDTO.ImageIndex;
            GameUser.GamesPlayed = savedGameDTO.GamesPlayed;
            GameUser.GamesWon = savedGameDTO.GamesWon;

            ChosenCategoryType = savedGameDTO.ChosenCategoryType;
            Dimensions = savedGameDTO.Dimensions;
            ChosenImages = GameImagesLoadService.LoadImages(chosenCategoryType, Dimensions);
            GameBoardCells = savedGameDTO.GameBoardCells;
            ChosenGameTime = savedGameDTO.ChosenGameTime;

            cardMatches = -1;
            for (UInt16 i = 0; i < int.Parse(Dimensions.Rows); i++)
            {
                for (UInt16 j = 0; j < int.Parse(Dimensions.Columns); j++)
                {
                    GameBoardCells[i][j].FlipCommand = new RelayCommand<GameCellControlViewModel>(FlipCardClick);
                    GameBoardCells[i][j].FrontCardImageSource = ChosenImages[GameBoardCells[i][j].Cell.ImageIndex];
                    if (GameBoardCells[i][j].IsMatched)
                    { 
                        cardMatches++; Debug.Print(GameBoardCells[i][j].Cell.ToString());
                    }
                }
            }
            
            GameUser.GamesPlayed++;
            InitializeGameTimer();
        }

        

        private void StartStandardGame()
        {
            // default size of 4x4
            Dimensions = new BoardDimensions();
            Dimensions.Rows = 4.ToString();
            Dimensions.Columns = 4.ToString();
            StartNewGame();
        }

        #endregion
        #region Can Execute Commands 
        private bool CanExecute_SaveCurrentGame()
        {
            return IsChosenGameTimeReadOnly;
        }

        private bool CanExecute_LoadCurrentGame()
        {
            return !CanExecute_SaveCurrentGame();
        }

        private bool CanExecute_NewGame()
        {
            int result;
            return int.TryParse(ChosenGameTime, out result) && ChosenGameTime != String.Empty && ChosenGameTime != "0" && ChosenCategoryType != CategoryType.Invalid;
        }
        #endregion

        #region Methods
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
            IsChosenGameTimeReadOnly = false;

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
                    GameCellControlViewModel cell = new GameCellControlViewModel(i, j, combined[k], ChosenImages[combined[k]]);
                    cell.FlipCommand = new RelayCommand<GameCellControlViewModel>(FlipCardClick);
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
            GameUser.GamesPlayed++;
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
        #endregion
    }
}
