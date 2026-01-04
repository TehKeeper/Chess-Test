using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model.Behaviour {
    public class ComplexMovementViability : ChessMovementViabilityBase {
        protected override FigType Type => FigType.None;

        private static readonly ChessMovementViabilityBase MovementLogic = new PawnMovementViability()
            .ThenUse(new RookMovementViability())
            .ThenUse(new KnightMovementViability())
            .ThenUse(new BishopMovementViability())
            .ThenUse(new QueenMovementViability())
            .ThenUse(new KingMovementViability());

        protected override bool MoveLogic(FigData figureData, Vector2Int startCoords, Vector2Int endCoords,
            Dictionary<Vector2Int, FigData> boardState,
            out (Vector2Int coord, FigAbilityType ability) abilityTrigger) {
            return MovementLogic.IsViableMove(figureData, startCoords, endCoords, boardState, out abilityTrigger);
        }
    }
}