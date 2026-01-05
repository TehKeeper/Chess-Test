using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class KingMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.King;
        private static ChessMovementViabilityBase _threats = new ComplexMovementViability();

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            abilityTrigger = (NonExistCoord, FigAbilityType.None);

            if (CheckForCastling(figureData,  endCoords, boardState, out Vector2Int rookCoords)) {
                abilityTrigger = (rookCoords, FigAbilityType.Castling);
                return true;
            }

            if ((endCoords - startCoords).sqrMagnitude == 1 && !Threatened(figureData, endCoords, boardState)) {
                return EndpointCheck(figureData, endCoords, boardState);
            }

            return false;
        }

        private bool Threatened(FigData figureData, Vector2Int kingEndPosition,
            Dictionary<Vector2Int, FigData> boardState) {
            (Vector2Int coord, FigAbilityType ability) placeholder;

            foreach (var figure in boardState) {
                if (figure.Value.Type == FigType.None || figure.Value.IsBlack == figureData.IsBlack)
                    continue;

                if (_threats.IsViableMove(figure.Value, figure.Key, kingEndPosition, boardState, out placeholder))
                    return true;
            }

            return false;
        }

        private bool CheckForCastling(FigData king, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState, out Vector2Int newRookPos) {

            newRookPos = NonExistCoord;
            if (king.PreviousCoordinates != NonExistCoord)
                return false;
            
            if (endCoords.y != king.Coordinates.y || endCoords.x != king.Coordinates.x + 2 || endCoords.x != king.Coordinates.x - 2)
                return false;


            //KingSide
            if (endCoords.x == king.Coordinates.x + 2) {
                for (int i = king.Coordinates.x + 1; i < king.Coordinates.x + 3; i++) {
                    if (!CanPerformCastlingMove(king, boardState, i)) return false;
                }

                newRookPos = endCoords - Vector2Int.right;
                return true;
            }

            //QueenSide
            if (endCoords.x == king.Coordinates.x - 2) {
                if (boardState[new Vector2Int(king.Coordinates.x - 3, king.Coordinates.y)].Type != FigType.None)
                    return false;
                
                for (int i = king.Coordinates.x - 1; i < king.Coordinates.x - 3; i++) {
                    if (!CanPerformCastlingMove(king, boardState, i)) return false;
                }
                newRookPos = endCoords + Vector2Int.right;
                return true;
            }

            return false;
        }

        private bool CanPerformCastlingMove(FigData king, Dictionary<Vector2Int, FigData> boardState, int i) {
            Vector2Int newCoords = new Vector2Int(i, king.Coordinates.y);
            if (boardState[newCoords].Type != FigType.None)
                return false;

            if (Threatened(king, newCoords, boardState))
                return false;
            return true;
        }
    }
}