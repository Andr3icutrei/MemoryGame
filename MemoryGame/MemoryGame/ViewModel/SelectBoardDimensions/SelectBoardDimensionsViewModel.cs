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

        public ICommand ButtonOKCommand { get; }
        public ICommand ButtonCancelCommand { get; }

        public Action CloseAction { get; set; }
        private ICommand closeCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand CloseCommand
        {
            get
            {
                return closeCommand ?? (closeCommand = new RelayCommand(ExecuteClose));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SelectBoardDimensionsViewModel(BoardDimensions boardDimensions) 
        { 
            Dimensions = boardDimensions;
            ButtonOKCommand = new RelayCommand(Execute_ButtonOKClick,CanExecute_ButtonOKClick);
            ButtonCancelCommand = new RelayCommand(Execute_ButtonCancel);
        }   

        private bool CanExecute_ButtonOKClick()
        {
            int resultRows,resultColumns;
            return Dimensions.Rows!=String.Empty && Dimensions.Columns!=String.Empty &&
                int.TryParse(Dimensions.Rows,out resultRows) && int.TryParse(Dimensions.Columns, out resultColumns) &&
                (resultRows % 2 == 0 || resultColumns % 2 == 0);
        }

        private void Execute_ButtonOKClick()
        {
            Dimensions.AreValidDimensions = true;
            ExecuteClose();
        }

        private void Execute_ButtonCancel()
        {
            Dimensions.AreValidDimensions = false;
            ExecuteClose();
        }
        private void ExecuteClose()
        {
            CloseAction?.Invoke();
            Debug.Print(Dimensions.Rows);
            Debug.Print(Dimensions.Columns);
        }

    }
}
