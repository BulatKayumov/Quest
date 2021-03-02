using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class Item : Interactable
    {
        public Room room;
        public ItemPlace itemPlace;
        public Item clone;

        protected override void Start()
        {
            base.Start();
        }
        protected override void Activate()
        {
            UIManager.instance.ShowMessage("I suppose I won't really need it");
        }
    }
}