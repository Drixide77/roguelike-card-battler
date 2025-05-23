using System.Collections.Generic;

namespace MindlessRaptorGames
{
    public interface ICardEffect
    {
        public EffectType GetEffectType();
        public void PerformEffect(List<Enemy> targets, int magnitude);
    }
}