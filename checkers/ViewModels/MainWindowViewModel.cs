using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Media;
using checkers.Models;
using ReactiveUI;

namespace checkers.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ObservableCollection<bool>> _squareTypes;
        
        public ObservableCollection<ObservableCollection<bool>> SquareTypes
        {
            get => _squareTypes;
            set => this.RaiseAndSetIfChanged(ref _squareTypes, value);
        }
        
        private ObservableCollection<ObservableCollection<bool>> _toSquares;
        
        public ObservableCollection<ObservableCollection<bool>> ToSquares
        {
            get => _toSquares;
            set => this.RaiseAndSetIfChanged(ref _toSquares, value);
        }
        
        
        private ObservableCollection<ObservableCollection<bool>> _fromSquares;
        
        public ObservableCollection<ObservableCollection<bool>> FromSquares
        {
            get => _fromSquares;
            set => this.RaiseAndSetIfChanged(ref _fromSquares, value);
        }
        

        private ObservableCollection<ObservableCollection<IBrush>> _piecesColors;
        
        public ObservableCollection<ObservableCollection<IBrush>> PiecesColors
        {
            get => _piecesColors;
            set => this.RaiseAndSetIfChanged(ref _piecesColors, value);
        }

        private void InitSquareColors()
        {
            SquareTypes = new ObservableCollection<ObservableCollection<bool>>();
            for (var i = 0; i < 8; i++)
            {
                SquareTypes.Add(new ObservableCollection<bool>());
                for (var j = 0; j < 8; j++)
                {
                    SquareTypes[i].Add((i + j) % 2 == 0);
                }
            }
        }
        
        private void InitFromToSquares()
        {
            ToSquares = new ObservableCollection<ObservableCollection<bool>>();
            FromSquares = new ObservableCollection<ObservableCollection<bool>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                ToSquares.Add(new ObservableCollection<bool>());
                FromSquares.Add(new ObservableCollection<bool>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    ToSquares[i].Add(false);
                    FromSquares[i].Add(false);
                }
            }
        }
        
        private void InitPiecesColors()
        {
            var darkPieceColor = new SolidColorBrush(Color.Parse("#000000"));
            var lightPieceColor = new SolidColorBrush(Color.Parse("#ffffff"));
            var transparent = new SolidColorBrush(Color.Parse("#00000000"));
            var board = new Board().GetBoardArray();
            PiecesColors = new ObservableCollection<ObservableCollection<IBrush>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                PiecesColors.Add(new ObservableCollection<IBrush>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    if (board[i, j] == null)
                    {
                        PiecesColors[i].Add(transparent);
                    }
                    else if (board[i, j]!.isLight)
                    {
                        PiecesColors[i].Add(lightPieceColor);
                    }
                    else
                    {
                        PiecesColors[i].Add(darkPieceColor);
                    }
                }
            }
        }
        
        public ReactiveCommand<Int32, Unit> SelectSquareCommand { get; }
        
        void SelectSquare(Int32 squareIndex)
        {
            var row = squareIndex / 8;
            var column = squareIndex % 8;
            FromSquares[row][column] = !FromSquares[row][column];
        }
        
        public MainWindowViewModel()
        {
            InitSquareColors();
            InitFromToSquares();
            InitPiecesColors();
            SelectSquareCommand = ReactiveCommand.Create<Int32>(SelectSquare);
        }
    }
}