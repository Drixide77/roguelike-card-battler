using UnityEngine;

namespace MindlessRaptorGames
{
    public class GameplaySceneController : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;

        private void Start()
        {
            GameSettingsController.Instance.RefreshSettings();
        }

        private void Update()
        {
            if (!GameSettingsController.Instance.SettingsPanelShowing())
            {
                if (Input.GetKeyDown(KeybindingsDefinition.PauseKey1) ||
                    Input.GetKeyDown(KeybindingsDefinition.PauseKey2))
                {
                    GameSettingsController.Instance.ShowSettings();
                }
            }
        }
    }
}
