using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace View {
    public abstract class BoardTile : MonoBehaviour {
        private List<IDisposable> _disposers = new();
        public Vector2Int BoardCoordinates { get; private set; }
        public event Action<Vector2Int, BoardTile, ChessFigView> OnFigureRelease;

        public void SetCoordinates(Vector2Int coords) => BoardCoordinates = coords;

        public abstract void SetColor(Color tileColor);

        public abstract Transform FigureRoot { get; }

        public Vector3 GetWorldPosition() {
            return transform.position;
        }

        public abstract void SetPosition(Vector2Int cachedCoord, float tileSize);

        protected void ReleaseFigure(BoardTile tile, ChessFigView figure) {
            OnFigureRelease?.Invoke(BoardCoordinates, tile, figure);
        }

        public void SubscribeHandler(Action<Vector2Int, BoardTile, ChessFigView> onReleaseHandler) {
            OnFigureRelease += onReleaseHandler;
            _disposers.Add(new Disposer(delegate { OnFigureRelease -= onReleaseHandler; }));
        }

        private void OnDestroy() {
            foreach (IDisposable disposer in _disposers) {
                disposer.Dispose();
            }
        }
    }
}