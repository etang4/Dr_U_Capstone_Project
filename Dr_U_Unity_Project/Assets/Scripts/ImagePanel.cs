using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    public Sprite[] SetImages;     // This will be replaced with a database call
    public GameObject containerRect;

    public GameObject MoreInfoImagePanel;

    private int listSize;
    private GridLayoutGroup imageGrid;
    private RectTransform imageRect;

    // Use this for initialization
    void Start()
    {
        listSize = SetImages.Length;


        //MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
        Debug.Log(MoreInfoImagePanel);

        //Create display list
        imageGrid = containerRect.GetComponent<GridLayoutGroup>();
        imageRect = containerRect.GetComponent<RectTransform>();

        imageGrid.cellSize = new Vector2(imageRect.rect.height, imageRect.rect.height);
        imageRect.sizeDelta = new Vector2(imageRect.rect.width - ((imageGrid.cellSize.x + imageGrid.spacing.x) * listSize), imageRect.sizeDelta.y);
        imageRect.offsetMax = new Vector2(imageRect.offsetMax.x, 0);

        for (int i = 0; i < listSize; i++)
        {
            //Instantiates buttons from image array.
            GameObject newImage = new GameObject("Image " + i);
            Image imageUI = newImage.AddComponent<Image>();
            imageUI.sprite = SetImages[i];
            Button imageButton = newImage.AddComponent<Button>();
            imageButton.targetGraphic = imageUI;
            imageButton.onClick.AddListener(() => { this.ActivateMoreInfoImagePanel(imageUI); });

            //Sets newly created button to display list.
            newImage.transform.SetParent(imageGrid.transform);
        }
    }

    //Event called onClick.
    public void ActivateMoreInfoImagePanel(Image currentImg) //Needs parameter at some point to determine what image/data to show
    {
        MoreInfoImagePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
        MoreInfoImagePanel.SetActive(true);
    }
}

