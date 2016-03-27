using SimpleSQL;

public class Unlock
{
	
	[PrimaryKey, AutoIncrement]
	public int unlockID { get; set; }
	
	public string name { get; set; }
	
	public string description { get; set; }
	
	public string iconFile { get; set; }
	
	public int isActive { get; set; }

	public int exhibitID { get; set; }

}