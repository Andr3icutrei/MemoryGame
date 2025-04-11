using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class User : INotifyPropertyChanged
    {
        #region Private fields
        private UInt16 _gamesPlayed;
        private UInt16 _gamesWon;
        private string _username;
        private UInt16 _imageIndex;
        #endregion

        #region Public properties
        public bool IsAdded { get; set; }

        public UInt16 GamesPlayed
        {
            get { return _gamesPlayed; }
            set
            {
                _gamesPlayed = value;
                OnPropertyChanged(nameof(GamesPlayed));
            }
        }

        public UInt16 GamesWon
        {
            get { return _gamesWon; }
            set
            {
                _gamesWon = value;
                OnPropertyChanged(nameof(GamesWon));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public UInt16 ImageIndex
        {
            get => _imageIndex;
            set
            {
                _imageIndex = value;
                OnPropertyChanged(nameof(ImageIndex));
            }
        }
        #endregion

        #region Constructors
        public User(string username)
        {
            Username = username;
            ImageIndex = 0;
            IsAdded = true;
            GamesPlayed = 0;
            GamesWon = 0;
        }

        public User(string username, bool isAdded, UInt16 imageIndex, UInt16 gamesPlayed, UInt16 gamesWon)
        {
            Username = username;
            IsAdded = isAdded;
            ImageIndex = imageIndex;
            GamesPlayed = gamesPlayed;
            GamesWon = gamesWon;
        }

        public User(User boundUser)
        {
            Username = boundUser.Username;
            ImageIndex = boundUser.ImageIndex;
            IsAdded = boundUser.IsAdded;
            GamesWon = boundUser.GamesWon;
            GamesPlayed = boundUser.GamesPlayed;
        }

        public User()
        {
            Username = String.Empty;
            ImageIndex = 0;
            IsAdded = true;
            GamesPlayed = 0;
            GamesWon = 0;
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
