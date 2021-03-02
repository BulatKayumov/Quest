using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using _Quest;

public class Counter : MonoBehaviour
{
    public Text myText;
    public int inStock;
    public int myNumber = 9;
    public GameObject myImage;
    public Puzzle puzzle;
    public MyButton[] buttons;


    void OnEnable()
    {
        myText.text = inStock + "/" + myNumber;
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].clone = puzzle.clone.controller.GetComponent<Counter>().buttons[i];
        }
    }

    public void AddStock()
    {
        inStock++;
        myText.text = inStock + "/" + myNumber;
        if (inStock == myNumber)
        {
            myImage.GetComponent<Image>().enabled = true;
            myImage.transform.GetChild(0).GetComponent<Text>().enabled = true;
            puzzle.Solve();
        }
    }
}
