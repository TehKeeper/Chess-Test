using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class BishopMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Bishop;

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);

            if (startCoords == endCoords)
                return false;

            int diffX = endCoords.x - startCoords.x;
            int diffY = endCoords.y - startCoords.y;

            if (diffX != diffY && diffX != -diffY)
                return false;


            Vector2Int delta = new ((endCoords.x > startCoords.x) ? 1 : -1,
                (endCoords.y > startCoords.y) ? 1 : -1);
            
            Vector2Int current = startCoords + delta;
            while (current != endCoords) {
                if (boardState[current].Type != FigType.None)
                    return false;

                current += delta;
            }

            return EndpointCheck(figureData, endCoords, boardState);;
        }
    }
}