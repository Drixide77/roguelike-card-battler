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
        Player, SingleEnemy, AllEnemies, AllBoard
    }
    
    [Serializable]
    public class EffectData
    {
        public int Magnitude;
        public EffectTarget Target;
        public ScriptableObject EffectSO;
        
        public ICombatEffect GetCombatEffect()
        {
            return EffectSO as ICombatEffect;
        }
    }
}