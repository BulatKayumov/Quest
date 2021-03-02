//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("load 1");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Debug.Log("exit");
        Application.Quit();
    }
}
