using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(Image))]
    public class BoardTile2D : BoardTile, IDropHandler {
        private Transform _cachedTransform;
        public override Transform FigureRoot => _cachedTransform ??= transform;

        public override void SetPosition(Vector2Int cachedCoord, float tileSize) {
            RectTransform rtf = GetComponent<RectTransform>();
            rtf.anchoredPosition = new Vector2((cachedCoord.x - 3.5f) * tileSize, (cachedCoord.y - 3.5f) * tileSize);
            rtf.sizeDelta = Vector2.one * tileSize;
        }


        public void OnDrop(PointerEventData eventData) {
            EndDragRelease(eventData);
        }

        private void EndDragRelease(PointerEventData eventData) {
            if (!eventData.pointerDrag)
                return;

            GameObject raycastedObject = eventData.pointerCurrentRaycast.gameObject;
            if (!raycastedObject) {
                return;
            }

            eventData.pointerDrag.TryGetComponent(out ChessFigView chessFig);

            raycastedObject.TryGetComponent(out BoardTile tile);

            ReleaseFigure(tile, chessFig);
        }

        public override void SetColor(Color tileColor) {
            GetComponent<Image>().color = tileColor;
        }
    }
}