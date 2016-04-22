using UnityEngine;
using System.Collections;

public class EraseScores : MonoBehaviour {

	private int experience { get; set; }
	private int rank { get; set; }
	private int rankUp { get; set; }
	private int questionsAsked { get; set; }
	private int questionsStumped { get; set; }
	private int faqsClicked { get; set; }
	private int imagesClicked { get; set; }
	private int exhibitsVisited { get; set; }
	private int planetsVisited { get; set; }
	private int mysteriesInvestigated { get; set; }
	private int mysteriesSolved { get; set; }
	private int drUSaved { get; set; }
	private int badgesCount { get; set; }
	private const int expToFirstRank = 25;

	// Use this for initialization
	void Start () {
	
	}


	public void eraseScores ()
	{
		experience = 0;
		rank = 0;
		rankUp = expToFirstRank;
		questionsAsked = 0;
		questionsStumped = 0;
		faqsClicked = 0;
		imagesClicked = 0;
		exhibitsVisited = 0;
		planetsVisited = 0;
		mysteriesInvestigated = 0;
		mysteriesSolved = 0;
		drUSaved = 0;
		badgesCount = 0;

		PlayerPrefs.SetInt("experience", experience);
		PlayerPrefs.SetInt("rank", rank);
		PlayerPrefs.SetInt("rankUp", rankUp);
		PlayerPrefs.SetInt("questionsAsked", questionsAsked);
		PlayerPrefs.SetInt("questionsStumped", questionsStumped);
		PlayerPrefs.SetInt("faqsClicked", faqsClicked);
		PlayerPrefs.SetInt("imagesClicked", imagesClicked);
		PlayerPrefs.SetInt("exhibitsVisited", exhibitsVisited);
		PlayerPrefs.SetInt("planetsVisited", planetsVisited);
		PlayerPrefs.SetInt("mysteriesInvestigated", mysteriesInvestigated);
		PlayerPrefs.SetInt("mysteriesSolved", mysteriesSolved);
		PlayerPrefs.SetInt("drUSaved", drUSaved);
		PlayerPrefs.SetInt ("badgesCount", badgesCount);
		PlayerPrefs.Save();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
