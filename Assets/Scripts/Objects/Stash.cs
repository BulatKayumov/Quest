using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class Stash : Interactable
    {
        public QuestItemPlace questItemPlace;
        public QuestItem activateItem;

        protected override void Activate()
        {
            if (GameManager.instance.ActiveQuestItem == activateItem)
            {
                //TODO animation
                //GetComponent<Animator>().SetTrigger("activate");
                questItemPlace.questItem.interactable = true;
            }
            else
            {
                Debug.Log("you can't");
                //TODO message
            }
        }
    }
}