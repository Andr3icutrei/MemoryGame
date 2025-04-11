using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Model;

namespace MemoryGame.ViewModel.SelectBoardDimensions
{
    public class SelectBoardDimensionsViewModel : INotifyPropertyChanged
    {
        public Action RequestClose { get; set; }
        private BoardDimensions dimensions;
        public BoardDimensions Dimensions
        {
            get { return dimensions; }
            set
            {
                dimensions = value;
                OnPropertyChanged(nameof(Dimensions));
            }
        }

        #region Commands
        public ICommand ButtonOKCommand { get; }
        public ICommand ButtonCancelCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructor
        public SelectBoardDimensionsViewModel(BoardDimensions boardDimensions) 
        { 
            Dimensions = boardDimensions;
            ButtonOKCommand = new RelayCommand(Execute_ButtonOKClick,CanExecute_ButtonOKClick);
            ButtonCancelCommand = new RelayCommand(Execute_ButtonCancel);
        }
        #endregion

        #region Commands implementations
        private bool CanExecute_ButtonOKClick()
        {
            int resultRows,resultColumns;
            return Dimensions.Rows!=String.Empty && Dimensions.Columns!=String.Empty &&
                int.TryParse(Dimensions.Rows,out resultRows) && int.TryParse(Dimensions.Columns, out resultColumns) &&
                (resultRows % 2 == 0 || resultColumns % 2 == 0) && 
                resultRows>=2 && resultRows<=6 && 
                resultColumns>=2 && resultColumns<=6;
        }

        private void Execute_ButtonOKClick()
        {
            Dimensions.AreValidDimensions = true;
            RequestClose?.Invoke();
        }

        private void Execute_ButtonCancel()
        {
            Dimensions.AreValidDimensions = false;
            RequestClose?.Invoke();
        }
        #endregion
    }
}
