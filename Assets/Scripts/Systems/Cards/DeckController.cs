using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    // Manages the total card library, and the current cards in your deck (not the draw pile)
    public class DeckController : GameplayModule
    {
        private List<Card> cardLibrary; // ALL the cards that exist in the game
        private List<Card> currentDeck; // Cards in the players deck (this is NOT the draw pile!)

        private void OnDestroy()
        {
            flowController.GameplaySceneController.UIController.DeckButtonPressed -= OnDeckButtonPressed;
        }
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            
            flowController.GameplaySceneController.UIController.DeckButtonPressed += OnDeckButtonPressed;
            cardLibrary = DataService.Instance.GetCardCollection(flowController);
            currentDeck = DataService.Instance.GetStartingDeck(flowController);
            flowController.GameplaySceneController.UIController.UpdateDeckLabel(currentDeck.Count);
        }

        public List<Card> GetCurrentDeck()
        {
            return currentDeck;
        }
        
        private void OnDeckButtonPressed()
        {
            // TODO - Implement
            Debug.Log("The card library contains " + cardLibrary.Count + " cards.");
        }
    }
}