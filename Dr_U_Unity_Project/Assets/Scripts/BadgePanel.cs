using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgePanel : MonoBehaviour
{
	public GameObject[] itemsList;
	public int listSize;
	public Sprite[] SetImages;
	public GameObject containerRect;
	
	public GameObject MoreInfoImagePanel;
	public GameObject[] badgeList;
	
	
	
	// Use this for initialization
	void Start()
	{
		//MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
		Debug.Log(MoreInfoImagePanel);
		
		defineBadges ();
		
		
		//Create display list
		containerRect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(containerRect.GetComponent<RectTransform>().rect.width / 5,
		                                                                     containerRect.GetComponent<RectTransform>().rect.width / 5);
		containerRect.GetComponent<RectTransform>().sizeDelta = new Vector2(containerRect.GetComponent<RectTransform>().sizeDelta.x,
		                                                                    containerRect.GetComponent<GridLayoutGroup>().cellSize.y * listSize);
		
		containerRect.GetComponent<RectTransform>().offsetMax = new Vector2(containerRect.GetComponent<RectTransform>().offsetMax.x, 0);
		
		for (int i = 0; i < listSize; i++)
		{
			//Instantiates buttons from image array.
			GameObject newImage = new GameObject("Badge " + i);
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
	
	void defineBadges ()
	{
		throw new System.NotImplementedException ();
	}
	
	//Event called onClick.
	public void ActivateMoreInfoImagePanel(Image currentImg) //Needs parameter at some point to determine what image/data to show
	{
		MoreInfoImagePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
		MoreInfoImagePanel.SetActive(true);
	}
	/*	
	public void awardBadge(int badge) {
		if (ResourceCounter.getPoints () < BadgePanel.getDrUSavedScore) {
			Button imageButton = itemsList[1];
			Image imageUI = imageButton.targetGraphic;
			imageUI.
		}

	}

*/
	
	
}