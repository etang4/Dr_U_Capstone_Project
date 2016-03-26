using SimpleSQL;

public class Answer {

	[PrimaryKey]
	public int aID { get; set; }

	public string answer { get; set; }

	public string answer_es { get; set; }

	public string source { get; set; }

	public string content_area { get; set; }

	public string expiration { get; set; }

	public int suggest_level { get; set; }

	public int locationID { get; set; }

	public int userID { get; set; }

	public string created { get; set; }

	public string modified { get; set; }
}