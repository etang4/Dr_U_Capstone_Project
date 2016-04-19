using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleSQL;

/*
    QuestionPanel handles FAQ results, language and the dynamic sizing of 
    the FAQ panels.
*/
public class QuestionPanel : MonoBehaviour
{

    public GameObject originalFAQ; //"FAQ" Prefab.

    //GUI Area which the FAQs are stored    
    private GameObject containerRect; 
    //Database Manager from SimpleSQL Plugin.
	public SimpleSQL.SimpleSQLManager dbManager; 

    private int listSize; //Number of FAQ questions

    //Properties needed to dynamically generate FAQs
    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;
	
    //Determine what language to display
	private string language;
    //Makes sure the FAQs stay the same size between languages.
    private bool _isInstantiated;

    // Use this for initialization
    void Start()
    {
        containerRect = this.gameObject;
        Debug.Log("starting QuestionPanel...");
        _isInstantiated = false;
        loadFAQs();
        _isInstantiated = true;
    }

    //Dynmically generates FAQ to be displayed in QuestionPanel
    //Is called when you change language
	public void loadFAQs()
    {
        //Delete previous FAQs
        foreach(Transform child in this.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        
        if (PlayerPrefs.GetString("language") == null)
        {
            //If no language is chosen. Default to English.
            language = PlayerPrefs.GetString("language", "English");
        }
        else
        {
            language = PlayerPrefs.GetString("language");
        }
        
        //Call Database for FAQs
        List<QuestionAnswerPair> FAQs = SelectQuestionAnswerPairs();

        listSize = FAQs.Count;

        /*
        During initialization, we are determining the size of the FAQ relative to the screen.
        Once it is determine, we don't want to change the FAQ size as these are determined by
        pre-existing variables.
        */
        if (!_isInstantiated)
        {
            faqGrid = containerRect.GetComponent<GridLayoutGroup>();
            faqRect = containerRect.GetComponent<RectTransform>();
            faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
             // +4 does not work for large data sets, this needs to be reconfigured

            faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        }
        //Adjusting area where FAQs are stored depending on number of FAQs.
        faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 4) - faqGrid.spacing.y * 4);


        //For each FAQ, create a UI button that displays the question and answers.
        foreach (QuestionAnswerPair pair in FAQs)
        {
            GameObject newButton = Instantiate(originalFAQ);
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
        }
    }

    //Calls Database for FAQs and their respective answers.
	public List<QuestionAnswerPair> SelectQuestionAnswerPairs()
	{
		
		string sql = "select `qID`, `question`, `question_es`, Answer.aiD, `answer`, `answer_es` from Question inner join Answer on Question.aID = Answer.aiD AND Question.qID != -1 LIMIT 50";
		List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair>(sql);
		
		return pair_list;
	}
}