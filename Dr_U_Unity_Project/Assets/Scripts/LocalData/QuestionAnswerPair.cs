using SimpleSQL;

public class QuestionAnswerPair
{
	[PrimaryKey] 
	public int qID { get; set; }

	[PrimaryKey]
	public int aID { get; set; }
	
	public string question { get; set; }

	public string question_es { get; set; }

	public string answer { get; set; }
	
	public string answer_es { get; set; }
	
}