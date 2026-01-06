using System;
using Model.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(Image))]
    public class ChessFigView : ChessFigureBase, IBeginDragHandler, IDragHandler, IEndDragHandler {
        private FigData _data;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private Transform _originalParent;

        private Func<bool, bool> _colorMatch;
        private Image _mainImage;
        private Image MainImage => _mainImage ??= GetComponent<Image>();

        private float _scaleFactor;
        private bool _isDragging;
        private GameObject _object;

        public override bool FigColor => _data.IsBlack;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _mainImage = GetComponent<Image>();
            _object = gameObject;
        }

        public override void MakeInteractable(bool b) {
            _mainImage.raycastTarget = b;
        }

        public override void Initialize(FigData figData, Func<bool, bool> isColorMatch, Sprite sprite,
            float scaleFactor,
            Vector2Int coordinates) {
            _data = figData;

            _colorMatch = isColorMatch;
            _scaleFactor = scaleFactor;
            MainImage.sprite = sprite;
            MainImage.color = _data.IsBlack ? Color.black : Color.white;
            SetCoordinates(coordinates);
        }

        private void SetCoordinates(Vector2Int coordinates) {
            BoardCoordinates = coordinates;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!_colorMatch.Invoke(_data.IsBlack) && _isDragging)
                return;

            _isDragging = true;

            _originalPosition = _rectTransform.anchoredPosition;

            MainImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) {
            if (!_isDragging)
                return;
            _rectTransform.anchoredPosition += eventData.delta / _scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!_isDragging)
                return;

            GameObject raycastedObject = eventData.pointerCurrentRaycast.gameObject;
            if (!raycastedObject) {
                ResetPosition();
            }
            else {
                if (!eventData.pointerDrag.TryGetComponent(out BoardTile chessFig))
                    ResetPosition();
            }


            _isDragging = false;
            MainImage.raycastTarget = true;
        }

        public override void ResetPosition() {
            Debug.Log("Reset Position");
            _rectTransform.anchoredPosition = _originalPosition;
        }

        public override void Activate(bool b) {
            _object.SetActive(b);
        }

        public override void SetWorldPosition(Vector3 worldPosition) {
            Debug.Log($"Set world position: {worldPosition}");
            _rectTransform.position = worldPosition;
            _originalPosition = _rectTransform.anchoredPosition;
        }
    }
}