using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Quest
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

        public int slotsCount = 25;

        public List<QuestItem> questItems = new List<QuestItem>();

        public bool Add(QuestItem questItem)
        {
            if (questItems.Count >= slotsCount)
            {
                Debug.Log("Not enough space");
                return false;
            }
            questItems.Add(questItem);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
            return true;
        }

        public void Remove(QuestItem questItem)
        {
            questItems.Remove(questItem);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }
}