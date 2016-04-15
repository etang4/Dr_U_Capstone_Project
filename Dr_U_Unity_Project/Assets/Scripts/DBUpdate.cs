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
			Debug.Log("Creating new database from scratch.");
			CreateDatabaseForFirstTime(dbManager);
			PlayerPrefs.SetInt("db_exists", 1);
			PlayerPrefs.SetString("db_date", currentDate_string);
			PlayerPrefs.Save();
		} 
		else if (db_exists == 1) 
		{
			var days = GetDBTimeDifference(currentDate, db_date);
			if (days > 1) 
			{
				Debug.Log("Updating existing database.");
				UpdateLocalDatabase(dbManager);
				PlayerPrefs.SetString("db_date", currentDate_string);
				PlayerPrefs.Save();
			}
			
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnApplicationQuit() {

		// Uncomment the folowing pieces of code to test the CreateDatabase and UpdateDatabase functions

		// For testing DBConnector.CreateDatabaseForFirstTime function - uncomment to test
		//PlayerPrefs.DeleteKey("db_exists");

		// For testing DBConnector.UpdateLocalDatabase function - uncomment to test
		/*var oldDate = new System.DateTime(2016, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		var oldDateString = DBConnector.ConvertDateTimeToBinaryString(oldDate);
		PlayerPrefs.SetString("db_date", oldDateString);
		PlayerPrefs.Save();*/
	}

	// This method will update the local databse with the information from the remote MySQL database
	public static void UpdateLocalDatabase(SimpleSQL.SimpleSQLManager dbManager)
	{

		//Delete current information
		DeleteAllCurrentData(dbManager);

		// Download data from remote MySQL database
		GetNewDataFromRemoteDatabase(dbManager);
	}
	
	// This method is for setting a baseline for the local database. This should only be called on fresh installs once.
	public static void CreateDatabaseForFirstTime(SimpleSQL.SimpleSQLManager dbManager)
	{
		// Drop all tables if they exist
		DropAllTablesIfExist(dbManager);
		
		// Recreate tables for a baseline
		CreateAllTablesForBaseline(dbManager);

		// Download data from remote MySQL database
		GetNewDataFromRemoteDatabase(dbManager);
		
		// TODO: Set defaults for scoring system
	}

	public static void DeleteAllCurrentData(SimpleSQL.SimpleSQLManager dbManager) 
	{
		var sql_delete_question = "DELETE FROM Question";
		var sql_delete_answer = "DELETE FROM Answer";
		var sql_delete_estimote = "DELETE FROM Estimote";
		var sql_delete_exhibit = "DELETE FROM Exhibit";
		
		dbManager.Execute (sql_delete_answer);
		dbManager.Execute (sql_delete_estimote);
		dbManager.Execute (sql_delete_exhibit);
		dbManager.Execute (sql_delete_question);
	}

	public static void GetNewDataFromRemoteDatabase(SimpleSQL.SimpleSQLManager dbManager) 
	{
		// Get the information from the web API
		var questions = APIConnector.GetQuestions();
		var answers = APIConnector.GetAnswers();
		var estimotes = APIConnector.GetEstimotes();
		var exhibits = APIConnector.GetExhibits();
		
		// Insert information from the web API
		dbManager.InsertAll(questions);
		dbManager.InsertAll(answers);
		dbManager.InsertAll(estimotes);
		dbManager.InsertAll(exhibits);
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
		
		dbManager.Execute (sql_drop_answer);
		dbManager.Execute (sql_drop_badge);
		dbManager.Execute (sql_drop_estimote);
		dbManager.Execute (sql_drop_exhibit);
		dbManager.Execute (sql_drop_question);
		dbManager.Execute (sql_drop_score);
		dbManager.Execute (sql_drop_scorecounter);
		dbManager.Execute (sql_drop_unlock);
		dbManager.Execute (sql_drop_upgrade);
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