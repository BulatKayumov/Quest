using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace _Quest
{
    public class ExitDoor : Door
    {
        public QuestItem questItemPrefab;

        protected override void Start()
        {
            base.Start();
        }

        public void GenerateQuestItem()
        {
            QuestItem questItem = Instantiate(questItemPrefab, GameStateData.instance.backpackRoot.transform);
            questItem.transform.position = GameStateData.instance.backpackRoot.transform.position;
            questItem.roomIndex = GetComponentInParent<Room>().index + 1;
            activateItem = questItem;
            GameManager.instance.RequiredQuestItems.Add(questItem);
        }

        protected override void OpenClose()
        {
            base.OpenClose();
            StartCoroutine(Final());
        }

        private IEnumerator Final()
        {
            Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = true;
            yield return new WaitForSecondsRealtime(2);
            SceneManager.LoadScene(3);
        }
    }
}