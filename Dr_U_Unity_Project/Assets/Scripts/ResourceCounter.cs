using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour 
{
	public Text counter;
	public static int count;
	
	// Use this for initialization
	void Start() {
		count = PlayerPrefs.GetInt("count", 0);
		counter = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		counter.text = count.ToString () + " EXP";
		PlayerPrefs.SetInt("count", count);
		PlayerPrefs.Save ();
	}

	void OnApplicationQuit() {
		PlayerPrefs.DeleteKey("count");
	}


	public static void addPoints(int points) {
		count += points;
	}
}
