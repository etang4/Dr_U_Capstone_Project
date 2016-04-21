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
	private int resultCount;

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

    public void filterList(string input)
    {
        
		if (searchResults == null) {
			searchResults = SqliteFTSSearchNoFilter(input);
			listSize = searchResults.Count;
			resultCount = searchResults.Count;
		} else {
			searchResults.Clear();
			searchResults = SqliteFTSSearchNoFilter(input);
			listSize = searchResults.Count;
			resultCount = searchResults.Count;
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
                stumpedItem.question_es = "You stumped Dr. Discovery!";
                stumpedItem.answer_es = "You get 10 points!";
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
                itemsList.Add(newButton);
                FAQButton FAQ = newButton.GetComponent<FAQButton>();
                QuestionAnswerPair newQAPair = new QuestionAnswerPair();

                if (language == "Espanol")
                {
                    newQAPair.question = pair.question_es;
                    newQAPair.answer = pair.answer_es;
                }
                else
                {
                    newQAPair.question = pair.question;
                    newQAPair.answer = pair.answer;
                }
                FAQ.faqPair = newQAPair;
                newButton.transform.GetChild(0).GetComponent<Text>().text = newQAPair.question;
                newButton.transform.SetParent(this.transform);
                numFound++;
            }
		}


		ResourceCounter.addQuestionsAsked(1,stumped ? 0 : numFound);	// If stumped send 0, else send number found (this is necessary because the stumped message will alter the numFound variable)
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
}
