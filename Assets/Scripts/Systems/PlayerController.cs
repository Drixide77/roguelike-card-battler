using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MindlessRaptorGames
{
    // Manages the health, gold, and all the buffs, debuffs, and character statuses
    public class PlayerController : MonoBehaviour, IGameplayModule
    {
        public GameFlowController FlowController { get; set; }
        
        [Header("Attributes")]
        [SerializeField] private int startingHealth = 50;
        private int maximumHealth = 0;
        private int currentHealth = 0;
        [Space(5)]
        [SerializeField] private int startingGold = 0;
        private int currentGold = 0;

        public void Initialize(GameFlowController gameFlowController)
        {
            FlowController = gameFlowController;
            
            maximumHealth = currentHealth = startingHealth;
            currentGold = startingGold;
            FlowController.GameplaySceneController.UpdateHealth(currentHealth, maximumHealth);
            FlowController.GameplaySceneController.UpdateGold(currentGold);
        }

        public void ModifyHealth(int value)
        {
            currentHealth = Math.Min(maximumHealth, Math.Max(0, currentHealth + value));
            if (currentHealth <= 0) FlowController.OnPlayerDeath();
        }
    }
}
