using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.View;
using System.Text.Json.Serialization;

namespace MemoryGame.ViewModel.GameCellControl
{
    public class GameCellControlViewModel : INotifyPropertyChanged
    {
        #region Private fields
        private ImageSource _frontCardImageSource;
        private bool _isCardFaceUp;
        private bool _isCardFaceDown;
        private bool _isMatched;
        #endregion

        #region Public properties
        public GameCell Cell { get; set; }

        [JsonIgnore]
        public ImageSource FrontCardImageSource
        {
            get => _frontCardImageSource;
            set
            {
                _frontCardImageSource = value;
                OnPropertyChanged(nameof(FrontCardImageSource));
            }
        }

        [JsonIgnore]
        public ICommand FlipCommand { get; set; }

        public bool IsCardFaceUp
        {
            get => _isCardFaceUp;
            set
            {
                _isCardFaceUp = value;
                OnPropertyChanged(nameof(IsCardFaceUp));
            }
        }

        public bool IsSelected { get; set; }

        public bool IsCardFaceDown
        {
            get => _isCardFaceDown;
            set
            {
                _isCardFaceDown = value;
                OnPropertyChanged(nameof(IsCardFaceDown));
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged(nameof(IsMatched));
            }
        }
        #endregion

        #region Constructors
        public GameCellControlViewModel(int i, int j, int imageIndex, ImageSource source)
        {
            IsCardFaceDown = true;
            IsCardFaceUp = !IsCardFaceDown;
            IsMatched = IsSelected = false;
            Cell = new GameCell((UInt16)i, (UInt16)j, (UInt16)imageIndex);
            FrontCardImageSource = source;
        }

        public GameCellControlViewModel()
        {
            IsCardFaceDown = IsCardFaceDown = IsMatched = IsSelected = false;
            FrontCardImageSource = null;
            FlipCommand = null;
            FrontCardImageSource = null;
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

}
