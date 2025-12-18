using UnityEngine;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BoardSpawner2D : BoardSpawnerBase {
        [SerializeField] private BoardTile _tilePrefab;
        private Transform _root;

        private void Awake() {
            _root = transform;
        }

        [ContextMenu("Spawn Tiles")]
        public void SpawnTiles() {
            BoardTile cachedTile;
            for (int i = 0; i < 64; i++) {
                cachedTile = Instantiate(_tilePrefab, _root.position, _root.rotation, _root);
                cachedTile.SetCoordinates(new Vector2Int(i % 8, i / 8));
                cachedTile.SetColor((i / 8 ^ i) % 2 == 0 ? BrightColor : DarkColor);
            }
        }
    }
}