using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionPanelExpandedFilter : MonoBehaviour
{
    public GameObject originalButton;
    public GameObject[] itemsList;
    public int listSize;
    public GameObject containerRect;
    public InputField SearchBarText;

    private GridLayoutGroup faqGrid;
    private RectTransform faqRect;
	public ResourceCounter resourceCounter;

    // Use this for initialization
    void Start()
    {
        faqGrid = containerRect.GetComponent<GridLayoutGroup>();
        faqRect = containerRect.GetComponent<RectTransform>();
        faqGrid.cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width, faqRect.rect.height / 7);
        faqRect.sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize + 1));

        faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);

        SearchBarText.onEndEdit.AddListener(filterList);
        filterList(SearchBarText.text);         //Initial run
    }


    public void filterList(string input)
    {
        string result = input.ToLower();
        Dictionary<string, string> searchResults = fakeSearch(result, 1);          //Update this to use real data

        int x = 0;
        foreach(GameObject item in itemsList){
            Destroy(item);
        }

        foreach (KeyValuePair<string, string> answer in searchResults)
        {
            GameObject newButton = Instantiate(originalButton);
            itemsList[x] = newButton;
            FAQButton FAQ = newButton.GetComponent<FAQButton>();
            FAQ.question = answer.Key;
            FAQ.answer = answer.Value;
            newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.question;

            newButton.transform.parent = this.transform;
            x++;
        }

		BadgePanel.addQuestionAsked(1);
		
		if (x == 0) {
			ResourceCounter.addPoints(10);
			BadgePanel.addStumped(1);
		} else if (x > 0)  {
			ResourceCounter.addPoints(5);
		}

    }



    private Dictionary<string, string> fakeSearch(string input, int id)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("test", "Works");
        dict.Add("fail", "Fails");
        Dictionary<string, string> results = new Dictionary<string, string>();
        foreach (string word in input.Split())
        {
            if (dict.ContainsKey(word.ToLower()))
            {
                results.Add(word, dict[word]);
            }
        }
        return results;
    }

}

