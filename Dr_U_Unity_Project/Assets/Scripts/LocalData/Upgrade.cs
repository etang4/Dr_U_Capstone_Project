using SimpleSQL;

public class Upgrade
{
	
	[PrimaryKey, AutoIncrement]
	public int upgradeID { get; set; }
	
	public string name { get; set; }

	public string description { get; set; }

	public string iconFile { get; set; }
	
	public int isActive { get; set; }

	public int price { get; set; }

	public int tracksCounterID { get; set; }

	public int effect { get; set; }
	
}