using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameCell
    {
        public UInt16 Row { get; set; }
        public UInt16 Col { get; set; }
        public UInt16 ImageIndex { get; set; }
    }
}
