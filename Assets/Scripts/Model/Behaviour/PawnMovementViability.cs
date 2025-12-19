using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class PawnMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.Pawn;

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState, out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            int deltaX = endCoords.x - startCoords.x;
            int deltaY = endCoords.y - startCoords.y;
            abilityTrigger = (NonExistCoord, FigAbilityType.None);
            
            
            int deltaMp = figureData.IsBlack ? -1 : 1;
            int gameStartY = figureData.IsBlack ? 6 : 1;
            if (deltaX == 0) {
                if (deltaY == deltaMp || startCoords.y == gameStartY && deltaY == 2 * deltaMp) {
                    return boardState[endCoords].Type == FigType.None;
                }
            }

            if (deltaX == 1 || deltaX == -1 && deltaY == deltaMp) {
                //capture
                if (boardState[endCoords].Type!=FigType.None && boardState[endCoords].IsBlack != figureData.IsBlack) {
                    
                    return true;
                }

                //En Passant
                
                Vector2Int enPassantTarget = new Vector2Int(endCoords.x, startCoords.y);
                FigData targetPawn = boardState[enPassantTarget];
                if (targetPawn.Type == FigType.Pawn && targetPawn.IsBlack != figureData.IsBlack) {
                    bool targetEaten = targetPawn.PreviousCoordinates.y - enPassantTarget.y == 2 * deltaMp;
                    if (targetEaten)
                        abilityTrigger = (enPassantTarget, FigAbilityType.EnPassant);
                    return targetEaten;
                }
            }


            return false;
        }
    }
}