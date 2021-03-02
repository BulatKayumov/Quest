using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Quest
{
    public class InventorySlot : Slot
    {
        public UnityEngine.UI.Button activateButton;

        public override void AddItem(QuestItem newQuestItem)
        {
            base.AddItem(newQuestItem);
            activateButton.interactable = true;
        }

        public override void ClearSlot()
        {
            base.ClearSlot();
            activateButton.interactable = false;
        }

        public void ActivateQuestItem()
        {
            if (questItem != null)
            {
                GameManager.instance.ActiveQuestItem = questItem;
            }
        }
    }
}