using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Services
{
    public interface IWindowService
    {
        #region Methods
        public void ShowWindow<TViewModel>(object[] constructorArgs, Action onClose = null) where TViewModel : class;
        #endregion
    }
}
