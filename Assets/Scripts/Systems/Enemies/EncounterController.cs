using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class EncounterController : GameplayModule
    {
        public Action<Enemy> EnemyDefeated;

        [Header("Prefabs and Data")]
        [SerializeField] private EnemyVisualDescriptor enemyVisualPrefab;
        [Header("Scene References")]
        [SerializeField] private CanvasGroup encounterCanvasGroup;
        [SerializeField] private Transform enemiesParent;
        
        private List<EncounterData> encounterCollection;
        
        private List<EnemyVisualDescriptor> visualPrefabs;
        private List<Enemy> encounterEnemies;

        private void OnDestroy()
        {
            EnemyDefeated -= OnEnemyDefeated;
        }

        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            EnemyDefeated += OnEnemyDefeated;
            encounterCollection = DataService.Instance.GetEncounterCollection();
            visualPrefabs = new List<EnemyVisualDescriptor>();
            encounterEnemies = new List<Enemy>();
        }

        public void OnCombatStart()
        {
            SetEncounterEnemies(encounterCollection.GetRandomElement());
            
            Utils.SetCanvasGroupVisible(encounterCanvasGroup, true);
        }

        public void OnCombatEnd()
        {
            Utils.SetCanvasGroupVisible(encounterCanvasGroup, false);
        }

        public List<Enemy> GetEnemies()
        {
            return encounterEnemies;
        }
        
        public void PerformEnemyActions()
        {
            foreach (var enemy in encounterEnemies)
            {
                enemy.PerformEnemyAction(flowController);
            }
            StartCoroutine(FinishEnemyActionsWithDelayCoroutine(1f));
        }

        public Enemy GetEnemyById(string id)
        {
            foreach (var enemy in encounterEnemies)
            {
                if (enemy.GetEnemyId() == id) return enemy;
            }
            return null;
        }

        private void SetEncounterEnemies(EncounterData encounter)
        {
            encounterEnemies.Clear();
            for (int i = 0; i < encounter.Enemies.Count; i++)
            {
                if (i >= visualPrefabs.Count)
                {
                    visualPrefabs.Add(Instantiate(enemyVisualPrefab.gameObject, enemiesParent).GetComponent<EnemyVisualDescriptor>());
                }

                Enemy enemy = encounter.Enemies[i];
                enemy.Initialize(flowController, visualPrefabs[i], "enemy"+i);
                encounterEnemies.Add(enemy);
            }
        }
        private void OnEnemyDefeated(Enemy enemy)
        {
            int index = encounterEnemies.FindIndex((encounterEnemy) => encounterEnemy == enemy);
            encounterEnemies.RemoveAt(index);
            visualPrefabs[index].gameObject.SetActive(false);
            if (encounterEnemies.Count <= 0)
            {
                flowController.EndEncounter();
            }
        }
        
        // TODO - TEMP! To be removed
        private IEnumerator FinishEnemyActionsWithDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            flowController.battleController.OnEnemyActionsFinished();
        }
    }
}