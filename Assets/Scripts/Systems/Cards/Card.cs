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
        private CardVisualDescriptor visualDescriptor;

        public Card(string name, int energyCost, Sprite art, CardRarity rarity, List<EffectData> effects, GameFlowController flowController)
        {
            Name = name;
            EnergyCost = energyCost;
            Art = art;
            Rarity = rarity;
            Effects = effects;
            this.flowController = flowController;
        }

        public void InitializeVisuals(CardVisualDescriptor descriptor)
        {
            visualDescriptor = descriptor;
            visualDescriptor.NameLabel.text = Name;
            visualDescriptor.EnergyCostLabel.text = EnergyCost.ToString();
            visualDescriptor.Image.sprite = Art;
            switch (Rarity)
            {
                case CardRarity.Common:
                    visualDescriptor.RarityIcon.color = Color.white;
                    break;
                case CardRarity.Uncommon:
                    visualDescriptor.RarityIcon.color = Color.green;
                    break;
                case CardRarity.Rare:
                    visualDescriptor.RarityIcon.color = Color.blue;
                    break;
            }

            visualDescriptor.EffectsLabel.text = "";
            foreach (EffectData effectData in Effects)
            {
                visualDescriptor.EffectsLabel.text += effectData.GetCombatEffect().GetDescription(effectData.Target, effectData.Magnitude);
            }
            visualDescriptor.SetReferences(this, flowController);
        }

        public CardVisualDescriptor GetVisualDescriptor()
        {
            return visualDescriptor;
        }
        
        public void RemoveVisuals()
        {
            visualDescriptor = null;
        }
        
        public bool CanBePlayed()
        {
            return HasSufficientEnergy();
        }

        public bool HasSufficientEnergy()
        {
            return flowController.battleController.CurrentEnergy >= EnergyCost;
        }

        public void PlayCard(List<Enemy> targets)
        {
            if (!CanBePlayed())
            {
                Debug.LogError("Card -> PlayCard: Attempted to play a card with insufficient energy.");
                return;
            }

            flowController.battleController.SpendEnergy(EnergyCost);
            
            foreach (var effect in Effects)
            {
                effect.GetCombatEffect().PerformEffect(targets, flowController.PlayerController, effect.Target, effect.Magnitude);
            }
            
            flowController.BoardController.DiscardCard(this, false);
        }
    }
}