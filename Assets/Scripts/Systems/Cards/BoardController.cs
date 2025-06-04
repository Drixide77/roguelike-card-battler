using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class BoardController : GameplayModule
    {
        [Header("Scene References")]
        [SerializeField] private Transform handPosition;
        [SerializeField] private Image boardHighlightImage;
        [Header("Settings")]
        [SerializeField] private int initialHandSize = 5;
        [SerializeField] private int maxHandSize = 10;
        [SerializeField] private float fanSpread = -7.5f;
        [SerializeField] private float cardSpacing = 150f;
        [SerializeField] private float verticalSpacing = 100f;
        [Header("Assets")]
        [SerializeField] private CardVisualDescriptor cardPrefab;
        
        private List<Card> drawPile; // Cards remaining to be drawn into the hand
        private List<Card> discardPile; // Cards that have been played, awaiting to be reshuffled into the draw pile
        private List<Card> exhaustPile; // Cards that have been exhausted and will not go back to the draw pile
        
        private List<Card> playerHand; // The current hand of cards
        private List<CardVisualDescriptor> visualPrefabs; // The visual prefabs for the hand cards
        
        private Card selectedCard; // The currently selected card
        private bool cardIsSelected = false; // Whether a card is selected at the momoent
        
        private bool requestBoardRedraw = false;

        private void Update()
        {
            if (cardIsSelected && Input.GetMouseButtonUp(0))
            {
                RaycastHit2D[] cubeHit = Physics2D.RaycastAll(Input.mousePosition, Vector2.zero);
                if (cubeHit.Length > 0)
                {
                    foreach (RaycastHit2D hit in cubeHit)
                    {
                        if (hit.collider.CompareTag("Board"))
                        {
                            selectedCard.PlayCard(flowController.EncounterController.GetEnemies());
                            SetSelectedCard(false, EffectTarget.SingleEnemy);
                            return;
                        }
                    }
                    foreach (RaycastHit2D hit in cubeHit)
                    {
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            string enemyId = hit.collider.gameObject.GetComponent<EnemyVisualDescriptor>().EnemyId;
                            selectedCard.PlayCard(new List<Enemy>() { flowController.EncounterController.GetEnemyById(enemyId) });
                            SetSelectedCard(false, EffectTarget.SingleEnemy);
                            return;
                        }
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (requestBoardRedraw)
            {
                flowController.GameplaySceneController.UIController.UpdateDrawPileLabel(drawPile.Count);
                flowController.GameplaySceneController.UIController.UpdateDiscardPileLabel(discardPile.Count);
                UpdateHandVisuals();
                requestBoardRedraw = false;
            }
        }

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
            visualPrefabs = new List<CardVisualDescriptor>();
            foreach (var card in flowController.DeckController.GetCurrentDeck())
            {
                drawPile.Add(card);
            }
            drawPile.Shuffle();
            RequestBoardRedraw();
        }

        public void OnCombatEnd()
        {
            DiscardHand();
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
                        flowController.BattleController.OnDrawingCompleted();
                        RequestBoardRedraw();
                        return;
                    }
                }
                if (playerHand.Count < maxHandSize)
                {
                    CardVisualDescriptor descriptor = GetVisualDescriptor();
                    drawPile[0].InitializeVisuals(descriptor);
                    descriptor.ResetInteractionVisuals();
                    playerHand.Add(drawPile[0]);
                    drawPile.RemoveAt(0);
                }
                else // Hand is full
                {
                    discardPile.Add(drawPile[0]);
                    drawPile.RemoveAt(0);
                }
            }
            flowController.BattleController.OnDrawingCompleted();
            RequestBoardRedraw();
        }
        

        public void DiscardCard(Card card, bool exhaust)
        {
            if (playerHand.Remove(card))
            {
                if (exhaust) exhaustPile.Add(card);
                else discardPile.Add(card);
                card.GetVisualDescriptor().gameObject.SetActive(false);
                card.RemoveVisuals();
            }
            else
            {
                Debug.LogError("BoardController -> DiscardCard: Attempted to discard card not present in hand.");
            }

            RequestBoardRedraw();
        }

        public void DiscardHand()
        {
            while (playerHand.Count > 0)
            {
                // TODO - Handle exhaust case
                DiscardCard(playerHand[0], false);
            }
        }

        public void DrawNewHand()
        {
            DrawCards(initialHandSize);
        }

        public void SetSelectedCard(bool selected, EffectTarget target, Card card = null)
        {
            SetTargetHighlights(target, selected);
            if (!selected)
            {
                cardIsSelected = false;
                return;
            }
            selectedCard = card;
            cardIsSelected = true;
        }

        private void RequestBoardRedraw()
        {
            requestBoardRedraw = true;
        }

        private CardVisualDescriptor GetVisualDescriptor()
        {
            foreach (CardVisualDescriptor prefab in visualPrefabs)
            {
                if (!prefab.gameObject.activeInHierarchy)
                {
                    prefab.gameObject.SetActive(true);
                    return prefab;
                }
            }
            
            CardVisualDescriptor descriptor = Instantiate(cardPrefab, handPosition.position, Quaternion.identity, handPosition).GetComponent<CardVisualDescriptor>();
            visualPrefabs.Add(descriptor);
            return descriptor;
        }

        private void SetTargetHighlights(EffectTarget target, bool active)
        {
            // TODO - Handle cards that target the player
            //flowController.PlayerController ?
            boardHighlightImage.gameObject.SetActive(target == EffectTarget.AllEnemies && active);
            foreach (Enemy enemy in flowController.EncounterController.GetEnemies())
            {
                enemy.SetHighlight(target == EffectTarget.SingleEnemy && active);
            }
        }
        
        private void UpdateHandVisuals()
        {
            List<CardVisualDescriptor> activePrefabs = new List<CardVisualDescriptor>();
            foreach (CardVisualDescriptor prefab in visualPrefabs)
            {
                if (prefab.gameObject.activeInHierarchy) activePrefabs.Add(prefab);
            }
            
            int cardCount = activePrefabs.Count;

            if (cardCount == 1)
            {
                activePrefabs[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                activePrefabs[0].transform.localPosition = new Vector3(0f, 0f, 0f);
                return;
            }

            for (int i = 0; i < cardCount; i++)
            {
                float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
                activePrefabs[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

                float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

                float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalize card position between -1, 1
                float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

                //Set card position
                activePrefabs[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
            }
        }
        
        private void OnDrawPileButtonPressed()
        {
            CardViewerController.Instance.ShowCardView("- Draw Pile (" + drawPile.Count + ") -", drawPile);
        }
        
        private void OnDiscardPileButtonPressed()
        {
            CardViewerController.Instance.ShowCardView("- Discard Pile (" + discardPile.Count + ") -", discardPile);
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