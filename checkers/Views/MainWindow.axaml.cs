using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
using checkers.Converters;
using checkers.Models;
using checkers.ViewModels;
using Brush = System.Drawing.Brush;

namespace checkers.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            var canvas = new Canvas
            {
                Width = 400,
                Height = 400
            };
            var grid = new Grid();
            for(var i=0;i<8;i++)
            {
                grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
                grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Auto));
            }
            var buttons = new Button[8, 8];
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].SetValue(Grid.RowProperty, i);
                    buttons[i, j].SetValue(Grid.ColumnProperty, j);
                    buttons[i, j].Width = 50;
                    buttons[i, j].Height = 50;
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
            Content = grid;
        }
    }
}