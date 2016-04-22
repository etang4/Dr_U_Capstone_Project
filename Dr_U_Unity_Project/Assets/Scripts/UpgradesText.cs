using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradesText : MonoBehaviour {

	private static Text upgradesText;

	// Use this for initialization
	void Start () {
		upgradesText = GetComponent<Text>();
		upgradesText.text = "Upgrades Available: " + PlayerPrefs.GetInt("upgradePoints");
	}
	
	// Update is called once per frame
	void Update () {
		upgradesText.text = "Upgrades Available: " + PlayerPrefs.GetInt("upgradePoints");
	}
}
