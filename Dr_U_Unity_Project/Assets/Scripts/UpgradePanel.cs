using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
	public GameObject[] itemsList;
	public int listSize;
	public Sprite[] SetImages;
	public GameObject containerRect;
    public GameObject UpgradesPanel;
    public GameObject MoreInfoUpgradesPanel;
	public GameObject MoreInfoUpgradePanel;

	private static string[] upgrades = {						// Make sure this is the same size as your list in Unity or you'll have an array out of bounds exception
		"Double your experience gained", 
		"Double your experience gained",
		"Double your experience gained", 
		"Double your experience gained",
		"Double your experience gained",
		"Double your experience gained",
	};	

    // Use this for initialization
    void Start()
    {
        //MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
        Debug.Log(MoreInfoUpgradePanel);

        //Create display list
        containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width / 6,
                                                                             containerRect.GetComponent<RectTransform>().rect.width / 6);
        containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x,
                                                                            containerRect.GetComponent<GridLayoutGroup>().cellSize.y * listSize);

        containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);

        for (int i = 0; i < listSize; i++)
        {
            //Instantiates buttons from image array.
            GameObject newImage = new GameObject("" + i);
            Image imageUI = newImage.AddComponent<Image>();
            imageUI.sprite = SetImages[i];
            Button imageButton = newImage.AddComponent<Button>();
            imageButton.targetGraphic = imageUI;
			var colors = imageButton.GetComponent<Button> ().colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = Color.grey;
			colors.pressedColor = Color.white;
			colors.disabledColor = Color.grey;
			imageButton.GetComponent<Button> ().colors = colors;
			//imageButton.interactable = false;
			imageButton.onClick.AddListener(() => { this.ActivateMoreInfoUpgradePanel(imageUI); });

            //Sets newly created button to display list.
            itemsList[i] = newImage;
            newImage.transform.parent = this.transform;
        }
    }

    public void Update()
    {
        if (!MoreInfoUpgradePanel.activeSelf){
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpgradesPanel.SetActive(false);
            }
    	}
    }

    //Event called onClick.
	public void ActivateMoreInfoUpgradePanel(Image currentImg) //Needs parameter at some point to determine what image/data to show
    {
		int i = Extension.IntParseFast (currentImg.name);
		MoreInfoUpgradePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
		MoreInfoUpgradePanel.transform.GetChild (1).GetChild(0).GetComponent<Text> ().text = upgrades[i];
        MoreInfoUpgradePanel.SetActive(true);
    }

	public void purchaseUpgrade() {
		int upgradePoints = PlayerPrefs.GetInt("upgradePoints");
		upgradePoints -= 1;
		PlayerPrefs.SetInt ("upgradePoints", upgradePoints);
		PlayerPrefs.Save();
	}

}

