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
        public BattleController BattleController;
        public MapController MapController;

        public GameplayState GameState { get; private set; }

        private void OnDestroy()
        {
            GameplaySceneController.UIController.EndRunExitButtonPressed -= OnRunEndExitButtonPressed;
        }
        
        public void Initialize(GameplaySceneController gameplaySceneController)
        {
            GameplaySceneController = gameplaySceneController;
            GameState = GameplayState.None;
            InitializeModules();
            GameplaySceneController.UIController.EndRunExitButtonPressed += OnRunEndExitButtonPressed;
        }

        public void StartRun()
        {
            GameState = GameplayState.RunStart;
            GameplaySceneController.UIController.SetEndTurnButtonStatus(false);
            GameplaySceneController.UIController.SetBoardUIVisibility(false);
            
            GameState = GameplayState.MapInteraction;
            MapController.OnRunStart();
        }

        public void StartEncounter()
        {
            GameState = GameplayState.Encounter;
            BattleController.BeginCombat();
        }
        
        public void EndEncounter()
        {
            GameState = GameplayState.EncounterEnd;
            BattleController.FinishCombat();
            EnterMapMode();
        }

        public void OnPlayerDeath()
        {
            BattleController.FinishCombat();
            OnRunEnded(true);
        }
        
        public void OnRunEnded(bool defeated = false)
        {
            GameState = GameplayState.RunEnd;
            DataService.Instance.SetGameInProgress(false);
            GameplaySceneController.UIController.SetEndRunUIVisibility(true,
                defeated
                    ? "You couldn't make it to the end of the dungeon..."
                    : "And so she left, her spirit hungering for more.");
        }

        private void OnRunEndExitButtonPressed()
        {
            GameplaySceneController.ReturnToMainMenu();
        }
        
        private void InitializeModules()
        {
            PlayerController.Initialize(this);
            EncounterController.Initialize(this);
            DeckController.Initialize(this);
            BoardController.Initialize(this);
            BattleController.Initialize(this);
            MapController.Initialize(this);
        }
        
        private void EnterMapMode()
        {
            GameState = GameplayState.MapInteraction;
            MapController.EnterMapMode();
        }
    }
}