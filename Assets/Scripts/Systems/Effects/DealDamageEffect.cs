using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "DealDamage", menuName = "Game Elements/Effects/Deal Damage", order = 1)]
    public class DealDamageEffect : ScriptableObject, ICardEffect
    {
        public EffectType GetEffectType()
        {
            return EffectType.DealDamage;
        }
        
        public void PerformEffect(List<Enemy> targets, int magnitude)
        {
            if (targets.Count == 0) Debug.LogError("DealDamageEffect -> PerformEffect: Attempted to perform effect without any targets.");
            foreach (var target in targets)
            {
                target.ModifyHealth(-magnitude);
            }
        }
    }
}