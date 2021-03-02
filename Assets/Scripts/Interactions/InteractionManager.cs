using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Quest
{
    public class InteractionManager : MonoBehaviour
    {
        public Camera mainCamera;
        void FixedUpdate()
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.collider.GetComponent<Interactable>() != null)
                    {
                        raycastHit.collider.GetComponent<Interactable>().Interact();
                    }
                    else
                    {
                        if (raycastHit.collider.GetComponentInParent<Interactable>() != null)
                        {
                            raycastHit.collider.GetComponentInParent<Interactable>().Interact();
                        }
                    }
                }
            }
        }
    }
}