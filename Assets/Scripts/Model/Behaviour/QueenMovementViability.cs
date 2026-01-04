using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class QueenMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Queen;
        private BishopMovementViability _diagMove = new ();
        private RookMovementViability _horMove = new();

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords, Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);
            if (_diagMove.IsViableMove(new FigData(figureData, FigType.Bishop), startCoords, endCoords, boardState,
                    out abilityTrigger)) {
                return true;
            }
            
            if (_horMove.IsViableMove(new FigData(figureData, FigType.Rook), startCoords, endCoords, boardState,
                    out abilityTrigger)) {
                return true;
            }

            return false;
        }
    }
}