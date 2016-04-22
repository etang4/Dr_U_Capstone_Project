using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgesButton : MonoBehaviour {

	public static GameObject _badgesButton;
	public static GameObject _badgesPanel;
	public static Button badgesButton;
	public static BadgePanel badgesPanel;

	// Use this for initialization
	void Start () {
		_badgesButton = GameObject.Find("BadgesButton");
		_badgesPanel = GameObject.Find("BadgesPanel");
	}

	public static void onClick () {
		// This sets the Badge button to white after the user checks for a new badge
		badgesButton = _badgesButton.transform.GetComponent<Button>();
		var colors = badgesButton.colors;
		colors.normalColor = Color.white;
		_badgesButton.transform.GetComponent<Button>().colors = colors;
	}

	public static void setButtonColor () {
		// This sets the Badge button to glow cyan when a new badge is awarded to encourage the user to check
		badgesButton = _badgesButton.transform.GetComponent<Button>();
		var colors = badgesButton.colors;
		colors.normalColor = Color.cyan;
		_badgesButton.transform.GetComponent<Button>().colors = colors;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
