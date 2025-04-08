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
        private UInt16 gamesPlayed;
        public UInt16 GamesPlayed
        {
            get { return gamesPlayed; }
            set
            {
                gamesPlayed = value;
                OnPropertyChanged(nameof(GamesPlayed));
            }
        }

        private UInt16 gamesWon;
        public UInt16 GamesWon
        {
            get { return gamesWon; }
            set
            {
                gamesWon = value;
                OnPropertyChanged(nameof(GamesWon));
            }
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private UInt16 imageIndex;
        public UInt16 ImageIndex
        {
            get => imageIndex;
            set
            {
                imageIndex = value;
                OnPropertyChanged(nameof(ImageIndex));
            }
        }
        public bool IsAdded { get; set; }

        public User(string username)
        {
            Username = username;
            ImageIndex = 0;
            IsAdded = true;
            GamesPlayed = 0;
            GamesWon = 0;
        }

        public User(string username,bool isAdded,UInt16 imageIndex,UInt16 gamesPlayed,UInt16 gamesWon)
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
