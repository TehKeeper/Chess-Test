using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class RookMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Rook;

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState, out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);
            if (startCoords.x != endCoords.x && startCoords.y != endCoords.y)
                return false;

            int deltaX = Mathf.Clamp(endCoords.x - startCoords.x, -1, 1);
            int deltaY = Mathf.Clamp(endCoords.y - startCoords.y, -1, 1);

            Vector2Int current = new Vector2Int(startCoords.x + deltaX, startCoords.y + deltaY);
            while (current != endCoords) {
                if (boardState[current].Type != FigType.None)
                    return false;

                current.x += deltaX;
                current.y += deltaY;
            }

            if (CastlingConditions(startCoords, endCoords, boardState,
                    out (Vector2Int coord, FigAbilityType ability) castlingTrigger)) {
                abilityTrigger = castlingTrigger;
                return true;
            }

            return EndpointCheck(figureData, endCoords, boardState);
        }

        private bool CastlingConditions(Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) castlingTrigger) {
            
            
            //Castling requires to check if King and Rook moved, which cannot be checked simply by defining king and rook positions - they need to marked as moved somehow,
            //Also King cannot move under Check - means, it cannot be threatened by opposing color pieces, so I'll leave this logic for later.
            castlingTrigger = (new Vector2Int(-1, -1), FigAbilityType.None);
            return false;
        }
    }
}