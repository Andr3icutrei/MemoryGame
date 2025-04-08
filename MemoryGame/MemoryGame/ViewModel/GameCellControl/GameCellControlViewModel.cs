﻿using MemoryGame.Model;
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
        public GameCell Cell {  get; set; }
        private ImageSource frontCardImageSource;

        [JsonIgnore]
        public ImageSource FrontCardImageSource
        {
            get => frontCardImageSource;
            set
            {
                frontCardImageSource = value;
                OnPropertyChanged(nameof(FrontCardImageSource));
            }
        }
        [JsonIgnore]
        public ICommand FlipCommand { get; set; }

        private bool isCardFaceUp;
        public bool IsCardFaceUp
        {
            get => isCardFaceUp;
            set 
            {
                isCardFaceUp = value; 
                OnPropertyChanged(nameof(IsCardFaceUp)); 
            }
        }
        public bool IsSelected {  get; set; }

        private bool isCardFaceDown;
        public bool IsCardFaceDown
        {
            get => isCardFaceDown;
            set
            {
                isCardFaceDown = value;
                OnPropertyChanged(nameof(IsCardFaceDown));
            }
        }


        private bool isMatched;

        public bool IsMatched
        {
            get => isMatched;
            set
            {
                isMatched = value; 
                OnPropertyChanged(nameof(IsMatched));
            }
        }

        public GameCellControlViewModel(int i,int j,int imageIndex,ImageSource source)
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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
