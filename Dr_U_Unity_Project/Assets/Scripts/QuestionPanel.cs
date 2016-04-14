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

    private int listSize;
    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;
	//private DBConnector localDB;
	private string language;
    private bool _isInstantiated;

    // Use this for initialization
    void Start()
    {
		// TODO TEMP
		Debug.Log("starting QuestionPanel...");
        _isInstantiated = false;
        loadFAQs();
        _isInstantiated = true;
    }

	public void loadFAQs()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (PlayerPrefs.GetString("language") == null)
        {
            language = PlayerPrefs.GetString("language", "English");
        }
        else
        {
            language = PlayerPrefs.GetString("language");
        }
        //localDB = new DBConnector();
        List<QuestionAnswerPair> FAQs = SelectQuestionAnswerPairs();
        listSize = FAQs.Count;

        if (!_isInstantiated)
        {
            faqGrid = containerRect.GetComponent<GridLayoutGroup>();
            faqRect = containerRect.GetComponent<RectTransform>();
            faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
            faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 4) - faqGrid.spacing.y * 4);
            // +4 does not work for large data sets, this needs to be reconfigured

            faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        }

        foreach (QuestionAnswerPair pair in FAQs)
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
    }

	public List<QuestionAnswerPair> SelectQuestionAnswerPairs()
	{
		
		string sql = "select `qID`, `question`, `question_es`, Answer.aiD, `answer`, `answer_es` from Question inner join Answer on Question.aID = Answer.aiD AND Question.qID != -1 LIMIT 50";
		List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair>(sql);
		
		return pair_list;
	}
}