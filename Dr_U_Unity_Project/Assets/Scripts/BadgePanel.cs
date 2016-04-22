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
	
	private static string[] badges = {  				// Make sure this is the same size as your list in Unity or you'll have an array out of bounds exception
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
	
	// Use this for initialization
	void Start()
	{
		//MoreInfoBadgePanel = GameObject.Find("MoreInfoBadgePanel Need to dynamically find this gameobject eventually.
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
			imageButton.onClick.AddListener(() => { this.ActivateMoreInfoBadgePanel(imageUI); });

			//Sets newly created button to display list.
			itemsList[i] = newImage;
			newImage.transform.parent = this.transform;
		}
	}
    void Update()
    {
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

/*	public void awardBadge(int badge) {
		if (ResourceCounter.getPoints () < BadgePanel.getDrUSavedScore) {
			Button imageButton = itemsList[1];
			Image imageUI = imageButton.targetGraphic;
			imageUI.
		}

	}*/
		
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