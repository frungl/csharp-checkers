using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Media;
using checkers.Models;
using ReactiveUI;

namespace checkers.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Board _board;
        private Move? _move;
        
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
            for (var i = 0; i < Board.BoardSize; i++)
            {
                SquareTypes.Add(new ObservableCollection<bool>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    SquareTypes[i].Add(_board.IsLightField(i, j));
                }
            }
        }
        
        private void UpdatePieces()
        {
            var darkPieceColor = new SolidColorBrush(Color.Parse("#000000"));
            var lightPieceColor = new SolidColorBrush(Color.Parse("#ffffff"));
            var transparent = new SolidColorBrush(Color.Parse("#00000000"));
            PiecesColors = new ObservableCollection<ObservableCollection<IBrush>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                PiecesColors.Add(new ObservableCollection<IBrush>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    if (_board.GetPiece(i, j) == null)
                    {
                        PiecesColors[i].Add(transparent);
                    }
                    else if (_board.GetPiece(i, j)!.IsLight())
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
        
        private void UpdateFromToSquares()
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
            if (_move != null)
            {
                FromSquares[_move.FromX][_move.FromY] = true;
                foreach (var (x, y) in _move.To)
                {
                    ToSquares[x][y] = true;
                }
            }
        }
        
        void UpdateMove(int x, int y)
        {
            if(_move != null)
            {
                if (_move.IsPossible(x, y))
                {
                    var movePiece = _board.GetPiece(_move.FromX, _move.FromY);
                    _board.ApplyMove(movePiece!, x, y, _move.IsTaking);
                    _move = null;
                    UpdatePieces();
                }
                else
                {
                    if (_move.FromX == x && _move.FromY == y)
                    {
                        _move = null;
                    }
                    else
                    {
                        var piece = _board.GetPiece(x, y);
                        if (piece != null)
                            _move = piece.getMoves(_board);
                        else
                            _move = null;
                    }
                }
            }
            else
            {
                var piece = _board.GetPiece(x, y);
                if (piece != null)
                    _move = piece.getMoves(_board);
            }
            UpdateFromToSquares();
        }
        
        public ReactiveCommand<Int32, Unit> SelectSquareCommand { get; }

        void SelectSquare(Int32 singleCoord)
        {
            var (x, y) = _board.ToCoords(singleCoord);
            UpdateMove(x, y);
        }
        
        public MainWindowViewModel()
        {
            _board = new Board();
            _move = null;
            InitSquareColors();
            UpdatePieces();
            UpdateFromToSquares();
            SelectSquareCommand = ReactiveCommand.Create<Int32>(SelectSquare);
        }
    }
}