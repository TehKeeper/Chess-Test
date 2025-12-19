using System;
using System.Collections.Generic;
using Model.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(Image))]
    public class ChessFigView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
      

        private FigData _data;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private Transform _originalParent;

        private Func<bool, bool> _colorMatch;
        private Image _mainImage;
        private Image MainImage => _mainImage ??= GetComponent<Image>();
        
        private float _scaleFactor;
        private bool _isDragging;
        public Vector2Int BoardCoordinates { get; private set; }

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _mainImage = GetComponent<Image>();
        }

        public void Initialize(FigData figData, Func<bool, bool> isColorMatch, Sprite sprite, float scaleFactor,
            Vector2Int coordinates) {
            _data = figData;

            _colorMatch = isColorMatch;
            _scaleFactor = scaleFactor;
            MainImage.sprite = sprite;
            MainImage.color = _data.IsBlack ? Color.black : Color.white;
            SetCoordinates(coordinates);
        }

        public void SetCoordinates(Vector2Int coordinates) {
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

            MainImage.raycastTarget = true;

            _isDragging = false;
        }

        public void ResetPosition() {
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }
}