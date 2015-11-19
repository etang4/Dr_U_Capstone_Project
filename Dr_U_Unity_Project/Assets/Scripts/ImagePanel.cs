using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    public GameObject[] itemsList;
    public int listSize;
    public GameObject[] SetImages;
    public GameObject containerRect;

    // Use this for initialization
    void Start()
    { 
        containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.height, containerRect.GetComponent<RectTransform>().rect.height);
        containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<GridLayoutGroup>().cellSize.x * (listSize - 1), containerRect.GetComponent<RectTransform>().sizeDelta.y);

        containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
        //containerRect.GetComponent<RectTransform>().offsetMin = new Vector2(containerRect.GetComponent<RectTransform>().offsetMin.x, bottom);
        for (int i = 0; i < listSize; i++)
        {
            // GameObject newButton = Instantiate(itemsList[i]) as GameObject;
            GameObject newImage = Instantiate(SetImages[i]);
            itemsList[i] = newImage;
            newImage.transform.parent = this.transform;
        }
    }
}

