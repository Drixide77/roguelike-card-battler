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
        
        public void PerformEffect()
        {
            //
        }
    }
}