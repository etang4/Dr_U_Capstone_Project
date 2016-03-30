using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FAQButton : MonoBehaviour {
    public string question = "";
    public string answer = "";
	public string language;

	public QuestionAnswerPair faqPair;
    public Color questionColor;
    public Color answerColor;


	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Image>().color = questionColor;

		//Default setting for language is english
		language = PlayerPrefs.GetString("language", "English");

	}
	
	// Update is called once per frame
	void Update () {
      
	}

    public void toggleFAQ()
    {
        Debug.Log("test");
        if (this.gameObject.GetComponent<Image>().color == answerColor)
        {
            switchToQuestion();
        }
        else
        {
            switchToAnswer();
        }
    }

    public void switchToAnswer()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = answerColor;
		if (language == "Espanol") {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question_es + "\n\n" + faqPair.answer_es;
		} else {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question + "\n\n" + faqPair.answer;
		}
        this.gameObject.GetComponent<Animation>().Play("FlipButtonReverse");
    }

    public void switchToQuestion()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = questionColor;
		if (language == "Espanol") {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question_es;
		} else {
			this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = faqPair.question;
		}
        this.gameObject.GetComponent<Animation>().Play("FlipButtonReverse");
    }
}
