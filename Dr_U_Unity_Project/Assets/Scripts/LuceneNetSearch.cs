using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

public class LuceneNetSearch : MonoBehaviour
{
	public string LastQuestion = "";
	public string LastAnswer = "";
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void SearchOnClick ()
	{
		GameObject inputFieldGo = GameObject.Find ("SearchBar");
		InputField inputFieldCo = inputFieldGo.GetComponent<InputField> ();
		UnityEngine.Debug.Log (inputFieldCo.text);

		DoSearch (inputFieldCo.text);  

	}

	public void DoSearch (string query)
	{

		var response = SearchForAnswer (query, 0);
		LastQuestion = response.question;
		LastAnswer = response.answer;

		UnityEngine.Debug.Log ("QUESTION:  " + response.question);
		UnityEngine.Debug.Log ("ANSWER:  " + response.answer);

	}

	private static string PathCombineSimple (string path, string addition)
	{
		return path + "/" + addition;
	}

	//
	public static void BuildIndex ()
	{

		UnityEngine.Debug.Log ("Building Index");
		string supDir = "/tmp";

		var qIndexPath = PathCombineSimple (supDir, "questions");

		UnityEngine.Debug.Log (qIndexPath);

		//Lists to store objects
		List<Question> questionList = new List<Question> (APIConnector.GetQuestions ());
		List<Answer> answerList = new List<Answer> (APIConnector.GetAnswers ());
		List<Estimote> estimoteList = new List<Estimote> (APIConnector.GetEstimotes ());
		List<Exhibit> exhibitList = new List<Exhibit> (APIConnector.GetExhibits ());

		UnityEngine.Debug.Log (qIndexPath);

		//Setting up lucene analyzers
		var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
		var indexDirectory = FSDirectory.Open (qIndexPath);
		var writer = new IndexWriter (indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);




		UnityEngine.Debug.Log ("Indexing DB");
		//Place each object in list in document for indexing
		foreach (var row in questionList) {
			//Create new document
			var doc = new Document ();
			//Create fields in the document based on database tables
			doc.Add (new Field ("qID", row.qID.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("aID", row.aID.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("question", row.question, Field.Store.YES, Field.Index.ANALYZED));
			doc.Add (new Field ("question_es", row.question_es, Field.Store.YES, Field.Index.ANALYZED));
			doc.Add (new Field ("content_area", row.content_area, Field.Store.YES, Field.Index.ANALYZED));
			doc.Add (new Field ("locationID", row.locationID.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("suggest_level", row.suggest_level.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			
			
			//Commit document
			writer.AddDocument (doc);
		}
		
		//Commit and store indexes into directory
		writer.Optimize ();
		writer.Commit ();
		writer.Dispose ();
		
		//Dispose index directory
		indexDirectory.Dispose ();

		//Same process as above
		var aIndexPath = PathCombineSimple (
			supDir,
			"answers"
		);
		
		indexDirectory = FSDirectory.Open (aIndexPath);
		writer = new IndexWriter (indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

		foreach (var row in answerList) {
			var doc = new Document ();
			doc.Add (new Field ("aID", row.aID.ToString ("D"), Field.Store.YES, Field.Index.ANALYZED));
			doc.Add (new Field ("answer", row.answer, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("answer_es", row.answer_es, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("source", row.source, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("content_area", row.content_area, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("expiration", row.expiration, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("suggest_level", row.suggest_level.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("locationID", row.locationID.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			writer.AddDocument (doc);
		}
		
		writer.Optimize ();
		writer.Commit ();
		writer.Dispose ();
		indexDirectory.Dispose ();

		//Same process as above
		var eIndexPath = PathCombineSimple (
			supDir,
			"estimotes"
		);
		
		
		indexDirectory = FSDirectory.Open (eIndexPath);
		writer = new IndexWriter (indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

		foreach (var row in estimoteList) {
			var doc = new Document ();
			doc.Add (new Field ("id", row.estimoteID.ToString ("D"), Field.Store.YES, Field.Index.ANALYZED));
			doc.Add (new Field ("name", row.name, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("description", row.description, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("major", row.major.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("minor", row.minor.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			writer.AddDocument (doc);
		}
		
		writer.Optimize ();
		writer.Commit ();
		writer.Dispose ();
		indexDirectory.Dispose ();

		//Same process as above
		var exIndexPath = PathCombineSimple (
			supDir,
			"exhibits"
		);
		
		
		indexDirectory = FSDirectory.Open (exIndexPath);
		writer = new IndexWriter (indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
		foreach (var row in exhibitList) {
			var doc = new Document ();
			doc.Add (new Field ("id", row.exhibitID.ToString ("D"), Field.Store.YES, Field.Index.NO));
			doc.Add (new Field ("estimote_id", row.estimoteID.ToString ("D"), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("name", row.name, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add (new Field ("intro", row.intro, Field.Store.YES, Field.Index.NO));
			doc.Add (new Field ("information", row.information, Field.Store.YES, Field.Index.NO));
			doc.Add (new Field ("image", row.image, Field.Store.YES, Field.Index.NO));
			writer.AddDocument (doc);

		}
		
		writer.Optimize ();
		writer.Commit ();
		writer.Dispose ();
		indexDirectory.Dispose ();


		UnityEngine.Debug.Log ("Building Index Finished");
	}


	/// <summary>
	/// Searches for proper question
	/// </summary>
	/// <param name="query">Query to match question to</param>
	/// <param name="limit">Maximum number of results to return</param>
	/// <returns></returns>
	private static List<Question> GetQuestionForQuery (Query query, int limit)
	{
		//Setting up list and file directory
		var data = new List<Question> ();
		var supDir = "/tmp";
		
		
		//Get index directory
		var indexPath = PathCombineSimple (
			supDir,
			"questions"
		);
		
		//Open directory and search using the query to find proper result
		var indexDirectory = FSDirectory.Open (indexPath);
		
		//If no directory then return null
		if (!IndexReader.IndexExists (indexDirectory))
			return null;
		
		using (var searcher = new IndexSearcher(indexDirectory)) {
			var hits = searcher.Search (query, limit);
			UnityEngine.Debug.Log (hits.TotalHits + " result(s) found for query: " + query);
			
			foreach (var scoreDoc in hits.ScoreDocs) {
				//UnityEngine.Debug.Log(hits.TotalHits + " Score: " + scoreDoc.ToString());
				var document = searcher.Doc (scoreDoc.Doc);
				data.Add (new Question ()
				         {
					qID = int.Parse(document.Get("qID")),
					aID = int.Parse(document.Get("aID")),
					question = document.Get("question"),
					question_es = document.Get("question_es"),
					content_area = document.Get("content_area"),
					suggest_level = int.Parse(document.Get("suggest_level")),
					locationID = int.Parse(document.Get("locationID"))
				});
				//var temp = searcher.Explain (query, scoreDoc.Doc);
				//UnityEngine.Debug.Log ("explain: " + temp);
			}
		}
		indexDirectory.Dispose ();
		return data;

	}


	/// <summary>
	/// Searches for proper answer
	/// </summary>
	/// <param name="query">Query to match answer to</param>
	/// <param name="limit">Maximum number of results</param>
	/// <returns></returns>
	private static List<Answer> GetAnswerForQuery (Query query, int limit)
	{
		//List and file directory
		var data = new List<Answer> ();
		var supDir = "/tmp";

		//Directory of index
		var indexPath = PathCombineSimple (
			supDir,
			"answers"
		);
		
		//Open directory
		var indexDirectory = FSDirectory.Open (indexPath);
		
		//If no directory then return null
		if (!IndexReader.IndexExists (indexDirectory))
			return null;
		
		//Search index for hits of query
		using (var searcher = new IndexSearcher(indexDirectory)) {
			var hits = searcher.Search (query, limit);
			UnityEngine.Debug.Log (hits.TotalHits + " result(s) found for query: " + query.ToString ());
			foreach (var scoreDoc in hits.ScoreDocs) {
				var document = searcher.Doc (scoreDoc.Doc);
				data.Add (new Answer ()
				         {
					aID = int.Parse(document.Get("aID")),
					answer = document.Get("answer"),
					answer_es = document.Get("answer_es"),
					source = document.Get("source"),
					content_area = document.Get("content_area"),
					expiration = document.Get("expiration"),
					suggest_level =int.Parse(document.Get("suggest_level"))
				});
			}
		}
		indexDirectory.Dispose ();
		return data;
	}

	/// <summary>
	/// Function other classes use to search index for proper answer
	/// </summary>
	/// <param name="query">Text input of question</param>
	/// <param name="isEsp">Return spanish=1 or english=0 answer</param>
	/// <returns>Response object</returns>
	public static Response SearchForAnswer (string query, int isEsp)
	{
		UnityEngine.Debug.Log ("Searching For Answer");

		if (!CheckFolders ()) {
			BuildIndex ();
		}
		const int suggest_level = 1;
		
		if (query.Equals ("")) {
			return isEsp == 0 ? new Response (query, -1, -1, -1, Response.NoQuestionEntered, -1, -1, -1, -1, -1, 1) : new Response (query, -1, -1, -1, Response.NoQuestionEntered_es, -1, -1, -1, -1, -1, 1);
		}
		
		//Remove ? from query string and symbols.
		//Spanish support needed here because this will remove the special characters.
		//Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		query = Regex.Replace (query, "[^a-zA-Z0-9áéíñóúüÁÉÍÑÓÚÜ'.]+", " ");
		query = query.Replace ("?", string.Empty);
		
		//Set up analyzer and parser
		var analyzer = new StandardAnalyzer (Lucene.Net.Util.Version.LUCENE_30);
		
		var parser = isEsp == 0 ? new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "question", analyzer){ DefaultOperator = QueryParser.AND_OPERATOR } : new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "question_es", analyzer){ DefaultOperator = QueryParser.AND_OPERATOR };
		
		//Set up parser
		var initParse = parser.Parse (query);
		
		//Gets list of questions with hits
		var questionList = GetQuestionForQuery (initParse, 3);
		
		//For outputting questions if needed 
		for (int i=0; i<questionList.Count; i++) {
			UnityEngine.Debug.Log (questionList.ElementAt (i).question.ToString ());
		}
		
		//Check if the list contains anything
		var aID = questionList.Any () ? questionList.First ().aID : -1;
		var qID = questionList.Any () ? questionList.First ().qID : -1;
		
		if (aID == -1 || questionList.Count () > 1) { /* If no aID found, need to try the OR operator once more and return a list of 1-3 possible questons. */
			UnityEngine.Debug.Log ("No perfect answer- try OR\n");
			parser = isEsp == 0 ? new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "question", analyzer){ DefaultOperator = QueryParser.OR_OPERATOR } : new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "question_es", analyzer){ DefaultOperator = QueryParser.OR_OPERATOR };
			initParse = parser.Parse (query);
			questionList = GetQuestionForQuery (initParse, 3);
			
			UnityEngine.Debug.Log ("\nOR Matches QList");
			for (int i=0; i<questionList.Count; i++) {
				UnityEngine.Debug.Log (questionList.ElementAt (i).question.ToString ());
			}
			//Check if the list contains anything
			aID = questionList.Any () ? questionList.First ().aID : -1;
			qID = questionList.Any () ? questionList.First ().qID : -1;
			
			//If aID is -1 again, that means no OR match either. 
			if (aID == -1) { 	
				//No matching question found, so give them a random fact.
				parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "suggest_level", analyzer);
				initParse = parser.Parse (suggest_level.ToString ("D"));
				var ans_w_suggestlevel = GetAnswerForQuery (initParse, 50);
				var trueAns_f = ans_w_suggestlevel [(new System.Random ()).Next (0, ans_w_suggestlevel.Count)];
				//public Response		(string q, int qId, int qIdSug1, int qIdSug2, string a, int aId, int tier, double queryScore,  double queryScoreSug1,  double queryScoreSug2, int organicFlag)
				return isEsp == 0 ? new Response (query, qID, -1, -1, Response.NoAnswer + trueAns_f.answer, -1, -1, -1, -1, -1, 1) : new Response (query, qID, -1, -1, Response.NoAnswer_es + trueAns_f.answer_es, -1, -1, -1, -1, -1, 1);
			}
		} else { /* We found a perfect match! Now grab the answer for the right aID */ 
			UnityEngine.Debug.Log ("FOUND PERFECT MATCH WITHOUT TIER. AID IS: " + aID);
			parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "aID", analyzer);
			initParse = parser.Parse (aID.ToString ("D"));
			var ans_f = GetAnswerForQuery (initParse, 50);
			
			var trueAns_f = ans_f.FirstOrDefault ();
			
			//public					 Response  (string q, int qId, int qIdSug1, int qIdSug2, string a, int aId, int tier, double queryScore,  double queryScoreSug1,  double queryScoreSug2, int organicFlag)
			return trueAns_f != null ? new Response (query, qID, -1, -1, isEsp == 0 ? trueAns_f.answer : trueAns_f.answer_es, trueAns_f.aID, 1, 1, -1, -1, 1) 
				: new Response (query, qID, -1, -1, isEsp == 0 ? Response.NoAnswer : Response.NoAnswer_es, -1, -1, -1, -1, -1, 1);
		}
		
		
		/* If we have reached this point, then we need to do Judd's matching algo */
		
		/* For QUERY: Splitting, counting length, and counting words */
		// Split query into list of strings
		var words_split_in = query.ToLower ().Split (' ').ToList ();
		// get length of query
		int length_in = query.Length;
		// get number of words in question
		int wordNum_in = GetWordCount (query);
		/*************************************************************/
		
		/* For MATCHES: Splitting, counting length, and counting words */
		// Declare some useful containers.
		var length_matches = new List<int> ();
		var wordNum_matches = new List<int> ();
		var matches_dict = new Dictionary<string, List<string>> ();
		
		// Add the matches to a dictionary with sug# as the key. For the
		// values in the dictionary, we split the matches by whitespace.
		// Also Get the length and numbers of words in each match, then add it 
		// to a corresponding list.
		int sugnum = 0;
		foreach (var match in questionList) {
			//Split matches into list of strings. First lowercase, remove question mark.
			matches_dict.Add ("sug" + sugnum.ToString (), isEsp == 0 ? match.question.Replace ("?", string.Empty).ToLower ().Split (' ').ToList () 
			                 : match.question_es.Replace ("?", string.Empty).ToLower ().Split (' ').ToList ());
			//get length of matches
			length_matches.Add (match.question.Length);
			//get number of words in matches
			wordNum_matches.Add (GetWordCount (match.question));
			//increment the suggestion count for the key 
			sugnum++;
		}
		
		//foreach (var temp in matches_dict)
		//	UnityEngine.Debug.Log ("key: "+ temp.Key + " val: " + string.Join(",",temp.Value)); 
		
		//UnityEngine.Debug.Log ("SUGNUM: " +sugnum + "  words_split_input: " + string.Join(",",words_split_in)); 
		/*************************************************************/
		
		/* Now setup and get each metric for every matched question */
		// Declare lists for the metrics.
		var metric1_list = new List<double> ();
		var metric2_list = new List<double> ();
		var metric3_list = new List<double> ();
		var metric4_list = new List<double> ();
		var total_list = new List<double> ();
		
		int j = 0;
		//Metric 1: Fraction of the words from the typed question found in a matched question.
		foreach (var qMatch in matches_dict) {
			var queryToMatch_intersect = words_split_in.Select (i => i.ToString ()).Intersect (qMatch.Value);
			metric1_list.Add (queryToMatch_intersect.Count () / (double)wordNum_in);
			//UnityEngine.Debug.Log ("Match: " + j + "==> NumIntersectingWords: "+  queryToMatch_intersect.Count()+" Metric 1: " + metric1_list.ElementAt(j));
			j++;
		}
		//UnityEngine.Debug.Log ("\n");
		//Metric 2: Fraction of the words for a matched question found in the typed question
		j = 0;
		int assoc_matchCount = 0;
		foreach (var qMatch in matches_dict) {
			//Iterate through each row in the dictionary and do an associative intersect.
			// Have to check which is longer, the matched question or the user query (to prevent OutOfBound errors)
			for (int z = 0; z < ((wordNum_in<qMatch.Value.Count())? wordNum_in: qMatch.Value.Count()); z++) {
				if (words_split_in.ElementAt (z).CompareTo (qMatch.Value.ElementAt (z)) == 0) {
					//UnityEngine.Debug.Log ("word match! : " + qMatch.Value.ElementAt (z));
					assoc_matchCount++;
				}
			}
			metric2_list.Add (assoc_matchCount / (double)wordNum_in);
			//UnityEngine.Debug.Log ("Match: " + j + "==> NumAssocIntersectingWords: " + assoc_matchCount+" Metric 2: " + metric2_list.ElementAt(j));
			assoc_matchCount = 0;
			j++;
		}
		//UnityEngine.Debug.Log ("\n");
		
		//Metric 3: Same as Metric 1, but running from the start of the typed sentence and stopping as soon as a difference is encountered.  
		// So this score is less than or equal to Metric 1 always.		
		j = 0;
		foreach (var qMatch in matches_dict) {
			//var queryToMatch_intersect2 = qMatch.Value.Select (i => i.ToString ()).Intersect (words_split_in);
			var queryToMatch_intersect2 = words_split_in.Select (i => i.ToString ()).Intersect (qMatch.Value);
			metric3_list.Add (queryToMatch_intersect2.Count () / (double)qMatch.Value.Count ());
			//UnityEngine.Debug.Log ("Match: " + j + "==> REVERSE NumIntersectingWords: "+  queryToMatch_intersect2.Count()+" Metric 3: " + metric3_list.ElementAt(j));
			j++;
		}
		//UnityEngine.Debug.Log ("\n");
		
		//Metric 4: Same Metric 2, but running from the start of the matched sentence and stopping as soon as a difference is encountered.  
		//So this score is less than or equal to Metric 2 always.
		j = 0;
		assoc_matchCount = 0;
		foreach (var qMatch in matches_dict) {
			//Iterate through each row in the dictionary and do an associative intersect.
			// Have to check which is longer, the matched question or the user query (to prevent OutOfBound errors)
			for (int z = 0; z < ((wordNum_in<qMatch.Value.Count())? wordNum_in: qMatch.Value.Count()); z++) {
				if (words_split_in.ElementAt (z).CompareTo (qMatch.Value.ElementAt (z)) == 0) {
					assoc_matchCount++;
				}
			}
			metric4_list.Add (assoc_matchCount / (double)qMatch.Value.Count ());
			//UnityEngine.Debug.Log ("Match: " + j + "==> REVERSE NumAssocIntersectingWords: " + assoc_matchCount+" Metric 4: " + metric4_list.ElementAt(j));
			assoc_matchCount = 0;
			j++;
		}
		//UnityEngine.Debug.Log ("\n");
		
		var winners_dict = new Dictionary<int, double> ();
		
		/* Get highest score and index of winner */
		for (int i=0; i<matches_dict.Count(); i++) {
			winners_dict.Add (i, ((metric1_list.ElementAt (i)
				+ metric2_list.ElementAt (i)
				+ metric3_list.ElementAt (i)
				+ metric4_list.ElementAt (i)
			                      ) / 4.0));
		}
		//foreach (var val in winners_dict)
		//	UnityEngine.Debug.Log ("Key: " + val.Key  + " SCORE : " + val.Value);
		
		
		/* Create and sort a dictionary with the scores and keys (0, 1, or 2) to find out which of the three question objects "win".*/
		var winner = winners_dict.Values.Max ();
		var sortedDict = from entry in winners_dict orderby entry.Value descending select entry;
		var winnerKey = sortedDict.First ().Key;
		
		//Declare safe variables and make sure we don't have OOB error.
		int secondPlaceKey, thirdPlaceKey;
		double secondPlaceScore, thirdPlaceScore;
		
		if (sugnum >= 2) {
			secondPlaceKey = sortedDict.ElementAt (1).Key;
			secondPlaceScore = sortedDict.ElementAt (1).Value;
		} else {
			secondPlaceKey = -1;
			secondPlaceScore = 0.0;
		}
		if (sugnum >= 3) {
			thirdPlaceKey = sortedDict.ElementAt (2).Key;
			thirdPlaceScore = sortedDict.ElementAt (2).Key;
		} else {
			thirdPlaceKey = -1;
			thirdPlaceScore = 0.0;
		}
		//foreach (var temp in sortedDict)
		//	UnityEngine.Debug.Log ("key:"+ temp.Key + "val: " + temp.Value);
		
		//UnityEngine.Debug.Log ("WinerKey:"+ winnerKey + " SecondPlaceKey: " + secondPlaceKey + " ThirdPKey: " + thirdPlaceKey);
		
		
		//Assign appropriate matched element to aID for retreiving below.
		if (winnerKey == 0)
			aID = questionList.First ().aID;
		else if (winnerKey == 1)
			aID = questionList.ElementAt (1).aID;
		else if (winnerKey == 2)
			aID = questionList.ElementAt (2).aID;
		
		/* If winner is equal to 1.0, we have a TIER ONE match, so do nothing until a response is returned after the if/else loop */
		if (winner == (double)1) {
		} 
		
		/* If winner is greater than 0.8, we have a TIER TWO match, so return a response with the correct params. 
			 *  Returns:    Response(string q, int qId, int qIdSug1, int qIdSug2, 
			 *              string a, int aId, 
			 *				int tier, double queryScore,  double queryScoreSug1,  double queryScoreSug2, int organicFlag) */
		else if (winner > 0.8) {
			parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "aID", analyzer);
			initParse = parser.Parse (aID.ToString ("D"));
			var ans_f = GetAnswerForQuery (initParse, 50);
			var trueAns_f = ans_f.FirstOrDefault ();
			
			return trueAns_f != null ? new Response (query, questionList.ElementAt (winnerKey).qID, -1, -1, 
			                                        isEsp == 0 ? Response.tierTwo + "\"" + questionList.ElementAt (winnerKey).question + "\"" + ", the answer is: " + trueAns_f.answer 
			                                        : Response.tierTwo_es + "\"" + questionList.ElementAt (winnerKey).question_es + "\"" + ", la respuesta es: " + trueAns_f.answer_es, 
			                                        trueAns_f.aID,
			                                        2, winner, -1, -1, 1)
				: new Response (query, questionList.ElementAt (winnerKey).qID, -1, -1, 
				               isEsp == 0 ? Response.NoAnswer : Response.NoAnswer_es, -1, 2, winner, (double)-1, (double)-1, 1);
			
			/* If winner is greater than 0.45, we have a TIER THREE match, so return a response with the correct parameters.
			 * Returns:     Response(string q, int qId, int qIdSug1, int qIdSug2, 
			 *              string a, int aId, 
			 *				int tier, double queryScore,  double queryScoreSug1,  double queryScoreSug2, int organicFlag) */
		} else if (winner >= .45) { 
			// This is a hotfix for duplicate questions being given for Tier Three.
			if (questionList.ElementAt (winnerKey).question.Equals (questionList.ElementAt (secondPlaceKey).question)) {
				secondPlaceKey = thirdPlaceKey;
			}
			return new Response (query, -1, questionList.ElementAt (winnerKey).qID, questionList.ElementAt (secondPlaceKey).qID, 
			                     isEsp == 0 ? Response.tierThree + "#<sug1>\"" + questionList.ElementAt (winnerKey).question + "\"#" 
				+ "#<sug2>\"" + questionList.ElementAt (secondPlaceKey).question + "\"#"
			                     : Response.tierThree_es + "#<sug1>\"" + questionList.ElementAt (winnerKey).question_es + "\"#" 
				+ "#<sug2>\"" + questionList.ElementAt (secondPlaceKey).question_es + "\"#",
			                     -1, 3, (double)-1, winner, secondPlaceScore, 1);
			
			/* If winner does not meet above conditions it is a TIER FOUR response, so return a response with the correct parameters.*/
		} else {
			//No matching question found, so give them a random fact.
			parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "suggest_level", analyzer);
			initParse = parser.Parse (suggest_level.ToString ("D"));
			var ans_w_suggestlevel = GetAnswerForQuery (initParse, 50);
			var trueAns_f = ans_w_suggestlevel [(new System.Random ()).Next (0, ans_w_suggestlevel.Count)];
			//public Response		(string q, int qId, int qIdSug1, int qIdSug2, string a, int aId, int tier, double queryScore,  double queryScoreSug1,  double queryScoreSug2, int organicFlag)
			return isEsp == 0 ? new Response (query, -1, questionList.ElementAt (winnerKey).qID, questionList.ElementAt (secondPlaceKey).qID, 
			                                  Response.tierFour + trueAns_f.answer, -1, 4, -1, winner, secondPlaceScore, 1) 
				: new Response (query, -1, questionList.ElementAt (winnerKey).qID, questionList.ElementAt (secondPlaceKey).qID, 
				                Response.tierFour_es + trueAns_f.answer_es, -1, 4, -1, winner, secondPlaceScore, 1);
		}
		
		//If it makes it to this point it is TIER ONE.
		parser = new QueryParser (Lucene.Net.Util.Version.LUCENE_30, "aID", analyzer);
		initParse = parser.Parse (aID.ToString ("D"));
		var ans = GetAnswerForQuery (initParse, 50);
		
		var trueAns = ans.FirstOrDefault ();
		return trueAns != null ? new Response (query, qID, -1, -1, isEsp == 0 ? trueAns.answer : trueAns.answer_es, trueAns.aID, 1, winner, -1, -1, 1) 
			: new Response (query, -1, -1, -1, isEsp == 0 ? Response.NoAnswer : Response.NoAnswer_es, -1, 4, winner, -1, -1, 1); //Need to check that the tier at this point makes sense.
		

	}

	/// <summary>
	/// Get word count
	/// </summary>
	/// <returns>Int</returns>
	public static int GetWordCount (String input)
	{
		//Count number of words in query.
		String text = input.Trim ();
		int wordCount = 0, index = 0;
		
		while (index < text.Length) {
			// check if current char is part of a word
			while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
				index++;
			
			wordCount++;
			
			// skip whitespace until next word
			while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
				index++;
		}
		return wordCount;
	}
	
	/// <summary>
	/// Check to see if index folders exist
	/// </summary>
	/// <returns>Boolean</returns>
	public static bool CheckFolders ()
	{
		var supDir = "/tmp";
		return  System.IO.Directory.Exists (PathCombineSimple (supDir, "questions")) &&
			System.IO.Directory.Exists (PathCombineSimple (supDir, "answers")) && 
			System.IO.Directory.Exists (PathCombineSimple (supDir, "estimotes")) && 
			System.IO.Directory.Exists (PathCombineSimple (supDir, "exhibits"));
	}
}

