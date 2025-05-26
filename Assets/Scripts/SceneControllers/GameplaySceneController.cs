using UnityEngine;
using UnityEngine.SceneManagement;

namespace MindlessRaptorGames
{
    public class GameplaySceneController : MonoBehaviour
    {
        [Header("Controllers")]
        public UIController UIController;
        [SerializeField] private GameFlowController flowController;
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;

        private void Awake()
        {
            flowController.Initialize(this);
        }

        private void Start()
        {
            flowController.StartRun();
        }
        
        public void OnRunEnded()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
