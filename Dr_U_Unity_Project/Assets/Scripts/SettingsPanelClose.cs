using UnityEngine;
using System;


public class SettingsPanelClose : MonoBehaviour {

    public GameObject SettingsViewPanel;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsViewPanel.SetActive(false);
        }
	}
}
