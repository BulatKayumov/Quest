using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using _Quest;

public class MyButton : Interactable
{
    private bool interactable;
    private Image myImage;
    public Counter myCounter;
    public MyButton clone;

    protected override void Start()
    {
        base.Start();
        interactable = true;
        myImage = GetComponent<Image>();
    }

    protected override void Activate()
    {
        if (interactable)
        {
            myCounter.AddStock();
            myImage.color = Color.red;
            interactable = false;
            clone.myCounter.AddStock();
            clone.myImage.color = Color.red;
            clone.interactable = false;
        }
    }
}
