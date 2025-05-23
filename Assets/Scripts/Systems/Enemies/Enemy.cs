using System;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class Enemy
    {
        public string Name;
        public int MaxHealth;
        public int Damage;

        private GameFlowController flowController;
        private int currentHealth;

        public void Initialize(GameFlowController flowController)
        {
            currentHealth = MaxHealth;
            this.flowController = flowController;
        }

        public void ModifyHealth(int value)
        {
            currentHealth = Math.Min(MaxHealth, Math.Max(0, currentHealth + value));
            // Update health on UI
            if (currentHealth <= 0)
            {
                flowController.EncounterController.EnemyDefeated?.Invoke(this);
            }
        }
        
        public void PerformEnemyAction(GameFlowController flowController)
        {
            // TODO - Have proper enemy action logic
            Debug.Log(Name + "(" + currentHealth + "/" + MaxHealth + ") dealt " + Damage + " damage.");
            flowController.PlayerController.ModifyHealth(-Damage);
        }
    }
}