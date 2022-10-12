using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using checkers.Models;
using ReactiveUI;

namespace checkers.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Game _currentGame;
        private Move? _currentMove;
        
        private ObservableCollection<ObservableCollection<Tile>> _gameTiles;
        
        public ObservableCollection<ObservableCollection<Tile>> GameTiles
        {
            get => _gameTiles;
            set => this.RaiseAndSetIfChanged(ref _gameTiles, value);
        }
        
        private ObservableCollection<ObservableCollection<Piece?>> _piecesTypes;
        
        public ObservableCollection<ObservableCollection<Piece?>> PiecesTypes
        {
            get => _piecesTypes;
            set => this.RaiseAndSetIfChanged(ref _piecesTypes, value);
        }
        
        private GameStatus _currentGameStatus;
        
        public GameStatus CurrentGameStatus
        {
            get => _currentGameStatus;
            set => this.RaiseAndSetIfChanged(ref _currentGameStatus, value);
        }

        private void UpdatePieces()
        {
            var tempPiecesTypes = new ObservableCollection<ObservableCollection<Piece?>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                tempPiecesTypes.Add(new ObservableCollection<Piece?>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    tempPiecesTypes[i].Add(_currentGame.Board.GetPiece(new Coordinate(i, j)));
                }
            }
            PiecesTypes = tempPiecesTypes;
        }

        private void UpdateGameTiles()
        {
            var tempGameTiles = new ObservableCollection<ObservableCollection<Tile>>();
            for (var i = 0; i < Board.BoardSize; i++)
            {
                tempGameTiles.Add(new ObservableCollection<Tile>());
                for (var j = 0; j < Board.BoardSize; j++)
                {
                    tempGameTiles[i].Add(Board.IsLightField(new Coordinate(i, j)) ? Tile.Light : Tile.Dark);
                }
            }
            if (_currentMove != null)
            {
                tempGameTiles[_currentMove.From.X][_currentMove.From.Y] = Tile.Selected;
                foreach (var (x, y) in _currentMove.To)
                {
                    tempGameTiles[x][y] = Tile.Highlighted;
                }
            }
            GameTiles = tempGameTiles;
        }
        
        private void UpdateGameStatus()
        {
            CurrentGameStatus = _currentGame.GetGameStatus();
        }
        
        private void UpdateMove(Coordinate moveTo)
        {
            if(_currentMove != null)
            {
                if (_currentMove.IsPossible(moveTo))
                {
                    var movePiece = _currentGame.Board.GetPiece(_currentMove.From);
                    _currentMove = _currentGame.ApplyMove(movePiece!, moveTo);
                    UpdatePieces();
                    UpdateGameStatus();
                }
                else
                {
                    if (_currentMove.From == moveTo)
                    {
                        if(_currentGame.IsPossibleMove(null)) 
                            _currentMove = null;
                    }
                    else
                    {
                        var piece = _currentGame.Board.GetPiece(moveTo);
                        Move? tempMove = null;
                        if (piece != null)
                            tempMove = piece.GetMoves(_currentGame.Board);
                        if (_currentGame.IsPossibleMove(tempMove))
                            _currentMove = tempMove;
                    }
                }
            }
            else
            {
                var piece = _currentGame.Board.GetPiece(moveTo);
                Move? tempMove = null;
                if (piece != null)
                    tempMove = piece.GetMoves(_currentGame.Board);
                if (_currentGame.IsPossibleMove(tempMove))
                {
                    _currentMove = tempMove;
                }
            }
            UpdateGameTiles();
        }
        
        public ReactiveCommand<Coordinate, Unit> SelectSquareCommand { get; }

        private void SelectSquare(Coordinate coordinate)
        {
            if(_currentGameStatus == GameStatus.DarkPlayerTurn || _currentGameStatus == GameStatus.LightPlayerTurn) 
                UpdateMove(coordinate);
        }
        
        public MainWindowViewModel()
        {
            _currentGame = new Game(null);
            _currentMove = null;
            _gameTiles = new ObservableCollection<ObservableCollection<Tile>>();
            _piecesTypes = new ObservableCollection<ObservableCollection<Piece?>>();
            _currentGameStatus = _currentGame.GetGameStatus();
            UpdatePieces();
            UpdateGameTiles();
            SelectSquareCommand = ReactiveCommand.Create<Coordinate>(SelectSquare);
        }
    }
}