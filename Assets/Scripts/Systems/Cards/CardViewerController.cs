using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class CardViewerController : MonoBehaviour
    {
        // Singleton pattern
        public static CardViewerController Instance { get; private set; }
        
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text viewerLabel;
        [SerializeField] private Button exitButton;
        [SerializeField] private Transform cardsContainer;
        [Header("Controllers")]
        [SerializeField] private GameFlowController flowController;
        [Header("Assets")]
        [SerializeField] private CardVisualDescriptor cardPrefab;

        private const AudioRepositoryEntryId ToggleCardViewSound = AudioRepositoryEntryId.UIButtonSound;
        
        private List<Card> cachedCards;
        private List<CardVisualDescriptor> visualPrefabs;
        private bool viewerIsVisible;
        
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
            exitButton.onClick.AddListener(HideCardView);
        }

        private void Start()
        {
            cachedCards = new List<Card>();
            visualPrefabs = new List<CardVisualDescriptor>();
            viewerIsVisible = false;
            Utils.SetCanvasGroupVisible(canvasGroup, false);
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveAllListeners();
        }
        
        private void LateUpdate()
        {
            if (viewerIsVisible)
            {
                if (Input.GetKeyDown(KeybindingsDefinition.ExitAndToggleSettings))
                {
                    AudioService.Instance.PlaySFXClip(ToggleCardViewSound);
                    HideCardView();
                }
            }
        }

        public bool IsShown()
        {
            return viewerIsVisible;
        }
        
        public void ShowCardView(string label, List<Card> cards)
        {
            viewerLabel.text = label;
            cachedCards = cards.ConvertAll(card => new Card(card));
            foreach (CardVisualDescriptor visual in visualPrefabs)
            {
                visual.gameObject.SetActive(false);
            }
            for (int i = 0; i < cachedCards.Count; i++)
            {
                if (i < visualPrefabs.Count)
                {
                    visualPrefabs[i].gameObject.SetActive(true);
                    cachedCards[i].InitializeVisuals(visualPrefabs[i]);
                }
                else
                {
                    CardVisualDescriptor descriptor = Instantiate(cardPrefab, cardsContainer.position, Quaternion.identity, cardsContainer).GetComponent<CardVisualDescriptor>();
                    descriptor.SetMovementLogic(false); // Disable Card Movement script
                    cachedCards[i].InitializeVisuals(descriptor);
                    visualPrefabs.Add(descriptor);
                }
            }
            viewerIsVisible = true;
            Utils.SetCanvasGroupVisible(canvasGroup, true);
        }

        public void HideCardView()
        {
            viewerIsVisible = false;
            Utils.SetCanvasGroupVisible(canvasGroup, false);
        }
    }
}
