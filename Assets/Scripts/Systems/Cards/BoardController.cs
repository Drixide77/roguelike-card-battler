using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class BoardController : GameplayModule
    {
        [Header("Settings")]
        [SerializeField] private int initialHandSize = 5;
        [SerializeField] private int maxHandSize = 10;
        
        private List<Card> drawPile; // Cards remaining to be drawn into the hand
        private List<Card> discardPile; // Cards that have been played, awaiting to be reshuffled into the draw pile
        private List<Card> exhaustPile; // Cards that have been exhausted and will not go back to the draw pile
        
        private List<Card> playerHand; // The current hand of cards
        
        private void OnDestroy()
        {
            flowController.GameplaySceneController.UIController.DrawPileButtonPressed -= OnDrawPileButtonPressed;
            flowController.GameplaySceneController.UIController.DiscardPileButtonPressed -= OnDiscardPileButtonPressed;
        }
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            
            flowController.GameplaySceneController.UIController.DrawPileButtonPressed += OnDrawPileButtonPressed;
            flowController.GameplaySceneController.UIController.DiscardPileButtonPressed += OnDiscardPileButtonPressed;
        }

        public void OnCombatStart()
        {
            discardPile = new List<Card>();
            exhaustPile = new List<Card>();
            drawPile = new List<Card>();
            playerHand = new List<Card>();
            foreach (var card in flowController.DeckController.GetCurrentDeck())
            {
                drawPile.Add(card);
            }
            drawPile.Shuffle();
            UpdateBoardUI();
        }

        public void OnCombatEnd()
        {
            // ---
        }

        public List<Card> GetDrawPile()
        {
            return drawPile;
        }

        public List<Card> GetDiscardPile()
        {
            return discardPile;
        }

        public List<Card> GetExhaustPile()
        {
            return exhaustPile;
        }

        public List<Card> GetHand()
        {
            return playerHand;
        }
        
        public void DrawCards(int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                if (drawPile.Count <= 0)
                {
                    if (discardPile.Count > 0)
                        ReshuffleDrawPile();
                    else
                    {
                        // The odd case where you entire deck has been exhausted?
                        flowController.TurnController.OnDrawingCompleted();
                        UpdateBoardUI();
                        return;
                    }
                }
                if (playerHand.Count < maxHandSize)
                {
                    playerHand.Add(drawPile[0]);
                    drawPile.RemoveAt(0);
                }
                else // Hand is full
                {
                    discardPile.Add(drawPile[0]);
                    drawPile.RemoveAt(0);
                }
            }
            flowController.TurnController.OnDrawingCompleted();
            UpdateBoardUI();
        }

        public void DiscardCard(Card card, bool exhaust)
        {
            if (playerHand.Remove(card))
            {
                if (exhaust) exhaustPile.Add(card);
                else discardPile.Add(card);
            }
            else
            {
                Debug.LogError("BoardController -> DiscardCard: Attempted to discard card not present in hand.");
            }
            UpdateBoardUI();
        }

        public void DiscardHand()
        {
            foreach (var card in playerHand)
            {
                discardPile.Add(card);
            }
            playerHand.Clear();
            UpdateBoardUI();
        }

        public void DrawNewHand()
        {
            DrawCards(initialHandSize);
        }

        public void UpdateBoardUI()
        {
            flowController.GameplaySceneController.UIController.UpdateDrawPileLabel(drawPile.Count);
            flowController.GameplaySceneController.UIController.UpdateDiscardPileLabel(discardPile.Count);
        }
        
        private void OnDrawPileButtonPressed()
        {
            // TODO - Implement
            Debug.Log("The draw pile contains " + drawPile.Count + " cards.");
        }
        
        private void OnDiscardPileButtonPressed()
        {
            // TODO - Implement
            Debug.Log("The discard pile contains " + discardPile.Count + " cards.");
        }
        
        private void ReshuffleDrawPile()
        {
            foreach (var card in discardPile)
            {
                drawPile.Add(card);
            }
            discardPile.Clear();
            drawPile.Shuffle();
        }
    }
}