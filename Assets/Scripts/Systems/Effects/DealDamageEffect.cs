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

        public string GetDescription(EffectTarget targetType, int magnitude)
        {
            string description = "";
            switch (targetType)
            {
                case EffectTarget.Self:
                    description = "Deal " + magnitude + " damage to yourself.";
                    break;
                case EffectTarget.SingleEnemy:
                    description = "Deal " + magnitude + " damage.";
                    break;
                case EffectTarget.AllEnemies:
                    description = "Deal " + magnitude + " damage to all enemies.";
                    break;
                case EffectTarget.AllBoard:
                    description = "Deal " + magnitude + " damage to all enemies and yourself.";
                    break;
                default:
                    break;
            }
            return description;
        }
    }
}