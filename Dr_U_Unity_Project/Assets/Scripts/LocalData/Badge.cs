using SimpleSQL;

public class Badge
{
	
	[PrimaryKey]
	public int badgeID { get; set; }
	
	public string name { get; set; }
	
	public string description { get; set; }

	public string iconFile { get; set; }
	
	public bool isActive { get; set; }
	
	public int countNeeded { get; set; }
	
	public int tracksCounterID { get; set; }

	public int effect { get; set; }
}