using System;
using UnityEngine;

namespace MindlessRaptorGames
{
    // Manages the health, gold, and all the buffs, debuffs, and character statuses
    public class PlayerController : GameplayModule
    {
        [Header("Attributes")]
        [SerializeField] private int startingHealth = 50;
        [SerializeField] private int startingGold = 0;
        [SerializeField] private int startingMaxEnergy = 3;
        
        private int maximumHealth = 0;
        private int currentHealth = 0;
        private int currentGold = 0;
        private int currentMaxEnergy = 0;

        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            
            maximumHealth = currentHealth = startingHealth;
            currentGold = startingGold;
            currentMaxEnergy = startingMaxEnergy;
            flowController.GameplaySceneController.UpdateHealth(currentHealth, maximumHealth);
            flowController.GameplaySceneController.UpdateGold(currentGold);
            flowController.GameplaySceneController.UpdateEnergyLabel(currentMaxEnergy, currentMaxEnergy, false);
        }

        public int GetMaxEnergy()
        {
            return currentMaxEnergy;
        }
        
        public void OnCombatStart()
        {
            flowController.GameplaySceneController.UpdateHealth(currentHealth, maximumHealth);
            flowController.GameplaySceneController.UpdateGold(currentGold);
            flowController.GameplaySceneController.UpdateEnergyLabel(currentMaxEnergy, currentMaxEnergy, true);
        }
        
        public void ModifyHealth(int value)
        {
            currentHealth = Math.Min(maximumHealth, Math.Max(0, currentHealth + value));
            flowController.GameplaySceneController.UpdateHealth(currentHealth, maximumHealth);
            if (currentHealth <= 0) flowController.OnPlayerDeath();
        }
    }
}
