using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using UnityEngine;

public class APIConnector : MonoBehaviour {

    // This is the link for the Dr. Discovery online API (connecting to the MySQL database)
    private const string api = "https://dev.askdrdiscovery.org/dru/api/services/";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    // Check if device is connected to the internet
    public static bool CheckNetworkStatus()
    {
		try { 
			System.Net.NetworkInformation.Ping host_ping = new System.Net.NetworkInformation.Ping();
			string host = "google.com";
			byte[] buffer = new byte[32];
			int timeout = 1000;
			PingOptions pingOptions = new PingOptions();
			PingReply reply = host_ping.Send(host, timeout, buffer, pingOptions);
			return (reply.Status == IPStatus.Success);
		}
		catch (Exception) {
			return false;
		}
	}

	// Get all the extimotes
	public static string GetEstimotes()
	{
		string streamData;
		Auth myAuth = new Auth();
		string authInfo = myAuth.GetAuthentication();
		string authInfoEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(authInfo));
		WebRequest request = WebRequest.Create(api + "estimotes");
		request.ContentType = "application/json";
		request.Method = "GET";
		request.Headers["Authorization"] = "Basic " + authInfoEncoded;
		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) 
		{
			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet))) 
			{
				streamData = reader.ReadToEnd(); 
			}
		}

		return streamData;
	}

	// Get all the questions
	public static string GetQuestions()
	{
		string streamData;
		Auth myAuth = new Auth();
		string authInfo = myAuth.GetAuthentication();
		string authInfoEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(authInfo));
		WebRequest request = WebRequest.Create(api + "questions");
		request.ContentType = "application/json";
		request.Method = "GET";
		request.Headers["Authorization"] = "Basic " + authInfoEncoded;
		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) 
		{
			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet))) 
			{
				streamData = reader.ReadToEnd(); 
			}
		}
		
		return streamData;
	}

	// Get all the answers
	public static string GetAnswers()
	{
		string streamData;
		Auth myAuth = new Auth();
		string authInfo = myAuth.GetAuthentication();
		string authInfoEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(authInfo));
		WebRequest request = WebRequest.Create(api + "answers");
		request.ContentType = "application/json";
		request.Method = "GET";
		request.Headers["Authorization"] = "Basic " + authInfoEncoded;
		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) 
		{
			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet))) 
			{
				streamData = reader.ReadToEnd(); 
			}
		}
		
		return streamData;
	}

	// Get all the exhibits
	public static string GetExhibits()
	{
		string streamData;
		Auth myAuth = new Auth();
		string authInfo = myAuth.GetAuthentication();
		string authInfoEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(authInfo));
		WebRequest request = WebRequest.Create(api + "exhibits");
		request.ContentType = "application/json";
		request.Method = "GET";
		request.Headers["Authorization"] = "Basic " + authInfoEncoded;
		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) 
		{
			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet))) 
			{
				streamData = reader.ReadToEnd(); 
			}
		}
		
		return streamData;
	}

}