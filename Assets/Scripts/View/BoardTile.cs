using System;
using UnityEngine;

namespace View {
    public abstract class BoardTile : MonoBehaviour {
        public Vector2Int BoardCoordinates { get; private set; }
        public event Action<Vector2Int> OnFigureRelease;

        public void SetCoordinates(Vector2Int coords) => BoardCoordinates = coords;

        public abstract void SetColor(Color tileColor);

        public abstract Transform FigureRoot { get; }

        public Vector3 GetWorldPosition() {
            return transform.position;
        }

        public abstract void SetPosition(Vector2Int cachedCoord, float tileSize);
    }
}