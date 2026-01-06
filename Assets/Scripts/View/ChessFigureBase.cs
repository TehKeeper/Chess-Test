using System;
using Model.Data;
using UnityEngine;

namespace View {
    public abstract class ChessFigureBase : MonoBehaviour {
        public abstract void MakeActive(bool b);

        public abstract void Initialize(FigData figData, Func<bool, bool> isColorMatch, Sprite sprite, float scaleFactor,
            Vector2Int coordinates);

        public abstract bool FigColor { get; }
        public Vector2Int BoardCoordinates { get; protected set; }
        public abstract void ResetPosition();

        public abstract void Activate(bool b);
    }
}