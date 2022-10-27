using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;

public class AlbumLoader : MonoBehaviour
{
    [SerializeField] private GameObject photoCardPrefab;
    [SerializeField] private Transform photoCards;

    private Story story;

    // UI
    private TextMeshProUGUI title;
    private TextMeshProUGUI description;
    private Button nextPhotoBtn;
    private Button closeAlbumBtn;

    private float rotation = 7.872f;
    private int photoCardCurrentIndex;
    private int photoToSwipe;

    [SerializeField] private List<Item> itemsToShowInAlbum;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization Part
        this.title = GameObject.Find("Image Title").GetComponent<TextMeshProUGUI>();
        this.description = GameObject.Find("Image Description").GetComponent<TextMeshProUGUI>();
        this.nextPhotoBtn = GameObject.Find("Next Photo Btn").GetComponent<Button>();
        this.closeAlbumBtn = GameObject.Find("Close Album Button").GetComponent<Button>();
        this.itemsToShowInAlbum = new List<Item>();
        this.photoCardCurrentIndex = 0;

        if (AlbumManager.Instance.isFirstAlbum == true)
        {
            print("FIRST ALBUM");
            foreach (ItemGiver itemGiver in AlbumManager.Instance.itemGivers)
            {
                foreach (Item item in itemGiver.itemsToGive)
                {
                    if (item.informationLink.Length > 0)
                    {
                        GameObject gameObject = Instantiate(this.photoCardPrefab, this.photoCards);
                        gameObject.transform.Rotate(new Vector3(0f, 0f, -rotation));
                        rotation = -rotation;


                        this.itemsToShowInAlbum.Add(item);

                        gameObject = null;
                    }
                }
            }
        }
        else
        {
            print("SECOND ALBUM");

            foreach (Item item in AlbumManager.Instance.items)
            {
                GameObject gameObject = Instantiate(this.photoCardPrefab, this.photoCards);
                gameObject.transform.Rotate(new Vector3(0f, 0f, -rotation));
                rotation = -rotation;

                if (item.informationLink.Length > 0)
                    this.itemsToShowInAlbum.Add(item);

                gameObject = null;
            }
        }

        int photoIndex = 0;

        for (int i = this.photoCards.childCount - 1; i >= 0; i--)
        {
            this.photoCards.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = 
                Resources.Load<Sprite>(this.itemsToShowInAlbum[photoIndex].imageLink);

            photoIndex++;
        }

        this.photoToSwipe = this.itemsToShowInAlbum.Count - 1;
        this.nextPhotoBtn.onClick.AddListener(this.Next);
        this.closeAlbumBtn.onClick.AddListener(()
            => SceneManager.UnloadSceneAsync(AlbumManager.Instance.isFirstAlbum ? "Delivery Info Scene" : "Showing Album"));

        this.StartShowingInformation();
    }

    public void StartShowingInformation()
    {
        if (this.photoCardCurrentIndex > this.itemsToShowInAlbum.Count - 1)
        {
            SceneManager.UnloadSceneAsync(AlbumManager.Instance.isFirstAlbum ? "Delivery Info Scene" : "Showing Album");
            return;
        }

        this.story = 
            new Story(Resources.Load<TextAsset>(this.itemsToShowInAlbum[this.photoCardCurrentIndex].informationLink).text);

        this.title.text = this.itemsToShowInAlbum[this.photoCardCurrentIndex].titleOfInformation;

        if (story.canContinue)
        {
            string storyText = story.Continue();
            
            if (storyText == "")
            {
                if (this.photoCardCurrentIndex == this.itemsToShowInAlbum.Count - 1)
                {
                    SceneManager.UnloadSceneAsync("Delivery Info Scene");
                    return;
                }
                return;
            }
            this.description.text = storyText;
        }
        else
        {
            SceneManager.UnloadSceneAsync("Delivery Info Scene");
        }
    }

    public void ContinueShowingDialogue()
    {
        if (story.canContinue)
        {
            string storyText = this.story.Continue();
            
            if (storyText == "")
            {
                if (this.photoCardCurrentIndex == this.itemsToShowInAlbum.Count - 1)
                {
                    SceneManager.UnloadSceneAsync("Delivery Info Scene");
                    return;
                }
                this.ExitTopicInformationPanel();
                return;
            }
            this.description.text = storyText;
        }
        else
        {
            this.ExitTopicInformationPanel();
        }
    }


    public void ExitTopicInformationPanel()
    {
        if (this.photoCardCurrentIndex <= this.itemsToShowInAlbum.Count - 1)
        {
            this.photoCardCurrentIndex++;
            LeanTween.moveLocalX(this.photoCards.GetChild(this.photoToSwipe).gameObject, -Screen.width * 3f, .3f);
            this.photoToSwipe--;
            this.StartShowingInformation();
        }
    }

    public void Next()
    {
        try
        {
            this.story.ChooseChoiceIndex(0);
            this.ContinueShowingDialogue();
        }
        catch (System.Exception e) {

            print(e.Message);
            ExitTopicInformationPanel();
        }
    }
}
