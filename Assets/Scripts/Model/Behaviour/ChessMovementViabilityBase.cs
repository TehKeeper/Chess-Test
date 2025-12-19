using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public abstract class ChessMovementViabilityBase {
        protected abstract FigType Type { get; }

        private ChessMovementViabilityBase _nextNode;
        private bool _hasNextNode;

        public bool IsViableMove(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState) {
            if (figureData.Type == Type)
                return MoveLogic(figureData, startCoords, endCoords, boardState);

            if (_hasNextNode) {
                return _nextNode.IsViableMove(figureData, startCoords, endCoords, boardState);
            }

            return false;
        }

        public ChessMovementViabilityBase ThenUse(ChessMovementViabilityBase node) {
            _nextNode = node;
            _hasNextNode = _nextNode != null;
            return this;
        }


        protected abstract bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState);
    }

    public class PawnMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Pawn;

        // Additional parameters that could be added to method signature if needed:
        // - bool isBlack: to determine movement direction (true for black, false for white)
        // - bool canCapture: if the move is a capture move
        // - bool isFirstMove: if it's the pawn's first move
        // - FigData[,] boardState: to check for pieces in the way and captures

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState) {
            // For now, we'll assume:
            // - White pawns move up (increasing y)
            // - Black pawns move down (decreasing y)
            // - This method only validates the movement pattern, not board state or captures

            if (startCoords == endCoords)
                return false;

            int deltaX = endCoords.x - startCoords.x;
            int deltaY = endCoords.y - startCoords.y;

            int deltaMp = figureData.IsBlack ? -1 : 1;
            int gameStartY = figureData.IsBlack ? 6 : 1;
            if (deltaX == 0) {
                if (deltaY == deltaMp || startCoords.y == gameStartY && deltaY == 2 * deltaMp) {
                    return true;
                }
            }

            if (deltaX == 1 && (deltaY == 1 || deltaY == -1)) {
                // Note: This assumes the capture is valid
                // In a full implementation, you would check if there's an opponent's piece to capture
                return true;
            }


            return false;
        }
    }
}