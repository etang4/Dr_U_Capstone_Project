using UnityEngine;
using System;


public class SettingsPanelClose : MonoBehaviour
{

    public GameObject MoreInfoUpgradesViewPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MoreInfoUpgradesViewPanel.SetActive(false);
        }
    }
}
