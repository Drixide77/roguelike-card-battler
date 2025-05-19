using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class GameSettingsController : MonoBehaviour
    {
        // Singleton pattern
        public static GameSettingsController Instance { get; private set; }
        
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup settingsPanelCanvasGroup;
        [Space(10)]
        [SerializeField] private Button closeSettingsButton;
        [SerializeField] private Button abandonRunButton;
        [SerializeField] private Button quitButton;
        [Space(10)]
        [SerializeField] private Toggle volumeToggle;
        [SerializeField] private Toggle sFXToggle;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Button resolutionButton;
        [SerializeField] private TMP_Text resolutionLabel;
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;
        [SerializeField] private string gameplaySceneName;
        
        
        private const string ResolutionIndexKey = "resolution_index";

        private bool settingsOpen = false;
        private bool settingsVisibilityChangedThisFrame = false;
        private bool nextSettingsVisibility = false;
        private int resolutionIndex = 0;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, false);
            
            closeSettingsButton.onClick.AddListener(OnCloseSettingsButtonPressed);
            abandonRunButton.onClick.AddListener(OnAbandonRunButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
            
            volumeToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            sFXToggle.onValueChanged.AddListener(OnSFXToggleChanged);
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
            resolutionButton.onClick.AddListener(OnResolutionButtonPressed);

            RefreshSettings();
        }

        private void Update()
        {
            // We want to ignore the input during the same frame the setting panel was shown/hidden.
            if (settingsVisibilityChangedThisFrame)
            {
                settingsVisibilityChangedThisFrame = false;
                settingsOpen = nextSettingsVisibility;
                return;
            }
            
            if (settingsOpen)
            {
                if (Input.GetKeyDown(KeybindingsDefinition.PauseKey1) ||
                    Input.GetKeyDown(KeybindingsDefinition.PauseKey2))
                {
                    HideSettings();
                }
            }
        }
        
        private void LateUpdate()
        {
            if (fullscreenToggle.isOn != Screen.fullScreen)
            {
                fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
                resolutionButton.interactable = !fullscreenToggle.isOn;
            }
        }

        private void OnDestroy()
        {
            closeSettingsButton.onClick.RemoveAllListeners();
            abandonRunButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
            
            volumeToggle.onValueChanged.RemoveAllListeners();
            sFXToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            resolutionButton.onClick.RemoveAllListeners();
        }

        public void ShowSettings()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, true);
        }

        public void HideSettings()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, false);
        }

        public bool SettingsPanelShowing()
        {
            return settingsOpen;
        }

        public void RefreshSettings()
        {
            fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
            resolutionButton.interactable = !Screen.fullScreen;
            if (PlayerPrefs.HasKey(ResolutionIndexKey))
            {
                resolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey);
                AppControlService.Instance.SetResolution(resolutionIndex, Screen.fullScreen);
                resolutionLabel.text = AppControlService.Instance.GetCurrentResolutionMultiplier().ToString(CultureInfo.InvariantCulture);
            };
            SetAbandonRunButtonVisibility();
        }
        
        private void SetAbandonRunButtonVisibility()
        {
            abandonRunButton.gameObject.SetActive(SceneManager.GetActiveScene().name == gameplaySceneName);
        }
        
        private void SetCanvasGroupEnabled(CanvasGroup canvasGroup, bool enabled)
        {
            canvasGroup.alpha = enabled ? 1 : 0;
            canvasGroup.interactable = enabled;
            canvasGroup.blocksRaycasts = enabled;
            nextSettingsVisibility = enabled;
            settingsVisibilityChangedThisFrame = true;
        }
        
        private void OnCloseSettingsButtonPressed()
        {
            HideSettings();
        }
        
        private void OnAbandonRunButtonPressed()
        {
            DataService.Instance.SetGameInProgress(false);
            DataService.Instance.SaveProgressData();
            
            HideSettings();
            SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
        }
        
        private void OnQuitButtonPressed()
        {
            if (SceneManager.GetActiveScene().name == gameplaySceneName)
            {
                HideSettings();
                SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
                return;
            }
            if (SceneManager.GetActiveScene().name == mainMenuSceneName)
            {
                DataService.Instance.SaveProgressData();
                AppControlService.Instance.ExitApplication();
            }
        }
        
        private void OnMusicToggleChanged(bool value)
        {
            AudioService.Instance.SetMusicVolume(value ? 1f : 0f);
        }
        
        private void OnSFXToggleChanged(bool value)
        {
            AudioService.Instance.SetSFXVolume(value ? 1f : 0f);
        }
        
        private void OnFullscreenToggleChanged(bool value)
        {
            AppControlService.Instance.SetFullscreen(value);
            fullscreenToggle.SetIsOnWithoutNotify(value);
            resolutionButton.interactable = !fullscreenToggle.isOn;
        }
        
        private void OnResolutionButtonPressed()
        {
            resolutionIndex = (resolutionIndex + 1) % AppControlService.Instance.GetResolutionPresetCount();
            AppControlService.Instance.SetResolution(resolutionIndex);
            resolutionLabel.text = "x " + AppControlService.Instance.GetCurrentResolutionMultiplier().ToString();
            
            PlayerPrefs.SetInt(ResolutionIndexKey, resolutionIndex);
        }
    }
}