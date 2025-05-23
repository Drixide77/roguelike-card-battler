using System.Collections;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class TurnController : GameplayModule
    {
        public enum TurnPhase
        {
            OutOfCombat, StartPlayerTurn, Draw, PlayerActions, EndPlayerTurn, EnemyActions, EndTurn
        }
        
        public TurnPhase CurrentTurnPhase { get; private set; }
        public int CurrentEnergy { get; private set; }

        private Coroutine turnLogicCoroutine;
        private bool combatInProgress = false;
        private bool drawInProgress = false;
        private bool playerActionsInProgress = false;
        private bool enemyActionsInProgress = false;

        private void OnDestroy()
        {
            flowController.GameplaySceneController.EndTurnButtonPressed -= OnEndTurnButtonPressed;
        }

        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            flowController.GameplaySceneController.EndTurnButtonPressed += OnEndTurnButtonPressed;
            CurrentTurnPhase = TurnPhase.OutOfCombat;
        }
        
        public void BeginCombat()
        {
            flowController.GameplaySceneController.SetEndTurnButtonStatus(false, true);
            CurrentEnergy = flowController.PlayerController.GetMaxEnergy();
            flowController.GameplaySceneController.UpdateEnergyLabel(CurrentEnergy, flowController.PlayerController.GetMaxEnergy(), true);
            flowController.PlayerController.OnCombatStart();
            combatInProgress = true;
            
            turnLogicCoroutine = StartCoroutine(TurnLogicCoroutine());
        }

        public void SpendEnergy(int value)
        {
            if (CurrentEnergy - value < 0)
            {
                Debug.LogError("TurnController -> SpendEnergy: attempted to spend " + value + " energy while having only " + CurrentEnergy + " left.");
                CurrentEnergy = 0;
            }
            else CurrentEnergy -= value;
            RefreshEnergyDisplay();
        }

        private void RefreshEnergyDisplay(bool shown = true)
        {
            flowController.GameplaySceneController.UpdateEnergyLabel(CurrentEnergy, flowController.PlayerController.GetMaxEnergy(), shown);
        }
        
        public void OnEnemyActionsFinished()
        {
            enemyActionsInProgress = false;
        }
        
        public void OnEndTurnButtonPressed()
        {
            playerActionsInProgress = false;
            flowController.GameplaySceneController.SetEndTurnButtonStatus(false, true);
        }
        
        public void FinishCombat()
        {
            combatInProgress = false;
            if (turnLogicCoroutine != null) StopCoroutine(turnLogicCoroutine);
            turnLogicCoroutine = null;
            flowController.GameplaySceneController.SetEndTurnButtonStatus(false, false);
            RefreshEnergyDisplay(false);
            CurrentTurnPhase = TurnPhase.OutOfCombat;
        }
        
        private IEnumerator TurnLogicCoroutine()
        {
            while (combatInProgress)
            {
                // Start Player Turn
                CurrentTurnPhase = TurnPhase.StartPlayerTurn;
                Debug.Log("### Start Player Turn");
                CurrentEnergy = flowController.PlayerController.GetMaxEnergy();
                RefreshEnergyDisplay();
                // TODO - Do the rest of initializations
                // TODO - Board Controller: Draw initial hand

                // Draw Phase
                CurrentTurnPhase = TurnPhase.Draw;
                drawInProgress = true;
                // TODO - Board Controller - Draw card
                Debug.Log("### Draw Phase");
                while (drawInProgress)
                {
                    yield return new WaitForSeconds(1f);
                    drawInProgress = false;
                    yield return null;
                }
                
                // Player Actions
                CurrentTurnPhase = TurnPhase.PlayerActions;
                flowController.GameplaySceneController.SetEndTurnButtonStatus(true, true);
                playerActionsInProgress = true;
                // TODO - Multiple controllers handling player actions
                Debug.Log("### Player Actions");
                while (playerActionsInProgress)
                {
                    yield return null;
                }
                
                // End Player Turn
                CurrentTurnPhase = TurnPhase.EndPlayerTurn;
                Debug.Log("### End Player Turn");
                
                // Enemy Actions
                CurrentTurnPhase = TurnPhase.EnemyActions;
                enemyActionsInProgress = true;
                Debug.Log("### Enemy Actions");
                flowController.EncounterController.PerformEnemyActions();
                while (enemyActionsInProgress)
                {
                    yield return null;
                }
                
                // End Turn
                CurrentTurnPhase = TurnPhase.EndTurn;
                Debug.Log("### End Turn");
                
                yield return null;
            }
        }
    }
}