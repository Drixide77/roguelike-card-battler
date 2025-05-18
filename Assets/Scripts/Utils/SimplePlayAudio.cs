using UnityEngine;

namespace MindlessRaptorGames
{
    public class SimplePlayAudio : MonoBehaviour
    {
        public enum AudioType
        {
            BGM, SFX
        }
        
        [Header("Settings")]
        [SerializeField] private AudioRepositoryEntryId clipToPlay;
        [SerializeField] private AudioType auidoType;
        [SerializeField] private bool loop = false;

        private void Start()
        {
            switch (auidoType)
            {
                case AudioType.BGM:
                    AudioService.Instance.PlayMusicClip(clipToPlay, loop);
                    break;
                case AudioType.SFX:
                    AudioService.Instance.PlaySFXClip(clipToPlay, loop);
                    break;
                default:
                    break;
            }
        }
    }
}