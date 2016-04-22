using UnityEngine;
using System;


public class MoreInfoUnlocksClose : MonoBehaviour
{

    public GameObject MoreInfoUnlocksViewPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MoreInfoUnlocksViewPanel.SetActive(false);
        }
    }
}
