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
        private Game _currentGame;
        private Move? _currentMove;
        
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
                    tempPiecesColors[i].Add(_currentGame._board.GetPiece(i, j));
                }
            }
            PiecesColors = tempPiecesColors;
        }
        
        private Player _currentPlayer;
        
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set => this.RaiseAndSetIfChanged(ref _currentPlayer, value);
        }

        private void UpdateGameTiles()
        {
            var tempGameTiles = new ObservableCollection<ObservableCollection<Tile>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                tempGameTiles.Add(new ObservableCollection<Tile>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    if (_currentGame._board.IsLightField(i, j))
                    {
                        tempGameTiles[i].Add(Tile.Light);
                    }
                    else
                    {
                        tempGameTiles[i].Add(Tile.Dark);
                    }
                }
            }
            if (_currentMove != null)
            {
                tempGameTiles[_currentMove.FromX][_currentMove.FromY] = Tile.Selected;
                foreach (var (x, y) in _currentMove.To)
                {
                    tempGameTiles[x][y] = Tile.Highlighted;
                }
            }
            GameTiles = tempGameTiles;
        }
        
        private void UpdateCurrentPlayer()
        {
            CurrentPlayer = _currentGame.GetCurrentPlayer();
        }
        
        private void UpdateMove(int x, int y)
        {
            if(_currentMove != null)
            {
                if (_currentMove.IsPossible(x, y))
                {
                    var movePiece = _currentGame._board.GetPiece(_currentMove.FromX, _currentMove.FromY);
                    _currentMove = _currentGame.ApplyMove(movePiece!, x, y);
                    UpdatePieces();
                    UpdateCurrentPlayer();
                }
                else
                {
                    if (_currentMove.FromX == x && _currentMove.FromY == y)
                    {
                        if(_currentGame.IsPossibleMove(null)) 
                            _currentMove = null;
                    }
                    else
                    {
                        var piece = _currentGame._board.GetPiece(x, y);
                        Move? tempMove = null;
                        if (piece != null)
                            tempMove = piece.getMoves(_currentGame._board);
                        if (_currentGame.IsPossibleMove(tempMove))
                            _currentMove = tempMove;
                    }
                }
            }
            else
            {
                var piece = _currentGame._board.GetPiece(x, y);
                Move? tempMove = null;
                if (piece != null)
                    tempMove = piece.getMoves(_currentGame._board);
                if (_currentGame.IsPossibleMove(tempMove))
                {
                    _currentMove = tempMove;
                }
            }
            UpdateGameTiles();
        }
        
        public ReactiveCommand<Int32, Unit> SelectSquareCommand { get; }

        void SelectSquare(Int32 singleCoord)
        {
            var (x, y) = _currentGame._board.ToCoords(singleCoord);
            UpdateMove(x, y);
        }
        
        public MainWindowViewModel()
        {
            _currentGame = new Game();
            _currentMove = null;
            _currentPlayer = _currentGame.GetCurrentPlayer();
            UpdatePieces();
            UpdateGameTiles();
            SelectSquareCommand = ReactiveCommand.Create<Int32>(SelectSquare);
        }
    }
}