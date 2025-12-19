using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class PawnMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Pawn;
        
        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState) {


            int deltaX = endCoords.x - startCoords.x;
            int deltaY = endCoords.y - startCoords.y;

            int deltaMp = figureData.IsBlack ? -1 : 1;
            int gameStartY = figureData.IsBlack ? 6 : 1;
            if (deltaX == 0) {
                if (deltaY == deltaMp || startCoords.y == gameStartY && deltaY == 2 * deltaMp) {
                    return boardState[endCoords].Type == FigType.None;
                }
            }

            if (deltaX == 1 || deltaX == -1 && deltaY == deltaMp) {
                //capture
                if (boardState[endCoords].IsBlack != figureData.IsBlack) {
                    return true;
                }

                //En Passant
                Vector2Int enPassantTarget = new Vector2Int(endCoords.x, startCoords.y);
                if (boardState.TryGetValue(enPassantTarget, out FigData targetPawn) &&
                    targetPawn.Type == FigType.Pawn &&
                    targetPawn.IsBlack != figureData.IsBlack &&
                    enPassantTarget.y - targetPawn.PreviousCoordinates.y == 2 * deltaMp) {
                    return true;
                }
            }


            return false;
        }
    }
}