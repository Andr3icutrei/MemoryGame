using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModel.StatisticsWindow
{
    public class StatisticsWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

        public StatisticsWindowViewModel(ObservableCollection<User> users)
        {
            this.Users = users; 
        }
    }
}
