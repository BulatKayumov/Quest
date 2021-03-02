using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _Quest
{
    public enum DoorType
    {
        Open, ClosedByKey, ClosedByCode
    }
    public class Door : Interactable
    {
        [HideInInspector]
        public Vector2 cords;
        [HideInInspector]
        public Orientation orientation;
        [HideInInspector]
        public Door clone;
        public DoorType doorType;
        public QuestItem activateItem;
        public Room entryRoom;

        public AudioClip openDoor;
        public AudioClip openKey;
        public AudioClip openCode;
        private AudioSource audioSource;

        protected override void Start()
        {
            base.Start();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Activate()
        {
            if (this.doorType == DoorType.Open)
            {
                OpenClose();
            }
            else
            {
                if (this.doorType == DoorType.ClosedByKey)
                {
                    if (this.activateItem == GameManager.instance.ActiveQuestItem)
                    {
                        Inventory.instance.Remove(activateItem);
                        GameManager.instance.ActiveQuestItem = null;
                        StartCoroutine(OpenByKey());
                    }
                    else
                    {
                        Debug.Log("Closed");
                        UIManager.instance.ShowMessage("This door is closed");
                    }
                }
                else
                {
                    Debug.Log("Closed");
                    UIManager.instance.ShowMessage("This door is closed");
                }
            }
        }

        protected IEnumerator OpenByKey()
        {
            audioSource.PlayOneShot(openKey);
            yield return new WaitForSecondsRealtime(2);
            this.doorType = DoorType.Open;
            OpenClose();
        }

        public IEnumerator OpenByCode()
        {
            audioSource.PlayOneShot(openCode);
            yield return new WaitForSecondsRealtime(2);
            this.doorType = DoorType.Open;
        }

        protected virtual void OpenClose()
        {
            GetComponent<Animator>().SetTrigger("activate");
            clone.GetComponent<Animator>().SetTrigger("activate");
            audioSource.PlayOneShot(openDoor);
        }

        public void SetPosition(Vector2 cords, Orientation orientation, bool isClone)
        {
            this.orientation = orientation;
            this.cords = cords + OrientationCords(this.orientation);
            gameObject.transform.position = new Vector3(this.cords.x - Generator.instance.maxMapSize / 2, 0, this.cords.y - Generator.instance.maxMapSize / 2) * Generator.instance.tileSize;
            if (isClone)
            {
                gameObject.transform.position += new Vector3(0, -50, 0);
            }
            switch (orientation)
            {
                case Orientation.North:
                    gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Orientation.East:
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Orientation.South:
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Orientation.West:
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
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

        public void CreateDomofon()
        {
            Domofon domofon = Instantiate(GameStateData.instance.domofonPrefab, GetComponentInParent<Room>().transform);
            domofon.Create(this);
        }
    }
}