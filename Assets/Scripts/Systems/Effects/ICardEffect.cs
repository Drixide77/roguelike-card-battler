namespace MindlessRaptorGames
{
    public interface ICardEffect
    {
        public EffectType GetEffectType();
        public void PerformEffect();
    }
}