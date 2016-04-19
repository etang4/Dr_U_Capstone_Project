using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
    This class handles the language settings for the GUI.
*/
public class SettingsLanguageManager : MonoBehaviour {

    //These are the list of UI gameobjects in the app.
    private GameObject _badgesButton;
    private GameObject _badgesPanel;
    private GameObject _faqTitle;
    private GameObject _faqPanelExpanded;
    private GameObject _moreInfoBadgesPanel;
    private GameObject _moreInfoImagePanel;
    private GameObject _moreInfoUpgradePanel;
    private GameObject _moreInfoUnlocksPanel;
    private GameObject _questionPanel;
    private GameObject _rankingText;
    private GameObject _searchBar;
    private GameObject _searchButton;
    private GameObject _settingsButton;
    private GameObject _settingsPanel;
    private GameObject _statsButton;
    private GameObject _statsPanel;
    private GameObject _upgradesButton;
    private GameObject _upgradesPanel;
	
    //Makes sure this is ran before all Start()
	void Awake () {
        //Default language (English) is set here.
        PlayerPrefs.SetString("language", "English");
    }

    // Use this for initialization
    void Start()
    {
        try {
            _badgesButton = GameObject.Find("BadgesButton");
            _badgesPanel = GameObject.Find("BadgesPanel");
            _faqTitle = GameObject.Find("FAQTitle");
            _faqPanelExpanded = GameObject.Find("FAQPanelExpanded");
            _moreInfoBadgesPanel = GameObject.Find("MoreInfoBadgesPanel");
            _moreInfoImagePanel = GameObject.Find("MoreInfoImagePanel");
            _moreInfoUpgradePanel = GameObject.Find("MoreInfoUpgradePanel");
            _moreInfoUnlocksPanel = GameObject.Find("MoreInfoUnlocksPanel");
            _questionPanel = GameObject.Find("QuestionPanel");
            _rankingText = GameObject.Find("RankingText");
            _searchBar = GameObject.Find("SearchBar");
            _searchButton = GameObject.Find("SearchButton");
            _settingsButton = GameObject.Find("SettingsButton");
            _settingsPanel = GameObject.Find("SettingsPanel");
            _statsButton = GameObject.Find("StatsButton");
            _statsPanel = GameObject.Find("StatsPanel");
            _upgradesButton = GameObject.Find("UpgradesButton");
            _upgradesPanel = GameObject.Find("UpgradesPanel");
        }
        catch
        {
            Debug.Log("Setting Language Manager Error. Prefab misnamed.");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Changes UI to English
    public void setEnglish()
    {
        if (PlayerPrefs.GetString("language") != "English")
        {
            PlayerPrefs.SetString("language", "English");
        }
        _questionPanel.GetComponent<QuestionPanel>().loadFAQs();

        //TODO: Need to change all UI text here!
        _badgesButton.transform.GetChild(0).GetComponent<Text>().text = "Badges";
        //_badgesPanel = GameObject.Find("BadgesPanel");
        _faqTitle.GetComponent<Text>().text = "Frequently Asked Questions";
        //_faqPanelExpanded.transform.FindChild("BackPanel").GetChild(0).GetChild(0).GetComponent<Text>().text = "Back";
        //_moreInfoBadgesPanel = GameObject.Find("MoreInfoBadgesPanel");
        //_moreInfoImagePanel = GameObject.Find("MoreInfoImagePanel");
        //_moreInfoUpgradePanel = GameObject.Find("MoreInfoUpgradePanel");
        //_moreInfoUnlocksPanel = GameObject.Find("MoreInfoUnlocksPanel");
        _rankingText.GetComponent<Text>().text = "Rank 1";
        _searchBar.transform.FindChild("Placeholder").GetComponent<Text>().text = "Have a Question?";
        _searchButton.transform.GetChild(0).GetComponent<Text>().text = "Search";
        _settingsButton.transform.GetChild(0).GetComponent<Text>().text = "Settings";
        _settingsPanel.transform.GetChild(0).FindChild("LanguageText").GetComponent<Text>().text = "Language";
        _settingsPanel.transform.GetChild(0).FindChild("SyncText").GetComponent<Text>().text = "Sync Database";
        _settingsPanel.transform.GetChild(0).FindChild("SyncButton").GetChild(0).GetComponent<Text>().text = "Sync";
        _settingsPanel.transform.GetChild(0).FindChild("EnglishButton").GetChild(0).GetComponent<Text>().text = "English";
        _settingsPanel.transform.GetChild(0).FindChild("SpanishButton").GetChild(0).GetComponent<Text>().text = "Spanish";
        _statsButton.transform.GetChild(0).GetComponent<Text>().text = "Stats";
        //_statsPanel.transform.FindChild("Text").GetComponent<Text>().text = "Rank 1: Cadet";
        _upgradesButton.transform.GetChild(0).GetComponent<Text>().text = "Upgrades";
        //_upgradesPanel = GameObject.Find("UpgradesPanel");
    }

    //Change UI to Spanish
    public void setSpanish()
    {
        if (PlayerPrefs.GetString("language") != "Espanol")
        {
            PlayerPrefs.SetString("language", "Espanol");
        }
        _questionPanel.GetComponent<QuestionPanel>().loadFAQs();
        //TODO: Need to change all UI text here!
        _badgesButton.transform.GetChild(0).GetComponent<Text>().text = "Distintivos";
        //_badgesPanel = GameObject.Find("BadgesPanel");
        _faqTitle.GetComponent<Text>().text = "Preguntas Mas Comun";
        //_faqPanelExpanded.transform.FindChild("BackPanel").GetChild(0).GetChild(0).GetComponent<Text>().text = "Back";
        //_moreInfoBadgesPanel = GameObject.Find("MoreInfoBadgesPanel");
        //_moreInfoImagePanel = GameObject.Find("MoreInfoImagePanel");
        //_moreInfoUpgradePanel = GameObject.Find("MoreInfoUpgradePanel");
        //_moreInfoUnlocksPanel = GameObject.Find("MoreInfoUnlocksPanel");
        _rankingText.GetComponent<Text>().text = "Rango 1";
        _searchBar.transform.FindChild("Placeholder").GetComponent<Text>().text = "Usted tiene una pregunta?";
        _searchButton.transform.GetChild(0).GetComponent<Text>().text = "Busca";
        _settingsButton.transform.GetChild(0).GetComponent<Text>().text = "Ajustes";
        _settingsPanel.transform.GetChild(0).FindChild("LanguageText").GetComponent<Text>().text = "Lenguaje";
        _settingsPanel.transform.GetChild(0).FindChild("SyncText").GetComponent<Text>().text = "Sincroniza la Base de Datos";
        _settingsPanel.transform.GetChild(0).FindChild("SyncButton").GetChild(0).GetComponent<Text>().text = "Sincroniza";
        _settingsPanel.transform.GetChild(0).FindChild("EnglishButton").GetChild(0).GetComponent<Text>().text = "Inglés";
        _settingsPanel.transform.GetChild(0).FindChild("SpanishButton").GetChild(0).GetComponent<Text>().text = "Español";
        _statsButton.transform.GetChild(0).GetComponent<Text>().text = "Estadísticas";
        //_statsPanel = GameObject.Find("StatsPanel");
        _upgradesButton.transform.GetChild(0).GetComponent<Text>().text = "Ascensos";
        //_upgradesPanel = GameObject.Find("UpgradesPanel");
    }
}
