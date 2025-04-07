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
        public UInt16 Column { get; set; }
        public UInt16 ImageIndex { get; set; }

        public GameCell(UInt16 row, UInt16 col, UInt16 imageIndex)
        {
            Row = row;
            Column = col;
            ImageIndex = imageIndex;    
        }

        public GameCell()
        {
            Row = Column = 0;
        }

        public override string ToString()
        {
            return Row.ToString() + " " + Column.ToString()+" "+ ImageIndex.ToString();
        }
    }
}
