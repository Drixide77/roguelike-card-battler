using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    [CreateAssetMenu(fileName = "CardCollection", menuName = "Game Elements/Card Collection SO", order = 1)]
    public class CardCollectionSO : ScriptableObject
    {
        [Header("Cards")]
        public List<CardSO> Cards;
        public List<CardSO> StartingDeck;
    }
}