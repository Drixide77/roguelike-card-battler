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
        public BattleController battleController;

        public GameplayState GameState { get; private set; }

        public void Initialize(GameplaySceneController gameplaySceneController)
        {
            GameplaySceneController = gameplaySceneController;
            GameState = GameplayState.None;
            InitializeModules();
        }
        
        private void InitializeModules()
        {
            PlayerController.Initialize(this);
            EncounterController.Initialize(this);
            DeckController.Initialize(this);
            BoardController.Initialize(this);
            battleController.Initialize(this);
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
            GameState = GameplayState.Encounter;
            battleController.BeginCombat();
        }
        
        public void EndEncounter()
        {
            GameState = GameplayState.EncounterEnd;
            battleController.FinishCombat();
        }

        public void OnPlayerDeath()
        {
            GameState = GameplayState.RunEnd;
            Debug.Log("Game Over!");
            battleController.FinishCombat();
            DataService.Instance.SetGameInProgress(false);
            GameplaySceneController.OnRunEnded();
        }
    }
}