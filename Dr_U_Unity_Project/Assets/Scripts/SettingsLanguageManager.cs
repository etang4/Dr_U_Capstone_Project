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

    public void setLanguage()
    {
        int dropdownLanguageIndex = this.gameObject.GetComponent<Dropdown>().value;
        switch (dropdownLanguageIndex)
        {
            case 0:
                setEnglish();
                break;
            case 1:
                setSpanish();
                break;
            default:
                setEnglish();
                break;
        }
        Debug.Log("Language changed to " + PlayerPrefs.GetString("language"));
        questionPanel.GetComponent<QuestionPanel>().loadFAQs(-99);  //-99 will set to previous number
        //TODO: Need to change all UI text here!
    }

    public void setEnglish()
    {
        if (PlayerPrefs.GetString("language") != "English")
        {
            PlayerPrefs.SetString("language", "English");
        }
    }

    public void setSpanish()
    {
        if (PlayerPrefs.GetString("language") != "Espanol")
        {
            PlayerPrefs.SetString("language", "Espanol");
        }
    }
}
