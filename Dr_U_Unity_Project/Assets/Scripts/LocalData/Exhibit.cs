using SimpleSQL;

public class Exhibit {

	[PrimaryKey]
	public int exhibitID { get; set; }

	public int estimoteID { get; set; }

	public string name { get; set; }

	public string intro { get; set; }

	public string information { get; set; }

	public string image { get; set; }

}