using UnityEngine;
using System;


public class MoreInfoImagesClose : MonoBehaviour
{

    public GameObject MoreInfoImagesPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoreInfoImagesPanel.SetActive(false);
        }
    }
}
