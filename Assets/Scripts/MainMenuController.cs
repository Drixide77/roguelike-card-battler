using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RoundBallGame.Systems.Services;

namespace RoundBallGame.Systems
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup mainPanelCanvasGroup;
        [SerializeField] private CanvasGroup buttonContainerCanvasGroup;
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [Space(10)]
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDelay;
        [SerializeField] private float alphaStepAmount;
        [SerializeField] private float stepTime;
        [SerializeField] private float holdTime;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private string levelSceneName;

        private Coroutine animationCoroutine;
        
        private void Awake()
        {
            Time.timeScale = 1f; // Making sure so it doesn't affect the animation coroutine

            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            SetCanvasGroupEnabled(buttonContainerCanvasGroup, true);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        private void Start()
        {
            // Done on Start to ensure AppControlService has been initialized
            if (AppControlService.Instance.firstTimeOnMainMenu)
            {
                SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
                SetCanvasGroupEnabled(buttonContainerCanvasGroup, false);
                GameSettingsController.Instance.HideSettings();
                animationCoroutine = StartCoroutine(AnimateMenuCoroutine());
                AppControlService.Instance.firstTimeOnMainMenu = false;
            }
            else
            {
                GameSettingsController.Instance.ShowSettings();
            }
        }
        
        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }
        
        private void OnPlayButtonClicked()
        {
            SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
            // TODO -
        }
        
        private void OnExitButtonClicked()
        {
            AppControlService.Instance.ExitApplication();
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
            GameSettingsController.Instance.ShowSettings();
        }
    }
}
