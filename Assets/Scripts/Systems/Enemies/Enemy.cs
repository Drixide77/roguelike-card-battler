using System;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class Enemy
    {
        public string Name;
        public Sprite Sprite;
        public int MaxHealth;
        public int Damage;

        private GameFlowController flowController;
        private EnemyVisualDescriptor visualDescriptor;
        private int currentHealth;
        private string enemyId;

        public void Initialize(GameFlowController flowController, EnemyVisualDescriptor visualDescriptor, string enemyId)
        {
            currentHealth = MaxHealth;
            this.enemyId = enemyId;
            this.flowController = flowController;
            this.visualDescriptor = visualDescriptor;
            
            this.visualDescriptor.gameObject.SetActive(true);
            this.visualDescriptor.NameLabel.text = Name;
            this.visualDescriptor.Image.sprite = Sprite;
            this.visualDescriptor.HealthFillImage.fillAmount = 1f;
            this.visualDescriptor.HealthLabel.text = currentHealth + "/" + MaxHealth;
            this.visualDescriptor.NextActionDisplay.text = "Attk " + Damage;
            this.visualDescriptor.EnemyId = enemyId;
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public string GetEnemyId()
        {
            return enemyId;
        }
        
        public void ModifyHealth(int value)
        {
            currentHealth = Math.Min(MaxHealth, Math.Max(0, currentHealth + value));
            visualDescriptor.HealthLabel.text = currentHealth + "/" + MaxHealth;
            visualDescriptor.HealthFillImage.fillAmount = currentHealth / (float)MaxHealth;
            // Update health on UI
            if (currentHealth <= 0)
            {
                flowController.EncounterController.EnemyDefeated?.Invoke(this);
            }
        }
        
        public void PerformEnemyAction(GameFlowController flowController)
        {
            // TODO - Have proper enemy action logic
            flowController.PlayerController.ModifyHealth(-Damage);
        }
    }
}