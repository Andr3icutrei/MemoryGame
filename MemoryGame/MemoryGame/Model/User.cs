using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class User
    {
        public string Username { get; set; }
        public string ImageName { get;set ; }

        public User(string username,string imageName)
        {
            Username = username;
            ImageName = imageName;
        }

        public User()
        {
            Username = string.Empty;
            ImageName = string.Empty;
        }
    }
}
