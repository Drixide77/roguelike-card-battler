using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace MindlessRaptorGames
{
    public class AppControlService : MonoBehaviour
    {
        // Singleton pattern
        public static AppControlService Instance { get; private set; }
        
        [HideInInspector] public bool firstTimeOnMainMenu;
        [Header("Resolution")]
        [SerializeField] private Vector2Int windowedResolution = new (1280, 720);
        [SerializeField] private float[] resolutionMultipliers = { 1f, 2f, 3f };
        [Header("Fader")]
        [SerializeField] private CanvasGroup fader;
        [SerializeField] private float fadeDuration = 0.5f;
        private bool fullscreen = false;
        private int resolutionIndex = 0;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.FullscreenToggleKey))
            {
                ToggleFullscreen();
            }
        }

        private void Initialize()
        {
            Utils.SetCanvasGroupVisible(fader, false);
            firstTimeOnMainMenu = true;
            // Required for 2D Raycasts
            Physics2D.queriesHitTriggers = true;
            // Initializing DOTween
            DOTween.Init();
        }
        
        public void ToggleFullscreen()
        {
            fullscreen = !Screen.fullScreen;
            SetFullscreen(fullscreen);
        }

        public void SetFullscreen(bool fullscreen)
        {
            this.fullscreen = fullscreen;
            if (this.fullscreen)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else
            {
                SetResolution(resolutionIndex);
            }
        }

        public int GetResolutionPresetCount()
        {
            return resolutionMultipliers.Length;
        }

        public float GetCurrentResolutionMultiplier()
        {
            return resolutionMultipliers[resolutionIndex];
        }

        public void SetResolution(int index, bool fullscreen = false)
        {
            resolutionIndex = Mathf.Clamp(index, 0, resolutionMultipliers.Length - 1);
            Screen.SetResolution(Mathf.RoundToInt(windowedResolution.x * resolutionMultipliers[resolutionIndex]), Mathf.RoundToInt(windowedResolution.y * resolutionMultipliers[resolutionIndex]), fullscreen);
        }

        public void LoadNewScene(string sceneName, bool doFade = true)
        {
            if (doFade) StartCoroutine(LoadNewSceneAsync(sceneName));
            else SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
        
        public void ExitApplication()
        {
            Debug.Log("Exiting game...");
            fader.interactable = true;
            fader.blocksRaycasts = true;
            fader.DOFade(1.0f, fadeDuration).OnComplete(() => { 
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            });
        }
        
        private IEnumerator LoadNewSceneAsync(string sceneName)
        {
            fader.interactable = true;
            fader.blocksRaycasts = true;
            yield return fader.DOFade(1.0f, fadeDuration).WaitForCompletion();
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (asyncLoad is { isDone: false })
            {
                yield return null;
            }
            
            yield return fader.DOFade(0.0f, fadeDuration).WaitForCompletion();
            fader.interactable = false;
            fader.blocksRaycasts = false;
        }
    }
}
