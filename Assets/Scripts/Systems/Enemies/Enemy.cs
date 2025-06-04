using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class Enemy
    {
        public string Name;
        public Sprite Sprite;
        public int MaxHealth;
        public List<EffectData> Actions;

        private GameFlowController flowController;
        private EnemyVisualDescriptor visualDescriptor;
        private int currentHealth;
        private string enemyId;
        private int currentActionIndex = 0;

        public void Initialize(GameFlowController flowController, EnemyVisualDescriptor visualDescriptor, string enemyId)
        {
            currentHealth = MaxHealth;
            this.enemyId = enemyId;
            this.flowController = flowController;
            this.visualDescriptor = visualDescriptor;
            currentActionIndex = 0;
            
            this.visualDescriptor.gameObject.SetActive(true);
            this.visualDescriptor.HighlightImage.gameObject.SetActive(false);
            this.visualDescriptor.NameLabel.text = Name;
            this.visualDescriptor.Image.sprite = Sprite;
            this.visualDescriptor.HealthFillImage.fillAmount = 1f;
            this.visualDescriptor.HealthLabel.text = currentHealth + "/" + MaxHealth;
            this.visualDescriptor.NextActionDisplay.text = "";
            this.visualDescriptor.EnemyId = enemyId;
            
            Actions.Shuffle();
            SetNextActionDisplay();
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

        public void SetHighlight(bool active)
        {
            visualDescriptor.HighlightImage.gameObject.SetActive(active);
        }
        
        public void PerformEnemyAction(GameFlowController flowController)
        {
            EffectData effect = Actions[currentActionIndex];
            effect.GetCombatEffect().PerformEffect(null, flowController.PlayerController, effect.Target, effect.Magnitude);
            currentActionIndex = (currentActionIndex + 1) % Actions.Count;
            SetNextActionDisplay();
        }

        private void SetNextActionDisplay()
        {
            visualDescriptor.NextActionDisplay.text = (Actions[currentActionIndex].EffectSO as ICombatEffect)?.GetDescription(EffectTarget.Player, Actions[currentActionIndex].Magnitude, false);
        }
    }
}