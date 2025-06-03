using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class CardVisualDescriptor : MonoBehaviour
    {
        [Header("Behaviours")]
        [SerializeField] private CardMovement cardMovement;
        [Header("UI Elements")]
        public TMP_Text NameLabel;
        public TMP_Text EnergyCostLabel;
        public Image Image;
        public Image RarityIcon;
        public TMP_Text EffectsLabel;

        private Card cardData;
        private GameFlowController flowController;

        public void SetReferences(Card card, GameFlowController gameFlowController)
        {
            cardData = card;
            flowController = gameFlowController;
            cardMovement.OnCardSelectionChanged += OnCardSelected;
        }

        private void OnCardSelected(bool selected)
        {
            flowController.BoardController.SetSelectedCard(selected, cardData.Effects[0].Target, cardData);
        }
    }
}