using UnityEngine;
using System.Collections;

public class MaxOutScores : MonoBehaviour {

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
	private int upgradePoints { get; set; }	// The amount of upgrades the player has earned. Increases each time the player gains rank or earns a badge.
	private int upgradesPurchased { get; set; }			// The mount of upgrades the player has purchased. Doubles the EXP gained from every activity. Effects are cumulative.

	// Use this for initialization
	void Start () {
	
	}


	public void maxOutScores ()
	{
		experience = 51200;
		rank = 11;
		rankUp = 102400;
		questionsAsked = 100;
		questionsStumped = 100;
		faqsClicked = 100;
		imagesClicked = 100;
		exhibitsVisited = 100;
		planetsVisited = 100;
		mysteriesInvestigated = 100;
		mysteriesSolved = 100;
		drUSaved = 100;
		badgesCount = 0;
		upgradePoints = 11;
		upgradesPurchased = 22;

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
		PlayerPrefs.SetInt ("upgradePoints", upgradePoints);
		PlayerPrefs.SetInt ("upgradesPurchased", upgradesPurchased);
		PlayerPrefs.Save();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
