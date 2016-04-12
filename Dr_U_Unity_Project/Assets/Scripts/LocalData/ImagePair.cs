using SimpleSQL;

public class ImagePair
{
    [PrimaryKey]
    public string name { get; set; }

    [PrimaryKey]
    public string intro { get; set; }

    public string information { get; set; }

    public string image { get; set; }

    public int exhibitID { get; set; }

    public int estimoteID { get; set; }


}
