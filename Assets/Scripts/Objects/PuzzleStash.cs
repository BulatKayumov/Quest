using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class PuzzleStash : Interactable
    {
        public QuestItemPlace questItemPlace;
        bool isOpened;
        public PuzzleStash clone;
        public AudioClip clip;

        protected override void Start()
        {
            base.Start();
            isOpened = false;
        }

        protected override void Activate()
        {
            if (!isOpened)
            {
                UIManager.instance.ShowMessage("Can't open this");
            }
        }

        public void Open()
        {
            GetComponent<Animator>().SetTrigger("activate");
            clone.GetComponent<Animator>().SetTrigger("activate");
            GetComponent<AudioSource>().PlayOneShot(clip);
            isOpened = true;
            questItemPlace.questItem.interactable = true;
        }
    }
}