using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Model;

namespace MemoryGame.ViewModel.Shared
{
    public class SharedViewModel: INotifyPropertyChanged
    {
        private User boundUser;
        public User BoundUser
        {
            get => boundUser;
            set
            {
                boundUser = value;
                OnPropertyChanged(nameof(BoundUser));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SharedViewModel(User u)
        {
            BoundUser = u;
        }
    }
}
