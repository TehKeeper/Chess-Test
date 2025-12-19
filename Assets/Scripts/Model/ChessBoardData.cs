using System;
using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model {
    [System.Serializable]
    public class ChessBoardData {
        private Dictionary<Vector2Int, FigData> _boardState = new();
        public Dictionary<Vector2Int, FigData> BoardState => _boardState;

        private bool _currentTurnBlack;

        public bool CurrentTurnBlack {
            get { return _currentTurnBlack; }
            set {
                _currentTurnBlack = value;
                OnTurnChange?.Invoke(_currentTurnBlack);
            }
        }

        public event Action<bool> OnTurnChange;

        private static readonly FigType[] MainPieces = new[] {
            FigType.Rook, FigType.Knight, FigType.Bishop, FigType.Queen, FigType.King, FigType.Bishop,
            FigType.Knight, FigType.Rook,
        };

        public void ResetBoard() {
            CurrentTurnBlack = false;
            
            for (int len = 0; len < 64; len++) {
                _boardState[new Vector2Int(len % 8, len / 8)] = default;
            }
        }

        public void SetupStandardGame() {
            ResetBoard();

            for (int i = 0; i < 8; i++) {
                PutToBoard(MainPieces[i], false, new Vector2Int(i, 0));
                PutToBoard(FigType.Pawn, false, new Vector2Int(i, 1));

                PutToBoard(MainPieces[i], true, new Vector2Int(i, 7));
                PutToBoard(FigType.Pawn, true, new Vector2Int(i, 6));
            }
        }

        public void PutToBoard(FigType type, bool isBlack, Vector2Int position) {
            FigData piece = new FigData(type, isBlack, position);
            _boardState[position] = piece;
        }

        public bool IsValidMove(Vector2Int startPoint, Vector2Int endPoint) {
            if (!IsInBounds(startPoint) || !IsInBounds(endPoint)) return false;
            if (startPoint == endPoint) return false;

            FigData piece = GetFigAt(startPoint);
            if (piece.Type == FigType.None) return false;

            FigData target = GetFigAt(endPoint);
            if (target.Type != FigType.None && target.IsBlack == piece.IsBlack) return false;

            return true;
        }

        public void MovePiece(Vector2Int startPoint, Vector2Int endPoint) {
            FigData piece = GetFigAt(startPoint);
            if (piece.Type == FigType.None) return;

            if (!IsValidMove(startPoint, endPoint))
                return;

            _boardState[startPoint] = new FigData(FigType.None, false, startPoint);
            piece.Coordinates = endPoint;
            _boardState[endPoint] = piece;

            _boardState[endPoint] = new FigData(piece, startPoint);

            //invoke here putting figure to graveyard later

            CurrentTurnBlack = !CurrentTurnBlack;
        }

        public FigData GetFigAt(Vector2Int position) {
            return _boardState.TryGetValue(position, out var piece) ? piece : default;
        }

        private bool IsInBounds(Vector2Int position) {
            return position.x is >= 0 and < 8 && position.y is >= 0 and < 8;
        }


        public void SetBoardData(Dictionary<Vector2Int, FigData> loadedData, bool currentTurnIsBlack) {
            _boardState = loadedData;
            CurrentTurnBlack = currentTurnIsBlack;
        }
    }
}