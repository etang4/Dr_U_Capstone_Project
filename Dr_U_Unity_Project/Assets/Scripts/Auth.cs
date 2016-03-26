using UnityEngine;
using System.Collections;

public class Auth
{
	private string auth;

	public Auth() 
	{
		auth = "drdisc:locolab"; 
	}

	// Authorization username and password for the api.
	public string GetAuthentication() 
	{
		return auth;
	}
}

