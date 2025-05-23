using System;
using UnityEngine;

namespace MindlessRaptorGames
{
    public enum EffectType
    {
        DealDamage, GainBlock
    }

    public enum EffectTarget
    {
        Self, SingleEnemy, AllEnemies, AllBoard
    }
    
    [Serializable]
    public class EffectData
    {
        public int Magnitude;
        public EffectTarget Target;
        public ScriptableObject EffectSO;
        
        public ICardEffect GetCardEffect()
        {
            return EffectSO as ICardEffect;
        }
    }
}