using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgePanel : MonoBehaviour
{
	public GameObject[] itemsList;
	public int listSize;
	public Sprite[] SetImages;
	public GameObject containerRect;
    public GameObject BadgesPanel;
	
	public GameObject MoreInfoBadgePanel;
	public GameObject[] badgeList;
	
	public static string[] badges = {  				// Make sure this is the same size as your list in Unity or you'll have an array out of bounds exception
		"Moon Badge: Earned for asking 1 question", 
		"Sun Badge: Earned for asking 5 questions",
		"Mercury Badge: Earned for asking 10 questions", 
		"Venus Badge: Earned for asking 15 questions",
		"Earth Badge: Earned for asking 20 questions",
		"Mars Badge: Earned for asking 25 questions",
		"Jupiter Badge: Earned for asking 30 questions", 
		"Saturn Badge: Earned for asking 35 questions", 
		"Uranus Badge: Earned for asking 40 questions", 
		"Neptune Badge: Earned for asking 45 questions", 
		"Pluto Badge: Earned for asking 50 questions", 
		"Galaxy Badge: Earned for asking 60 questions", 
	};	

	public static int[] pointsNeeded = { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 9999 }; // Make sure this is one item larger than your list in Unity or you'll have an array out of bounds exception
	
	// Use this for initialization
	void Start()
	{
		Debug.Log(MoreInfoBadgePanel);
		
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
			colors.pressedColor = Color.grey;
			colors.disabledColor = Color.grey;
			imageButton.GetComponent<Button> ().colors = colors;
			imageButton.interactable = false;
			imageButton.onClick.AddListener(() => { this.ActivateMoreInfoBadgePanel(imageUI); });

			//Sets newly created button to display list.
			itemsList[i] = newImage;
			newImage.transform.parent = this.transform;
		}
	}
    void Update()
    {
		checkBadges();
        if (Input.GetKey(KeyCode.Escape))
        {
            BadgesPanel.SetActive(false);
        }
    }

	//Event called onClick.
	public void ActivateMoreInfoBadgePanel(Image currentImg) //Needs parameter at some point to determine what image/data to show
	{
		int i = Extension.IntParseFast (currentImg.name);
		MoreInfoBadgePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
		MoreInfoBadgePanel.transform.GetChild (1).GetChild(0).GetComponent<Text> ().text = badges[i];
		MoreInfoBadgePanel.SetActive(true);
	}

	// Checks the specified badge to see whether it can be awarded or removed based on the number of questions asked, and updates the badge counter to watch the next badge in order
	public void checkBadges() {
		int badgeCount = PlayerPrefs.GetInt ("badgesCount");
		if (PlayerPrefs.GetInt ("questionsAsked") >= pointsNeeded [badgeCount]) {
			itemsList [badgeCount].GetComponent<Button> ().interactable = true;
			badgeCount++;
		}
		PlayerPrefs.SetInt ("badgesCount", badgeCount);
		PlayerPrefs.Save();
	}

	// Sets the specified badge number to active in the Badges Panel, making the badge button clickable and normal color
	// Also awards an upgrade point for earning the badge
	public void awardNewBadge(int badge) {
		itemsList [badge].GetComponent<Button>().interactable = true;
		int upgradePoints = PlayerPrefs.GetInt("upgradePoints");
		upgradePoints += 1;
		PlayerPrefs.SetInt ("upgradePoints", upgradePoints);
		PlayerPrefs.Save();

		//Image currentImg = itemsList [badge].GetComponent<Image> ();
		//BadgesButton.setButtonColor ();
	}

	// Resets all badges to uninteractable - be aware that checkBadges() is constantly running in Update(), which may set badges back to interactable if the player has enough questions Asked
	public void clearBadges() {
		PlayerPrefs.SetInt ("badgesCount", 0);
		for (int i = 0; i < badges.Length; i++) {
			itemsList [i].GetComponent<Button>().interactable = false;
		}
	}


		
}

public static class Extension
{
	public static int IntParseFast( this string s )
	{
		int r = 0;
		for (var i = 0; i < s.Length; i++)
		{
			char letter = s[i];
			r = 10 * r;
			r = r + (int)char.GetNumericValue(letter);
		}
		return r;
	}
}