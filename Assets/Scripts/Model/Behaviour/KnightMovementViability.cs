using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class KnightMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Knight;

        private static readonly Vector2Int[] PossibleSquares = new [] {
            new Vector2Int(1,2),
            new Vector2Int(2,1),
            
            new Vector2Int(-1,2),
            new Vector2Int(-2,1),
            
            new Vector2Int(1,-2),
            new Vector2Int(2,-1),
            
            new Vector2Int(-1,-2),
            new Vector2Int(-2,-1),
        };

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords, Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);
            
            for (int i = 0; i < PossibleSquares.Length; i++) {
                Debug.Log($"End Coords: {endCoords}, possible coords: {startCoords + PossibleSquares[i]}, conition: {startCoords + PossibleSquares[i] == endCoords}" );
                if (startCoords + PossibleSquares[i] == endCoords) {
                   
                    return EndpointCheck(figureData, endCoords, boardState);
                }
            }

            return false;
        }
    }
}