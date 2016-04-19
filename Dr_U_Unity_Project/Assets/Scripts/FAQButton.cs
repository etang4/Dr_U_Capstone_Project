using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
    This class manages the content of the FAQ
*/

public class FAQButton : MonoBehaviour {
    public string question = "";
    public string answer = "";
    //Determines what language is displayed
	public string language;

	public QuestionAnswerPair faqPair;
    //Colors can be adjusted in prefab "FAQ"
    public Color questionColor;
    public Color answerColor;


	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Image>().color = questionColor;
    }
	
	// Update is called once per frame
	void Update () {
      
	}

    //Onclick Event, toggles FAQ
    public void toggleFAQ()
    {
        if (this.gameObject.GetComponent<Image>().color == answerColor)
        {
            switchToQuestion();
        }
        else
        {
            switchToAnswer();
        }
    }

    //Toggles FAQ to Answer
    public void switchToAnswer()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = answerColor;

        if (PlayerPrefs.GetString("language") == "Espanol") {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question_es + "\n\n" + faqPair.answer_es;
		} else {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question + "\n\n" + faqPair.answer;
		}
        this.gameObject.GetComponent<Animation>().Play("FlipButtonReverse");
    }

    //Toggles FAQ to Question
    public void switchToQuestion()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = questionColor;

		if (PlayerPrefs.GetString("language") == "Espanol") {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question_es;
		} else {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question;
		}
        this.gameObject.GetComponent<Animation>().Play("FlipButtonReverse");
    }
}
