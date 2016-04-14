using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour 
{
	public Text counter;
	public static int count;
    private static GameObject alert;
    private static Text alertText;
    private static Animation addScore;
	
	// Use this for initialization
	void Start() {
		count = PlayerPrefs.GetInt("count");
		counter = GetComponent<Text>();

        alert = GameObject.Find("ScoreAlert");
        alertText = alert.GetComponentsInChildren<Text>()[0];

        addScore = alert.gameObject.GetComponent<Animation>();
        alert.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!addScore.isPlaying)
        {
            alert.SetActive(false);
            counter.text = count.ToString() + " EXP";
            PlayerPrefs.SetInt("count", count);
            PlayerPrefs.Save();
        }
	}

	void OnApplicationQuit() {
		PlayerPrefs.DeleteKey("count");
	}

	public static void addPoints(int points) {
        alert.SetActive(true);
        alertText.text = points.ToString();
        addScore.Play("AddScore");
        count += points;
	}
    
}