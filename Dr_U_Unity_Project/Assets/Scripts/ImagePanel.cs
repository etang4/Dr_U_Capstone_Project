using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using SimpleSQL;

using UnityEngine.UI;

/*
    ImagePanel handles Image results, language and the dynamic sizing of 
    the Images.
*/
public class ImagePanel : MonoBehaviour
{
    //GUI Area which the images are stored  
    private GameObject containerRect;
    //Gameobject that displays the information of the image.
    //Attach MoreInfoImagePanel Prefab here.
    public GameObject MoreInfoImagePanel;
    //Database Manager from SimpleSQL Plugin.
    public SimpleSQL.SimpleSQLManager dbManager;
    //Make sure Images stay the same size.
    private bool _isInstantiated;

    private int listSize;
    private GridLayoutGroup imageGrid;
    private RectTransform imageRect;
    private string image_path;

    //class that stores image info and sprite.
    private class ImageStorage
    {
        public ImagePair imageInfo;
        public Sprite imageSprite;
    }

    // Use this for initialization
    void Start()
    {
        containerRect = this.gameObject;
        int estimoteID = 1; ////////////////////// This should auto update with location changes
        _isInstantiated = false;
        loadImages(estimoteID);
        _isInstantiated = true;
    }
    public void loadImages(int estimoteID)
    { 

        image_path = Application.persistentDataPath + "/";  // Set storage path

        List<ImagePair> image_list = SelectImagePairs(estimoteID);  // Grab image data from DB
        downloadImages(image_list);     // Download missing immages

        List<ImageStorage> image_storage = new List<ImageStorage>();
        foreach (ImagePair image_info in image_list)    // Make image object with all info
        {
			// Try catch block to ignore broken images
			try
            {
				ImageStorage newImage = new ImageStorage();
				newImage.imageInfo = image_info;
				newImage.imageSprite = getImage(image_info);
				image_storage.Add(newImage);
			}catch(Exception e){
				Debug.Log(e);
			}
        }
        
        //Retrieves all images from Assets/Resources/Test_Images
        listSize = image_list.Count;


        //MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.
        if (!_isInstantiated)
        {
            //Create display list
            imageGrid = containerRect.GetComponent<GridLayoutGroup>();
            imageRect = containerRect.GetComponent<RectTransform>();

            imageGrid.cellSize = new Vector2(imageRect.rect.height, imageRect.rect.height);
            imageRect.offsetMax = new Vector2(imageRect.offsetMax.x, 0);
        }
        //Adjusting area of where images are stored depending on number of images.
        imageRect.sizeDelta = new Vector2((imageGrid.cellSize.x + imageGrid.spacing.x) * (listSize - 2) - imageGrid.spacing.x * 2, imageRect.sizeDelta.y);

        //Creating and storing images in the image panel.
        foreach (ImageStorage storage in image_storage)
        {
			Sprite image = storage.imageSprite;
			//Instantiates buttons from image array.
			GameObject newImage = new GameObject(storage.imageSprite.name);
			Image imageUI = newImage.AddComponent<Image>();
			imageUI.sprite = image;
			Button imageButton = newImage.AddComponent<Button>();
			imageButton.targetGraphic = imageUI;
            string currentInfo = storage.imageInfo.information; // Required to create temp variable or else each listener will be set to the last object in the callback
            imageButton.onClick.AddListener(() => { this.ActivateMoreInfoImagePanel(imageUI, currentInfo); });
			//Sets newly created button to display list.
			newImage.transform.SetParent(imageRect.transform);
        }
    }

    //Event called onClick.
    public void ActivateMoreInfoImagePanel(Image currentImg, String currentInfo) //Needs parameter at some point to determine what image/data to show
    {
       MoreInfoImagePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
       MoreInfoImagePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = currentInfo;
       MoreInfoImagePanel.SetActive(true);
    }

    //Database query for selecting images and their information.
    public List<ImagePair> SelectImagePairs(int estimoteID)
    {
        string sql = string.Format("select `name`, `intro`, `information`, `image`, `exhibitID` from Exhibit WHERE estimoteID == {0} LIMIT 50", estimoteID);
        List<ImagePair> image_list = dbManager.Query<ImagePair>(sql);
        return image_list;
    }

    //To download images from the public database.
    private void downloadImages(List<ImagePair> imageList)
    {
        DirectoryInfo dir = new DirectoryInfo(image_path);

        FileInfo[] info = dir.GetFiles("*.*");      // Grab all files in storage
        
        HashSet<string> allImageNames = new HashSet<string>();
        foreach(FileInfo f in info){
            allImageNames.Add(f.Name);  // Add their names to a hashset
            
        }
        // Get image name here
        List<string> imageNames = new List<string>();

        foreach (ImagePair pair in imageList)
        {
            imageNames.Add(pair.image);
        }
        
        // Check if image exists locally here
        foreach (string imageName in imageNames){
            if (allImageNames.Contains(imageName)){
                return;  //Do not download if image already exists
            }

            // Get image remotely here 
            WWW www = new WWW("http://ec2-52-32-1-167.us-west-2.compute.amazonaws.com/dru/api/services/images/" + imageName);
            while (!www.isDone)
            {
                //do something, or nothing, I dunno... Maybe a progress bar?
            }
            string fullPath = image_path + imageName;
            try
            {
                File.WriteAllBytes(fullPath, www.bytes);
            }catch (ArgumentException e){
                Debug.Log(e);
            }
        }
    }

    //Retrieving the image from the database.
    private Sprite getImage(ImagePair image)
    {
        DirectoryInfo dir = new DirectoryInfo(image_path);
        FileInfo[] info = dir.GetFiles(image.image);  // Grab first file with name match

        if (info.Length == 0)
        {
            throw new FileNotFoundException();  // Throw error if you can't find it
        }

        string path = image_path + info[0].Name;
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(128, 128, TextureFormat.ARGB32, false);   // Create texture of image
        texture.LoadImage(data);
        texture.name = Path.GetFileNameWithoutExtension(path);
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1000);  // Translate texture to sprite
    }
}

