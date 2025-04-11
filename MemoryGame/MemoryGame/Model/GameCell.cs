using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameCell
    {
        #region Private fields
        private UInt16 _row;
        private UInt16 _column;
        private UInt16 _imageIndex;
        #endregion

        #region Public Properties
        public UInt16 Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public UInt16 Column
        {
            get { return _column; }
            set { _column = value; }
        }

        public UInt16 ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }
        #endregion

        #region Constructors
        public GameCell(UInt16 row, UInt16 col, UInt16 imageIndex)
        {
            _row = row;
            _column = col;
            _imageIndex = imageIndex;
        }

        public GameCell()
        {
            _row = _column = 0;
            _imageIndex = 0;
        }
        #endregion

        public override string ToString()
        {
            return _row.ToString() + " " + _column.ToString() + " " + _imageIndex.ToString();
        }
    }

}
