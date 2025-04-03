using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MemoryGame.ViewModel.SelectBoardDimensions;
using MemoryGame.Model;
using System.Diagnostics;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for SelectBoardDimensionsWindow.xaml
    /// </summary>
    public partial class SelectBoardDimensionsWindow : Window
    {
        public SelectBoardDimensionsWindow(BoardDimensions boardDimensions)
        {
            InitializeComponent();
            DataContext = new SelectBoardDimensionsViewModel(boardDimensions);
            var viewModel = (SelectBoardDimensionsViewModel)DataContext;
            viewModel.CloseAction = () => this.Close();
        }
    }
}
