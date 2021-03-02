using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace _Quest
{
    public class Puzzle : Interactable
    {
        public PuzzleStash puzzleStash;
        public QuestItem activateItem;
        public QuestItem questItemPrefab;
        public GameObject controller;
        public bool ready = false;
        bool solved = false;
        public Puzzle clone;

        protected override void Start()
        {
            base.Start();
            ready = false;
            solved = false;
        }

        public void Solve()
        {
            solved = true;
            puzzleStash.Open();
        }

        protected override void Activate()
        {
            if (ready)
            {
                if (solved)
                {
                    UIManager.instance.ShowMessage("I have already solved this puzzle");
                }
            }
            else
            {
                if(GameManager.instance.ActiveQuestItem == null)
                {
                    Debug.Log("Not ready");
                    UIManager.instance.ShowMessage("it seems that something is missing");
                }
                else
                {
                    if (GameManager.instance.ActiveQuestItem == activateItem)
                    {
                        Inventory.instance.Remove(activateItem);
                        GameManager.instance.ActiveQuestItem = null;
                        ready = true;
                        controller.SetActive(true);
                        StartCoroutine(SetActiveClone());
                    }
                    else
                    {
                        UIManager.instance.ShowMessage("This item doesn't fit");
                    }
                }
            }
        }

        private IEnumerator SetActiveClone()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            clone.controller.SetActive(true);
        }

        public void GenerateQuestItem()
        {
            QuestItem questItem = Instantiate(questItemPrefab, GameStateData.instance.backpackRoot.transform);
            questItem.roomIndex = GetComponentInParent<Room>().index;
            questItem.transform.position = GameStateData.instance.backpackRoot.transform.position;
            GameManager.instance.RequiredQuestItems.Add(questItem);
            activateItem = questItem;
        }
    }
}