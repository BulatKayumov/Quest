using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Quest
{
    public class Generator : MonoBehaviour
    {
        #region Singleton

        public static Generator instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        [Range (1, 100)]
        public int roomsCount = 10;
        [Range (10, 100)]
        public int maxMapSize = 20;
        public int tileSize = 30;

        public Room StartRoom;
        public Room ExitRoom;

        GameManager manager;
        GameStateData data;

        void Start()
        {
            manager = GameManager.instance;
            data = GameStateData.instance;
            //Generate();
        }

        public void Generate()
        {
            foreach(Room room in manager.Rooms)
            {
                Destroy(room.clone.gameObject);
                Destroy(room.gameObject);
            }
            if (StartRoom.clone != null)
            {
                Destroy(StartRoom.clone.gameObject);
            }
            foreach(Item thing in manager.Items)
            {
                Destroy(thing.clone.gameObject);
                Destroy(thing.gameObject);
            }
            manager.SpawnedRooms = new Room[maxMapSize, maxMapSize];
            manager.SpawnedRooms[maxMapSize / 2, maxMapSize / 2] = StartRoom;
            manager.SpawnedRooms[maxMapSize / 2 + 1, maxMapSize / 2] = StartRoom;
            StartRoom.Coordinates = new Vector2Int(maxMapSize / 2, maxMapSize / 2);
            int limit = 500;
            while(!AllRequiredRoomsSpawned() && limit-- > 0)
            {
                PlaceRoom();
            }
            manager.RoomPrefabs.Add(new RoomPrefab { room = ExitRoom, count = 1, index = manager.RoomPrefabs.Count });

            limit = 500;
            while (!AllRequiredRoomsSpawned() && limit-- > 0)
            {
                PlaceRoom();
            }

            CloneBunker();
            CloneObjects();
            AddDoors();
            AddInterior();
            AddQuestItems();
            AddItems();
        }

        private void PlaceRoom()
        {
            HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
            for(int x = 0; x < manager.SpawnedRooms.GetLength(0); x++)
            {
                for(int y = 0; y < manager.SpawnedRooms.GetLength(1); y++)
                {
                    if (manager.SpawnedRooms[x, y] == null) continue;
                    if (manager.SpawnedRooms[x, y].Coordinates != new Vector2Int(x, y)) continue;

                    int maxX = manager.SpawnedRooms.GetLength(0) - 1;
                    int maxY = manager.SpawnedRooms.GetLength(1) - 1;

                    foreach(DoorPlace doorPlace in manager.SpawnedRooms[x, y].doorPlaces)
                    {
                        if(x + doorPlace.cords.x + doorPlace.OrientationCords().x > 0 &&
                            x + doorPlace.cords.x + doorPlace.OrientationCords().x < maxX &&
                            y + doorPlace.cords.y + doorPlace.OrientationCords().y > 0 &&
                            y + doorPlace.cords.y + doorPlace.OrientationCords().y < maxY &&
                            manager.SpawnedRooms[x + doorPlace.cords.x + doorPlace.OrientationCords().x, y + doorPlace.cords.y + doorPlace.OrientationCords().y] == null)
                        {
                            vacantPlaces.Add(new Vector2Int(x + doorPlace.cords.x + doorPlace.OrientationCords().x, y + doorPlace.cords.y + doorPlace.OrientationCords().y));
                        }
                    }
                }
            }

            if(vacantPlaces.Count == 0)
            {
                Debug.LogWarning("Not enough places");
                return;
            }

            RoomPrefab roomPrefab;

            var backupVacantPlaces = new HashSet<Vector2Int>(vacantPlaces);
            int limit = 100;
            do
            {
                vacantPlaces = new HashSet<Vector2Int>(backupVacantPlaces);
                roomPrefab = GetRandomRoom(vacantPlaces.Count);
                foreach(Vector2Int vacantPlace in vacantPlaces.ToList<Vector2Int>())
                {
                    for (int x = Mathf.Max(0, vacantPlace.x - roomPrefab.room.radius); x < Mathf.Min(Generator.instance.maxMapSize - 1, vacantPlace.x + roomPrefab.room.radius); x++)
                    {
                        for (int y = Mathf.Max(0, vacantPlace.y - roomPrefab.room.radius); y < Mathf.Min(Generator.instance.maxMapSize - 1, vacantPlace.y + roomPrefab.room.radius); y++)
                        {
                            if (GameManager.instance.SpawnedRooms[x, y] != null &&
                                roomPrefab.room.roomName == GameManager.instance.SpawnedRooms[x, y].roomName)
                            {
                                vacantPlaces.Remove(vacantPlace);
                            }
                        }
                    }
                }

            } while (vacantPlaces.Count == 0 && limit-- > 0);

            if(vacantPlaces.Count == 0)
            {
                Debug.LogWarning("Not enough vacant places");
                return;
            }

            Room newRoom = Instantiate(roomPrefab.room, data.roomsRoot.transform);

            limit = 100;
            while (limit-- > 0)
            {
                Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
                newRoom.RotateRandomly();
                List<Vector2Int> availablePositions = new List<Vector2Int>();
                for(int x = position.x - newRoom.size.x + 1; x <= position.x; x++)
                {
                    for(int y = position.y - newRoom.size.y + 1; y <= position.y; y++)
                    {
                        if (x > 0 &&
                            x + newRoom.size.x < manager.SpawnedRooms.GetLength(0) &&
                            y > 0 &&
                            y + newRoom.size.y < manager.SpawnedRooms.GetLength(1))
                        {
                            bool flag = true;
                            for(int x1 = x; x1 < x + newRoom.size.x; x1++)
                            {
                                for (int y1 = y; y1 < y + newRoom.size.y; y1++)
                                {
                                    if (manager.SpawnedRooms[x1,y1] != null)
                                    {
                                        flag = false;
                                    }
                                }
                            }
                            if (flag)
                            {
                                availablePositions.Add(new Vector2Int(x, y));
                            }
                        }
                    }
                }

                if(availablePositions.Count == 0)
                {
                    Debug.LogWarning("Destroyed 2");
                    DestroyImmediate(newRoom.gameObject);
                    return;
                }

                position = availablePositions[Random.Range(0, availablePositions.Count)];
                newRoom.Coordinates = position;

                if (ConnectToSomething(newRoom, position))
                {
                    newRoom.transform.position = new Vector3(position.x + 0.5f * (newRoom.size.x - 1) - maxMapSize / 2, 0, position.y + 0.5f * (newRoom.size.y - 1) - maxMapSize / 2) * tileSize;
                    for(int x = 0; x < newRoom.size.x; x++)
                    {
                        for(int y = 0; y < newRoom.size.y; y++)
                        {
                            manager.SpawnedRooms[position.x + x, position.y + y] = newRoom;
                        }
                    }
                    if(roomPrefab.room.roomType == RoomType.Required)
                    {
                        roomPrefab.count--;
                        manager.RoomPrefabs[roomPrefab.index] = roomPrefab;
                    }
                    newRoom.index = manager.Rooms.Count;
                    newRoom.way.Add(newRoom.index);
                    manager.Rooms.Add(newRoom);
                    return;
                }
            }

            Debug.LogWarning("Destroyed 1");
            DestroyImmediate(newRoom.gameObject);

        }

        private bool ConnectToSomething(Room room, Vector2Int position)
        {
            int maxX = manager.SpawnedRooms.GetLength(0) - 1;
            int maxY = manager.SpawnedRooms.GetLength(1) - 1;

            List<DoorPlace> availableDoorPlaces = new List<DoorPlace>();

            foreach(DoorPlace doorPlace in room.doorPlaces)
            {
                if(doorPlace.forDoor &&
                    position.x + doorPlace.cords.x + doorPlace.OrientationCords().x > 0 &&
                    position.x + doorPlace.cords.x + doorPlace.OrientationCords().x < maxX &&
                    position.y + doorPlace.cords.y + doorPlace.OrientationCords().y > 0 &&
                    position.y + doorPlace.cords.y + doorPlace.OrientationCords().y < maxY &&
                    manager.SpawnedRooms[position.x + doorPlace.cords.x + doorPlace.OrientationCords().x, position.y + doorPlace.cords.y + doorPlace.OrientationCords().y] != null &&
                    manager.SpawnedRooms[position.x + doorPlace.cords.x + doorPlace.OrientationCords().x, position.y + doorPlace.cords.y + doorPlace.OrientationCords().y].HasDoorTo(room.Coordinates, doorPlace))
                {
                    availableDoorPlaces.Add(doorPlace);
                }
            }

            if (availableDoorPlaces.Count == 0)
            {
                return false;
            }

            DoorPlace selectedDoorPlace = availableDoorPlaces[Random.Range(0, availableDoorPlaces.Count)];
            selectedDoorPlace.CreateDoorway();
            selectedDoorPlace.neighbour.CreateDoorway();
            room.selectedDoorPlace = selectedDoorPlace;
            room.way = new List<int>(selectedDoorPlace.neighbour.transform.GetComponentInParent<Room>().way);
            return true;
        }

        public RoomPrefab GetRandomRoom(int vacantPlacesCount)
        {
            List<float> chances = new List<float>();
            for(int i = 0; i < manager.RoomPrefabs.Count; i++)
            {
                if(manager.RoomPrefabs[i].count > 0)
                {
                    chances.Add(manager.RoomPrefabs[i].room.GetRating(vacantPlacesCount));
                }
                else
                {
                    chances.Add(0);
                }
            }

            float value = Random.Range(0, chances.Sum());
            float sum = 0;

            for(int i = 0; i < chances.Count; i++)
            {
                sum += chances[i];
                if(value < sum)
                {
                    RoomPrefab roomPrefab = manager.RoomPrefabs[i];
                    //if(roomPrefab.room.roomType == RoomType.Required)
                    //{
                    //    roomPrefab.count--;
                    //}
                    //manager.RoomPrefabs[i] = roomPrefab;
                    //if(manager.RoomPrefabs[i].count < 1)
                    //{
                    //    manager.RoomPrefabs.RemoveAt(i);
                    //}
                    return roomPrefab;
                }
            }
            Debug.LogWarning("Rating fail O_0");
            return manager.RoomPrefabs[0];
        }

        public bool AllRequiredRoomsSpawned()
        {
            foreach(RoomPrefab roomPrefab in manager.RoomPrefabs)
            {
                if(roomPrefab.room.roomType == RoomType.Required && roomPrefab.count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void CloneBunker()
        {
            for(int i = 0; i < data.roomsRoot.transform.childCount; i++)
            {
                Transform cloneRoomTransform = Instantiate(data.roomsRoot.transform.GetChild(i), data.cloneRoomsRoot.transform);
                Room cloneRoom = cloneRoomTransform.GetComponent<Room>();
                data.roomsRoot.transform.GetChild(i).GetComponent<Room>().clone = cloneRoom;
                cloneRoom.clone = data.roomsRoot.transform.GetChild(i).GetComponent<Room>();
            }
        }

        private void CloneObjects()
        {
            foreach(Room room in manager.Rooms)
            {
                if(room.roomName == "ExitRoom")
                {
                    room.transform.GetComponentInChildren<ExitDoor>().GenerateQuestItem();
                    room.transform.GetComponentInChildren<ExitDoor>().clone = room.clone.transform.GetComponentInChildren<ExitDoor>();
                    room.clone.transform.GetComponentInChildren<ExitDoor>().clone = room.transform.GetComponentInChildren<ExitDoor>();
                }
                if(room.roomName == "Kitchen")
                {
                    Transform interiorRoot;
                    Transform interiorRootClone;
                    for(int i = 0; i < room.transform.childCount; i++)
                    {
                        if(room.transform.GetChild(i).tag == "interior")
                        {
                            interiorRoot = room.transform.GetChild(i);
                            interiorRootClone = room.clone.transform.GetChild(i);
                            for (int j = 0; j < interiorRoot.childCount; j++)
                            {
                                if(interiorRoot.GetChild(j).TryGetComponent<Interior>(out Interior interior))
                                {
                                    interior.clone = interiorRootClone.GetChild(j).GetComponent<Interior>();
                                    interiorRootClone.GetChild(j).GetComponent<Interior>().clone = interior;
                                    foreach (ItemPlace itemPlace in interior.itemPlaces)
                                    {
                                        itemPlace.room = room;
                                        room.itemPlaces.Add(itemPlace);
                                    }
                                }
                            }
                        }
                    }
                }
                if(room.roomName == "PuzzleRoom")
                {
                    room.GetComponentInChildren<Puzzle>().clone = room.clone.GetComponentInChildren<Puzzle>();
                    room.clone.GetComponentInChildren<Puzzle>().clone = room.GetComponentInChildren<Puzzle>();
                    room.GetComponentInChildren<Puzzle>().puzzleStash.clone = room.clone.GetComponentInChildren<Puzzle>().puzzleStash;
                    room.clone.GetComponentInChildren<Puzzle>().puzzleStash.clone = room.GetComponentInChildren<Puzzle>().puzzleStash;
                    room.GetComponentInChildren<Puzzle>().puzzleStash.questItemPlace.clone = room.clone.GetComponentInChildren<Puzzle>().puzzleStash.questItemPlace;
                    room.clone.GetComponentInChildren<Puzzle>().puzzleStash.questItemPlace.clone = room.GetComponentInChildren<Puzzle>().puzzleStash.questItemPlace;
                }
            }
        }

        private void AddDoors()
        {
            foreach(Room room in manager.Rooms)
            {
                room.AddDoor();
            }
        }

        private void AddInterior()
        {
            List<float> chances = new List<float>();
            List<float> WallFurnitureChances = new List<float>();
            List<float> FloorFurnitureChances = new List<float>();
            List<Interior> floorFurniture = new List<Interior>();
            List<Interior> wallFurniture = new List<Interior>();
            for (int i = 0; i < data.interiors.Count(); i++)
            {
                chances.Add(data.interiors[i].rating);
                if(data.interiors[i].interiorType == InteriorType.FloorFurniture)
                {
                    FloorFurnitureChances.Add(data.interiors[i].rating);
                    floorFurniture.Add(data.interiors[i]);
                }
                else
                {
                    WallFurnitureChances.Add(data.interiors[i].rating);
                    wallFurniture.Add(data.interiors[i]);
                }
            }
            foreach (Room room in manager.Rooms)
            {
                foreach(DoorPlace doorPlace in room.doorPlaces)
                {
                    if (doorPlace.isActive)
                    {
                        float value = Random.Range(0, chances.Sum());
                        if(value < FloorFurnitureChances.Sum())
                        {
                            InteriorPlace interiorPlace = doorPlace.gameObject.AddComponent<InteriorPlace>();
                            interiorPlace.doorPlace = doorPlace;
                            room.interiorPlaces.Add(interiorPlace);
                        }
                        else
                        {
                            ShelfPlace shelfplace = doorPlace.gameObject.AddComponent<ShelfPlace>();
                            shelfplace.doorPlace = doorPlace;
                            room.shelfPlaces.Add(shelfplace);
                        }
                    }
                }
                foreach (InteriorPlace interiorPlace in room.interiorPlaces)
                {
                    float value = Random.Range(0, FloorFurnitureChances.Sum());
                    float sum = 0;
                    for (int i = 0; i < FloorFurnitureChances.Count(); i++)
                    {
                        sum += FloorFurnitureChances[i];
                        if (value < sum)
                        {
                            interiorPlace.CreateInterior(floorFurniture[i]);
                            i = FloorFurnitureChances.Count();
                        }
                    }
                }
                foreach (ShelfPlace shelfPlace in room.shelfPlaces)
                {
                    float value = Random.Range(0, WallFurnitureChances.Sum());
                    float sum = 0;
                    for (int i = 0; i < WallFurnitureChances.Count(); i++)
                    {
                        sum += WallFurnitureChances[i];
                        if (value < sum)
                        {
                            shelfPlace.CreateShelf(wallFurniture[i]);
                            i = WallFurnitureChances.Count();
                        }
                    }
                }
            }
        }

        private void AddQuestItems()
        {
            foreach(Room room in manager.Rooms)
            {
                if(room.roomName == "PuzzleRoom")
                {
                    room.GetComponentInChildren<Puzzle>().GenerateQuestItem();
                    manager.questItemPlaces.Add(room.GetComponentInChildren<Puzzle>().puzzleStash.questItemPlace);
                }
            }
            int limit = 500;
            while(manager.questItemPlaces.Count > 0 && limit-- > 0)
            {
                int maxIndex = 0;
                QuestItem maxQuestItem = null;
                foreach(QuestItem questItem in manager.RequiredQuestItems)
                {
                    if(questItem.roomIndex > maxIndex)
                    {
                        maxIndex = questItem.roomIndex;
                        maxQuestItem = questItem;
                    }
                }
                maxIndex = 0;
                QuestItemPlace maxQuestItemPlace = null;
                foreach (QuestItemPlace questItemPlace in manager.questItemPlaces)
                {
                    if (questItemPlace.room.index > maxIndex)
                    {
                        maxIndex = questItemPlace.room.index;
                        maxQuestItemPlace = questItemPlace;
                    }
                }
                manager.RequiredQuestItems.Remove(maxQuestItem);
                manager.questItemPlaces.Remove(maxQuestItemPlace);
                maxQuestItemPlace.questItem = maxQuestItem;
                maxQuestItem.transform.parent = maxQuestItemPlace.transform;
                maxQuestItem.transform.localPosition = new Vector3(0, 0, 0);
                maxQuestItem.transform.rotation = Quaternion.Euler(0, Random.Range(-179, 180), 0);
                maxQuestItem.interactable = false;
                QuestItem cloneQuestItem = Instantiate(maxQuestItem, maxQuestItemPlace.clone.transform);
                cloneQuestItem.transform.localPosition = new Vector3(0, 0, 0);
                cloneQuestItem.transform.rotation = maxQuestItem.transform.rotation;
                maxQuestItem.clone = cloneQuestItem;
            }
            foreach(QuestItem questItem in manager.RequiredQuestItems)
            {
                List<ItemPlace> availableItemPlaces = new List<ItemPlace>();
                for(int i = 0; i < questItem.roomIndex; i++)
                {
                    foreach(ItemPlace itemPlace in manager.Rooms[i].itemPlaces)
                    {
                        availableItemPlaces.Add(itemPlace);
                    }
                }
                ItemPlace selectedItemPlace = availableItemPlaces[Random.Range(0, availableItemPlaces.Count)];
                selectedItemPlace.room.itemPlaces.Remove(selectedItemPlace);
                selectedItemPlace.item = questItem;
                questItem.transform.parent = selectedItemPlace.transform;
                questItem.transform.localPosition = new Vector3(0, 0, 0);
                questItem.transform.rotation = Quaternion.Euler(0, Random.Range(-179, 180), 0);
                questItem.interactable = true;
                QuestItem cloneQuestItem = Instantiate(questItem, selectedItemPlace.transform);
                cloneQuestItem.transform.localPosition = new Vector3(0, -50, 0);
                cloneQuestItem.transform.rotation = questItem.transform.rotation;
                questItem.clone = cloneQuestItem;
            }
        }

        private void AddItems()
        {
            List<ItemPlace> emptyItemPlaces = new List<ItemPlace>();
            foreach(Room room in manager.Rooms)
            {
                foreach (ItemPlace itemPlace in room.itemPlaces)
                {
                    emptyItemPlaces.Add(itemPlace);
                }
            }

            int count = emptyItemPlaces.Count / 10;

            for(int i = 0; i < count; i++)
            {
                ItemPlace selectedItemPlace = emptyItemPlaces[Random.Range(0, emptyItemPlaces.Count)];
                Item item = Instantiate(data.Items[Random.Range(0, data.Items.Length)], selectedItemPlace.transform);
                item.transform.localPosition = new Vector3(0, 0, 0);
                item.itemPlace = selectedItemPlace;
                item.room = selectedItemPlace.room;
                emptyItemPlaces.Remove(selectedItemPlace);
                manager.Items.Add(item);
                Item cloneItem = Instantiate(item, selectedItemPlace.transform);
                cloneItem.transform.localPosition = new Vector3(0, -50, 0);
                item.clone = cloneItem;
                cloneItem.clone = item;
            }
        }
    }
}