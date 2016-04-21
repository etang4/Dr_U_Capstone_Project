/// <summary>
/// API connector provides basic functions to interact with the remote API.
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using UnityEngine;
using LitJson;

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
	public static Estimote[] GetEstimotes()
	{
		Estimote[] json;
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
				JsonMapper.RegisterImporter<string, int>(Int32.Parse);
				json = JsonMapper.ToObject<Estimote[]>(reader.ReadToEnd());
			}
		}

		return json;
	}

	// Get all the questions
	public static Question[] GetQuestions()
	{
		Question[] json;
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
				JsonMapper.RegisterImporter<string, int>(Int32.Parse);
				json = JsonMapper.ToObject<Question[]>(reader.ReadToEnd());
			}
		}
		
		return json;
	}

	// Get all the answers
	public static Answer[] GetAnswers()
	{
		Answer[] json;
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
				JsonMapper.RegisterImporter<string, int>(Int32.Parse);
				json = JsonMapper.ToObject<Answer[]>(reader.ReadToEnd());
			}
		}
		
		return json;
	}

	// Get all the exhibits
	public static Exhibit[] GetExhibits()
	{
		Exhibit[] json;
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
				JsonMapper.RegisterImporter<string, int>(Int32.Parse);
				json = JsonMapper.ToObject<Exhibit[]>(reader.ReadToEnd());
			}
		}
		
		return json;
	}

}