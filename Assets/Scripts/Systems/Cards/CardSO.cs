using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Card", menuName = "Game Elements/Card", order = 1)]
    public class CardSO : ScriptableObject
    {
        [Header("Attributes")]
        public string Name;
        public int EnergyCost;
        public Sprite Art;
        public CardRarity Rarity;
        public List<EffectData> Effects;

        public Card ToCard()
        {
            Card card = new Card
            {
                Name = Name,
                EnergyCost = EnergyCost,
                Art = Art,
                Rarity = Rarity,
                Effects = Effects
            };
            return card;
        }
    }
}