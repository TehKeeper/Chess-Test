using UnityEngine;
using UnityEngine.EventSystems;

namespace View {
    public class BoardTile : MonoBehaviour, IEndDragHandler {
        public Vector2Int BoardCoordinates { get; set; }

        public void OnEndDrag(PointerEventData eventData) {
        }
    }
}