using System.Collections;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class EncounterController : GameplayModule
    {
        // TODO - Have proper encounters and multiples enemies!
        [SerializeField] private EnemySO enemySO;

        private Enemy enemy;
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            enemy = enemySO.ToEnemy();
        }

        public void PerformEnemyActions()
        {
            enemy.PerformEnemyAction(flowController);
            StartCoroutine(FinishEnemyActionsWithDelayCoroutine(1f));
        }
        
        // TEMP
        private IEnumerator FinishEnemyActionsWithDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            flowController.TurnController.OnEnemyActionsFinished();
        }
    }
}