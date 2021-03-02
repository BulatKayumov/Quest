using UnityEngine;
using System.Collections;

namespace _Quest
{
    public class Interactable : MonoBehaviour
    {
        protected GameObject player;
        [SerializeField]
        float radius = 20f;
        public Transform interactionTransform;
        float distance;

        protected virtual void Start()
        {
            player = GameStateData.instance.Player;
            if (interactionTransform == null)
            {
                interactionTransform = transform;
            }
        }

            public virtual void Interact()
            {
                distance = Vector3.Distance(player.transform.position, interactionTransform.position);
                if (distance < radius)
                {
                    Debug.Log("Interact with " + gameObject.name);
                    Activate();
                }
            }

            protected virtual void Activate()
            {

            }

        public void OnDrawGizmosSelected()
        {
            if (interactionTransform == null)
            {
                interactionTransform = transform;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }
    }
}