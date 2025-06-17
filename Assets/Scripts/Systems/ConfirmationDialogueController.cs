using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class ConfirmationDialogueController : MonoBehaviour
    {
        // Singleton pattern
        public static ConfirmationDialogueController Instance { get; private set; }
        
        [Header("Components")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text confirmationMessageLabel;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private Action confirmationCallback;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            yesButton.onClick.AddListener(OnYesButtonPressed);
            noButton.onClick.AddListener(OnNoButtonPressed);
        }

        private void Start()
        {
            Utils.SetCanvasGroupVisible(canvasGroup, false);
        }

        private void OnDestroy()
        {
            yesButton.onClick.RemoveListener(OnYesButtonPressed);
            noButton.onClick.RemoveListener(OnNoButtonPressed);
        }

        public void ShowConfirmationDialogue(string message, Action onConfirmedCallback)
        {
            confirmationMessageLabel.text = message;
            confirmationCallback = onConfirmedCallback;
            Utils.SetCanvasGroupVisible(canvasGroup, true);
        }

        private void OnYesButtonPressed()
        {
            Utils.SetCanvasGroupVisible(canvasGroup, false);
            confirmationCallback();
        }

        private void OnNoButtonPressed()
        {
            Utils.SetCanvasGroupVisible(canvasGroup, false);
        }
    }
}
