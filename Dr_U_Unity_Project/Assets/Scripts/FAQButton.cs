using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FAQButton : MonoBehaviour {
    public string question = "";
    public string answer = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleFAQ()
    {
        GameObject button = transform.gameObject;
        if (button.GetComponent<Image>().color == Color.blue)
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
        GameObject button = transform.gameObject;
        button.GetComponent<Image>().color = Color.blue;

        button.transform.GetChild(0).GetComponent<Text>().text = question + "\n\n" + answer; 
    }

    public void switchToQuestion()
    {
        GameObject button = transform.gameObject;
        button.GetComponent<Image>().color = Color.red;

        button.transform.GetChild(0).GetComponent<Text>().text = question;
    }
}
