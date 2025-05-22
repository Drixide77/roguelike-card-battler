using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class GameplaySceneController : MonoBehaviour
    {
        public Action DeckButtonPressed;
        public Action EndTurnButtonPressed;
        
        [Header("Controllers")]
        [SerializeField] private GameFlowController flowController;
        [Header("UI Elements")]
        [SerializeField] private TMP_Text healthLabel;
        [SerializeField] private TMP_Text goldLabel;
        [SerializeField] private TMP_Text deckLabel;
        [SerializeField] private TMP_Text energyLabel;
        [SerializeField] private Button deckButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button endTurnButton;
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;
        
        private const AudioRepositoryEntryId ToggleSettingsSound = AudioRepositoryEntryId.UIButtonSound;

        private void Awake()
        {
            flowController.Initialize(this);
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            deckButton.onClick.AddListener(OnDeckButtonPressed);
            endTurnButton.onClick.AddListener(OnEndTurnButtonPressed);
        }

        private void Start()
        {
            GameSettingsController.Instance.RefreshSettings();
            
            flowController.StartRun();
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

        public void SetEndTurnButtonStatus(bool interactable, bool visible)
        {
            endTurnButton.interactable = interactable;
            endTurnButton.gameObject.SetActive(visible);
        }
        
        public void OnRunEnded()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
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
    }
}
