using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleSQL;

public class QuestionPanel : MonoBehaviour
{

    public GameObject originalButton;    
    public GameObject containerRect;
	public SimpleSQL.SimpleSQLManager dbManager;

    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;
	//private DBConnector localDB;
	private string langauge;

    // Use this for initialization
    void Start()
    {
		// TODO TEMP
		Debug.Log("starting QuestionPanel...");
        //Dictionary<string, string> FAQs = GetFAQs();
		langauge = PlayerPrefs.GetString("language", "English");
		//localDB = new DBConnector();
		List<QuestionAnswerPair> FAQs = SelectQuestionAnswerPairs();
        int listSize = FAQs.Count;
        

        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();
        
        faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
        faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, ((faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize)));

        // magic number, not sure why +4 works, I think I'm missing something in the y range...

        faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        
		foreach (QuestionAnswerPair pair in FAQs) {
			GameObject newButton = Instantiate(originalButton);
			FAQButton FAQ = newButton.GetComponent<FAQButton>();
			FAQ.faqPair = pair;
			if (langauge == "Espanol"){
				newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question_es;
			} else {
				newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.faqPair.question;
			}
			newButton.transform.SetParent(faqRect.transform);
		}
    }

	public List<QuestionAnswerPair> SelectQuestionAnswerPairs()
	{
		
		string sql = "select `qID`, `question`, `question_es`, Answer.aiD, `answer`, `answer_es` from Question inner join Answer on Question.aID = Answer.aiD AND Question.qID != -1 LIMIT 50";
		List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair>(sql);

		return pair_list;
        
	}
}