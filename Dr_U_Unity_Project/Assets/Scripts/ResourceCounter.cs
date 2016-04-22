using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour 
{
	/*
	public DBConnector localDB;
	private SimpleSQL.SimpleSQLManager dbManager;

	public List<ScoreCounter> ScoreCounterList;
	public ScoreCounter experience { get; set; }
	public ScoreCounter rank { get; set; }
	public ScoreCounter rankUp { get; set; }
	public ScoreCounter questionsAsked { get; set; }
	public ScoreCounter questionsStumped { get; set; }
	public ScoreCounter faqsClicked { get; set; }
	public ScoreCounter imagesClicked { get; set; }
	public ScoreCounter exhibitsVisited { get; set; }
	public ScoreCounter planetsLanded { get; set; }
	public ScoreCounter mysteriesInvestigated { get; set; }
	public ScoreCounter mysteriesSolved { get; set; }
	public ScoreCounter drUSaved { get; set; }	*/
	
	public Text resourceCounter;


	private const int expToFirstRank = 25;

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
	private int badgesCount { get; set; }				// Number of badges that have been awarded due to questions asked. See BadgePanel.checkBadges to see how they are awarded. Indexes at 0.

	public static GameObject alert;
	public static Text alertText;
	public static Animation addScore;

	public static void scoreAlert(string message) {
			alert.SetActive(true);
			alertText.text = message;
			addScore.Play("AddScore");
	}
	
	// Use this for initialization
	void Start() {

		alert = GameObject.Find("ScoreAlert");
		alertText = alert.GetComponentsInChildren<Text>()[0];

		addScore = alert.gameObject.GetComponent<Animation>();
		alert.SetActive(false);

		resourceCounter = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (!addScore.isPlaying)
		{
			experience = PlayerPrefs.GetInt ("experience");
			alert.SetActive(false);
			resourceCounter.text = experience.ToString() + " EXP";
			checkRankUp ();								// check if the player earned enough experience to level up
		}
	}


	private void saveScores() {
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
	
	void OnApplicationQuit() {
		PlayerPrefs.DeleteKey("experience");
	}
	
	private void addExperience(int amount) {
		experience += amount;
		scoreAlert (amount.ToString());
		saveScores ();								// Every time the player does something to gain experience save it to global PlayerPrefs object.

	}

    private void addRank(int amount)
    {
		rank += amount;
	}
	
	private void checkRankUp ()
	{
		if (experience >= rankUp && rank <= 11) {
			rankUp = rankUp * 2;
			addRank (1);
		}
	}
}




/*	

	public void addExhibitsVisited (int amount)
	{
		exhibitsVisited += amount;
		addExperience(5, true);
	}
	
	public void addPlanetsLanded (int amount)
	{
		planetsVisited += amount;
		addExperience(5, true);
	}
	
	public void addMysteriesInvestigated (int amount)
	{
		mysteriesInvestigated += amount;
		addExperience(5, false);
	}
	
	public void addMysteriesSolved (int amount)
	{
		mysteriesSolved += amount;
		addExperience(50, true);
	}
	
	public void addDrUSaved (int amount)
	{
		drUSaved += amount;
		addExperience(100, true);
	}
	
	void defineCounters ()
	{
		localDB = new DBConnector();
		DBConnector.InsertScoreCounter (dbManager, "Experience", PlayerPrefs.GetInt ("experience"));
		DBConnector.InsertScoreCounter (dbManager, "Rank", 1);												// Current Rank
		DBConnector.InsertScoreCounter (dbManager, "Rank Up", 50);											// Experience Needed to Rank Up
		DBConnector.InsertScoreCounter (dbManager, "Questions Asked", 0);
		DBConnector.InsertScoreCounter (dbManager, "Questions That Stumped Dr U", 0);
		DBConnector.InsertScoreCounter (dbManager, "Questions Asked", 0);
		DBConnector.InsertScoreCounter (dbManager, "Images Clicked", 0);
		DBConnector.InsertScoreCounter (dbManager, "Exhibits Visited", 0);
		DBConnector.InsertScoreCounter (dbManager, "Planets Landed", 0);
		DBConnector.InsertScoreCounter (dbManager, "Mysteries Investigated", 0);
		DBConnector.InsertScoreCounter (dbManager, "Mysteries Solved", 0);
		DBConnector.InsertScoreCounter (dbManager, "Dr. U. Saved", 0);

		ScoreCounter experience = new ScoreCounter (0, "Experience", PlayerPrefs.GetInt("count"));
		ScoreCounter rank = new ScoreCounter (1, "Rank", 1);												// Current Rank
		ScoreCounter rankUp = new ScoreCounter (2, "Rank Up", 50);											// Experience Needed to Rank Up
		ScoreCounter questionsAsked = new ScoreCounter (3, "Questions Asked", 0);
		ScoreCounter questionsStumped = new ScoreCounter (4, "Stumped Dr U", 0);
		ScoreCounter faqsClicked = new ScoreCounter (5, "FAQs Clicked", 0);
		ScoreCounter imagesClicked = new ScoreCounter (6, "Images Clicked", 0);
		ScoreCounter exhibitsVisited = new ScoreCounter (7, "Exhibits Visited", 0);
		ScoreCounter planetsLanded = new ScoreCounter (8, "Planets Landed", 0);
		ScoreCounter mysteriesInvestigated = new ScoreCounter (9, "Mysteries Investigated", 0);
		ScoreCounter mysteriesSolved = new ScoreCounter (10, "Mysteries Solved", 0);
		ScoreCounter drUSaved = new ScoreCounter (11, "Dr. U Saved", 0);

		List<ScoreCounter> ScoreCounterList = DBConnector.SelectScoreCounters(dbManager);  // Grab ScoreCounter data from DB

	}
*/