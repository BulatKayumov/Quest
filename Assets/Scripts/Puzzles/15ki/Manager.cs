using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Cube[] cube;
    public GameObject winPanel;
    public bool isWin;

    public void win()
    {
        for (int i = 0; i < cube.Length; i++)
        {
            if (cube[i].number != cube[i].numberCell)
                return;
        }

        winPanel.SetActive(true);
        isWin = true;
    }

    public void Update()
    {
        if(isWin)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(0);
        }
    }
}
