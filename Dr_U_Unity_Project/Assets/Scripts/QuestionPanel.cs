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
	private string langauge;

    // Use this for initialization
    void Start()
    {
		// TODO TEMP
		Debug.Log("starting QuestionPanel...");
        loadFAQs();
    }

    public void loadFAQs()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (PlayerPrefs.GetString("language") == null)
        {
            langauge = PlayerPrefs.GetString("language", "English");
        }
        else
        {
            langauge = PlayerPrefs.GetString("language");
        }
        //localDB = new DBConnector();
        List<QuestionAnswerPair> FAQs = SelectQuestionAnswerPairs();


        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();
        faqGrid.cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width, faqRect.rect.height / 7);
        faqRect.sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize + 4));
        // magic number, not sure why +4 works, I think I'm missing something in the y range...

        faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);

        foreach (QuestionAnswerPair pair in FAQs)
        {
            GameObject newButton = Instantiate(originalButton);
            FAQButton FAQ = newButton.GetComponent<FAQButton>();
            FAQ.faqPair = pair;
            if (langauge == "Espanol")
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

    private Dictionary<string, string> GetFAQs()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
		dict ["When Were dinosaurs found?"] = "From 250 million years ago up until 65 million years.";
		dict ["How many horns did Triceratops have?"] = "Three";
		dict ["Which came first, the Jurassic or Cretaceous Period?"] = "The Jurassic Period";
		dict ["Was Diplodocus a carnivore or herbivore?"] = "Herbivore";
		dict ["Did Theropods such as Allosaurus and Carnotaurus move on two legs or four?"] = "Two";
		dict ["Apatosaurus is also widely known by what other name?"] = "Brontosaurus";
		dict ["What type of dinosaur features on the logo of the Toronto based NBA basketball team?"] = "Raptor (Velociraptor)";
		dict ["What dinosaur themed book was turned into a blockbuster movie in 1993?"] = "Jurassic Park";
		dict ["Did Sauropods such as Brachiosaurus and Diplodocus move on two legs or four?"] = "Four";
		dict ["Which came first, the Jurassic or Triassic Period?"] = "The Triassic Period";
		dict ["What weighed more, a fully grown Spinosaurus or Deinonychus?"] = "Spinosaurus";
		dict ["A person who studies fossils and prehistoric life such as dinosaurs is known as a what?"] = "Paleontologist";
		dict ["Did birds evolved from dinosaurs."] = "Yes!";

        listSize = dict.Count;

        return dict;
    }
}