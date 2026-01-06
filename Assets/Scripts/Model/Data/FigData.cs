using UnityEngine;

namespace Model.Data {
    [System.Serializable]
    public struct FigData {
        public FigType Type;
        public bool IsBlack;
        public Vector2Int Coordinates;
        public Vector2Int PreviousCoordinates;

        public FigData(FigType type, bool isBlack, Vector2Int coordinates) {
            Type = type;
            IsBlack = isBlack;
            Coordinates = coordinates;
            PreviousCoordinates = Vector2Int.one * -1;
        }

        public FigData(FigData piece, Vector2Int previousCoordinates) {
            Type = piece.Type;
            IsBlack = piece.IsBlack;
            Coordinates = piece.Coordinates;
            PreviousCoordinates = previousCoordinates;
        }
        
        public FigData(FigData piece, FigType type) {
            Type = type;
            IsBlack = piece.IsBlack;
            Coordinates = piece.Coordinates;
            PreviousCoordinates = piece.PreviousCoordinates;
        }

        
    }
}