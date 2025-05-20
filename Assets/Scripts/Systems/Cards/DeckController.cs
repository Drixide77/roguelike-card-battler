using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    // Manages the total card library, and the current cards in your deck (not the draw pile)
    public class DeckController : MonoBehaviour, IGameplayModule
    {
        public GameFlowController FlowController { get; set; }
        
        private List<Card> cardLibrary; // ALL the cards that exist in the game
        private List<Card> currentDeck; // Cards in the players deck

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            FlowController.GameplaySceneController.DeckButtonPressed -= OnDeckButtonPressed;
        }

        public void Initialize(GameFlowController gameFlowController)
        {
            FlowController = gameFlowController;
            FlowController.GameplaySceneController.DeckButtonPressed += OnDeckButtonPressed;
            cardLibrary = DataService.Instance.GetCardCollection();
            currentDeck = DataService.Instance.GetStartingDeck();
            FlowController.GameplaySceneController.UpdateDeckLabel(currentDeck.Count);
        }

        private void OnDeckButtonPressed()
        {
            // TODO - Implement
            Debug.Log("The card library contains " + cardLibrary.Count + " cards.");
        }
    }
}