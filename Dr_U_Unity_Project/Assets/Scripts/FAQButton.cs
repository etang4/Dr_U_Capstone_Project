using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FAQButton : MonoBehaviour {
    public string question = "";
    public string answer = "";
    public Color questionColor;
    public Color answerColor;


	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Image>().color = questionColor;

	}
	
	// Update is called once per frame
	void Update () {
      
	}

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

    public void switchToAnswer()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = answerColor;
        this.gameObject.transform.GetChild(0).GetComponent<Text>().text = question + "\n\n" + answer;
        
    }

    public void switchToQuestion()
    {
        this.gameObject.GetComponent<Animation>().Play("FlipButton");
        this.gameObject.GetComponent<Image>().color = questionColor;
        this.gameObject.transform.GetChild(0).GetComponent<Text>().text = question;
        
    }
}
