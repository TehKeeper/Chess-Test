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
        [SerializeField] private ChessFigureBase _figPrefab;

        [Header("Sprites")]
        [SerializeField] private Pair<FigType, Sprite>[] _figSprites;

        private Dictionary<FigType, Sprite> _figuresSpriteDictionary = new();

        private ChessBoardData _boardData = new ChessBoardData();

        public bool CurrentTurnBlack => _boardData.CurrentTurnBlack;

        [SerializeField] private BoardSpawnerBase _boardSpawner;
        private List<ChessFigureBase> _onBoardFigures = new();


        private void Awake() {
            foreach (var item in _figSprites) {
                _figuresSpriteDictionary.Add(item.A, item.B);
            }

            _boardSpawner.SpawnTiles(OnReleaseHandler);
        }

        private void OnReleaseHandler(Vector2Int coordinates, BoardTile tile, ChessFigureBase figure) {
            if(figure==null)
                return;

            if (figure != null && tile != null) {
                Vector2Int from = figure.BoardCoordinates;
                Vector2Int to = tile.BoardCoordinates;
                if (_boardData.IsValidMove(from, to)) {
                    _boardData.MovePiece(from, to);
                    UpdateBoard();
                }
                else {
                    figure.ResetPosition();
                }
            }
        }

        private void Start() {
            _boardData.SetupStandardGame();
            UpdateBoard();
        }

        private void UpdateBoard() {
            //todo very crude update. need to eliminate "eaten" figure while not touching the others
            foreach (var piece in _onBoardFigures) {
                Destroy(piece.gameObject); //todo pool?
            }

            _onBoardFigures.Clear();

            ChessFigureBase cachedFig = null;
            Transform canvasTransform = _canvas.transform;
            foreach (var kvp in _boardData.BoardState) {
                if (kvp.Value.Type == FigType.None)
                    continue;

                
                cachedFig = Instantiate(_figPrefab, _boardSpawner.Tiles[kvp.Key].GetWorldPosition(),
                    Quaternion.identity,canvasTransform);
                cachedFig.Initialize(kvp.Value, MatchColorToCurrentTurn, _figuresSpriteDictionary[kvp.Value.Type],
                    _canvas.scaleFactor, kvp.Key);

                _onBoardFigures.Add(cachedFig);
            }

            foreach (ChessFigureBase boardFig in _onBoardFigures) {
                boardFig.MakeActive(_boardData.CurrentTurnBlack == boardFig.FigColor);
            }
        }

        private bool MatchColorToCurrentTurn(bool arg) => arg == _boardData.CurrentTurnBlack;

        public void OnDrop(PointerEventData eventData) {
            
        }


        [ContextMenu("Reset Game")]
        private void ResetGame() {
            _boardData.ResetBoard();
            _boardData.SetupStandardGame();
            UpdateBoard();
        }
    }
}