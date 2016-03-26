using SimpleSQL;

public class Unlock
{
	
	[PrimaryKey]
	public int unlockID { get; set; }
	
	public string name { get; set; }
	
	public string description { get; set; }
	
	public string iconFile { get; set; }
	
	public bool isActive { get; set; }

	public int exhibitID { get; set; }

}