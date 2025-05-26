using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public enum CardRarity
    {
        Common, Uncommon, Rare
    }
    
    [Serializable]
    public class Card
    {
        public string Name;
        public int EnergyCost;
        public Sprite Art;
        public CardRarity Rarity;
        public List<EffectData> Effects;

        private GameFlowController flowController;

        public Card(string name, int energyCost, Sprite art, CardRarity rarity, List<EffectData> effects, GameFlowController flowController)
        {
            Name = name;
            EnergyCost = energyCost;
            Art = art;
            Rarity = rarity;
            Effects = effects;
            this.flowController = flowController;
        }
        
        public bool CanBePlayed()
        {
            return HasSufficientEnergy();
        }

        public bool HasSufficientEnergy()
        {
            return flowController.TurnController.CurrentEnergy >= EnergyCost;
        }

        public void PlayCard(List<Enemy> targets)
        {
            if (!CanBePlayed())
            {
                Debug.LogError("Card -> PlayCard: Attempted to play a card with insufficient energy.");
                return;
            }

            flowController.TurnController.SpendEnergy(EnergyCost);
            
            foreach (var effect in Effects)
            {
                effect.GetCardEffect().PerformEffect(targets, effect.Magnitude);
            }
            
            flowController.BoardController.DiscardCard(this, false);
        }
    }
}