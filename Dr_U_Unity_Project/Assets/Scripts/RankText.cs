using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RankText : MonoBehaviour {
	
	private Text rankingText;
	
	private string[] ranks = {"Cadet", "Ensign", "Lieutenant Junior Grade", "Lieutenant",
		"Lieutenant Commander", "Commander", "Captain", "Rear Admiral One Star", 
		"Rear Admiral Two Star", "Vice Admiral", "Admiral", "Fleet Admiral"};
	
	// Use this for initialization
	void Start () {
		rankingText = GetComponent<Text>();
		rankingText.text = "Rank: " + ranks [PlayerPrefs.GetInt("rank")];
	}
	
	// Update is called once per frame
	void Update () {	
		Console.WriteLine (PlayerPrefs.GetInt ("rank"));	
		rankingText.text = "Rank: " + ranks [PlayerPrefs.GetInt("rank")];
	}
}
