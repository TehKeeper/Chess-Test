using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace View {
    public abstract class BoardSpawnerBase : MonoBehaviour {
        [SerializeField] protected Color BrightColor = new Color(0.8679245f, 0.7908455f, 0.5035f);
        [SerializeField] protected Color DarkColor = new Color(0.4528302f, 0.2387267f, 0.1772873f);
        public Dictionary<Vector2Int, BoardTile> Tiles = new ();
        protected abstract Transform Root { get; }
        public abstract void SpawnTiles(Action<Vector2Int, BoardTile, ChessFigView> onReleaseHandler);
    }
}