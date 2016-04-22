/// <summary>
/// The DBConnector class provides basic CRUD (create, read, update, delete) functions for all of the tables in the local database.
/// This class is meant to only house static functions.
/// </summary>

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

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// CRUD Functions for Answer table

	/// <summary>
	/// Selects all of the answers in the Answer table.
	/// </summary>
	/// <returns>A list of Answer objects.</returns>
	public static List<Answer> SelectAnswers(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Answer> (from answer in dbManager.Table<Answer> () select answer);

		return temp; 
	}

	/// <summary>
	/// Inserts a single answer into the Answer table.
	/// </summary>
	public static void InsertAnswer(SimpleSQL.SimpleSQLManager dbManager, int id, string ans, string ans_es, string src, string cont_area, string expr, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Answer temp = new Answer { aID = id, answer = ans, answer_es = ans_es, source = src, content_area = cont_area, expiration = expr, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single answer into the Answer table.
	/// </summary>
	public static void UpdateAnswer(SimpleSQL.SimpleSQLManager dbManager, int id, string ans, string ans_es, string src, string cont_area, string expr, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Answer temp = new Answer { aID = id, answer = ans, answer_es = ans_es, source = src, content_area = cont_area, expiration = expr, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single answer into the Answer table.
	/// </summary>
	public static void DeleteAnswer(SimpleSQL.SimpleSQLManager dbManager, int id)
	{
		Answer temp = new Answer { aID = id };
		dbManager.Delete<Answer>(temp);
	}

	/// CRUD Functions for Badge Table

	/// <summary>
	/// Selects all the badges in the Badge table.
	/// </summary>
	/// <returns>A list of Badge objects.</returns>
	public static List<Badge> SelectBadges(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Badge> (from b in dbManager.Table<Badge> () select b);

		return temp;
	}

	/// <summary>
	/// Inserts a single badge into the Badge table.
	/// </summary>
	public static void InsertBadge(SimpleSQL.SimpleSQLManager dbManager, string nme, string dsc, string ifile, int act, int cntneed, int tcID, int effct) 
	{
		Badge temp = new Badge { name = nme, description = dsc, iconFile = ifile, isActive = act, countNeeded = cntneed, tracksCounterID = tcID, effect = effct };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single badge into the Badge table.
	/// </summary>
	public static void UpdateBadge(SimpleSQL.SimpleSQLManager dbManager, int bID, string nme, string dsc, string ifile, int act, int cntneed, int tcID, int effct) 
	{
		Badge temp = new Badge { badgeID = bID, name = nme, description = dsc, iconFile = ifile, isActive = act, countNeeded = cntneed, tracksCounterID = tcID, effect = effct };
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single badge into the Badge table.
	/// </summary>
	public static void DeleteBadge(SimpleSQL.SimpleSQLManager dbManager, int bID) 
	{
		Badge temp = new Badge { badgeID = bID };
		dbManager.Delete<Badge>(temp);
	}

	/// CRUD Functions for Estimote Table

	/// <summary>
	/// Selects all of the estimotes in the Estimote table.
	/// </summary>
	/// <returns>A list of Estimote objects.</returns>
	public static List<Estimote> SelectEstimotes(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Estimote> (from b in dbManager.Table<Estimote> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single single estimote into the Estimote table.
	/// </summary>
	public static void InsertEstimote(SimpleSQL.SimpleSQLManager dbManager, int eID, string nme, string dsc, int mjr, int mnr) 
	{
		Estimote temp = new Estimote { estimoteID = eID, name = nme, description = dsc, major = mjr, minor = mnr };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single single estimote into the Estimote table.
	/// </summary>
	public static void UpdateEstimote(SimpleSQL.SimpleSQLManager dbManager, int eID, string nme, string dsc, int mjr, int mnr) 
	{
		Estimote temp = new Estimote { estimoteID = eID, name = nme, description = dsc, major = mjr, minor = mnr };
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single single estimote into the Estimote table.
	/// </summary>
	public static void DeleteEstimote(SimpleSQL.SimpleSQLManager dbManager, int eID) 
	{
		Estimote temp = new Estimote { estimoteID = eID };
		dbManager.Delete<Estimote>(temp);
	}

	/// CRUD Functions for Exhibit Table

	/// <summary>
	/// Selects all of the exhibits in the Exhibit table.
	/// </summary>
	/// <returns>A list of Exhibit objects.</returns>
	public static List<Exhibit> SelectExhibits(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Exhibit> (from b in dbManager.Table<Exhibit> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single exhibit into the Exhibit table.
	/// </summary>
	public static void InsertExhibit(SimpleSQL.SimpleSQLManager dbManager, int ID, int eID, string nme, string itro, string info, string imge) 
	{
		Exhibit temp = new Exhibit { exhibitID = ID, estimoteID = eID, name = nme, intro = itro, information = info, image = imge };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single exhibit into the Exhibit table.
	/// </summary>
	public static void UpdateExhibit(SimpleSQL.SimpleSQLManager dbManager, int ID, int eID, string nme, string itro, string info, string imge) 
	{
		Exhibit temp = new Exhibit { exhibitID = ID, estimoteID = eID, name = nme, intro = itro, information = info, image = imge };
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single exhibit into the Exhibit table.
	/// </summary>
	public static void DeleteExhibit(SimpleSQL.SimpleSQLManager dbManager, int eID) 
	{
		Exhibit temp = new Exhibit { exhibitID = eID };
		dbManager.Delete<Exhibit>(temp);
	}

	// CRUD Functions for Question Table

	/// <summary>
	/// Selects all the questions in the Question table.
	/// </summary>
	/// <returns>A list of Question objects.</returns>
	public static List<Question> SelectQuestions(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Question> (from b in dbManager.Table<Question> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single question into the Question table.
	/// </summary>
	public static void InsertQuestion(SimpleSQL.SimpleSQLManager dbManager, int id, string qes, string qes_es, int aid, string cont_area, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Question temp = new Question { qID = id, question = qes, question_es = qes_es, aID = aid, content_area = cont_area, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single question into the Question table.
	/// </summary>
	public static void UpdateQuestion(SimpleSQL.SimpleSQLManager dbManager, int id, string qes, string qes_es, int aid, string cont_area, int sggst_lvl, int locID, int uID, string crtd, string mdfd) 
	{
		Question temp = new Question { qID = id, question = qes, question_es = qes_es, aID = aid, content_area = cont_area, suggest_level = sggst_lvl, locationID = locID, userID = uID, created = crtd, modified = mdfd};
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single question into the Question table.
	/// </summary>
	public static void DeleteQuestion(SimpleSQL.SimpleSQLManager dbManager, int ID) 
	{
		Question temp = new Question { qID = ID };
		dbManager.Delete<Question>(temp);
	}

	/// CRUD Functions for Score Table

	/// <summary>
	/// Selects all the player scores in the database.
	/// </summary>
	/// <returns>A list of Score objects.</returns>
	public static List<Score> SelectScores(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Score> (from b in dbManager.Table<Score> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single player score into the Score table.
	/// </summary>
	public static void InsertScore(SimpleSQL.SimpleSQLManager dbManager, string nme, int c0, int c1, int c2, int c3, int c4, int c5, int c6, int c7, int c8, int c9, int c10) 
	{
		Score temp = new Score { player_name = nme, counter00 = c0, counter01 = c1, counter02 = c2, counter03 = c3, counter04 = c4, counter05 = c5, counter06 = c6, counter07 = c7, counter08 = c8, counter09 = c9, counter10 = c10};
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single player score into the Score table.
	/// </summary>
	public static void UpdateScore(SimpleSQL.SimpleSQLManager dbManager, int id, string nme, int c0, int c1, int c2, int c3, int c4, int c5, int c6, int c7, int c8, int c9, int c10) 
	{
		Score temp = new Score { playerID = id, player_name = nme, counter00 = c0, counter01 = c1, counter02 = c2, counter03 = c3, counter04 = c4, counter05 = c5, counter06 = c6, counter07 = c7, counter08 = c8, counter09 = c9, counter10 = c10};
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single player score into the Score table.
	/// </summary>
	public static void DeleteScore(SimpleSQL.SimpleSQLManager dbManager, int ID) 
	{
		Score temp = new Score { playerID = ID };
		dbManager.Delete<Score>(temp);
	}

	/// CRUD Functions for ScoreCounter Table

	/// <summary>
	/// Selects all of the score counters in the ScoreCounter table.
	/// </summary>
	/// <returns>A list of ScoreCounter objects.</returns>
	public static List<ScoreCounter> SelectScoreCounters(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<ScoreCounter> (from b in dbManager.Table<ScoreCounter> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single score counter into the ScoreCounter table.
	/// </summary>
	public static void InsertScoreCounter(SimpleSQL.SimpleSQLManager dbManager, string tpe, int dflt) 
	{
		ScoreCounter temp = new ScoreCounter { type = tpe, defaultCount = dflt };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single score counter into the ScoreCounter table.
	/// </summary>
	public static void UpdateScoreCounter(SimpleSQL.SimpleSQLManager dbManager, int id, string tpe, int dflt) 
	{
		ScoreCounter temp = new ScoreCounter { counterID = id, type = tpe, defaultCount = dflt };
		dbManager.UpdateTable(temp);
	}
	
	/// <summary>
	/// Deletes a single score counter into the ScoreCounter table.
	/// </summary>
	public static void DeleteScoreCounter(SimpleSQL.SimpleSQLManager dbManager, int ID) 
	{
		ScoreCounter temp = new ScoreCounter { counterID = ID };
		dbManager.Delete<ScoreCounter>(temp);
	}

	/// CRUD Functions for Unlock Table

	/// <summary>
	/// Selects all of the unlocks in the Unlock table.
	/// </summary>
	/// <returns>A list of Unlock objects.</returns>
	public static List<Unlock> SelectUnlocks(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Unlock> (from b in dbManager.Table<Unlock> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single unlock into the Unlock table.
	/// </summary>
	public static void InsertUnlock(SimpleSQL.SimpleSQLManager dbManager, string nme, string dsc, string ifile, int act, int eID) 
	{
		Unlock temp = new Unlock { name = nme, description = dsc, iconFile = ifile, isActive = act, exhibitID = eID };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single unlock into the Unlock table.
	/// </summary>
	public static void UpdateUnlock(SimpleSQL.SimpleSQLManager dbManager, int id, string nme, string dsc, string ifile, int act, int eID) 
	{
		Unlock temp = new Unlock { unlockID = id, name = nme, description = dsc, iconFile = ifile, isActive = act, exhibitID = eID };
		dbManager.UpdateTable(temp);
	}
	
	/// <summary>
	/// Deletes a single unlock into the Unlock table.
	/// </summary>
	public static void DeleteUnlock(SimpleSQL.SimpleSQLManager dbManager, int ID) 
	{
		Unlock temp = new Unlock { unlockID = ID };
		dbManager.Delete<Unlock>(temp);
	}

	/// CRUD Functions for Upgrade Table

	/// <summary>
	/// Selects all the upgrades in the Upgrade table.
	/// </summary>
	/// <returns>A list of Upgrade objects.</returns>
	public static List<Upgrade> SelectUpgrades(SimpleSQL.SimpleSQLManager dbManager) {
		var temp = new List<Upgrade> (from b in dbManager.Table<Upgrade> () select b);
		
		return temp;
	}

	/// <summary>
	/// Inserts a single upgrade into the Upgrade table.
	/// </summary>
	public static void InsertUpgrade(SimpleSQL.SimpleSQLManager dbManager, string nme, string dsc, string ifile, int act, int prce, int tcID, int effct) 
	{
		Upgrade temp = new Upgrade { name = nme, description = dsc, iconFile = ifile, isActive = act, price = prce, tracksCounterID = tcID, effect = effct };
		dbManager.Insert(temp);
	}

	/// <summary>
	/// Updates a single upgrade into the Upgrade table.
	/// </summary>
	public static void UpdateUpgrade(SimpleSQL.SimpleSQLManager dbManager, int id, string nme, string dsc, string ifile, int act, int prce, int tcID, int effct) 
	{
		Upgrade temp = new Upgrade { upgradeID = id, name = nme, description = dsc, iconFile = ifile, isActive = act, price = prce, tracksCounterID = tcID, effect = effct };
		dbManager.UpdateTable(temp);
	}

	/// <summary>
	/// Deletes a single upgrade into the Upgrade table.
	/// </summary>
	public static void DeleteUpgrade(SimpleSQL.SimpleSQLManager dbManager, int id) 
	{
		Upgrade temp = new Upgrade { upgradeID = id };
		dbManager.Delete<Upgrade>(temp);
	}

}