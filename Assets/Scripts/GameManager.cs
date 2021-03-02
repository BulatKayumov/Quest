using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace _Quest
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public List<RoomPrefab> RoomPrefabs;
        public List<Room> Rooms;
        public List<Item> Items;
        public List<Door> ClosedByKeyDoors;
        public List<QuestItem> RequiredQuestItems;
        public List<QuestItemPlace> questItemPlaces;
        public Room[,] SpawnedRooms;
        GameStateData data;
        public int closedByKeyRoomsCount;
        public int closedByCodeRoomsCount;
        bool generated = false;

        private QuestItem activeQuestItem;
        public QuestItem ActiveQuestItem {
            get {
                return activeQuestItem;
            }
            set {
                if(activeQuestItem == value)
                {
                    activeQuestItem = null;
                    UIManager.instance.SetActiveQuestItem(null);
                }
                else
                {
                    activeQuestItem = value;
                    UIManager.instance.SetActiveQuestItem(activeQuestItem);
                }
            }
        }

        IEnumerator Start()
        {
            data = GameStateData.instance;
            RoomPrefabs = new List<RoomPrefab>();
            for(int i = 0; i < data.RoomPrefabs.Length; i++)
            {
                RoomPrefabs.Add(new RoomPrefab { room = data.RoomPrefabs[i], count = data.RoomPrefabs[i].Count, index = i });
            }
            Rooms = new List<Room>();
            Items = new List<Item>();
            questItemPlaces = new List<QuestItemPlace>();
            Cursor.lockState = CursorLockMode.Locked;
            closedByCodeRoomsCount = data.closedDoorsCount / 2;
            closedByKeyRoomsCount = data.closedDoorsCount - closedByCodeRoomsCount;
            yield return new WaitForSeconds(1);
            Generator.instance.Generate();
        }

        void Update()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                InventoryUI.instance.ChangeActive();
                if(Cursor.lockState == CursorLockMode.Locked)
                {
                    //Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    //Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                PlayerController.instance.canMove = !PlayerController.instance.canMove;
            }
        }
    }
}