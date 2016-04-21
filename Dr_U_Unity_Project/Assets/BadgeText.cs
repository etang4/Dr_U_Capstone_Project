using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeText : MonoBehaviour {

	private static Text badgeText;

	private static string[] badges = {"Moon Badge: Earned for asking 1 question", 
		"", "Lieutenant Junior Grade", "Lieutenant",
		"Lieutenant Commander", "Commander", "Captain", "Rear Admiral One Star", 
		"Rear Admiral Two Star", "Vice Admiral", "Admiral", "Fleet Admiral"};

	// Use this for initialization
	void Start () {
		badgeText = GetComponent<Text>();
		badgeText.text = "Badge";	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
