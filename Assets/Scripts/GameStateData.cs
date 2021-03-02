using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _Quest
{
    public struct RoomPrefab
    {
        public Room room;
        public int count;
        public int index;
    }

    public class GameStateData : MonoBehaviour
    {
        #region Singleton

        public static GameStateData instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public GameObject roomsRoot;
        public GameObject cloneRoomsRoot;
        public GameObject itemsRoot;
        public GameObject cloneItemsRoot;
        public GameObject Player;
        public GameObject backpackRoot;
        public int closedDoorsCount;
        public Door doorPrefab;
        public Domofon domofonPrefab;

        public Room[] RoomPrefabs;
        public Item[] Items;
        public List<Hint> Hints;
        public Interior[] interiors;
        public List<QuestItem> keys;
    }
}