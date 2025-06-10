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
        public Action<int> OnMapButtonPressed;
        
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
        [Header("Map UI")]
        [SerializeField] private CanvasGroup mapCanvasGroup;
        [SerializeField] private Button leftMapButton;
        [SerializeField] private Button middleMapButton;
        [SerializeField] private Button rightMapButton;
        [SerializeField] private TMP_Text leftMapLabel;
        [SerializeField] private TMP_Text middleMapLabel;
        [SerializeField] private TMP_Text rightMapLabel;
        
        private const AudioRepositoryEntryId ToggleSettingsSound = AudioRepositoryEntryId.UIButtonSound;
        
        private void Awake()
        {
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            deckButton.onClick.AddListener(OnDeckButtonPressed);
            endTurnButton.onClick.AddListener(OnEndTurnButtonPressed);
            drawPileButton.onClick.AddListener(OnDrawPileButtonPressed);
            discardPileButton.onClick.AddListener(OnDiscardPileButtonPressed);
            leftMapButton.onClick.AddListener(() => OnMapButtonPressed.Invoke(0));
            middleMapButton.onClick.AddListener(() => OnMapButtonPressed.Invoke(1));
            rightMapButton.onClick.AddListener(() => OnMapButtonPressed.Invoke(2));
        }

        private void Start()
        {
            GameSettingsController.Instance.RefreshSettings();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.ExitAndToggleSettings))
            {
                if (!GameSettingsController.Instance.SettingsPanelShowing() && !CardViewerController.Instance.IsShown())
                {
                    AudioService.Instance.PlaySFXClip(ToggleSettingsSound);
                    GameSettingsController.Instance.ShowSettings(true);
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
            leftMapButton.onClick.RemoveAllListeners();
            middleMapButton.onClick.RemoveAllListeners();
            rightMapButton.onClick.RemoveAllListeners();
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
            Utils.SetCanvasGroupVisible(boardUICanvasGroup, shown);
        }

        public void SetMapUIVisibility(bool shown, string leftLabel = "", string middleLabel = "", string rightLabel = "")
        {
            Utils.SetCanvasGroupVisible(mapCanvasGroup, shown);
            leftMapButton.gameObject.SetActive(leftLabel != "");
            leftMapLabel.text = leftLabel;
            middleMapButton.gameObject.SetActive(middleLabel != "");
            middleMapLabel.text = middleLabel;
            rightMapButton.gameObject.SetActive(rightLabel != "");
            rightMapLabel.text = rightLabel;
        }
        
        private void OnDeckButtonPressed()
        {
            DeckButtonPressed?.Invoke();
        }
        
        private void OnSettingsButtonPressed()
        {
            GameSettingsController.Instance.ShowSettings(true);
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