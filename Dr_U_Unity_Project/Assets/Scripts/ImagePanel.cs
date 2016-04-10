using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using SimpleSQL;




using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    private Sprite[] SetImages;     // This will be replaced with a database call
    public GameObject containerRect;
    public GameObject MoreInfoImagePanel;
    public SimpleSQL.SimpleSQLManager dbManager;
    
    private int listSize;
    private GridLayoutGroup imageGrid;
    private RectTransform imageRect;
    private string image_path;

    private class ImageStorage
    {
        public ImagePair imageInfo;
        public Sprite imageSprite;
    }

    // Use this for initialization
    void Start()
    {
        int estimoteID = 1; ////////////////////// This should auto update with location changes
        image_path = Application.persistentDataPath + "/Images/";
        
        List<ImagePair> image_list = SelectImagePairs(estimoteID);
        downloadImages(image_list);

        List<ImageStorage> image_storage = new List<ImageStorage>();
        foreach (ImagePair image_info in image_list)
        {
            ImageStorage newImage = new ImageStorage();
            newImage.imageInfo = image_info;
            newImage.imageSprite = getImage(image_info);
            image_storage.Add(newImage);
        }
        
        //Retrieves all images from Assets/Resources/Test_Images
        listSize = image_list.Count;


        //MoreInfoImagePanel = GameObject.Find("MoreInfoImagePanel Need to dynamically find this gameobject eventually.

        //Create display list
        imageGrid = containerRect.GetComponent<GridLayoutGroup>();
        imageRect = containerRect.GetComponent<RectTransform>();

        imageGrid.cellSize = new Vector2(imageRect.rect.height, imageRect.rect.height);
        imageRect.sizeDelta = new Vector2(imageGrid.cellSize.x * (listSize - 2), imageRect.sizeDelta.y);
        imageRect.offsetMax = new Vector2(imageRect.offsetMax.x, 0);


        foreach (ImageStorage storage in image_storage)
        {
            try
            {
                Sprite image = storage.imageSprite;
                //Instantiates buttons from image array.
                GameObject newImage = new GameObject(storage.imageSprite.name);
                Image imageUI = newImage.AddComponent<Image>();
                imageUI.sprite = image;
                Button imageButton = newImage.AddComponent<Button>();
                imageButton.targetGraphic = imageUI;
                imageButton.onClick.AddListener(() => { this.ActivateMoreInfoImagePanel(imageUI, image.name, storage.imageInfo.information); });
                //Sets newly created button to display list.
                newImage.transform.SetParent(imageRect.transform);
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
            }
        }
    }

    //Event called onClick.
    public void ActivateMoreInfoImagePanel(Image currentImg, String currentName, String currentInfo) //Needs parameter at some point to determine what image/data to show
    {
       MoreInfoImagePanel.transform.GetChild(0).GetComponent<Image>().sprite = currentImg.sprite;
       MoreInfoImagePanel.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = currentInfo;
       MoreInfoImagePanel.SetActive(true);
    }
    public List<ImagePair> SelectImagePairs(int estimoteID)
    {
        string sql = string.Format("select `name`, `intro`, `information`, `image`, `exhibitID` from Exhibit WHERE estimoteID == {0} LIMIT 50", estimoteID);
        List<ImagePair> image_list = dbManager.Query<ImagePair>(sql);

        return image_list;
    }

    private void downloadImages(List<ImagePair> imageList)
    {
        DirectoryInfo dir = new DirectoryInfo(image_path);

        FileInfo[] info = dir.GetFiles("*.*");
        
        HashSet<string> allImageNames = new HashSet<string>();
        foreach(FileInfo f in info){
            allImageNames.Add(f.Name);
            
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
                //do something, or nothing, I dunno... Maybe a progress bar
            }
            string fullPath = image_path + imageName;
            System.IO.Directory.CreateDirectory(image_path);
            try
            {
                File.WriteAllBytes(fullPath, www.bytes);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e);
            }
        }
    }

    private Sprite getImage(ImagePair image)
    {
        Sprite requestedImage = new Sprite();
        HashSet<string> requestedImageNames = new HashSet<string>();

        DirectoryInfo dir = new DirectoryInfo(image_path);

        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (f.Name == image.image)
            {
                string path = image_path + f.Name;
                byte[] data = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
                texture.LoadImage(data);
                texture.name = Path.GetFileNameWithoutExtension(path);
                return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1000);
            }
        }
        return null;
    }
}

