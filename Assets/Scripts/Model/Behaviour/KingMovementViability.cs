using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class KingMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.King;
        private static ChessMovementViabilityBase _threats = new ComplexMovementViability();

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords, Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);

            if (CheckForCastling(startCoords, boardState)) {
                abilityTrigger = (endCoords, FigAbilityType.Castling);
                return true;
            }

            if ((endCoords - startCoords).sqrMagnitude == 1 && !Threatened(figureData,endCoords, boardState)) {
                return EndpointCheck(figureData, endCoords, boardState);
            }

            return false;
        }

        private bool Threatened(FigData figureData, Vector2Int kingEndPosition, Dictionary<Vector2Int, FigData> boardState) {
            (Vector2Int coord, FigAbilityType ability) placeholder;
             
            foreach (var figure in boardState) {
                if(figure.Value.Type == FigType.None || figure.Value.IsBlack == figureData.IsBlack)
                    continue;

                if (_threats.IsViableMove(figure.Value, figure.Key, kingEndPosition, boardState, out placeholder))
                    return true;
            }

            return false;
        }

        private bool CheckForCastling(Vector2Int startCoords, Dictionary<Vector2Int, FigData> boardState) {
            //todo
            return false;
        }
    }
}