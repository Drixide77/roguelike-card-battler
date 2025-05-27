using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Game Elements/Enemy", order = 1)]
    public class EnemySO : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public int Health;
        public int Damage;

        public Enemy ToEnemy()
        {
            return new Enemy()
            {
                Name = Name,
                Sprite = Sprite,
                MaxHealth = Health,
                Damage = Damage
            };
        }
    }
}