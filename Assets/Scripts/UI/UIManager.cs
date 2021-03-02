using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace _Quest
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        public static UIManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion
        public GameObject notification;
        public Slot notificationSlot;
        public Text itemName;
        public Slot ActiveQuestItem;
        public GameObject message;
        public Text messageText;
        private float notificationTimer;
        private float messageTimer;
        void Start()
        {
            notification.SetActive(false);
            message.SetActive(false);
            notificationTimer = 0;
            messageTimer = 0;
        }

        private void Update()
        {
            notificationTimer -= Time.deltaTime;
            messageTimer -= Time.deltaTime;
            if(notificationTimer < 0 && notification.activeSelf == true)
            {
                notification.SetActive(false);
            }
            if(messageTimer < 0 && message.activeSelf == true)
            {
                message.SetActive(false);
            }
        }

        public void Notify(QuestItem questItem)
        {
            notificationSlot.AddItem(questItem);
            itemName.text = questItem.questItemName;
            notification.SetActive(true);
            notificationTimer = 5;
        }

        public void ShowMessage(string text)
        {
            messageText.text = text;
            message.SetActive(true);
            messageTimer = 7;
        }

        public void SetActiveQuestItem(QuestItem questItem)
        {
            if(questItem == null)
            {
                ActiveQuestItem.ClearSlot();
            }
            else
            {
                ActiveQuestItem.AddItem(questItem);
            }
        }
    }
}