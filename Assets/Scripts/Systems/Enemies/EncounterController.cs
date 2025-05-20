using UnityEngine;

namespace MindlessRaptorGames
{
    public class EncounterController : MonoBehaviour, IGameplayModule
    {
        public GameFlowController FlowController { get; set; }
        
        // TODO - Have proper encounters and multiples enemies!
        [SerializeField] private EnemySO enemySO;

        private Enemy enemy;
        
        public void Initialize(GameFlowController gameFlowController)
        {
            FlowController = gameFlowController;
            enemy = enemySO.ToEnemy();
        }
    }
}