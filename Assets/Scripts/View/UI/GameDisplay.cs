using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View.UI {
    public class GameDisplay : MonoBehaviour {
        [SerializeField] private TMP_Text _currentTurnText;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _quickSaveButton;
        [SerializeField] private Button _quickLoadButton;

        public void UpdateTurn(bool isBlack) {
            _currentTurnText.text = $"Ходят {(isBlack ? "<color#=000000>чёрные</color>" : "<color#=ffffff>белые</color>")}";
        }

        public void SetupHandlers(UnityAction reset, UnityAction quickSave, UnityAction quickLoad) {
            _resetButton.onClick.AddListener(reset);
            _quickSaveButton.onClick.AddListener(quickSave);
            _quickLoadButton.onClick.AddListener(quickLoad);
        }


        private void OnDestroy() {
            _resetButton.onClick.RemoveAllListeners();
            _quickSaveButton.onClick.RemoveAllListeners();
            _quickLoadButton.onClick.RemoveAllListeners();
        }
    }
}