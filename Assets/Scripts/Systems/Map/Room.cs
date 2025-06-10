using System;

namespace MindlessRaptorGames
{
    [Serializable]
    public enum RoomType
    {
        Starting, Encounter, EliteEncounter, BossEncounter, Shop, Rest, Treasure, Event
    }
    
    [Serializable]
    public class Room
    {
        public RoomType RoomType;

        public Room(RoomType roomType)
        {
            RoomType = roomType;
        }
    }
}