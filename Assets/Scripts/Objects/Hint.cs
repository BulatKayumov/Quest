using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class Hint : MonoBehaviour
    {
        public string code;

        public virtual void Create(DoorPlace doorPlace, int randomIndex)
        {
            Vector2 cords = doorPlace.GetComponentInParent<Room>().Coordinates + doorPlace.cords + (Vector2)doorPlace.OrientationCords() * 0.484f;
            transform.position = new Vector3(cords.x - Generator.instance.maxMapSize / 2, 0, cords.y - Generator.instance.maxMapSize / 2) * Generator.instance.tileSize;
            transform.position += new Vector3(0, 14, 0);
            doorPlace.isActive = false;
            switch (doorPlace.orientation)
            {
                case Orientation.North:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Orientation.East:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Orientation.South:
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Orientation.West:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            Hint hintClone = Instantiate(GameStateData.instance.Hints[randomIndex], doorPlace.GetComponentInParent<Room>().clone.transform);
            hintClone.transform.rotation = transform.rotation;
            hintClone.transform.position = transform.position + new Vector3(0, -50, 0);
            GameStateData.instance.Hints.RemoveAt(randomIndex);
        }
    }
}