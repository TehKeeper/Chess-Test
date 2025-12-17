using UnityEngine;

namespace Model.Data {
    [System.Serializable]
    public struct FigData
    {
        public FigType Type;
        public bool IsBlack;
        public Vector2Int Coordinates;

        public FigData(FigType type, bool isBlack, Vector2Int coordinates)
        {
            Type = type;
            IsBlack = isBlack;
            Coordinates = coordinates;
        }
    }
}