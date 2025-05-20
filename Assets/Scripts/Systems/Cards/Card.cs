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
    }
}