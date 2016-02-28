using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public GameObject originalButton;
    public GameObject[] itemsList;
    public int listSize;
    public GameObject[] items;
    public GameObject containerRect;

    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;

    // Use this for initialization
    void Start()
    {
        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();
        faqGrid.cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width, faqRect.rect.height / 7);
        faqRect.sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize + 1));

        faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        
        //containerRect.GetComponent<RectTransform>().offsetMin = new Vector2(containerRect.GetComponent<RectTransform>().offsetMin.x, bottom);

        Dictionary<string, string> FAQs = GetFAQs();
        int i = 0;
        foreach (KeyValuePair<string, string> question in FAQs)
        {
            GameObject newButton = Instantiate(originalButton);
            FAQButton FAQ = newButton.GetComponent<FAQButton>();
            FAQ.question = question.Key;
            FAQ.answer = question.Value;
            itemsList[i] = newButton;
            newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.question;
            newButton.transform.parent = this.transform;
            i++;
        }
    }


    private Dictionary<string, string> GetFAQs()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < listSize; i++)
        {
            dict["Frequently Asked Question #" + i + "?"] = "Answer #" + i;
        }
        return dict;
    }
}

