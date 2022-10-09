using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using checkers.Converters;
using checkers.ViewModels;
namespace checkers.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            
            MinWidth = 1000;
            MinHeight = 1024;
            CanResize = true;
            WindowState = WindowState.Maximized;
            Background = Brushes.Aquamarine;
            
            grid.Width = 800;
            grid.Height = 800;
            for(var i=0;i<8;i++)
            {
                grid.RowDefinitions.Add(new RowDefinition(0.125, GridUnitType.Star));
                grid.ColumnDefinitions.Add(new ColumnDefinition(0.125, GridUnitType.Star));
            }
            var buttons = new Button[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    
                    buttons[i, j] = new Button();
                    buttons[i, j].SetValue(Grid.RowProperty, i);
                    buttons[i, j].SetValue(Grid.ColumnProperty, j);
                    buttons[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    buttons[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                    buttons[i, j].Padding = new Thickness(10);
                    buttons[i, j].Command = ((MainWindowViewModel)DataContext).SelectSquareCommand;
                    buttons[i, j].CommandParameter = i * 8 + j;
                    
                    var bindingTileColor = new Binding 
                    { 
                        Source = (MainWindowViewModel)DataContext, 
                        Path = "GameTiles[" + i + "][" + j + "]",
                        Converter = new ConverterToTileColor(),
                    };
                    buttons[i, j].Bind(BackgroundProperty, bindingTileColor);

                    var bindingPiecesTypes = new Binding 
                    { 
                        Source = (MainWindowViewModel)DataContext, 
                        Path = "PiecesColors[" + i + "][" + j + "]",
                        Converter = new ConverterToButtonContent()
                    };
                    buttons[i, j].Bind(ContentProperty, bindingPiecesTypes);

                    grid.Children.Add(buttons[i, j]);
                }
            }
            
            var bindingText = new Binding 
            { 
                Source = (MainWindowViewModel)DataContext, 
                Path = "CurrentPlayer",
                Converter = new ConverterPlayerToText()
            };

            textBlock.Bind(TextBlock.TextProperty, bindingText);
            textBlock.FontSize = 20;
            textBlock.FontWeight = FontWeight.Bold;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}