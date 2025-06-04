using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Game Elements/Enemy", order = 1)]
    public class EnemySO : ScriptableObject
    {
        [Header("Attributes")]
        public string Name;
        public Sprite Sprite;
        public int Health;
        [Header("Actions")]
        public List<EffectData> Actions;

        public Enemy ToEnemy()
        {
            return new Enemy()
            {
                Name = Name,
                Sprite = Sprite,
                MaxHealth = Health,
                Actions = Actions,
            };
        }
    }
}