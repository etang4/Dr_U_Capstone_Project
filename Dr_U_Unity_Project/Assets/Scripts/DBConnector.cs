using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleSQL;

public class DBConnector : MonoBehaviour {

	public SimpleSQL.SimpleSQLManager dbManager;

	private int db_exists;
	private string db_date_string;
	private string currentDate_string;
	private System.DateTime currentDate;
	private System.DateTime db_date;

	// Use this for initialization
	void Start () {

		// This code is for DB versioning. It will update the data in the local database once a day.
		currentDate = System.DateTime.UtcNow;
		currentDate_string = ConvertDateTimeToBinaryString(currentDate);

		db_exists = PlayerPrefs.GetInt("db_exists", 0);
		db_date_string = PlayerPrefs.GetString("db_date", currentDate_string);
		db_date = ConvertBinaryStringToDateTime(db_date_string);

		if (db_exists == 0) 
		{
			CreateDatabaseForFirstTime();
			PlayerPrefs.SetInt("db_exists", 1);
			PlayerPrefs.SetString("db_date", currentDate_string);
			PlayerPrefs.Save();
		} 
		else if (db_exists == 1) 
		{
			var days = GetDBTimeDifference(currentDate, db_date);
			if (days > 1) 
			{
				UpdateLocalDatabase();
				PlayerPrefs.SetString("db_date", currentDate_string);
				PlayerPrefs.Save();
			}

		}

		var exh = SelectExhibits ();
		foreach (Exhibit e in exh) 
		{
			Debug.Log(e.ToString());
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Functions for Answer table
	public List<Answer> SelectAnswers() {
		var temp = new List<Answer> (from answer in dbManager.Table<Answer> () select answer);

		return temp; 
	}

	public void InsertAnswer(int id, string ans, string ans_es, string src, string cont_area, string expr, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Answer temp = new Answer { aID = id, answer = ans, answer_es = ans_es, source = src, content_area = cont_area, expiration = expr, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.Insert(temp);
	}

	public void UpdateAnswer(int id, string ans, string ans_es, string src, string cont_area, string expr, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Answer temp = new Answer { aID = id, answer = ans, answer_es = ans_es, source = src, content_area = cont_area, expiration = expr, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.UpdateTable(temp);
	}

	public void DeleteAnswer(int id)
	{
		Answer temp = new Answer { aID = id };
		dbManager.Delete<Answer>(temp);
	}

	//Functions for Badge Table
	public List<Badge> SelectBadges() {
		var temp = new List<Badge> (from b in dbManager.Table<Badge> () select b);

		return temp;
	}

	public void InsertBadge(string nme, string dsc, string ifile, int act, int cntneed, int tcID, int effct) 
	{
		Badge temp = new Badge { name = nme, description = dsc, iconFile = ifile, isActive = act, countNeeded = cntneed, tracksCounterID = tcID, effect = effct };
		dbManager.Insert(temp);
	}

	public void UpdateBadge(int bID, string nme, string dsc, string ifile, int act, int cntneed, int tcID, int effct) 
	{
		Badge temp = new Badge { badgeID = bID, name = nme, description = dsc, iconFile = ifile, isActive = act, countNeeded = cntneed, tracksCounterID = tcID, effect = effct };
		dbManager.UpdateTable(temp);
	}

	public void DeleteBadge(int bID) 
	{
		Badge temp = new Badge { badgeID = bID };
		dbManager.Delete<Badge>(temp);
	}

	//Functions for Estimote Table
	public List<Estimote> SelectEstimotes() {
		var temp = new List<Estimote> (from b in dbManager.Table<Estimote> () select b);
		
		return temp;
	}
	
	public void InsertEstimote(int eID, string nme, string dsc, int mjr, int mnr) 
	{
		Estimote temp = new Estimote { estimoteID = eID, name = nme, description = dsc, major = mjr, minor = mnr };
		dbManager.Insert(temp);
	}
	
	public void UpdateEstimote(int eID, string nme, string dsc, int mjr, int mnr) 
	{
		Estimote temp = new Estimote { estimoteID = eID, name = nme, description = dsc, major = mjr, minor = mnr };
		dbManager.UpdateTable(temp);
	}
	
	public void DeleteEstimote(int eID) 
	{
		Estimote temp = new Estimote { estimoteID = eID };
		dbManager.Delete<Estimote>(temp);
	}

	//Functions for Exhibit Table
	public List<Exhibit> SelectExhibits() {
		var temp = new List<Exhibit> (from b in dbManager.Table<Exhibit> () select b);
		
		return temp;
	}
	
	public void InsertExhibit(int ID, int eID, string nme, string itro, string info, string imge) 
	{
		Exhibit temp = new Exhibit { exhibitID = ID, estimoteID = eID, name = nme, intro = itro, information = info, image = imge };
		dbManager.Insert(temp);
	}
	
	public void UpdateExhibit(int ID, int eID, string nme, string itro, string info, string imge) 
	{
		Exhibit temp = new Exhibit { exhibitID = ID, estimoteID = eID, name = nme, intro = itro, information = info, image = imge };
		dbManager.UpdateTable(temp);
	}
	
	public void DeleteExhibit(int eID) 
	{
		Exhibit temp = new Exhibit { exhibitID = eID };
		dbManager.Delete<Exhibit>(temp);
	}

	//Functions for Question Table
	public List<Question> SelectQuestions() {
		var temp = new List<Question> (from b in dbManager.Table<Question> () select b);
		
		return temp;
	}
	
	public void InsertQuestion(int id, string qes, string qes_es, int aid, string cont_area, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Question temp = new Question { qID = id, question = qes, question_es = qes_es, aID = aid, content_area = cont_area, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.Insert(temp);
	}
	
	public void UpdateQuestion(int id, string qes, string qes_es, int aid, string cont_area, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Question temp = new Question { qID = id, question = qes, question_es = qes_es, aID = aid, content_area = cont_area, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.UpdateTable(temp);
	}
	
	public void DeleteQuestion(int ID) 
	{
		Question temp = new Question { qID = ID };
		dbManager.Delete<Question>(temp);
	}

	//Functions for Score Table
	public List<Score> SelectScores() {
		var temp = new List<Score> (from b in dbManager.Table<Score> () select b);
		
		return temp;
	}
	
	public void InsertScore(string nme, int c0, int c1, int c2, int c3, int c4, int c5, int c6, int c7, int c8, int c9, int c10) 
	{
		Score temp = new Score { player_name = nme, counter00 = c0, counter01 = c1, counter02 = c2, counter03 = c3, counter04 = c4, counter05 = c5, counter06 = c6, counter07 = c7, counter08 = c8, counter09 = c9, counter10 = c10};
		dbManager.Insert(temp);
	}
	
	public void UpdateScore(int id, string nme, int c0, int c1, int c2, int c3, int c4, int c5, int c6, int c7, int c8, int c9, int c10) 
	{
		Score temp = new Score { playerID = id, player_name = nme, counter00 = c0, counter01 = c1, counter02 = c2, counter03 = c3, counter04 = c4, counter05 = c5, counter06 = c6, counter07 = c7, counter08 = c8, counter09 = c9, counter10 = c10};
		dbManager.UpdateTable(temp);
	}

	
	public void DeleteScore(int ID) 
	{
		Score temp = new Score { playerID = ID };
		dbManager.Delete<Score>(temp);
	}

	//Functions for ScoreCounter Table
	public List<ScoreCounter> SelectScoreCounters() {
		var temp = new List<ScoreCounter> (from b in dbManager.Table<ScoreCounter> () select b);
		
		return temp;
	}
	
	public void InsertScoreCounter(string tpe, int dflt) 
	{
		ScoreCounter temp = new ScoreCounter { type = tpe, defaultCount = dflt };
		dbManager.Insert(temp);
	}
	
	public void UpdateScoreCounter(int id, string tpe, int dflt) 
	{
		ScoreCounter temp = new ScoreCounter { counterID = id, type = tpe, defaultCount = dflt };
		dbManager.UpdateTable(temp);
	}
	
	
	public void DeleteScoreCounter(int ID) 
	{
		ScoreCounter temp = new ScoreCounter { counterID = ID };
		dbManager.Delete<ScoreCounter>(temp);
	}

	//Functions for Unlock Table
	public List<Unlock> SelectUnlocks() {
		var temp = new List<Unlock> (from b in dbManager.Table<Unlock> () select b);
		
		return temp;
	}
	
	public void InsertUnlock(string nme, string dsc, string ifile, int act, int eID) 
	{
		Unlock temp = new Unlock { name = nme, description = dsc, iconFile = ifile, isActive = act, exhibitID = eID };
		dbManager.Insert(temp);
	}
	
	public void UpdateUnlock(int id, string nme, string dsc, string ifile, int act, int eID) 
	{
		Unlock temp = new Unlock { unlockID = id, name = nme, description = dsc, iconFile = ifile, isActive = act, exhibitID = eID };
		dbManager.UpdateTable(temp);
	}
	
	
	public void DeleteUnlock(int ID) 
	{
		Unlock temp = new Unlock { unlockID = ID };
		dbManager.Delete<Unlock>(temp);
	}

	//Functions for Upgrade Table
	public List<Upgrade> SelectUpgrades() {
		var temp = new List<Upgrade> (from b in dbManager.Table<Upgrade> () select b);
		
		return temp;
	}
	
	public void InsertUpgrade(string nme, string dsc, string ifile, int act, int prce, int tcID, int effct) 
	{
		Upgrade temp = new Upgrade { name = nme, description = dsc, iconFile = ifile, isActive = act, price = prce, tracksCounterID = tcID, effect = effct };
		dbManager.Insert(temp);
	}
	
	public void UpdateUpgrade(int id, string nme, string dsc, string ifile, int act, int prce, int tcID, int effct) 
	{
		Upgrade temp = new Upgrade { upgradeID = id, name = nme, description = dsc, iconFile = ifile, isActive = act, price = prce, tracksCounterID = tcID, effect = effct };
		dbManager.UpdateTable(temp);
	}
	
	public void DeleteUpgrade(int id) 
	{
		Upgrade temp = new Upgrade { upgradeID = id };
		dbManager.Delete<Upgrade>(temp);
	}


	// Specialized Functions for specific use
	public void UpdateLocalDatabase()
	{

		//Get the information from the web API
		var questions = APIConnector.GetQuestions();
		var answers = APIConnector.GetAnswers();
		var estimotes = APIConnector.GetEstimotes();
		var exhibits = APIConnector.GetExhibits();

		dbManager.InsertAll(questions);
		dbManager.InsertAll(answers);
		dbManager.InsertAll(estimotes);
		dbManager.InsertAll(exhibits);
	}

	public void CreateDatabaseForFirstTime()
	{
		dbManager.CreateTable<Answer>();
		dbManager.CreateTable<Badge>();
		dbManager.CreateTable<Estimote>();
		dbManager.CreateTable<Exhibit>();
		dbManager.CreateTable<Question>();
		dbManager.CreateTable<Score>();
		dbManager.CreateTable<ScoreCounter>();
		dbManager.CreateTable<Unlock>();
		dbManager.CreateTable<Upgrade>();

		//Get the information from the web API
		var questions = APIConnector.GetQuestions();
		var answers = APIConnector.GetAnswers();
		var estimotes = APIConnector.GetEstimotes();
		var exhibits = APIConnector.GetExhibits();
		
		dbManager.InsertAll(questions);
		dbManager.InsertAll(answers);
		dbManager.InsertAll(estimotes);
		dbManager.InsertAll(exhibits);
	}

	public static double GetDBTimeDifference(System.DateTime currentDate, System.DateTime db_date) 
	{
		System.TimeSpan difference = currentDate.Subtract(db_date);

		return difference.TotalDays;
	}

	public static string ConvertDateTimeToBinaryString(System.DateTime time) 
	{
		string time_str = time.ToBinary().ToString();

		return time_str;
	}

	public static System.DateTime ConvertBinaryStringToDateTime(string binary) 
	{
		System.DateTime time = System.DateTime.FromBinary(System.Convert.ToInt64(binary));

		return time;
	}

	/*
	public KeyValuePair<Question, Answer> SelectQuestionAnswerPairs() {


	}*/

}