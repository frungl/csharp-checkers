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
        
        private ObservableCollection<ObservableCollection<Tile>> _gameTiles;
        
        public ObservableCollection<ObservableCollection<Tile>> GameTiles
        {
            get => _gameTiles;
            set => this.RaiseAndSetIfChanged(ref _gameTiles, value);
        }
        
        private ObservableCollection<ObservableCollection<Piece?>> _piecesColors;
        
        public ObservableCollection<ObservableCollection<Piece?>> PiecesColors
        {
            get => _piecesColors;
            set => this.RaiseAndSetIfChanged(ref _piecesColors, value);
        }

        private void UpdatePieces()
        {
            var tempPiecesColors = new ObservableCollection<ObservableCollection<Piece?>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                tempPiecesColors.Add(new ObservableCollection<Piece?>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    tempPiecesColors[i].Add(_board.GetPiece(i, j));
                }
            }
            PiecesColors = tempPiecesColors;
        }
        
        private void UpdateGameTiles()
        {
            var tempGameTiles = new ObservableCollection<ObservableCollection<Tile>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                tempGameTiles.Add(new ObservableCollection<Tile>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    if (_board.IsLightField(i, j))
                    {
                        tempGameTiles[i].Add(Tile.Light);
                    }
                    else
                    {
                        tempGameTiles[i].Add(Tile.Dark);
                    }
                }
            }
            if (_move != null)
            {
                tempGameTiles[_move.FromX][_move.FromY] = Tile.Selected;
                foreach (var (x, y) in _move.To)
                {
                    tempGameTiles[x][y] = Tile.Highlighted;
                }
            }
            GameTiles = tempGameTiles;
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
            UpdateGameTiles();
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
            UpdatePieces();
            UpdateGameTiles();
            SelectSquareCommand = ReactiveCommand.Create<Int32>(SelectSquare);
        }
    }
}