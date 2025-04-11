using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class BoardDimensions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Private fields
        private string _rows;
        private string _columns;
        private bool _areValidDimensions;
        #endregion

        #region Public Properties 
        public string Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public bool AreValidDimensions
        {
            get { return _areValidDimensions; }
            set
            {
                _areValidDimensions = value;
                OnPropertyChanged(nameof(AreValidDimensions));
            }
        }

        public string Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructors
        public BoardDimensions(string rows, string columns)
        {
            Rows = rows;
            Columns = columns;
            AreValidDimensions = false;
        }

        public BoardDimensions()
        {
            Rows = string.Empty;
            Columns = string.Empty;
            AreValidDimensions = false;
        }
        #endregion
    }

}
