using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public GameObject[] itemsList;
    public int listSize;
    public Sprite[] SetImages;
    public GameObject containerRect;

    public GameObject MoreInfoImagePanel;

    // Use this for initialization
    void Start()
    {
        //MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
        Debug.Log(MoreInfoImagePanel);

        //Create display list
        containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width / 3,
                                                                             containerRect.GetComponent<RectTransform>().rect.width / 3);
        containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x,
                                                                            containerRect.GetComponent<GridLayoutGroup>().cellSize.y * listSize);

        containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);

        for (int i = 0; i < listSize; i++)
        {
            //Instantiates buttons from image array.
            GameObject newImage = new GameObject("Upgrades " + i);
            Image imageUI = newImage.AddComponent<Image>();
            imageUI.sprite = SetImages[i];
            Button imageButton = newImage.AddComponent<Button>();
            imageButton.targetGraphic = imageUI;
            imageButton.onClick.AddListener(() => { this.ActivateMoreInfoImagePanel(imageUI); });

            //Sets newly created button to display list.
            itemsList[i] = newImage;
            newImage.transform.parent = this.transform;
        }
    }

    //Event called onClick.
    public void ActivateMoreInfoImagePanel(Image currentImg) //Needs parameter at some point to determine what image/data to show
    {
        MoreInfoImagePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
        MoreInfoImagePanel.SetActive(true);
    }
}

