using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
    This class handles the search results and dynamically generates
    result buttons.
*/
public class QuestionPanelExpandedFilter : MonoBehaviour
{
    public GameObject originalButton;
    public List<GameObject> itemsList;
    private int listSize;
    public GameObject containerRect;
    public InputField SearchBarText;
    private bool _isInstantiated;
    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;
	public ResourceCounter resourceCounter;
	private string language;
	public SimpleSQL.SimpleSQLManager dbManager;
	private List<QuestionAnswerPair> searchResults;
    public GameObject FAQPanel;
    public GameObject FAQPanelExpanded;
    public GameObject ImagePanel;


    // Use this for initialization 
    void Start()
    {
        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();
		SearchBarText.onEndEdit.AddListener(filterList);
		if (PlayerPrefs.GetString("language") == null)
		{
			language = PlayerPrefs.GetString("language", "English");
		}
		else
		{
			language = PlayerPrefs.GetString("language");
		}
        _isInstantiated = false;
        
        filterList(SearchBarText.text);         //Initial run
        _isInstantiated = true;
    }

    void Update()
    {
        // TODO:  Fix issue where if user has settings, badges or unlocks panel open and hit back, it will close the search as well
        if (Input.GetKeyDown(KeyCode.Escape))   
        {
            FAQPanel.SetActive(true);
            FAQPanelExpanded.SetActive(false);
            ImagePanel.SetActive(true);
            SearchBarText.text = "";
        }
    }

    public void filterList(string input)
    {
		if (searchResults == null) {
			searchResults = SqliteFTSSearchNoFilter(input);
			listSize = searchResults.Count;
		} else {
			searchResults.Clear();
			searchResults = SqliteFTSSearchNoFilter(input);
			listSize = searchResults.Count;
		}
		foreach(GameObject item in itemsList){
			Destroy(item);
		}
        bool stumped = false;

        if (!_isInstantiated)
        {
            faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
            faqRect.offsetMax = new Vector2(faqRect.offsetMax.x, 0);
            // +4 does not work for large data sets, this needs to be reconfigured
        }
        //Adjusting area where FAQs are stored depending on number of FAQs.
        faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 3));
        // Set position of scroll to very top
        containerRect.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1;
		
		if (searchResults.Count == 0){
            stumped = true;
            QuestionAnswerPair stumpedItem = new QuestionAnswerPair();
            if (language == "Espanol"){
                stumpedItem.question_es = "¡Usted perplejó a Dr. Discovery!";
                stumpedItem.answer_es = "¡Usted recibe 10 puntos!";
            }else{
                stumpedItem.question = "You stumped Dr. Discovery!";
                stumpedItem.answer = "You get 10 points!";
            }
            searchResults.Add(stumpedItem);
        }
        int numFound = 0;
		foreach (QuestionAnswerPair pair in searchResults)
		{
            if (pair.question != "" || pair.question_es != "")      // Prune out empty returns
            {
                GameObject newButton = Instantiate(originalButton);
                FAQButton FAQ = newButton.GetComponent<FAQButton>();
                FAQ.faqPair = pair;

                //Determine which language to display
                if (language == "Espanol")
                {
                    newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question_es;
                }
                else
                {
                    newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question;
                }
                newButton.transform.SetParent(faqRect.transform);
                itemsList.Add(newButton);
                numFound++;
            }
		}

		addQuestionsAsked(1,stumped ? 0 : numFound);	// If stumped send 0, else send number found (this is necessary because the stumped message will alter the numFound variable)
        _isInstantiated = true;
		
	}

	public List<QuestionAnswerPair> SelectQuestionAnswerPairs()
	{
		
		string sql = "select `qID`, `question`, `question_es`, Answer.aiD, `answer`, `answer_es` from Question inner join Answer on Question.aID = Answer.aiD AND Question.qID != -1 LIMIT 50";
		List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair>(sql);
		
		return pair_list;
	}


	public List<QuestionAnswerPair> SqliteFTSSearchNoFilter(string input) 
	{
		string sql_search_string_part_1;
		string sql_search_string_part_2;
		string sql_search_string_part_3;

		//Remove any semi colons from the input string
		input.Replace(";", string.Empty);

		sql_search_string_part_1 = "select docid, question, question_es, Answer.aID, answer, answer_es from Question_search inner join Answer on Question_search.aID = Answer.aID AND Question_search.docid != -1 where Question_search match";
		if (language == "English") {
			sql_search_string_part_2 = "'question:" + input + "'";
		} else {
			sql_search_string_part_2 = "'question_es:" + input + "'";
		}
		sql_search_string_part_3 = "limit 50 offset 0";

		string sql_search = sql_search_string_part_1 + sql_search_string_part_2 + sql_search_string_part_3;
		List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair> (sql_search);

		return pair_list;

	}
	
	// Scoring Code
	public void addQuestionsAsked (int amount, int questionsFound)
	{
		int questionsAsked = PlayerPrefs.GetInt("questionsAsked");
		int questionsStumped = PlayerPrefs.GetInt("questionsStumped");
		int experience = PlayerPrefs.GetInt ("experience");
		int upgradesPurchased = PlayerPrefs.GetInt ("upgradesPurchased");
		int experienceAdded = 5 * upgradesPurchased;

		if (questionsFound == 0) {			// If questionsFound = 0, then the search term returned no results, and thus Dr. U was stumped.		
			questionsStumped += 1;	
			experienceAdded = 10 * upgradesPurchased;
		}

		questionsAsked += amount;
		experience += experienceAdded;
		ResourceCounter.scoreAlert (experienceAdded.ToString());  // Uncomment this to make it play a score alert when you click an FAQ

		PlayerPrefs.SetInt("questionsAsked", questionsAsked);
		PlayerPrefs.SetInt("questionsStumped", questionsStumped);
		PlayerPrefs.SetInt("experience", experience);
		PlayerPrefs.Save();
	}
}
