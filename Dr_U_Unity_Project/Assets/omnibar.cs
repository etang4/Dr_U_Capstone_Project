using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class omnibar : MonoBehaviour {
    Dictionary<string, string> FAQs = new Dictionary<string, string>();         // This should be covered elsewhere with real data.

	// Use this for initialization
	void Start () {
        initFAQ();          //Delete when we have real data.
        var input = gameObject.GetComponent<InputField>();
        var changeEvent = new InputField.OnChangeEvent();
        changeEvent.AddListener(SubmitSearch);
        input.onValueChange = changeEvent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SubmitSearch(string input) {
        if (input.EndsWith(" ") || input.EndsWith("\n")){
            // Searches only when the user hits space or newline
            Debug.Log(input);
            string FAQResults = searchFAQs(input);                //Update this to use real data
            string searchResults = fakeSearch(input, 1);          //Update this to use real data
            // Input answers into the panel here
        }
    }
    private string searchFAQs(string input)
    {
        foreach (string word in input.Split())
        {
            if (FAQs.ContainsKey(word))
            {
                return FAQs[word];
            }
        }
        return null;
    }




    // DOWN HERE BE CODE THAT NEEDS TO BE DELETED ////////////////////////////////////////
    private void initFAQ()
    {
        FAQs.Add("one", "1");
        FAQs.Add("two", "2");
        FAQs.Add("Three", "3");
    }
    private string fakeSearch(string input, int id)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("test", "Works");
        dict.Add("fail", "Fails");
        foreach (string word in input.Split())
        {
            if (dict.ContainsKey(word)){
                return dict[word];
            }
        }
        return null;
    }
}
