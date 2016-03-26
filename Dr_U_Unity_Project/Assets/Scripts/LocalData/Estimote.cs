using SimpleSQL;

public class Estimote {

	[PrimaryKey]
	public int estimoteID { get; set; }

	public string name { get; set; }

	public string description { get; set; }

	public int major { get; set; }

	public int minor { get; set; }

}