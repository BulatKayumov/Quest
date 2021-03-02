using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSoundController : MonoBehaviour
{
    public AudioClip FootstepsSound;
    public AudioClip JumpSound;
    private AudioSource audioSource;
    private CharacterController cController;
    private bool wasGrounded = false;
    float timeLeft = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cController = GetComponent<CharacterController>();
    }


    void FixedUpdate()

    {
        if (cController.isGrounded) //персонаж на земле

        {
            //    //if (cController.velocity.sqrMagnitude > 0f) 
            //    if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
            //    {
            //        Debug.Log("Walk");
            //        //audioSource.clip = FootstepsSound;
            //        //audioSource.Play();
            //        audioSource.PlayOneShot(FootstepsSound);

            //    }
            //    else
            //        Debug.Log("Idle");
            if (Input.GetButton("Jump"))
            {
                Debug.Log("Jump");
                audioSource.PlayOneShot(JumpSound);
            }
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
                {
                    audioSource.PlayOneShot(FootstepsSound);
                    audioSource.Play();
                    timeLeft = 0.5f;
                }
            }
            
        }
            



    }

}

