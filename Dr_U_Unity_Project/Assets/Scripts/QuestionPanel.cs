using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public GameObject originalButton;
    public GameObject[] itemsList;
    public int listSize;
    public GameObject[] items;
    public GameObject containerRect;

    // Use this for initialization
    void Start()
    { 
		// TODO TEMP
		Debug.Log("starting QuestionPanel...");
	
        containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width , containerRect.GetComponent<RectTransform>().rect.height / listSize);
        containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x, containerRect.GetComponent<GridLayoutGroup>().cellSize.y * listSize);

        containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        //containerRect.GetComponent<RectTransform>().offsetMin = new Vector2(containerRect.GetComponent<RectTransform>().offsetMin.x, bottom);
        for (int i = 0; i < listSize; i++)
        {
            // GameObject newButton = Instantiate(itemsList[i]) as GameObject;
            GameObject newButton = Instantiate(originalButton);
            itemsList[i] = newButton;
            newButton.transform.GetChild(0).GetComponent<Text>().text = "Frequently Asked Question #" + i + "?";
            newButton.transform.parent = this.transform;
        }
    }
}

