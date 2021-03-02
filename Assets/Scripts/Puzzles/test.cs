using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    void onMouseDown()
    {
        //System.Diagnostics.Debug.Print("clicked on mouse");
        UnityEngine.Debug.Log("123");
    }


    //Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("123");
        //onMouseDown();
        //System.Diagnostics.Debug.Print("123");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UnityEngine.Debug.Log("555");
            SceneManager.LoadScene("SampleScene");
        }
        //onMouseDown();

    }
}
