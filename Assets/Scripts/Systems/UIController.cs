using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class UIController : MonoBehaviour
    {
        public Action DeckButtonPressed;
        public Action EndTurnButtonPressed;
        public Action DrawPileButtonPressed;
        public Action DiscardPileButtonPressed;
        
        [Header("Top Bar UI")]
        [SerializeField] private TMP_Text healthLabel;
        [SerializeField] private TMP_Text goldLabel;
        [SerializeField] private TMP_Text deckLabel;
        [SerializeField] private Button deckButton;
        [SerializeField] private Button settingsButton;
        [Header("Board UI")]
        [SerializeField] private CanvasGroup boardUICanvasGroup;
        [SerializeField] private TMP_Text energyLabel;
        [SerializeField] private TMP_Text drawPileLabel;
        [SerializeField] private TMP_Text discardPileLabel;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button drawPileButton;
        [SerializeField] private Button discardPileButton;
        
        private const AudioRepositoryEntryId ToggleSettingsSound = AudioRepositoryEntryId.UIButtonSound;
        
        private void Awake()
        {
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            deckButton.onClick.AddListener(OnDeckButtonPressed);
            endTurnButton.onClick.AddListener(OnEndTurnButtonPressed);
            drawPileButton.onClick.AddListener(OnDrawPileButtonPressed);
            discardPileButton.onClick.AddListener(OnDiscardPileButtonPressed);
        }

        private void Start()
        {
            GameSettingsController.Instance.RefreshSettings();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.ExitAndToggleSettings))
            {
                if (!GameSettingsController.Instance.SettingsPanelShowing())
                {
                    AudioService.Instance.PlaySFXClip(ToggleSettingsSound);
                    GameSettingsController.Instance.ShowSettings();
                }
            }
        }

        private void OnDestroy()
        {
            settingsButton.onClick.RemoveAllListeners();
            deckButton.onClick.RemoveAllListeners();
            endTurnButton.onClick.RemoveAllListeners();
            drawPileButton.onClick.RemoveAllListeners();
            discardPileButton.onClick.RemoveAllListeners();
        }

        public void UpdateHealth(int current, int max)
        {
            healthLabel.text = "HP: " + current + "/" + max;
        }
        
        public void UpdateGold(int current)
        {
            goldLabel.text = "Gold: " + current;
        }
        
        public void UpdateDeckLabel(int count)
        {
            deckLabel.text = "Deck(" + count + ")";
        }

        public void UpdateEnergyLabel(int current, int max)
        {
            energyLabel.text = current + "/" + max;
        }

        public void UpdateDrawPileLabel(int count)
        {
            drawPileLabel.text = count.ToString();
        }
        
        public void UpdateDiscardPileLabel(int count)
        {
            discardPileLabel.text = count.ToString();
        }

        public void SetEndTurnButtonStatus(bool interactable)
        {
            endTurnButton.interactable = interactable;
        }

        public void SetBoardUIVisibility(bool shown)
        {
            boardUICanvasGroup.alpha = shown ? 1 : 0;
            boardUICanvasGroup.interactable = shown;
            boardUICanvasGroup.blocksRaycasts = shown;
        }
        
        private void OnDeckButtonPressed()
        {
            DeckButtonPressed?.Invoke();
        }
        
        private void OnSettingsButtonPressed()
        {
            GameSettingsController.Instance.ShowSettings();
        }

        private void OnEndTurnButtonPressed()
        {
            EndTurnButtonPressed?.Invoke();
        }
        
        private void OnDrawPileButtonPressed()
        {
            DrawPileButtonPressed?.Invoke();
        }
        
        private void OnDiscardPileButtonPressed()
        {
            DiscardPileButtonPressed?.Invoke();
        }
    }
}