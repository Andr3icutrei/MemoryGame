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
        }

        public User(string username,bool isAdded,UInt16 imageIndex)
        {
            Username = username;
            IsAdded = isAdded;
            ImageIndex = imageIndex;
        }

        public User(User boundUser)
        {
            Username = boundUser.Username;
            ImageIndex = boundUser.ImageIndex;
            IsAdded = boundUser.IsAdded;
        }

        public User()
        {
            Username = String.Empty;
            ImageIndex = 0;
            IsAdded = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
