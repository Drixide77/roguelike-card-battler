using System.Collections.Generic;

namespace MindlessRaptorGames
{
    public interface ICombatEffect
    {
        public EffectType GetEffectType();
        public void PerformEffect(List<Enemy> enemyTargets, PlayerController playerController, EffectTarget targetType, int magnitude);
        public string GetDescription(EffectTarget targetType, int magnitude, bool isCard);
    }
}