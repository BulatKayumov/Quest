using UnityEngine;
using System.Collections;

namespace _Quest
{
    public enum Orientation
    {
        North, East, South, West
    }
    public class DoorPlace : MonoBehaviour
    {
        public bool forDoor = true;
        [HideInInspector]
        public bool isActive = true;
        public Vector2Int cords;
        [HideInInspector]
        public DoorPlace neighbour;
        public Orientation orientation;

        void Start()
        {
            isActive = true;
        }

        public void Rotate(int sizeY)
        {
            if(orientation == Orientation.West)
            {
                orientation = Orientation.North;
            }
            else
            {
                orientation++;
            }

            int tmp = cords.y;
            cords.y = -cords.x + sizeY - 1;
            cords.x = tmp;
        }

        public Vector2Int OrientationCords()
        {
            Vector2Int orientationCords = new Vector2Int();
            switch (orientation)
            {
                case Orientation.North:
                    orientationCords = Vector2Int.up;
                    break;
                case Orientation.East:
                    orientationCords = Vector2Int.right;
                    break;
                case Orientation.South:
                    orientationCords = Vector2Int.down;
                    break;
                case Orientation.West:
                    orientationCords = Vector2Int.left;
                    break;
            }
            return orientationCords;
        }

        public void CreateDoorway()
        {
            isActive = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}