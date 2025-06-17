using UnityEngine;

namespace MindlessRaptorGames
{
    public class GameplaySceneController : MonoBehaviour
    {
        [Header("Scene Controllers")]
        public UIController UIController;
        [SerializeField] private GameFlowController flowController;
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;

        private void Start()
        {
            flowController.Initialize(this);
            flowController.StartRun();
        }
        
        public void ReturnToMainMenu()
        {
            AppControlService.Instance.LoadNewScene(mainMenuSceneName);
        }
    }
}
