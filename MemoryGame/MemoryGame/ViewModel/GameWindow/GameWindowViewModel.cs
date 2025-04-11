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
        private CategoryType _chosenCategoryType;
        public CategoryType ChosenCategoryType
        {
            get => _chosenCategoryType;
            set
            {
                _chosenCategoryType = value;
                OnPropertyChanged(nameof(ChosenCategoryType));
                ChosenCategoryToString = "Category: " + CategoryTypeToStringService.CategoryTypeToString(ChosenCategoryType);
            }
        }
        #endregion

        private IWindowService _windowService { get; set; }

        #region Private fields
        private string _chosenCategoryToString;
        private SelectBoardDimensionsWindow _selectBoardDimensionsWindow { get; set; }
        private Int16 _cardMatches { get; set; }
        private GameCellControlViewModel _firstSelectedCell { get; set; }
        private GameCellControlViewModel _secondSelectedCell { get; set; }
        private ObservableCollection<ObservableCollection<GameCellControlViewModel>> _gameBoardCells;
        private DispatcherTimer _gameTimer;
        private bool _isCardClickBusy;
        private bool _isChosenGameTimeReadOnly;
        private string _chosenGameTime;
        private User _gameUser { get; set; }
        private string _currentUsername;
        private ObservableCollection<User> _allUsers;
        #endregion

        #region Public properties
        public string CurrentUsername
        {
            get => _currentUsername;
            set
            {
                _currentUsername = value;
                OnPropertyChanged(nameof(CurrentUsername));
            }
        }
        public string ChosenCategoryToString
        {
            get => _chosenCategoryToString;
            set
            {
                _chosenCategoryToString = value;
                OnPropertyChanged(nameof(ChosenCategoryToString));
            }
        }
        public BoardDimensions Dimensions { get; set; }
        public ObservableCollection<ImageSource> ChosenImages { get; set; }
        public ObservableCollection<ObservableCollection<GameCellControlViewModel>> GameBoardCells
        {
            get => _gameBoardCells;
            set
            {
                _gameBoardCells = value;
                OnPropertyChanged(nameof(GameBoardCells));
            }
        }
        public string ChosenGameTime
        {
            get => _chosenGameTime;
            set
            {
                _chosenGameTime = value;
                OnPropertyChanged(nameof(ChosenGameTime));
            }
        }

        public bool IsChosenGameTimeReadOnly
        {
            get => _isChosenGameTimeReadOnly;
            set
            {
                _isChosenGameTimeReadOnly = value;
                OnPropertyChanged(nameof(IsChosenGameTimeReadOnly));
            }
        }
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructor
        public GameWindowViewModel(User u, ObservableCollection<User> users)
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

            _gameUser = u;
            CurrentUsername = "User: " + _gameUser.Username;
            _allUsers = users;

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
            if (_isCardClickBusy)
                return;

            GameCellControlViewModel cellVM = parameter as GameCellControlViewModel;
            if (cellVM == null)
            {
                return;
            }

            FlipCard(cellVM);

            if (_firstSelectedCell == null)
            {
                _firstSelectedCell = cellVM;
                SelectCell(_firstSelectedCell, cellVM);
                return;
            }

            if (_secondSelectedCell == null)
            {
                _secondSelectedCell = cellVM;
                SelectCell(_secondSelectedCell, cellVM);
            }

            if (_firstSelectedCell != null && _secondSelectedCell != null &&
                _firstSelectedCell.IsSelected && _secondSelectedCell.IsSelected)
            {
                _isCardClickBusy = true;
                await Task.Delay(1300);
                _isCardClickBusy = false;
                if (_firstSelectedCell.Cell.ImageIndex == _secondSelectedCell.Cell.ImageIndex)
                {
                    RemoveCell(_firstSelectedCell);
                    RemoveCell(_secondSelectedCell);

                    _cardMatches++;
                    if (_cardMatches == ChosenImages.Count)
                    {
                        _gameUser.GamesWon++;
                        MessageBox.Show("You won!");
                        ResetRound();
                    }
                }
                else
                {
                    FlipCardToFaceDown(_firstSelectedCell);
                    FlipCardToFaceDown(_secondSelectedCell);
                }
                _firstSelectedCell = null;
                _secondSelectedCell = null;
            }
        }

        private void ButtonCustomGameClick()
        {
            Dimensions = new BoardDimensions();
            _windowService = new WindowService();

            _windowService.ShowWindow<SelectBoardDimensionsViewModel>(
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
            MemoryGame.View.StatisticsWindow w = new MemoryGame.View.StatisticsWindow(_allUsers);
            w.Show();
        }

        private void SaveCurrentGame()
        {
            UserGameSerializerService.SaveGame(_gameUser, this);
        }

        private void LoadCurrentGame()
        {
            SavedGameDTO savedGameDTO = UserGameSerializerService.LoadGame(_gameUser.Username);

            if (savedGameDTO.GamesPlayed == 0)
            {
                MessageBox.Show("You have not saved a game before!");
                return;
            }

            _gameUser.Username = savedGameDTO.Username;
            _gameUser.IsAdded = savedGameDTO.IsAdded;
            _gameUser.ImageIndex = savedGameDTO.ImageIndex;
            _gameUser.GamesPlayed = savedGameDTO.GamesPlayed;
            _gameUser.GamesWon = savedGameDTO.GamesWon;

            ChosenCategoryType = savedGameDTO.ChosenCategoryType;
            Dimensions = savedGameDTO.Dimensions;
            ChosenImages = GameImagesLoadService.LoadImages(_chosenCategoryType, Dimensions);
            GameBoardCells = savedGameDTO.GameBoardCells;
            ChosenGameTime = savedGameDTO.ChosenGameTime;

            _cardMatches = -1;
            for (UInt16 i = 0; i < int.Parse(Dimensions.Rows); i++)
            {
                for (UInt16 j = 0; j < int.Parse(Dimensions.Columns); j++)
                {
                    GameBoardCells[i][j].FlipCommand = new RelayCommand<GameCellControlViewModel>(FlipCardClick);
                    GameBoardCells[i][j].FrontCardImageSource = ChosenImages[GameBoardCells[i][j].Cell.ImageIndex];
                    if (GameBoardCells[i][j].IsMatched)
                    {
                        _cardMatches++; Debug.Print(GameBoardCells[i][j].Cell.ToString());
                    }
                }
            }

            _gameUser.GamesPlayed++;
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
        #region CanExecute Commands 
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
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
        }

        private void ResetRound()
        {
            GameBoardCells = null;
            _firstSelectedCell = null;
            _secondSelectedCell = null;
            _cardMatches = 0;
            _isCardClickBusy = false;
            IsChosenGameTimeReadOnly = false;

            _gameTimer.Stop();
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
            ChosenImages = GameImagesLoadService.LoadImages(_chosenCategoryType, Dimensions);

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

            _firstSelectedCell = null;
            _secondSelectedCell = null;
            _cardMatches = 0;
            _isCardClickBusy = false;
        }

        private void StartNewGame() // when the second window is closed this is called
        {
            _gameUser.GamesPlayed++;
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