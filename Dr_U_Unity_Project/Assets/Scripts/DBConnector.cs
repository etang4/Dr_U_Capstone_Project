using UnityEngine;
using UnityEngineInternal;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleSQL;

public class DBConnector : MonoBehaviour {

	public SimpleSQL.SimpleSQLManager dbManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<Answer> SelectAnswers() {
		var temp = new List<Answer> (from answer in dbManager.Table<Answer> () select answer);

		return temp; 
	}

}