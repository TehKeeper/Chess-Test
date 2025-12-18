using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(Image))]
    public class BoardTile2D : BoardTile, IEndDragHandler {
        public void OnEndDrag(PointerEventData eventData) {
           
            if (!eventData.pointerDrag)
                return;

            GameObject raycastedObject = eventData.pointerCurrentRaycast.gameObject;
            if (!raycastedObject) {
                return;
            }


            /*ChessFigView chessFig;
            if (!eventData.pointerDrag.TryGetComponent(out chessFig)) {
                return;
            }*/

            BoardTile tile;
            if (!raycastedObject.TryGetComponent(out tile)) {
                return;
            }

            Debug.Log($"End Drag Tile: {tile.BoardCoordinates}");



        }

        public override void SetColor(Color tileColor) {
            GetComponent<Image>().color = tileColor;
        }
    }
}