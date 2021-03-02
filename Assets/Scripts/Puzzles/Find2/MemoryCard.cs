using UnityEngine;
using System.Collections;
using _Quest;

namespace _Quest.Find2
{
    public class MemoryCard : Interactable
    {
        [SerializeField] private GameObject cardBack;
        [SerializeField] private SceneController controller;
        public MemoryCard clone;

        private int _id;
        public int id
        {
            get
            {
                return _id;
            }
        }

        public SceneController Controller { set => controller = value; }

        public void SetCard(int id, Sprite image)
        {
            _id = id;
            GetComponent<SpriteRenderer>().sprite = image;
        }

        protected override void Activate()
        {
            if (cardBack.activeSelf && controller.canReveal)
            {
                cardBack.SetActive(false);
                clone.cardBack.SetActive(false);
                controller.CardRevealed(this);
            }
        }
        //public void OnMouseDown()
        //{
        //    if (cardBack.activeSelf && controller.canReveal)
        //    {
        //        cardBack.SetActive(false);
        //        controller.CardRevealed(this);
        //    }
        //}

        public void Unreveal()
        {
            cardBack.SetActive(true);
            clone.cardBack.SetActive(true);
        }
    }
}