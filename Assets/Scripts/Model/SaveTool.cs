using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Model {
    public class SaveTool {
        private const string SAVE_KEY = "Chessboard_Quicksave";
        public void SaveBoard(ChessBoardData boardData) {
            PlayerPrefs.SetString(SAVE_KEY, WrapState(boardData.BoardState, boardData.CurrentTurnBlack));
        }
        
        
        private string WrapState(Dictionary<Vector2Int, FigData> boardState, bool isBlackTurn) {
            ChessBoardDataWrapper wrapper = new ChessBoardDataWrapper(isBlackTurn, boardState);


            return JsonUtility.ToJson(wrapper);
        }

        public void LoadBoard(ChessBoardData boardData) {
            if(!PlayerPrefs.HasKey(SAVE_KEY))
                return;
            
            UnwrapState(PlayerPrefs.GetString(SAVE_KEY), out bool turnIsBlack, out Dictionary<Vector2Int, FigData> loadedData);
            boardData.SetBoardData(loadedData, turnIsBlack);

        }
        
        private void UnwrapState(string json, out bool currentTurnIsBlack, out  Dictionary<Vector2Int, FigData> loadedData) {
            if (string.IsNullOrEmpty(json)) {
                Debug.LogError("Saved data is empty! Restore default chess board");
            }
            
           loadedData = new();

            ChessBoardDataWrapper wrapper = JsonUtility.FromJson<ChessBoardDataWrapper>(json);
            currentTurnIsBlack = wrapper.CurrentTurn;

            FigData cachedData;
            foreach (FigData figData in wrapper.ChessPieces) {
                cachedData = new FigData(figData.Type, figData.IsBlack, figData.Coordinates);

                loadedData[cachedData.Coordinates] = cachedData;
            }
        }
        
        
    }
}