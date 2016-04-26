using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgesText : MonoBehaviour {

	private Text badgesText;

	// Use this for initialization
	void Start () {
		badgesText = GetComponent<Text>();
		badgesText.text = "Badges: " + PlayerPrefs.GetInt("badgesCount");
	}

	// Update is called once per frame
	void Update () {
		badgesText.text = "Badges: " + PlayerPrefs.GetInt("badgesCount");
	}
}
