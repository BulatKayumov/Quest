using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HighscoreText : MonoBehaviour {

	Text score;

	public void Wait(float seconds, System.Action action)
	{
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, System.Action callback)
	{
		yield return new WaitForSeconds(time);
		callback();

	}

	void OnEnable() {
		score = GetComponent<Text>();
		Debug.Log(score);
		score.text = "High Score: " +PlayerPrefs.GetInt("HighScore").ToString();
		var number = Convert.ToInt32(score);
		Debug.Log(number);
		if (number > 1)
        {
			Debug.Log("тут я жду 5 сек и перехожу в другую сцену");
			Wait(5, () => {
				Debug.Log("тут я жду 5 сек и перехожу в другую сцену");
				//SceneManager.LoadScene("ИМЯ ТВОЕЙ СЦЕНЫ"); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!111
			});
		}
	}
}
