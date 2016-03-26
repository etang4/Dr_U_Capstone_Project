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
	
	public static int questionCounter;
	public static int stumpedCounter;
	public static int drUSavedCounter;
	public static int spaceshipLandedCounter;
	
	public const int drUSavedScore = 50;
	public const int spaceshipLandedScore = 100;
	
	public ResourceCounter resourceCounter;
	
	
	public static void addQuestionAsked(int num) {
		questionCounter += num;
	}
	
	public static int getQuestionCounter() {
		return questionCounter;
	}
	
	public static void addStumped(int num) {
		stumpedCounter += num;
	}
	
	public static int getStumpedCounter() {
		return stumpedCounter;
	}
	
	public static void addDrUSaved(int num) {
		drUSavedCounter += num;
	}
	
	public static int getDrUSavedCounter() {
		return drUSavedCounter;
	}
	
	public static void addSpaceShipLanded(int num) {
		spaceshipLandedCounter += num;
	}
	
	public static int getSpaceShipLandedCounter() {
		return spaceshipLandedCounter;
	}
	
	// Applies upgrades to get actual score at which Dr U is saved
	public static int getDrUSavedScore() {
		int score = drUSavedScore;
		score += UpgradePanel.getDrUUpgrade1 ();
		return score;
	}
	
	// Use this for initialization
	void Start()
	{
		//MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
		Debug.Log(MoreInfoImagePanel);
		
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