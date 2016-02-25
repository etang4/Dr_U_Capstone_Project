using UnityEngine;
using System.Collections;

public class ImageScript : MonoBehaviour {
    GameObject MoreInfoImagePanel;

	// Use this for initialization
	void Start () {
        MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateMoreInfoImagePanel()
    {
        MoreInfoImagePanel.SetActive(true);
    }
}
