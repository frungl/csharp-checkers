using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using checkers.Converters;
using checkers.Models;
using checkers.ViewModels;
namespace checkers.Views
{
    /// <inheritdoc />
    public partial class MainWindow : Window
    {
        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            
            MinWidth = 1000;
            MinHeight = 1024;
            CanResize = true;
            WindowState = WindowState.Maximized;
            Background = new SolidColorBrush(Color.Parse("#7bb5b8"));
            
            grid.Width = 800;
            grid.Height = 800;
            
            grid.ColumnDefinitions.Add(new ColumnDefinition(0.05, GridUnitType.Star));
            
            for(var i=0;i<8;i++)
            {
                grid.RowDefinitions.Add(new RowDefinition(0.11875, GridUnitType.Star));
                grid.ColumnDefinitions.Add(new ColumnDefinition(0.11875, GridUnitType.Star));
            }
            
            grid.RowDefinitions.Add(new RowDefinition(0.05, GridUnitType.Star));
            
            var buttons = new Button[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    
                    buttons[i, j] = new Button();
                    buttons[i, j].SetValue(Grid.RowProperty, i);
                    buttons[i, j].SetValue(Grid.ColumnProperty, j + 1);
                    buttons[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    buttons[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                    buttons[i, j].Padding = new Thickness(10);
                    buttons[i, j].Command = ((MainWindowViewModel)DataContext).SelectSquareCommand;
                    buttons[i, j].CommandParameter = new Coordinate(i, j);
                    
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
                        Path = "PiecesTypes[" + i + "][" + j + "]",
                        Converter = new ConverterToButtonContent()
                    };
                    buttons[i, j].Bind(ContentProperty, bindingPiecesTypes);

                    grid.Children.Add(buttons[i, j]);
                }
            }

            var horizontalCoordinates = new TextBlock[8];
            var verticalCoordinates = new TextBlock[8];

            for (var i = 0; i < 8; i++)
            {
                horizontalCoordinates[i] = new TextBlock();
                horizontalCoordinates[i].SetValue(Grid.ColumnProperty, i + 1);
                horizontalCoordinates[i].SetValue(Grid.RowProperty, 8);
                horizontalCoordinates[i].HorizontalAlignment = HorizontalAlignment.Center;
                horizontalCoordinates[i].VerticalAlignment = VerticalAlignment.Center;
                horizontalCoordinates[i].Text = ((char)(i + 'a')).ToString();
                horizontalCoordinates[i].FontWeight = FontWeight.Bold;
                horizontalCoordinates[i].FontSize = 20;
                grid.Children.Add(horizontalCoordinates[i]);
            }
            
            for (var i = 0; i < 8; i++)
            {
                verticalCoordinates[i] = new TextBlock();
                verticalCoordinates[i].SetValue(Grid.ColumnProperty, 0);
                verticalCoordinates[i].SetValue(Grid.RowProperty, i);
                verticalCoordinates[i].HorizontalAlignment = HorizontalAlignment.Center;
                verticalCoordinates[i].VerticalAlignment = VerticalAlignment.Center;
                verticalCoordinates[i].Text = (8 - i).ToString();
                verticalCoordinates[i].FontWeight = FontWeight.Bold;
                verticalCoordinates[i].FontSize = 20;
                grid.Children.Add(verticalCoordinates[i]);
            }

            var bindingText = new Binding 
            { 
                Source = (MainWindowViewModel)DataContext, 
                Path = "CurrentGameStatus",
                Converter = new ConverterGameStatusToText()
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