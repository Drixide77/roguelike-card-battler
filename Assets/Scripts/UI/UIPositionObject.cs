using UnityEngine;

namespace MindlessRaptorGames
{
    public class UIObjectPositioner : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private RectTransform objectToPosition;
        [Header("Settings")]
        [SerializeField] private int widthDivider = 2;
        [SerializeField] private int heightDivider = 2;
        [SerializeField] private float widthMultiplier = 1f;
        [SerializeField] private float heightMultiplier = 1f;

        [SerializeField] private bool updatePosition = false;

        void Start()
        {
            SetUIObjectPosition();
        }

        void Update()
        {
            if (updatePosition)
            {
                SetUIObjectPosition();
            }
        }

        public void SetUIObjectPosition()
        {
            if (objectToPosition != null && widthDivider != 0 && heightDivider != 0)
            {
                // Calculate the anchor position
                float anchorX = widthMultiplier / widthDivider;
                float anchorY = heightMultiplier / heightDivider;

                // Set the anchor and pivot
                objectToPosition.anchorMin = new Vector2(anchorX, anchorY);
                objectToPosition.anchorMax = new Vector2(anchorX, anchorY);
                objectToPosition.pivot = new Vector2(0.5f, 0.5f);

                // Set the local position to zero to align with the anchor point
                objectToPosition.anchoredPosition = Vector2.zero;
            }
        }
    }
}
