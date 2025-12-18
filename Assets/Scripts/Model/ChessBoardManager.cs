using System;
using System.Collections.Generic;
using Model.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using View;

namespace Model {
    public class ChessBoardManager : MonoBehaviour, IDropHandler {
        [Header("UI References")]
        [SerializeField] private Canvas _canvas;

        [Header("Prefabs")]
        [SerializeField] private ChessFigView _figPrefab;

        [Header("Sprites")]
        [SerializeField] private Pair<FigType, Sprite>[] _figSprites;

        private Dictionary<FigType, Sprite> _figuresSpriteDictionary = new();

        private ChessBoardData boardData = new ChessBoardData();

        public bool CurrentTurnBlack => boardData.CurrentTurnBlack;

        [SerializeField] private BoardSpawnerBase _boardSpawner;
        private List<ChessFigView> _onBoardFigures = new();


        private void Awake() {
            foreach (var item in _figSprites) {
                _figuresSpriteDictionary.Add(item.A, item.B);
            }

            _boardSpawner.SpawnTiles();
        }

        private void Start() {
            

            boardData.SetupStandardGame();
            UpdateBoard();
        }

        private void UpdateBoard() {
            foreach (var piece in _onBoardFigures) {
                Destroy(piece.gameObject); //todo pool?
            }

            _onBoardFigures.Clear();

            ChessFigView cachedFig = null;
            Transform canvasTransform = _canvas.transform;
            foreach (var kvp in boardData.BoardState) {
                if (kvp.Value.Type == FigType.None)
                    continue;

                
                cachedFig = Instantiate(_figPrefab, _boardSpawner.Tiles[kvp.Key].GetWorldPosition(),
                    Quaternion.identity,canvasTransform);
                cachedFig.Initialize(kvp.Value, MatchColorToCurrentTurn, _figuresSpriteDictionary[kvp.Value.Type],
                    _canvas.scaleFactor);

                _onBoardFigures.Add(cachedFig);
            }
        }

        private bool MatchColorToCurrentTurn(bool arg) => arg == boardData.CurrentTurnBlack;

        public void OnDrop(PointerEventData eventData) {
            var piece = eventData.pointerDrag?.GetComponent<ChessFigView>();
            var tile = eventData.pointerCurrentRaycast.gameObject?.GetComponent<BoardTile>();
            if (piece != null && tile != null) {
                Vector2Int from = piece.BoardCoordinates;
                Vector2Int to = tile.BoardCoordinates;
                if (boardData.IsValidMove(from, to)) {
                    boardData.MovePiece(from, to);
                    UpdateBoard();
                }
                else {
                    //piece.UpdatePosition(piece.originalPosition);
                    //piece.transform.SetParent(piece.originalParent); todo
                }
            }
        }


        [ContextMenu("Reset Game")]
        private void ResetGame() {
            boardData.ResetBoard();
            boardData.SetupStandardGame();
            UpdateBoard();
        }
    }
}