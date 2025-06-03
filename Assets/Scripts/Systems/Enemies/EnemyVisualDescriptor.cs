using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MindlessRaptorGames
{
    public class EnemyVisualDescriptor : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image Image;
        public Image HealthFillImage;
        public Image HighlightImage;
        public TMP_Text NameLabel;
        public TMP_Text HealthLabel;
        public TMP_Text NextActionDisplay;
        public string EnemyId = "";
    }
}