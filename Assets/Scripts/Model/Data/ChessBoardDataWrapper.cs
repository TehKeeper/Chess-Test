using System.Collections.Generic;
using UnityEngine;

namespace Model.Data {
    [System.Serializable]
    internal struct ChessBoardDataWrapper {
        public bool CurrentTurn;
        public FigData[] ChessPieces;

        public ChessBoardDataWrapper(bool currentTurn, Dictionary<Vector2Int, FigData> boardState) {
            List<FigData> tmpFigData = new();
            foreach (FigData figure in boardState.Values) {
                tmpFigData.Add(figure);
            }

            ChessPieces = tmpFigData.ToArray();
            CurrentTurn = currentTurn;
        }
    }
}