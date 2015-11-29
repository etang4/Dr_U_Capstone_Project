using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BackScript : MonoBehaviour {
    public InputField SearchBarText;

    public void BackToMain()
    {
        SearchBarText.GetComponent<RectTransform>().offsetMin = new Vector2(12, 2);
    }
}
