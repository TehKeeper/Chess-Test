using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model {
    public class SaveTool {
        public string WrapState(Dictionary<Vector2Int, FigData> boardState, bool isBlackTurn) {
            ChessBoardDataWrapper wrapper = new ChessBoardDataWrapper(isBlackTurn, boardState);


            return JsonUtility.ToJson(wrapper);
        }

        public Dictionary<Vector2Int, FigData> UnwrapState(string json, out bool currentTurnIsBlack) {
            if (string.IsNullOrEmpty(json)) {
                Debug.LogError("Saved data is empty! Restore default chess board");
            }
            
            
            Dictionary<Vector2Int, FigData> loadedData = new();

            ChessBoardDataWrapper wrapper = JsonUtility.FromJson<ChessBoardDataWrapper>(json);
            currentTurnIsBlack = wrapper.CurrentTurn;

            FigData cachedData;
            foreach (FigData figData in wrapper.ChessPieces) {
                cachedData = new FigData(figData.Type, figData.IsBlack, figData.Coordinates);

                loadedData[cachedData.Coordinates] = cachedData;
            }

            return loadedData;
        }
    }
}