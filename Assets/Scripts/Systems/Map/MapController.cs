using System.Collections.Generic;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class MapController : GameplayModule
    {
        [Header("Settings")]
        [SerializeField] [Range(1, 99)] private int numberOfFloors = 1;
        [SerializeField] [Range(4, 99)] private int floorLength = 5;
        [SerializeField] private int restHealAmount = 35;
        
        private List<List<Room>> rooms;
        private Vector2Int currentRoomPosition;
        private int currentFloor;
        private bool newRun = true;
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            currentFloor = 0;
            currentRoomPosition = Vector2Int.zero;
            rooms = new List<List<Room>>();
        }

        public void EnterMapMode()
        {
            // TODO - Implement UI interaction

            int newDepth = currentRoomPosition.x + 1;
            if (newDepth == floorLength)
            {
                Debug.Log("# You win!");
                flowController.OnPlayerDeath();
                return;
            }
            currentRoomPosition = new Vector2Int(newDepth, Random.Range(0, rooms[newDepth].Count));
            PerformRoomLogic(currentRoomPosition);
        }
        
        public void OnRunStart()
        {
            currentFloor = 0;
            currentRoomPosition = Vector2Int.zero;
            GenerateFloor(0);
            //EnterMapMode();
            PerformRoomLogic(currentRoomPosition);
        }

        private void GenerateFloor(int floorLevel)
        {
            rooms.Clear();
            // Starting room
            rooms.Add(new List<Room>());
            rooms[0].Add(new Room(RoomType.Starting));
            // First room (always an encounter)
            rooms.Add(new List<Room>());
            rooms[1].Add(new Room(RoomType.Encounter));
            
            // The rest
            for (int i = 2; i < floorLength - 2; i++)
            {
                rooms.Add(new List<Room>());
                rooms[i].Add(new Room(RoomType.Rest));
                rooms[i].Add(new Room(RoomType.Encounter));
                rooms[i].Add(new Room(RoomType.Encounter));
            }
            
            // Pre-boss rooms
            rooms.Add(new List<Room>());
            rooms[floorLength - 2].Add(new Room(RoomType.Rest));
            // Boos room
            rooms.Add(new List<Room>());
            rooms[floorLength - 1].Add(new Room(RoomType.BossEncounter));
        }

        private void PerformRoomLogic(Vector2Int roomPosition)
        {
            switch (rooms[roomPosition.x][roomPosition.y].RoomType)
            {
                case RoomType.Starting:
                {
                    Debug.Log("New Room: Starting");
                    EnterMapMode();
                    break;
                }
                case RoomType.Encounter:
                {
                    Debug.Log("New Room: Encounter");
                    flowController.StartEncounter();
                    break;
                }
                case RoomType.Rest:
                {
                    Debug.Log("New Room: Rest");
                    flowController.PlayerController.ModifyHealth(restHealAmount);
                    EnterMapMode();
                    break;
                }
                case RoomType.BossEncounter:
                {
                    Debug.Log("New Room: Boss");
                    flowController.StartEncounter();
                    break;
                }
                default:
                    break;
            }
        }
    }
}