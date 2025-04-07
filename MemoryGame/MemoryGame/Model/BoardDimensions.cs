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
        private string rows;
        private string columns;
        private bool areValidDimensions;
        public bool RowDimension { get; set; }
        public string Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }
        public bool AreValidDimensions
        {
            get { return areValidDimensions; }
            set
            {
                areValidDimensions = value;
                OnPropertyChanged(nameof(AreValidDimensions));
            }
        }
        public string Columns
        {
            get { return columns; }
            set 
            { 
                columns = value; 
                OnPropertyChanged(nameof(Columns));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
    }
}
