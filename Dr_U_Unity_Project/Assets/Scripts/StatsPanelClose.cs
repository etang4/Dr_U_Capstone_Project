﻿using UnityEngine;
using System.Collections;

public class StatsPanelClose : MonoBehaviour {

    public GameObject StatsPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            StatsPanel.SetActive(false);
        }
    }
}