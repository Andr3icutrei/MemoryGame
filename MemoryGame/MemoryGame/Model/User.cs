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
        public UInt16 ImageIndex { get;set ; }

        public User(string username)
        {
            Username = username;
            ImageIndex = 0;
        }

        public User()
        {
            Username = string.Empty;
            ImageIndex = 0;
        }
    }
}
