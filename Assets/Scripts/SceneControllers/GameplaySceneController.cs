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
        
        [Header("Controllers")]
        [SerializeField] private GameFlowController flowController;
        [Header("UI Elements")]
        [SerializeField] private TMP_Text healthLabel;
        [SerializeField] private TMP_Text goldLabel;
        [SerializeField] private TMP_Text deckLabel;
        [SerializeField] private Button deckButton;
        [SerializeField] private Button settingsButton;
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;
        
        private const AudioRepositoryEntryId ToggleSettingsSound = AudioRepositoryEntryId.UIButtonSound;

        private void Awake()
        {
            flowController.GameplaySceneController = this;
            settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            deckButton.onClick.AddListener(OnDeckButtonPressed);
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
    }
}
