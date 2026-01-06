using System;
using System.Collections.Generic;
using Model.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using View;
using View.UI;

namespace Model {
    public class ChessBoardManager : MonoBehaviour, IDropHandler {
        [Header("UI References")]
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Transform _uiFigRoot;

        [Header("Prefabs")]
        [SerializeField] private ChessFigureBase _figPrefab;

        [Header("Sprites")]
        [SerializeField] private Pair<FigType, Sprite>[] _figSprites;

        [Header("UI")]
        [SerializeField] private GameDisplay _display;

        private Dictionary<FigType, Sprite> _figuresSpriteDictionary = new();

        private ChessBoardData _boardData = new ChessBoardData();

        public bool CurrentTurnBlack => _boardData.CurrentTurnBlack;

        [SerializeField] private BoardSpawnerBase _boardSpawner;
        private Dictionary<Vector2Int, ChessFigureBase> _onBoardFigures = new();

        private List<IDisposable> _disposers = new List<IDisposable>();

        private SaveTool _saveTool = new SaveTool();

        private Queue<ChessFigureBase> _pool = new();

        private void Awake() {
            foreach (var item in _figSprites) {
                _figuresSpriteDictionary.Add(item.A, item.B);
            }

            _boardSpawner.SpawnTiles(OnReleaseHandler);

            _display.SetupHandlers(ResetGame, delegate { _saveTool.SaveBoard(_boardData); },
                delegate {
                    _saveTool.LoadBoard(_boardData);
                    ResetBoard();
                });

            _boardData.OnTurnChange += _display.UpdateTurn;
            _disposers.AddDisposer(delegate { _boardData.OnTurnChange -= _display.UpdateTurn; });
        }

        private void OnReleaseHandler(Vector2Int coordinates, BoardTile tile, ChessFigureBase figure) {
            if (figure == null)
                return;

            if (tile != null) {
                Vector2Int from = figure.BoardCoordinates;
                Vector2Int to = tile.BoardCoordinates;
                if (_boardData.TryMovePiece(from, to, out List<(Vector2Int position, FigData data)> changedPieces)) {
                    UpdateBoard(changedPieces);

                    //ResetBoard();
                }
                else {
                    figure.ResetPosition();
                }
            }
        }


        private void Start() {
            _boardData.SetupStandardGame();
            ResetBoard();
        }

        private void ResetBoard() {
            foreach (var piece in _onBoardFigures) {
                piece.Value.Activate(false);
                _pool.Enqueue(piece.Value);
            }

            _onBoardFigures.Clear();

            ChessFigureBase cachedFig = null;
            foreach (var kvp in _boardData.BoardState) {
                if (kvp.Value.Type == FigType.None)
                    continue;
                
                AddFigure(kvp, ref cachedFig);
            }

            ActivateByColor();
        }

        private void ActivateByColor() {
            foreach (var boardFig in _onBoardFigures) {
                boardFig.Value.MakeInteractable(_boardData.CurrentTurnBlack == boardFig.Value.FigColor);
            }
        }

        private void AddFigure(KeyValuePair<Vector2Int, FigData> kvp, ref ChessFigureBase cachedFig) {
            Vector3 worldPosition = _boardSpawner.Tiles[kvp.Key].GetWorldPosition();
            cachedFig = GetFigure(worldPosition);
            cachedFig.Initialize(kvp.Value, MatchColorToCurrentTurn, _figuresSpriteDictionary[kvp.Value.Type],
                _canvas.scaleFactor, kvp.Key);
            
            cachedFig.SetWorldPosition(worldPosition);

            _onBoardFigures.Add(kvp.Key, cachedFig);
        }

        private void UpdateBoard(List<(Vector2Int position, FigData data)> changedPieces) {
            ChessFigureBase cachedFig = null;
            foreach (var tuple in changedPieces) {
                Debug.Log($"Changed Piece: {tuple.position} : {tuple.data}");
                if (tuple.data.Type == FigType.None) {
                    Debug.Log($"Remove piece on {tuple.position}");
                    cachedFig = _onBoardFigures[tuple.position];
                    cachedFig.Activate(false);
                    _pool.Enqueue(cachedFig);
                    _onBoardFigures.Remove(tuple.position);
                } else {
                    
                    AddFigure(new KeyValuePair<Vector2Int, FigData>(tuple.position, tuple.data), ref cachedFig);
                }
            }

            ActivateByColor();
        }

        private ChessFigureBase GetFigure(Vector3 worldPosition) {
            if (_pool.Count > 0) {
                ChessFigureBase fig = _pool.Dequeue();
                fig.Activate(true);
                fig.SetWorldPosition(worldPosition);
                return fig;
            }
            
            return Instantiate(_figPrefab,worldPosition, Quaternion.identity, _uiFigRoot);
        }

        private bool MatchColorToCurrentTurn(bool arg) => arg == _boardData.CurrentTurnBlack;

        public void OnDrop(PointerEventData eventData) { }


        [ContextMenu("Reset Game")]
        private void ResetGame() {
            _boardData.ResetBoard();
            _boardData.SetupStandardGame();
            ResetBoard();
        }
    }
}