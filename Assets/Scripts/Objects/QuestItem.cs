using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class QuestItem : Item
    {
        public string questItemName;
        public int roomIndex;
        public Sprite sprite;
        UIManager ui;
        public bool interactable = true;

        protected override void Start()
        {
            base.Start();
            ui = UIManager.instance;
        }

        public override void Interact()
        {
            if (interactable)
            {
                base.Interact();
            }
        }

        protected override void Activate()
        {
            ui.Notify(this);
            PickUp();
        }


        void PickUp()
        {
            bool pickedUp = Inventory.instance.Add(this);

            if (pickedUp)
            {
                this.transform.parent = GameStateData.instance.backpackRoot.transform;
                this.clone.transform.parent = GameStateData.instance.backpackRoot.transform;
                this.transform.position = GameStateData.instance.backpackRoot.transform.position;
                this.clone.transform.position = GameStateData.instance.backpackRoot.transform.position;
            }
        }
    }
}