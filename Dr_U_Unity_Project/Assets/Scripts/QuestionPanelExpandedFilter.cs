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
    public GameObject[] items;
    public GameObject containerRect;
    public InputField SearchBarText;
    public Button BackButton;

    // Use this for initialization
    void Start()
    {
        containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width,
                                                                             containerRect.GetComponent<RectTransform>().rect.height / listSize);
        containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x,
                                                                            containerRect.GetComponent<GridLayoutGroup>().cellSize.y * listSize);
        containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        SearchBarText.onEndEdit.AddListener(filterList);

        RectTransform searchPanelRect = SearchBarText.GetComponent<RectTransform>();
        RectTransform backButtonRect = BackButton.GetComponent<RectTransform>();

        searchPanelRect.offsetMin = new Vector2(backButtonRect.rect.width + 25, 2);
    }

    public void filterList(string input)
    {
        string result = input.ToLower();
        Dictionary<string, string> searchResults = fakeSearch(result, 1);          //Update this to use real data

        int x = 0;
        Array.Clear(itemsList, 0, itemsList.Length);

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

