using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionPanelExpandedFilter : MonoBehaviour
{
    public GameObject originalButton;
    public GameObject[] itemsList;
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
		Debug.Log (gameObject.name);
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
		/*string result = input.ToLower();
        Dictionary<string, string> searchResults = fakeSearch(result, 1);          //Update this to use real data

        int x = 0;
        foreach(GameObject item in itemsList){
            Destroy(item);
        }*/
        

        //TODO: Cannot test if this works until search is implemented
        if (!_isInstantiated)
        {
            faqGrid = containerRect.GetComponent<GridLayoutGroup>();
            faqRect = containerRect.GetComponent<RectTransform>();
            faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
            faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 4) - faqGrid.spacing.y * 4);
            // +4 does not work for large data sets, this needs to be reconfigured

            faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        }

		foreach (QuestionAnswerPair pair in searchResults)
		{
			GameObject newButton = Instantiate(originalButton);
			FAQButton FAQ = newButton.GetComponent<FAQButton>();
			FAQ.faqPair = pair;
			if (language == "Espanol")
			{
				newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question_es;
			}
			else
			{
				newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question;
			}
			newButton.transform.SetParent(faqRect.transform);
		}

		BadgePanel.addQuestionAsked(1);
		
		if (resultCount == 0) {
			ResourceCounter.addPoints(10);
			BadgePanel.addStumped(1);
		} else if (resultCount > 0)  {
			ResourceCounter.addPoints(5);
		}

        /*foreach (KeyValuePair<string, string> answer in searchResults)
        {
            GameObject newButton = Instantiate(originalButton);
            itemsList[x] = newButton;
            FAQButton FAQ = newButton.GetComponent<FAQButton>();
            FAQ.question = answer.Key;
            FAQ.answer = answer.Value;
            newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.question;

            newButton.transform.parent = this.transform;
            x++;
        }

		BadgePanel.addQuestionAsked(1);
		
		if (x == 0) {
			ResourceCounter.addPoints(10);
			BadgePanel.addStumped(1);
		} else if (x > 0)  {
			ResourceCounter.addPoints(5);
		}*/

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

    private Dictionary<string, string> fakeSearch(string input, int id)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("test", "Works");
        dict.Add("fail", "Fails");
        Dictionary<string, string> results = new Dictionary<string, string>();
        foreach (string word in input.Split())
        {
            if (dict.ContainsKey(word.ToLower()))
            {
                results.Add(word, dict[word]);
            }
        }
        return results;
    }

}

