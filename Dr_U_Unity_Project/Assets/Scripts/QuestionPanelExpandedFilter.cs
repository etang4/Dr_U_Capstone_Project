using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionPanelExpandedFilter : MonoBehaviour
{
	public GameObject originalButton;
	public GameObject[] itemsList;
	private int listSize;
	public GameObject containerRect;
	public InputField SearchBarText;
	private bool _isInstantiated;
	private GridLayoutGroup faqGrid;
	private RectTransform faqRect;

	// Use this for initialization
	void Start() {
		
		SearchBarText.onEndEdit.AddListener(filterList);
		_isInstantiated = false;
		filterList(SearchBarText.text);         //Initial run
		_isInstantiated = true;
	}

	public void filterList(string input)
	{
		string result = input.ToLower();
		Dictionary<string, string> searchResults = fakeSearch(result, 1);          //Update this to use real data
		
		int questionsCount = 0;
		foreach(GameObject item in itemsList){
			Destroy(item);
		}
		
		listSize = searchResults.Count;
		
		//TODO: Cannot test if this works until search is implemented
		if (!_isInstantiated)
		{
			faqGrid = containerRect.GetComponent<GridLayoutGroup>();
			faqRect = containerRect.GetComponent<RectTransform>();
			faqGrid.cellSize = new Vector2(faqRect.rect.width, faqRect.rect.height / 7);
			faqRect.sizeDelta = new Vector2(faqRect.sizeDelta.x, (faqGrid.cellSize.y + faqGrid.spacing.y) * (listSize - 4) - faqGrid.spacing.y * 4);
			// +4 does not work for large data sets, this needs to be reconfigured
			
			faqRect.offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
		}
		
		foreach (KeyValuePair<string, string> answer in searchResults)
		{
			GameObject newButton = Instantiate(originalButton);
			itemsList[questionsCount] = newButton;
			FAQButton FAQ = newButton.GetComponent<FAQButton>();
			FAQ.question = answer.Key;
			FAQ.answer = answer.Value;
			newButton.transform.GetChild(0).GetComponent<Text>().text = FAQ.question;
			
			newButton.transform.parent = this.transform;
			questionsCount++;
		}
		
		addQuestionsAsked(1,questionsCount);	// If question = 0, then the search term returned no results, and thus Dr. U was stumped.
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

	// Scoring Code
	public void addQuestionsAsked (int amount, int questionsFound)
	{
		int questionsAsked = PlayerPrefs.GetInt("questionsAsked");
		int questionsStumped = PlayerPrefs.GetInt("questionsStumped");
		int experience = PlayerPrefs.GetInt ("experience");
		int experienceAdded = 5;

		if (questionsFound == 0) {			// If questionsFound = 0, then the search term returned no results, and thus Dr. U was stumped.		
			questionsStumped += 1;	
			experienceAdded = 10;
		}

		questionsAsked += amount;
		experience += experienceAdded;
		ResourceCounter.scoreAlert (experienceAdded.ToString());  // Uncomment this to make it play a score alert when you click an FAQ

		PlayerPrefs.SetInt("questionsAsked", questionsAsked);
		PlayerPrefs.SetInt("questionsStumped", questionsStumped);
		PlayerPrefs.SetInt("experience", experience);
		PlayerPrefs.Save();
	}
}

