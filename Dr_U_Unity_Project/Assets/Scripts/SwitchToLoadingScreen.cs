using UnityEngine;
using System.Collections;

public class SwitchToLoadingScreen : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SwitchLoadingScreen() {

		var oldDate = new System.DateTime(2016, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		var oldDateString = DBUpdate.ConvertDateTimeToBinaryString(oldDate);
		PlayerPrefs.SetString("db_date", oldDateString);
		PlayerPrefs.Save();
		Application.LoadLevel ("Loading_Screen");

	}
}

