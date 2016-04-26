using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradesText : MonoBehaviour {

	private Text upgradesText;

	// Use this for initialization
	void Start () {
		upgradesText = GetComponent<Text>();
		upgradesText.text = "Upgrades: " + PlayerPrefs.GetInt("upgradePoints");
	}
	
	// Update is called once per frame
	void Update () {
		upgradesText.text = "Upgrades: " + PlayerPrefs.GetInt("upgradePoints");
	}
}
