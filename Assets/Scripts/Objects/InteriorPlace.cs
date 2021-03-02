using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class InteriorPlace : MonoBehaviour
    {
        public Interior interior;
        [HideInInspector]
        public Door clone;
        [HideInInspector]
        public DoorPlace doorPlace;

        public void CreateInterior(Interior interior)
        {
            Room room = GetComponentInParent<Room>();
            Interior newInterior = Instantiate(interior, room.transform);
            Vector2 cords = room.Coordinates + doorPlace.cords + OrientationCords(doorPlace.orientation);
            newInterior.transform.position = new Vector3(cords.x - Generator.instance.maxMapSize / 2, 0, cords.y - Generator.instance.maxMapSize / 2) * Generator.instance.tileSize;
            switch (doorPlace.orientation)
            {
                case Orientation.North:
                    newInterior.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Orientation.East:
                    newInterior.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Orientation.South:
                    newInterior.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Orientation.West:
                    newInterior.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            Interior newInteriorClone = Instantiate(interior, room.clone.transform);
            newInteriorClone.transform.rotation = newInterior.transform.rotation;
            newInteriorClone.transform.position = newInterior.transform.position + new Vector3(0, -50, 0);
            newInterior.clone = newInteriorClone;
            newInteriorClone.clone = newInterior;
            this.interior = newInterior;
            newInterior.room = room;
            foreach(ItemPlace itemPlace in newInterior.itemPlaces)
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