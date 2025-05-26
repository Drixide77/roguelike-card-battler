using UnityEngine;

namespace MindlessRaptorGames
{
    public class GameFlowController : MonoBehaviour
    {
        public enum GameplayState
        {
            None, RunStart, MapInteraction, Encounter, EncounterEnd, RunEnd
        }
        
        public GameplaySceneController GameplaySceneController { get; private set; }
        
        [Header("Gameplay Controllers")]
        public PlayerController PlayerController;
        public EncounterController EncounterController;
        public DeckController DeckController;
        public BoardController BoardController;
        public TurnController TurnController;

        public GameplayState GameState { get; private set; }

        public void Initialize(GameplaySceneController gameplaySceneController)
        {
            GameplaySceneController = gameplaySceneController;
            InitializeModules();
        }
        
        private void InitializeModules()
        {
            PlayerController.Initialize(this);
            EncounterController.Initialize(this);
            DeckController.Initialize(this);
            BoardController.Initialize(this);
            TurnController.Initialize(this);
        }

        public void StartRun()
        {
            GameState = GameplayState.RunStart;
            GameplaySceneController.UIController.SetEndTurnButtonStatus(false);
            GameplaySceneController.UIController.SetBoardUIVisibility(false);
            StartEncounter();
        }

        public void StartEncounter()
        {
            TurnController.BeginCombat();
        }
        
        public void EndEncounter()
        {
            TurnController.FinishCombat();
        }

        public void OnPlayerDeath()
        {
            Debug.Log("Game Over!");
            TurnController.FinishCombat();
            DataService.Instance.SetGameInProgress(false);
            GameplaySceneController.OnRunEnded();
        }
    }
}