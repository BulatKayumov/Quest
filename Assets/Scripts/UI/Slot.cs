using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Quest
{
    public class Slot : MonoBehaviour
    {
        public Image icon;

        public QuestItem questItem;

        public virtual void AddItem(QuestItem newQuestItem)
        {
            questItem = newQuestItem;
            icon.sprite = newQuestItem.sprite;
            icon.enabled = true;
            Debug.Log("Added questItem " + questItem.name);
        }

        public virtual void ClearSlot()
        {
            questItem = null;

            icon.sprite = null;
            icon.enabled = false;
        }
    }
}