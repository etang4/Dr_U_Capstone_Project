using UnityEngine;
using System;


public class MoreInfoBadgesClose : MonoBehaviour
{

    public GameObject MoreInfoBadgesViewPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MoreInfoBadgesViewPanel.SetActive(false);
        }
    }
}
