using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsLanguageManager : MonoBehaviour {

    private GameObject questionPanel;
	
    //Makes sure this is ran before all Start()
	void Awake () {
        PlayerPrefs.SetString("language", "English");
    }

    // Use this for initialization
    void Start()
    {
        questionPanel = GameObject.Find("QuestionPanel");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setEnglish()
    {
        if (PlayerPrefs.GetString("language") != "English")
        {
            PlayerPrefs.SetString("language", "English");
        }
        questionPanel.GetComponent<QuestionPanel>().loadFAQs();
        //TODO: Need to change all UI text here!
    }

    public void setSpanish()
    {
        if (PlayerPrefs.GetString("language") != "Espanol")
        {
            PlayerPrefs.SetString("language", "Espanol");
        }
        questionPanel.GetComponent<QuestionPanel>().loadFAQs();
        //TODO: Need to change all UI text here!
    }
}
