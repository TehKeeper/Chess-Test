using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public abstract class ChessMovementViabilityBase {
        protected abstract FigType Type { get; }

        private ChessMovementViabilityBase _nextNode;
        private bool _hasNextNode;

        public static readonly Vector2Int NonExistCoord = new Vector2Int(-1, -1);

        public bool IsViableMove(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState, out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            if (figureData.Type == Type)
                return MoveLogic(figureData, startCoords, endCoords, boardState, out abilityTrigger);

            if (_hasNextNode) {
                return _nextNode.IsViableMove(figureData, startCoords, endCoords, boardState, out abilityTrigger);
            }

            abilityTrigger = (NonExistCoord, FigAbilityType.None);
            return false;
        }

        public ChessMovementViabilityBase ThenUse(ChessMovementViabilityBase node) {
            _nextNode = node;
            _hasNextNode = _nextNode != null;
            return this;
        }


        protected abstract bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState, out (Vector2Int coord, FigAbilityType ability) abilityTrigger);

        protected static bool EndpointCheck(FigData figureData, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState) {
            return boardState[endCoords].Type == FigType.None || figureData.IsBlack != boardState[endCoords].IsBlack;
        }
    }

    public enum FigAbilityType {
        None,
        EnPassant,
        Castling
    }
}