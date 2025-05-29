using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup mainPanelCanvasGroup;
        [SerializeField] private CanvasGroup buttonContainerCanvasGroup;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [Space(10)]
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDelay;
        [SerializeField] private float alphaStepAmount;
        [SerializeField] private float stepTime;
        [SerializeField] private float holdTime;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private string gameplaySceneName;

        private const AudioRepositoryEntryId ToggleSettingsSound = AudioRepositoryEntryId.UIButtonSound;
        
        private Coroutine animationCoroutine;
        
        private void Awake()
        {
            Time.timeScale = 1f; // Making sure so it doesn't affect the animation coroutine

            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            SetCanvasGroupEnabled(buttonContainerCanvasGroup, true);
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        private void Start()
        {
            GameSettingsController.Instance.HideSettings();
            
            // Done on Start to ensure AppControlService has been initialized
            if (AppControlService.Instance.firstTimeOnMainMenu)
            {
                SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
                SetCanvasGroupEnabled(buttonContainerCanvasGroup, false);
                animationCoroutine = StartCoroutine(AnimateMenuCoroutine());
                AppControlService.Instance.firstTimeOnMainMenu = false;
            }

            UpdateButtonState();
        }

        private void Update()
        {
            if (!GameSettingsController.Instance.SettingsPanelShowing())
            {
                if (Input.GetKeyDown(KeybindingsDefinition.ExitAndToggleSettings))
                {
                    AudioService.Instance.PlaySFXClip(ToggleSettingsSound);
                    GameSettingsController.Instance.ShowSettings(false);
                }
            }
        }

        private void OnDestroy()
        {
            continueButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }
        
        private void OnContinueButtonClicked()
        {
            AppControlService.Instance.LoadNewScene(gameplaySceneName);
            StartGameplay();
        }
        
        private void OnPlayButtonClicked()
        {
            DataService.Instance.SetGameInProgress(true);
            DataService.Instance.SaveProgressData();
            StartGameplay();
        }
        
        private void OnSettingsButtonClicked()
        {
            GameSettingsController.Instance.ShowSettings(false);
        }
        
        private void OnExitButtonClicked()
        {
            DataService.Instance.SaveProgressData();
            AppControlService.Instance.ExitApplication();
        }

        private void UpdateButtonState()
        {
            if (DataService.Instance.HasGameInProgress())
            {
                continueButton.interactable = true;
            }
            else
            {
                continueButton.interactable = false;
            }
        }

        private void StartGameplay()
        {
            GameSettingsController.Instance.HideSettings();
            AppControlService.Instance.LoadNewScene(gameplaySceneName);
        }
        
        private void SetCanvasGroupEnabled(CanvasGroup canvasGroup, bool enabled)
        {
            canvasGroup.alpha = enabled ? 1 : 0;
            canvasGroup.interactable = enabled;
            canvasGroup.blocksRaycasts = enabled;
        }
        
        private IEnumerator AnimateMenuCoroutine()
        {
            yield return new WaitForSeconds(fadeInDelay);
            while (mainPanelCanvasGroup.alpha < 1f)
            {
                mainPanelCanvasGroup.alpha += alphaStepAmount;
                yield return new WaitForSeconds(stepTime);
            }
            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            yield return new WaitForSeconds(holdTime);
            SetCanvasGroupEnabled(buttonContainerCanvasGroup, true);
        }
    }
}
