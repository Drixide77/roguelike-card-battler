using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class EncounterController : GameplayModule
    {
        public Action<Enemy> EnemyDefeated;
        
        // TODO - Have proper encounters and multiples enemies!
        [SerializeField] private EnemySO enemySO;

        private List<Enemy> enemies;

        private void OnDestroy()
        {
            EnemyDefeated -= OnEnemyDefeated;
        }

        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            EnemyDefeated += OnEnemyDefeated;
            Enemy tempEnemy = enemySO.ToEnemy();
            tempEnemy.Initialize(gameFlowController);
            enemies = new List<Enemy> { tempEnemy };
        }

        public List<Enemy> GetEnemies()
        {
            return enemies;
        }
        
        public void PerformEnemyActions()
        {
            foreach (var enemy in enemies)
            {
                enemy.PerformEnemyAction(flowController);
            }
            StartCoroutine(FinishEnemyActionsWithDelayCoroutine(1f));
        }

        private void OnEnemyDefeated(Enemy enemy)
        {
            enemies.Remove(enemy);
            if (enemies.Count <= 0)
            {
                flowController.EndEncounter();
            }
        }
        
        // TEMP
        private IEnumerator FinishEnemyActionsWithDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            flowController.TurnController.OnEnemyActionsFinished();
        }
    }
}