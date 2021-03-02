using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Quest
{
    public class Domofon : MonoBehaviour
    {
        [SerializeField]
        GameObject textObject;
        Text myText;
        string myString = "";
        public string password;
        public bool flag = false;
        public Domofon clone;
        Door door;
        bool solved;
        float timer;

        private AudioSource audioSource;
        public AudioClip pipClip;

        void Start()
        {
            solved = false;
            myText = textObject.GetComponent<Text>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
        }

        public void EnterNumber(string number)
        {
            if(timer > 0.1)
            {
                timer = 0;
                if (!solved)
                {
                    audioSource.PlayOneShot(pipClip);
                    if (number.Equals("C"))
                    {
                        if (myString.Length > 0)
                        {
                            myString = myString.Remove(myString.Length - 1, 1);
                            myText.text = myString;
                            clone.myText.text = myString;
                        }
                        return;
                    }
                    if (number.Equals("X"))
                    {
                        myString = "";
                        myText.text = myString;
                        clone.myText.text = myString;
                        return;
                    }
                    Debug.Log("myString = " + myString + ", number = " + number);
                    myString += number;
                    Debug.Log("myString = " + myString);
                    myText.text = myString;
                    clone.myText.text = myString;

                    if (myString.Length == password.Length)
                    {

                        if (myString.Equals(password))
                        {
                            solved = true;
                            StartCoroutine(door.OpenByCode());
                        }
                        else
                        {
                            myString = "";
                            myText.text = myString;
                            clone.myText.text = myString;
                        }
                    }
                }
            }
        }

        public void Create(Door door)
        {
            this.door = door;
            transform.position = door.transform.position;
            transform.position += OrientationVector(door.orientation);
            switch (door.orientation)
            {
                case Orientation.North:
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Orientation.East:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Orientation.South:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Orientation.West:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
            }
            Domofon cloneDomofon = Instantiate(GameStateData.instance.domofonPrefab, door.entryRoom.clone.transform);
            cloneDomofon.transform.position = transform.position + new Vector3(0, -50, 0);
            cloneDomofon.transform.rotation = transform.rotation;
            clone = cloneDomofon;
            cloneDomofon.clone = this;

            List<DoorPlace> activeDoorPlaces = new List<DoorPlace>();
            foreach (DoorPlace doorPlace in door.entryRoom.doorPlaces)
            {
                if (doorPlace.isActive)
                {
                    activeDoorPlaces.Add(doorPlace);
                }
            }
            DoorPlace selectedDoorPlace = activeDoorPlaces[Random.Range(0, activeDoorPlaces.Count)];
            int randomIndex = Random.Range(0, GameStateData.instance.Hints.Count);
            Hint hint = Instantiate(GameStateData.instance.Hints[randomIndex], door.entryRoom.transform);
            password = hint.code;
            hint.Create(selectedDoorPlace, randomIndex);
        }


        private Vector3 OrientationVector(Orientation orientation)
        {
            Vector3 orientationVector = new Vector3();
            switch (orientation)
            {
                case Orientation.North:
                    orientationVector = new Vector3(-7.25f, 14, 0.75f);
                    break;
                case Orientation.East:
                    orientationVector = new Vector3(0.75f, 14, 7.25f);
                    break;
                case Orientation.South:
                    orientationVector = new Vector3(7.25f, 14, -0.75f);
                    break;
                case Orientation.West:
                    orientationVector = new Vector3(-0.75f, 14, -7.25f);
                    break;
            }
            return orientationVector;
        }
    }
}