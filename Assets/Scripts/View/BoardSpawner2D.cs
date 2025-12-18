using UnityEngine;

namespace View {
    public class BoardSpawner2D : BoardSpawnerBase {
        [SerializeField] private BoardTile _tilePrefab;
        [SerializeField] private float _tileSize = 25;
        private Transform _root;

        protected override Transform Root => _root ??= transform;

        private void Awake() {
            _root = transform;
        }

        [ContextMenu("Spawn Tiles")]
        public override void SpawnTiles() {
            BoardTile cachedTile;
            Vector2Int cachedCoord;
            for (int i = 0; i < 64; i++) {
                cachedCoord = new Vector2Int(i % 8, i / 8);
                
                cachedTile = Instantiate(_tilePrefab, Root.position, Root.rotation, _root);
                cachedTile.SetCoordinates(new Vector2Int(i % 8, i / 8));

                cachedTile.SetPosition(cachedCoord, _tileSize);
                
                
                
                cachedTile.SetColor((i / 8 ^ i) % 2 == 0 ? BrightColor : DarkColor);

                Tiles[cachedCoord] = cachedTile;
            }
            
            
        }
    }
}