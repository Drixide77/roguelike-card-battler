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

        private void OnDestroy()
        {
            flowController.GameplaySceneController.UIController.OnMapButtonPressed -= OnMapButtonPressed;
        }
        
        public override void Initialize(GameFlowController gameFlowController)
        {
            base.Initialize(gameFlowController);
            currentFloor = 0;
            currentRoomPosition = Vector2Int.zero;
            rooms = new List<List<Room>>();
            flowController.GameplaySceneController.UIController.OnMapButtonPressed += OnMapButtonPressed;
        }

        public void EnterMapMode()
        {
            if (currentRoomPosition.x + 1 == floorLength)
            {
                Debug.Log("# You win!");
                flowController.OnPlayerDeath();
                return;
            }
            
            SetMapButtons();
        }
        
        public void OnRunStart()
        {
            currentFloor = 0;
            currentRoomPosition = Vector2Int.zero;
            GenerateFloor(0);
            EnterMapMode();
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
                {
                    Debug.LogError("MapController -> PerformRoomLogic: Unimplemented RoomType handling.");
                    EnterMapMode();
                    break;
                }
            }
        }
        
        private void OnMapButtonPressed(int buttonIndex)
        {
            int newDepth = currentRoomPosition.x + 1;
            int index;
            switch (rooms[newDepth].Count)
            {
                case 1:
                {
                    index = 0;
                    break;
                }
                case 2:
                {
                    index = buttonIndex == 2 ? 1 : 0;
                    break;
                }
                case 3:
                {
                    index = buttonIndex;
                    break;
                }
                default:
                {
                    index = 0;
                    Debug.LogError("MapController -> OnMapButtonPressed: Unexpected number of rooms.");
                    break;
                }
            }
            currentRoomPosition = new Vector2Int(newDepth, index);
            flowController.GameplaySceneController.UIController.SetMapUIVisibility(false);
            PerformRoomLogic(currentRoomPosition);
        }

        private void SetMapButtons()
        {
            string leftButton = "", middleButton = "", rightButton = "";
            
            int newDepth = currentRoomPosition.x + 1;
            switch (rooms[newDepth].Count)
            {
                case 1:
                {
                    middleButton = rooms[newDepth][0].RoomType.ToString();
                    break;
                }
                case 2:
                {
                    leftButton = rooms[newDepth][0].RoomType.ToString();
                    rightButton = rooms[newDepth][1].RoomType.ToString();
                    break;
                }
                case 3:
                {
                    leftButton = rooms[newDepth][0].RoomType.ToString();
                    middleButton = rooms[newDepth][1].RoomType.ToString();
                    rightButton = rooms[newDepth][2].RoomType.ToString();
                    break;
                }
                default:
                    break;
            }
            
            flowController.GameplaySceneController.UIController.SetMapUIVisibility(true, leftButton, middleButton, rightButton);
        }
    }
}