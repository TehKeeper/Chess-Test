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
        private List<ChessFigureBase> _onBoardFigures = new();

        private List<IDisposable> _disposers = new List<IDisposable>();

        private SaveTool _saveTool = new SaveTool();

        private Queue<ChessFigureBase> _pool = new ();
        
        private void Awake() {
            foreach (var item in _figSprites) {
                _figuresSpriteDictionary.Add(item.A, item.B);
            }

            _boardSpawner.SpawnTiles(OnReleaseHandler);
            
            _display.SetupHandlers(ResetGame, delegate { _saveTool.SaveBoard(_boardData); }  ,
                delegate {
                    _saveTool.LoadBoard(_boardData);
                    UpdateBoard();
                } );

            _boardData.OnTurnChange += _display.UpdateTurn;
            _disposers.AddDisposer(delegate {   _boardData.OnTurnChange -= _display.UpdateTurn; });

        }

        private void OnReleaseHandler(Vector2Int coordinates, BoardTile tile, ChessFigureBase figure) {
            if(figure==null)
                return;

            if (tile != null) {
                Vector2Int from = figure.BoardCoordinates;
                Vector2Int to = tile.BoardCoordinates;
                if (_boardData.TryMovePiece(from, to)) {
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
                piece.Activate(false);
                _pool.Enqueue(piece);
            }

            _onBoardFigures.Clear();

            ChessFigureBase cachedFig = null;
            foreach (var kvp in _boardData.BoardState) {
                if (kvp.Value.Type == FigType.None)
                    continue;

                
                cachedFig = GetFigure(kvp);
                cachedFig.Initialize(kvp.Value, MatchColorToCurrentTurn, _figuresSpriteDictionary[kvp.Value.Type],
                    _canvas.scaleFactor, kvp.Key);

                _onBoardFigures.Add(cachedFig);
            }

            foreach (ChessFigureBase boardFig in _onBoardFigures) {
                boardFig.MakeActive(_boardData.CurrentTurnBlack == boardFig.FigColor);
            }
        }

        private ChessFigureBase GetFigure(KeyValuePair<Vector2Int, FigData> kvp) {
            if (_pool.Count > 0) {
                ChessFigureBase fig = _pool.Dequeue();
                fig.Activate(true);
            }
            
            return Instantiate(_figPrefab, _boardSpawner.Tiles[kvp.Key].GetWorldPosition(),
                Quaternion.identity,_uiFigRoot);
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