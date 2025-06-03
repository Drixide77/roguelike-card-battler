using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [Serializable]
    public class EncounterData
    {
        public List<Enemy> Enemies;
    }
    
    [Serializable]
    public class EncounterSOData
    {
        public List<EnemySO> Enemies;

        public EncounterData ToEncounter()
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (var enemySO in Enemies)
            {
                enemies.Add(enemySO.ToEnemy());
            }
            return new EncounterData() { Enemies = enemies };
        }
    }
    
    [CreateAssetMenu(fileName = "EncounterCollection", menuName = "Game Elements/Encounter Collection SO", order = 1)]
    public class EncounterCollectionSO : ScriptableObject
    {
        [Header("Enemies")]
        public List<EncounterSOData> Encounters = new List<EncounterSOData>();
    }
}