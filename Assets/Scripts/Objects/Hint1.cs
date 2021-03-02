using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class Hint1 : Hint
    {
        public QuestItem questItemPrefab;

        public override void Create(DoorPlace doorPlace, int randomIndex)
        {
            base.Create(doorPlace, randomIndex);
            QuestItem questItem = Instantiate(questItemPrefab, GameStateData.instance.backpackRoot.transform);
            questItem.transform.position = GameStateData.instance.backpackRoot.transform.position;
            questItem.roomIndex = GetComponentInParent<Room>().index;
            GameManager.instance.RequiredQuestItems.Add(questItem);
        }
    }
}