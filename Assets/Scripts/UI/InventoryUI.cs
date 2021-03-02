using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Quest
{
    public class InventoryUI : MonoBehaviour
    {
        #region Singleton

        public static InventoryUI instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        Inventory inventory;
        public Transform itemsParent;
        public GameObject inventoryUI;
        InventorySlot[] slots;

        void Start()
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateUI;
            slots = itemsParent.GetComponentsInChildren<InventorySlot>();
            inventoryUI.SetActive(false);
        }

        public void ChangeActive()
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.questItems.Count)
                {
                    slots[i].AddItem(inventory.questItems[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }
    }
}