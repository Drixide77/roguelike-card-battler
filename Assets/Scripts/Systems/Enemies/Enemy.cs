using UnityEngine;

namespace MindlessRaptorGames
{
    public class Enemy
    {
        public string Name;
        public int Health;
        public int Damage;

        public void PerformEnemyAction(GameFlowController flowController)
        {
            // TODO - Have proper enemy action logic
            Debug.Log(Name + "(" + Health + ") dealt " + Damage + " damage.");
            flowController.PlayerController.ModifyHealth(-Damage);
        }
    }
}