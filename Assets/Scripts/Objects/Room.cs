using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _Quest
{
    public enum RoomType
    {
        Required, Simple
    }
    public class Room : MonoBehaviour
    {
        public string roomName;
        public int index;
        public List<int> way;
        public DoorPlace[] doorPlaces;
        public Door door;
        public List<ItemPlace> itemPlaces;
        public List<InteriorPlace> interiorPlaces;
        public List<ShelfPlace> shelfPlaces;
        public List<HintPlace> hintPlaces;
        public Vector2Int Coordinates /*{ get; set; }*/;
        public Vector2Int size;
        [Range(0, 100)]
        public int rating = 50;
        [Range(1, 30)]
        public int Count = 1;
        public int radius = 1;
        public RoomType roomType;
        public int vacantPlacesRequiredAmount = 1;
        public int doorsLimit;

        [HideInInspector]
        public DoorPlace selectedDoorPlace;
        public Room clone;

        public bool HasDoorTo(Vector2Int cords, DoorPlace otherDoorPlace)
        {
            int doorsCount = 0;
            foreach(DoorPlace doorPlace in doorPlaces)
            {
                if (!doorPlace.isActive)
                {
                    doorsCount++;
                }
            }
            if(doorsCount >= doorsLimit)
            {
                return false;
            }
            foreach (DoorPlace doorPlace in doorPlaces)
            {
                if (doorPlace.isActive &&
                    doorPlace.forDoor &&
                    Coordinates + doorPlace.cords + doorPlace.OrientationCords() == cords + otherDoorPlace.cords)
                {
                    otherDoorPlace.neighbour = doorPlace;
                    doorPlace.neighbour = otherDoorPlace;
                    return true;
                }
            }
            return false;
        }

        public void RotateRandomly()
        {
            int count = Random.Range(0, 4);

            for (int i = 0; i < count; i++)
            {
                int tmp = size.x;
                size.x = size.y;
                size.y = tmp;

                transform.Rotate(0, 90, 0);

                foreach(DoorPlace doorPlace in doorPlaces)
                {
                    doorPlace.Rotate(size.y);
                }
            }
        }

        public void AddDoor()
        {
            Door newDoor = Instantiate(GameStateData.instance.doorPrefab, gameObject.transform);
            newDoor.SetPosition(this.Coordinates + selectedDoorPlace.cords, selectedDoorPlace.orientation, false);
            Door newCloneDoor = Instantiate(GameStateData.instance.doorPrefab, clone.gameObject.transform);
            newCloneDoor.SetPosition(this.Coordinates + selectedDoorPlace.cords, selectedDoorPlace.orientation, true);
            newDoor.clone = newCloneDoor;
            newCloneDoor.clone = newDoor;
            this.door = newDoor;
            newDoor.entryRoom = selectedDoorPlace.neighbour.GetComponentInParent<Room>();
            if(this.roomName == "PuzzleRoom" || this.roomName == "ExitRoom")
            {
                if(GameManager.instance.closedByCodeRoomsCount == 0)
                {
                    newDoor.doorType = DoorType.ClosedByKey;
                    GameManager.instance.closedByKeyRoomsCount--;
                    GameManager.instance.ClosedByKeyDoors.Add(newDoor);
                    int randomIndex = Random.Range(0, GameStateData.instance.keys.Count);
                    QuestItem questItem = Instantiate(GameStateData.instance.keys[randomIndex], GameStateData.instance.backpackRoot.transform);
                    GameStateData.instance.keys.RemoveAt(randomIndex);
                    questItem.roomIndex = this.index;
                    questItem.transform.position = GameStateData.instance.backpackRoot.transform.position;
                    GameManager.instance.RequiredQuestItems.Add(questItem);
                    newDoor.activateItem = questItem;
                }
                else
                {
                    if(GameManager.instance.closedByKeyRoomsCount == 0)
                    {
                        newDoor.doorType = DoorType.ClosedByCode;
                        GameManager.instance.closedByCodeRoomsCount--;
                        newDoor.CreateDomofon();
                    }
                    else
                    {
                        int random = Random.Range(0, 1);
                        if(random == 0)
                        {
                            newDoor.doorType = DoorType.ClosedByKey;
                            GameManager.instance.closedByKeyRoomsCount--;
                            GameManager.instance.ClosedByKeyDoors.Add(newDoor);
                            int randomIndex = Random.Range(0, GameStateData.instance.keys.Count);
                            QuestItem questItem = Instantiate(GameStateData.instance.keys[randomIndex], GameStateData.instance.backpackRoot.transform);
                            GameStateData.instance.keys.RemoveAt(randomIndex);
                            questItem.roomIndex = this.index;
                            questItem.transform.position = GameStateData.instance.backpackRoot.transform.position;
                            GameManager.instance.RequiredQuestItems.Add(questItem);
                            newDoor.activateItem = questItem;
                        }
                        else
                        {
                            newDoor.doorType = DoorType.ClosedByCode;
                            GameManager.instance.closedByCodeRoomsCount--;
                            newDoor.CreateDomofon();
                        }
                    }
                }
            }
            else
            {
                newDoor.doorType = DoorType.Open;
            }
        }

        public int GetRating(int vacantPlacesCount)
        {
            if(vacantPlacesCount < vacantPlacesRequiredAmount)
            {
                return 0;
            }
            return rating;
        }
    }
}