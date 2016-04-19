using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour 
{
	/*
	public DBConnector localDB;
	public static SimpleSQL.SimpleSQLManager dbManager;

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
	
	public Text counter;
	private static GameObject alert;
	private static Text alertText;
	private static Animation addScore;
	
	private static int experience { get; set; }
	public static int rank { get; set; }
	private static int rankUp { get; set; }
	private static int questionsAsked { get; set; }
	private static int questionsStumped { get; set; }
	private static int faqsClicked { get; set; }
	private static int imagesClicked { get; set; }
	private static int exhibitsVisited { get; set; }
	private static int planetsLanded { get; set; }
	private static int mysteriesInvestigated { get; set; }
	private static int mysteriesSolved { get; set; }
	private static int drUSaved { get; set; }
	
	// Use this for initialization
	void Start() {
		//		defineCounters ();
		
		experience = PlayerPrefs.GetInt("Experience");
		counter = GetComponent<Text>();

		rankUp = 25; // Experience points needed to rank up
		
		alert = GameObject.Find("ScoreAlert");
		alertText = alert.GetComponentsInChildren<Text>()[0];
		
		addScore = alert.gameObject.GetComponent<Animation>();
		alert.SetActive(false);
		
		// Preload the database with relevant information
	}
	
	/*	
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
	// Update is called once per frame
	void Update () {
		if (!addScore.isPlaying)
		{
			alert.SetActive(false);
			counter.text = experience.ToString() + " EXP";
			PlayerPrefs.SetInt("experience", experience);
			PlayerPrefs.Save();
		}
	}
	
	void OnApplicationQuit() {
		PlayerPrefs.DeleteKey("experience");
	}
	
	public static void addExperience(int amount) {
		alert.SetActive(true);
		alertText.text = amount.ToString();
		addScore.Play("AddScore");
		experience += amount;
		checkRankUp ();								// check if the player earned enough experience to level up
	}
	
	public static void addRank(int amount) {
		alert.SetActive(true);
		alertText.text = amount.ToString();
		addScore.Play("AddScore");
		rank += amount;
	}
	
	static void checkRankUp ()
	{
		if (experience >= rankUp && rank <= 11) {
			rankUp = rankUp * 2;
			addRank (1);
		}
	}
	
	public static void addQuestionsAsked(int amount, int questionsFound) {
		questionsAsked += amount;
		
		if (questionsFound == 0) {			// If questionsFound = 0, then the search term returned no results, and thus Dr. U was stumped.					
			addExperience(10);
			addQuestionsStumped(1);	
		} else if (questionsFound > 0)  {
			addExperience(5);
		}
	}
	
	static void addQuestionsStumped (int amount)
	{
		questionsStumped += amount;
	}
	
	public static void addFaqsClicked (int amount)
	{
		faqsClicked += amount;
		addExperience(5);
	}
	
	public static void addImagesClicked (int amount)
	{
		imagesClicked += 1;
		addExperience(5);
	}
	
	public static void addExhibitsVisited (int amount)
	{
		exhibitsVisited += amount;
		addExperience(5);
	}
	
	public static void addPlanetsLanded (int amount)
	{
		planetsLanded += amount;
		addExperience(5);
	}
	
	public static void addMysteriesInvestigated (int amount)
	{
		mysteriesInvestigated += amount;
		addExperience(5);
	}
	
	public static void addMysteriesSolved (int amount)
	{
		mysteriesSolved += amount;
		addExperience(50);
	}
	
	public static void addDrUSaved (int amount)
	{
		drUSaved += amount;
		addExperience(100);
	}
}