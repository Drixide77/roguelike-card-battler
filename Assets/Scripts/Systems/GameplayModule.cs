using UnityEngine;

namespace MindlessRaptorGames
{
    public abstract class GameplayModule : MonoBehaviour
    {
        protected GameFlowController flowController { get; private set; }

        public virtual void Initialize(GameFlowController gameFlowController)
        {
            flowController = gameFlowController;
        }
    }
}