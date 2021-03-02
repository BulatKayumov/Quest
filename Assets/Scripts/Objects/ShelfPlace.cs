using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class ShelfPlace : MonoBehaviour
    {
        public Interior interior;
        [HideInInspector]
        public Door clone;
        [HideInInspector]
        public DoorPlace doorPlace;

        public void CreateShelf(Interior interior)
        {
            Room room = GetComponentInParent<Room>();
            Interior newShelf = Instantiate(interior, room.transform);
            Vector2 cords = room.Coordinates + doorPlace.cords + OrientationCords(doorPlace.orientation);
            newShelf.transform.position = new Vector3(cords.x - Generator.instance.maxMapSize / 2, 0, cords.y - Generator.instance.maxMapSize / 2) * Generator.instance.tileSize;
            newShelf.transform.position += new Vector3((OrientationCords(doorPlace.orientation) * -0.4f).x, 13, (OrientationCords(doorPlace.orientation) * -0.4f).y);
            switch (doorPlace.orientation)
            {
                case Orientation.North:
                    newShelf.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Orientation.East:
                    newShelf.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Orientation.South:
                    newShelf.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Orientation.West:
                    newShelf.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            Interior shelfClone = Instantiate(interior, room.clone.transform);
            shelfClone.transform.rotation = newShelf.transform.rotation;
            shelfClone.transform.position = newShelf.transform.position + new Vector3(0, -50, 0);
            this.interior = newShelf;
            newShelf.room = room;
            foreach (ItemPlace itemPlace in newShelf.itemPlaces)
            {
                itemPlace.room = room;
                room.itemPlaces.Add(itemPlace);
            }
        }

        private Vector2 OrientationCords(Orientation orientation)
        {
            Vector2 orientationCords = new Vector2();
            switch (orientation)
            {
                case Orientation.North:
                    orientationCords = Vector2.up / 2f;
                    break;
                case Orientation.East:
                    orientationCords = Vector2.right / 2f;
                    break;
                case Orientation.South:
                    orientationCords = Vector2.down / 2f;
                    break;
                case Orientation.West:
                    orientationCords = Vector2.left / 2f;
                    break;
            }
            return orientationCords;
        }
    }
}