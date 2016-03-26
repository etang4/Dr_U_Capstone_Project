using SimpleSQL;

public class Question 
{
	[PrimaryKey] 
	public int qID { get; set; }

	public string question { get; set; }

	public string question_es { get; set; }

	public int aID { get; set; }

	public string content_area { get; set; }

	public int suggest_level { get; set; }

	public int locationID { get; set; }

	public int userID { get; set; }

	public string created { get; set; }

	public string modified { get; set; }

}