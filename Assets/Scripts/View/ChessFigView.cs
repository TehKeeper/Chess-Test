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
        [SerializeField] private Canvas _refCanvas;
        
        private FigData _data;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private Transform _originalParent;

        private Func<bool, bool> _colorMatch;
        private Image _mainImage;

        private static readonly Dictionary<FigType, string> FigSymbols = new Dictionary<FigType, string> {
            {FigType.King, "♔"},
            {FigType.Queen, "♕"},
            {FigType.Rook, "♖"},
            {FigType.Bishop, "♗"},
            {FigType.Knight, "♘"},
            {FigType.Pawn, "♙"}
        };

        private TMP_Text _textIcon;
        private float _scaleFactor;
        private bool _isDragging;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _textIcon = GetComponentInChildren<TMP_Text>();
            _mainImage = GetComponent<Image>();

            if (_refCanvas)
                _scaleFactor = _refCanvas.scaleFactor;
        }

        public void Initialize(FigData figData, Func<bool, bool> isColorMatch, float scaleFactor) {
            _data = figData;

            _colorMatch = isColorMatch;
            _scaleFactor = scaleFactor;

            _textIcon.text = FigSymbols[figData.Type];
        }

        public void OnBeginDrag(PointerEventData eventData) {
            /*if (!_colorMatch.Invoke(_data.IsBlack) && _isDragging) 
                return;*/

            _isDragging = true;

            _originalPosition = _rectTransform.anchoredPosition;

            //_mainImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) {
            if (!_isDragging) 
                return;
            _rectTransform.anchoredPosition += eventData.delta / _scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!_isDragging) 
                return;

            _mainImage.raycastTarget = true;

            // If not dropped on a valid drop zone, return to original position

            _rectTransform.anchoredPosition = _originalPosition;
            
            _isDragging = false;
        }
    }
}