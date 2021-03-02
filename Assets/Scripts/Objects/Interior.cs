using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _Quest
{
    public enum InteriorType
    {
        WallFurniture, FloorFurniture
    }
    public class Interior : Interactable
    {
        public Room room;
        public List<ItemPlace> itemPlaces = new List<ItemPlace>();
        [Range(0, 100)]
        public int rating = 50;
        public InteriorType interiorType;
        public Interior clone;

        public AudioClip clip;

        protected override void Activate()
        {
            if(TryGetComponent<Animator>(out Animator animator))
            {
                animator.SetTrigger("activate");
                clone.GetComponent<Animator>().SetTrigger("activate");
                if(TryGetComponent<AudioSource>(out AudioSource audioSource))
                {
                    audioSource.PlayOneShot(clip);
                }
            }
        }
    }
}