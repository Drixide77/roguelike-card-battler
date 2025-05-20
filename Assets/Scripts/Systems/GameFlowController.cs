using UnityEngine;

namespace MindlessRaptorGames
{
    public class GameFlowController : MonoBehaviour
    {
        [Header("Gameplay Controllers")]
        public PlayerController PlayerController;
        public EncounterController EncounterController;
        public DeckController DeckController;
        
        [HideInInspector] public GameplaySceneController GameplaySceneController;

        private void Start()
        {
            InitializeModules();
        }

        private void InitializeModules()
        {
            PlayerController.Initialize(this);
            EncounterController.Initialize(this);
            DeckController.Initialize(this);
        }

        public void OnPlayerDeath()
        {
            Debug.Log("Game Over!");
            GameplaySceneController.OnRunEnded();
        }
    }
}