using SimpleSQL;

public class ScoreCounter
{
	
	[PrimaryKey, AutoIncrement]
	public int counterID { get; set; }
	
	public string type { get; set; }
	
	public int defaultCount { get; set; }

}