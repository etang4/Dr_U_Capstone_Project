using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

	public class ResourceCounter : MonoBehaviour 
	{
		public Text counter;
		public static int count;

		// Use this for initialization
		void Start() {
			count = 0;
			counter = GetComponent<Text>();
		}

		// Update is called once per frame
		void Update () {
			counter.text = "$" + count.ToString ();
		}

		public static void addPoints(int points) {
			count += points;
		}
	}

