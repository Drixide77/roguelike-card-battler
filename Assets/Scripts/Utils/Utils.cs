using UnityEngine;

namespace MindlessRaptorGames
{
    public static class Utils
    {
        public static void SetCanvasGroupVisible(CanvasGroup canvasGroup, bool visible)
        {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}