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
    public GameObject containerRect;
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

    //Keep global of last estimote ID so we can reload easily.
    private int estimoteID;

    // Use this for initialization
    void Start()
    {
        Debug.Log("starting QuestionPanel...");
        // Assign globals.
        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();

        loadFAQs(-1);    // Start with beacon -1 (pull all info)
        InputSmootherHelper.ClosestBeaconChangedEvent += triggerChange;
    }

    public void triggerChange(HashSet<Beacon> new_closest, HashSet<Beacon> old_closest)
    {
        int estimoteID = -1;
        foreach (Beacon beacon in new_closest)  // Update estimote ID by using major and minor.  If there are more than one (equidistant), grab the last in the list.
        {
            // beacon.minor = 36901;       // TEST CODE
            // beacon.major = 60773;       // TEST CODE
            string sql = string.Format("select `estimoteID` from Estimote WHERE major == {0} AND minor = {1} LIMIT 1", beacon.major, beacon.minor);
            estimoteID = dbManager.Query<int>(sql)[0];

        }
        loadFAQs(estimoteID);
    }

    //Dynamically generates FAQ to be displayed in QuestionPanel
    //Is called when you change language
    public void loadFAQs(int eID)   // Load the FAQs up based on the estimote ID
    {
        if (eID != -99) // Update estimodeID if the previous was not requested. (IE, reload FAQs)
        {
            estimoteID = eID;
        }
        foreach (Transform child in this.gameObject.transform)  // Destroy all current FAQs
        {
            GameObject.Destroy(child.gameObject);
        }
        if (PlayerPrefs.GetString("language") == null)  // Set language
        {
            language = PlayerPrefs.GetString("language", "English");
        }
        else
        {
            language = PlayerPrefs.GetString("language");
        }
        //localDB = new DBConnector();
        List<QuestionAnswerPair> FAQs = SelectQuestionAnswerPairs();    // Get new FAQs
        listSize = FAQs.Count;

        /*
       During initialization, we are determining the size of the FAQ relative to the screen.
       Once it is determine, we don't want to change the FAQ size as these are determined by
       pre-existing variables.
       */
        if (!_isInstantiated)
        {

            faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
            faqRect.offsetMax = new Vector2(faqRect.offsetMax.x, 0);
        }
        //Adjusting area where FAQs are stored depending on number of FAQs.
        faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 3));
        // Set position of scroll to very top
        containerRect.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1;

        foreach (QuestionAnswerPair pair in FAQs)   // Create FAQs from database results.
        {
            if (pair.question != "" || pair.question_es != "")      // Prune out empty returns
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
    }

    //Calls Database for FAQs and their respective answers.
    public List<QuestionAnswerPair> SelectQuestionAnswerPairs()
    {
        string sql = string.Format("select `qID`, `question`, `question_es`, Answer.aiD, `answer`, `answer_es` from Question inner join Answer on Question.aID = Answer.aiD AND Question.qID != {0} LIMIT 50", estimoteID);
        List<QuestionAnswerPair> pair_list = dbManager.Query<QuestionAnswerPair>(sql);

        return pair_list;
    }
}