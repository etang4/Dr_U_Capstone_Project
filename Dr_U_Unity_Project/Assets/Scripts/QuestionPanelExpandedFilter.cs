using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestionPanelExpandedFilter : MonoBehaviour
{

    public GameObject originalButton;
    public GameObject[] itemsList;
    public int listSize;
    public GameObject[] items;
    public GameObject containerRect;
    public GameObject OmniSearchBarText;

    // Use this for initialization
    void Start()
    { 
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

    public void filterList()
    {
        string result = OmniSearchBarText.GetComponent<Text>().text;
        for (int i = 0; i < listSize; i++)
        {
            if(!itemsList[i].transform.GetChild(0).GetComponent<Text>().text.StartsWith(result))
            {
                itemsList[i].SetActive(false);
            }
            else
            {
                itemsList[i].SetActive(true);
            }
        }
    }
}

