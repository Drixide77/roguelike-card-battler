using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "DealDamage", menuName = "Game Elements/Effects/Deal Damage", order = 1)]
    public class DealDamageEffect : ScriptableObject, ICombatEffect
    {
        public EffectType GetEffectType()
        {
            return EffectType.DealDamage;
        }

        public void PerformEffect(List<Enemy> enemyTargets, PlayerController playerController, EffectTarget targetType, int magnitude)
        {
            Enemy[] tempList = new Enemy[enemyTargets.Count];
            switch (targetType)
            {
                case EffectTarget.Player:
                    playerController.ModifyHealth(-magnitude);
                    break;
                case EffectTarget.SingleEnemy: case EffectTarget.AllEnemies:
                    enemyTargets.CopyTo(tempList);
                    for(int i = 0; i < tempList.Length; ++i)
                    {
                        tempList[i]?.ModifyHealth(-magnitude);
                    }
                    break;
                case EffectTarget.AllBoard:
                    enemyTargets.CopyTo(tempList);
                    for(int i = 0; i < tempList.Length; ++i)
                    {
                        tempList[i]?.ModifyHealth(-magnitude);
                    }
                    playerController.ModifyHealth(-magnitude);
                    break;
                default:
                    break;
            }
        }

        public string GetDescription(EffectTarget targetType, int magnitude)
        {
            string description = "";
            switch (targetType)
            {
                case EffectTarget.Player:
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