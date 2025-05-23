using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    // Manages the total card library, and the current cards in your deck (not the draw pile)
    public class DeckController : GameplayModule
    {
        private List<Card> cardLibrary; // ALL the cards that exist in the game
        private List<Card> currentDeck; // Cards in the players deck

        private void OnDestroy()
        {
            flowController.GameplaySceneController.DeckButtonPressed -= OnDeckButtonPressed;
        }
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            
            flowController.GameplaySceneController.DeckButtonPressed += OnDeckButtonPressed;
            cardLibrary = DataService.Instance.GetCardCollection(flowController);
            currentDeck = DataService.Instance.GetStartingDeck(flowController);
            flowController.GameplaySceneController.UpdateDeckLabel(currentDeck.Count);
        }

        private void OnDeckButtonPressed()
        {
            // TODO - Implement
            Debug.Log("The card library contains " + cardLibrary.Count + " cards.");
            
            // TODO- TEMP! Remove
            if (currentDeck[0].CanBePlayed())
                currentDeck[0].PlayCard(new List<Enemy>() {flowController.EncounterController.GetEnemies()[0]});
        }
    }
}