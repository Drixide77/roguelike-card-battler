namespace MindlessRaptorGames
{
    public interface IGameplayModule
    {
        public GameFlowController FlowController { get; set; }
        public void Initialize(GameFlowController gameFlowController);
    }
}