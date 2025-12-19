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
}