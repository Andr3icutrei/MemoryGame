using MemoryGame.ViewModel.GameCellControl;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for GameCellView.xaml
    /// </summary>
    public partial class GameCellControl : UserControl
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(GameCellControl),
            new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
    DependencyProperty.Register(
        "CommandParameter",
        typeof(object),
        typeof(GameCellControl),
        new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public bool IsCardFaceDown
        {
            get { return (bool)GetValue(IsCardFaceDownProperty); }
            set { SetValue(IsCardFaceDownProperty, value); }
        }
        public static readonly DependencyProperty IsCardFaceDownProperty =
            DependencyProperty.Register("IsCardFaceDown", typeof(bool), typeof(GameCellControl), new PropertyMetadata(true));
        public bool IsCardFaceUp
        {
            get { return (bool)GetValue(IsCardFaceUpProperty); }
            set { SetValue(IsCardFaceUpProperty, value); }
        }
        public static readonly DependencyProperty IsCardFaceUpProperty =
            DependencyProperty.Register("IsCardFaceUp", typeof(bool), typeof(GameCellControl), new PropertyMetadata(true));

        public ICommand FlipCommand
        {
            get { return (ICommand)GetValue(FlipCommandProperty); }
            set { SetValue(FlipCommandProperty, value); }
        }
        public static readonly DependencyProperty FlipCommandProperty =
            DependencyProperty.Register("FlipCommand", typeof(ICommand), typeof(GameCellControl), new PropertyMetadata(null));
        public ImageSource FrontCardImageSource
        {
            get { return (ImageSource)GetValue(FrontCardImageSourceProperty); }
            set { SetValue(FrontCardImageSourceProperty, value); }
        }

        public static readonly DependencyProperty FrontCardImageSourceProperty =
            DependencyProperty.Register("FrontCardImageSource", typeof(ImageSource), typeof(GameCellControl), new PropertyMetadata(null));

        public GameCellControl()
        {
            InitializeComponent();
        }
    }
}
