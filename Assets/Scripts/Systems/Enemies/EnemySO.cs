using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Game Elements/Enemy", order = 1)]
    public class EnemySO : ScriptableObject
    {
        public string Name;
        public int Health;

        public Enemy ToEnemy()
        {
            return new Enemy()
            {
                Name = Name,
                Health = Health
            };
        }
    }
}