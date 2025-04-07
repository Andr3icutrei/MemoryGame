using MemoryGame.ViewModel.SelectBoardDimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.Services
{
    public class WindowService : IWindowService
    {
        public void ShowWindow<TViewModel>(object[] constructorArgs, Action onClose = null) where TViewModel : class
        {
            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), constructorArgs);

            var windowType = GetWindowTypeFromViewModel(typeof(TViewModel));
            if (windowType == null)
            {
                throw new InvalidOperationException($"No matching window found for {typeof(TViewModel).Name}");
            }

            var window = (Window)Activator.CreateInstance(windowType, constructorArgs);
            window.DataContext = viewModel;

            if (viewModel is SelectBoardDimensionsViewModel vm)
            {
                vm.RequestClose = () => window.Close();
            }

            if (onClose != null)
            {
                window.Closed += (s, e) => onClose.Invoke();
            }

            window.Show();
        }

        private Type GetWindowTypeFromViewModel(Type viewModelType)
        {
            string viewName = viewModelType.Name.Replace("ViewModel", "Window");
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == viewName);
        }
    }

}
