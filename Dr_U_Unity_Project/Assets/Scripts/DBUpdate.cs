/// <summary>
/// The DBUpdate class handles updating the local database with the data in the remote MySQL database.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleSQL;

public class DBUpdate : MonoBehaviour {
	
	public SimpleSQL.SimpleSQLManager dbManager;
	public GameObject loadingText;
	
	private int db_exists;
	private string db_date_string;
	private string currentDate_string;
	private System.DateTime currentDate;
	private System.DateTime db_date;
	private bool db_update;

	private Question[] questions;
	private Answer[] answers;
	private Estimote[] estimotes;
	private Exhibit[] exhibits;

	
	// Use this for initialization
	void Start () {

		questions = null;
		answers = null;
		estimotes = null;
		exhibits = null;
		db_update = false;

		// This code is for DB versioning. It will update the data in the local database once a day.
		currentDate = System.DateTime.UtcNow;
		currentDate_string = ConvertDateTimeToBinaryString(currentDate);
		
		db_exists = PlayerPrefs.GetInt("db_exists", 0);
		db_date_string = PlayerPrefs.GetString("db_date", currentDate_string);
		db_date = ConvertBinaryStringToDateTime(db_date_string);

		StartCoroutine(CheckForLocalDatabaseUpdates(dbManager));

	}
	
	// Update is called once per frame
	void Update () {

		if (db_update == true) {
			Application.LoadLevel("Main_Mode");
            
		}

	}


	void OnApplicationQuit() {

		// Uncomment the folowing pieces of code to test the CreateDatabase and UpdateDatabase functions

		// For testing DBConnector.CreateDatabaseForFirstTime function - uncomment to test
		//PlayerPrefs.DeleteKey("db_exists");

		// For testing DBConnector.UpdateLocalDatabase function - uncomment to test
		var oldDate = new System.DateTime(2016, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		var oldDateString = ConvertDateTimeToBinaryString(oldDate);
		PlayerPrefs.SetString("db_date", oldDateString);
		PlayerPrefs.Save();
	}

	public IEnumerator CheckForLocalDatabaseUpdates(SimpleSQL.SimpleSQLManager dbManager){

		if (db_exists == 0) 
		{
			// Start a transcation for the DB update
			Debug.Log("Creating new database from scratch.");
			dbManager.BeginTransaction();
			yield return new WaitForSeconds(1);

			// Get the information from the web API
			Debug.Log("Connecting to remote API.");
			questions = APIConnector.GetQuestions();
			yield return new WaitForSeconds(1);
			answers = APIConnector.GetAnswers();
			yield return new WaitForSeconds(1);
			estimotes = APIConnector.GetEstimotes();
			yield return new WaitForSeconds(1);
			exhibits = APIConnector.GetExhibits();
			yield return new WaitForSeconds(1);

			// Drop all tables if they exist
			DropAllTablesIfExist(dbManager);
			yield return new WaitForSeconds(1);
			
			// Recreate tables for a baseline
			CreateAllTablesForBaseline(dbManager);
			yield return new WaitForSeconds(1);
			
			// Insert data from remote MySQL database
			dbManager.InsertAll(questions);
			yield return new WaitForSeconds(1);
			dbManager.InsertAll(answers);
			yield return new WaitForSeconds(1);
			dbManager.InsertAll(estimotes);
			yield return new WaitForSeconds(1);
			dbManager.InsertAll(exhibits);
			yield return new WaitForSeconds(1);
			
			// Create indexes
			CreateIndexes(dbManager);
			yield return new WaitForSeconds(1);
			
			// Create FTS tables
			CreateSearchTable(dbManager);
			yield return new WaitForSeconds(1);
			
			// Commit current transcation
			dbManager.Commit();
			PlayerPrefs.SetInt("db_exists", 1);
			PlayerPrefs.SetString("db_date", currentDate_string);
			PlayerPrefs.Save();

		} 
		else if (db_exists == 1) 
		{
			var days = GetDBTimeDifference(currentDate, db_date);
			if (days > 1) 
			{
				// Start a transaction for the DB update
				Debug.Log("Updating existing database.");
				dbManager.BeginTransaction();
				yield return new WaitForSeconds(1);

				// Get the information from the web API
				Debug.Log("Connecting to remote API.");
				questions = APIConnector.GetQuestions();
				yield return new WaitForSeconds(1);
				answers = APIConnector.GetAnswers();
				yield return new WaitForSeconds(1);
				estimotes = APIConnector.GetEstimotes();
				yield return new WaitForSeconds(1);
				exhibits = APIConnector.GetExhibits();
				yield return new WaitForSeconds(1);

				// Delete current information
				DeleteAllCurrentData(dbManager);
				yield return new WaitForSeconds(1);
				
				// Download data from remote MySQL database
				dbManager.InsertAll(questions);
				yield return new WaitForSeconds(1);
				dbManager.InsertAll(answers);
				yield return new WaitForSeconds(1);
				dbManager.InsertAll(estimotes);
				yield return new WaitForSeconds(1);
				dbManager.InsertAll(exhibits);
				yield return new WaitForSeconds(1);
				
				// Create indexes
				CreateIndexes(dbManager);
				yield return new WaitForSeconds(1);
				
				// Create FTS tables
				CreateSearchTable(dbManager);
				yield return new WaitForSeconds(1);
				
				// Commit current transcation
				dbManager.Commit();
				PlayerPrefs.SetString("db_date", currentDate_string);
				PlayerPrefs.Save();
				
			}
			
		}

		yield return new WaitForSeconds(1);
		db_update = true;
	}

	public static void DeleteAllCurrentData(SimpleSQL.SimpleSQLManager dbManager) 
	{
		var sql_delete_question = "DELETE FROM Question";
		var sql_delete_answer = "DELETE FROM Answer";
		var sql_delete_estimote = "DELETE FROM Estimote";
		var sql_delete_exhibit = "DELETE FROM Exhibit";
		var sql_drop_question_search = "DROP TABLE IF EXISTS Question_search";
		var sql_drop_question_index = "DROP INDEX IF EXISTS Question_content";
		var sql_drop_answer_index = "DROP INDEX IF EXISTS Answer_content";

		dbManager.Execute (sql_delete_answer);
		dbManager.Execute (sql_delete_estimote);
		dbManager.Execute (sql_delete_exhibit);
		dbManager.Execute (sql_delete_question);
		dbManager.Execute (sql_drop_question_search);
		dbManager.Execute (sql_drop_question_index);
		dbManager.Execute (sql_drop_answer_index);
	}

	public static void DropAllTablesIfExist(SimpleSQL.SimpleSQLManager dbManager)
	{
		var sql_drop_answer = "DROP TABLE IF EXISTS Answer";
		var sql_drop_badge = "DROP TABLE IF EXISTS Badge";
		var sql_drop_estimote = "DROP TABLE IF EXISTS Estimote";
		var sql_drop_exhibit = "DROP TABLE IF EXISTS Exhibit";
		var sql_drop_question = "DROP TABLE IF EXISTS Question";
		var sql_drop_score = "DROP TABLE IF EXISTS Score";
		var sql_drop_scorecounter = "DROP TABLE IF EXISTS ScoreCounter";
		var sql_drop_unlock = "DROP TABLE IF EXISTS Unlock";
		var sql_drop_upgrade = "DROP TABLE IF EXISTS Upgrade";
		var sql_drop_question_search = "DROP TABLE IF EXISTS Question_search";
		var sql_drop_question_index = "DROP INDEX IF EXISTS Question_content";
		var sql_drop_answer_index = "DROP INDEX IF EXISTS Answer_content";
		
		dbManager.Execute (sql_drop_answer);
		dbManager.Execute (sql_drop_badge);
		dbManager.Execute (sql_drop_estimote);
		dbManager.Execute (sql_drop_exhibit);
		dbManager.Execute (sql_drop_question);
		dbManager.Execute (sql_drop_score);
		dbManager.Execute (sql_drop_scorecounter);
		dbManager.Execute (sql_drop_unlock);
		dbManager.Execute (sql_drop_upgrade);
		dbManager.Execute (sql_drop_question_search);
		dbManager.Execute (sql_drop_question_index);
		dbManager.Execute (sql_drop_answer_index);
	}

	public static void CreateAllTablesForBaseline(SimpleSQL.SimpleSQLManager dbManager)
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
	}

	public static void CreateIndexes(SimpleSQL.SimpleSQLManager dbManager)
	{
		var questionIndex = "CREATE INDEX Question_content ON Question(content_area)";
		var answerIndex = "CREATE INDEX Answer_content ON Answer(content_area)";

		//dbManager.Execute(questionIndex);
		//dbManager.Execute(answerIndex);
		
	}

	public static void CreateSearchTable(SimpleSQL.SimpleSQLManager dbManager)
	{
		var createTable = "CREATE VIRTUAL TABLE Question_search using fts4(question, question_es, content_area, aID)";
		var insertQuestions = "INSERT INTO Question_search(docid, question, question_es, content_area, aID) SELECT qID, question, question_es, content_area, aID FROM Question";

		dbManager.Execute(createTable);
		dbManager.Execute(insertQuestions);
	}

	public static void LoadOkapiExtension(SimpleSQL.SimpleSQLManager dbManger)
	{
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
}