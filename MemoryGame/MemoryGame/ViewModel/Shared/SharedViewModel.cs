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
        #region Private fields

        private User boundUser;

        #endregion

        #region Public properties 
        public User BoundUser
        {
            get => boundUser;
            set
            {
                boundUser = value;
                OnPropertyChanged(nameof(BoundUser));
            }
        }
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region Constructors
        public SharedViewModel(User u)
        {
            BoundUser = u;
        }

        public SharedViewModel()
        {
            BoundUser = new User();
        }
        #endregion
    }
}
