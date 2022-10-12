using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;

public class AlbumLoader : MonoBehaviour
{
    [SerializeField] private GameObject photoCardPrefab;
    [SerializeField] private Transform photoCards;

    private Story story;
    private Button closeAlbumBtn;

    private float rotation = 7.872f;
    private int photoCardCurrentIndex;

    /**TO-DO
     * - Next and previous button of the album.
     * 
     * -- THE NEXT BUTTON CAN HAVE 2 EVENTS
     * - First is the loading of information from the ink file.
     * - Second is for loading the next photo and information.
     */

    /**
     * FLOW OF SETTING UP the PhotoCards
     * 
     * When the item is delivered to NPC, the giveItemToNPCBtn will change the event.
     */

     /**
      * NOTE: Refactor the code and reuse some functionalities. 
      */

    // Start is called before the first frame update
    void Start()
    {
        // Initialization Part
        this.closeAlbumBtn = GameObject.Find("Close Album Button").GetComponent<Button>();

        int length = AlbumManager.Instance.itemGivers.Length; // Number of photocards.
        this.photoCardCurrentIndex = 0;

        for (int i = 1; i <= length; i++)
        {
            GameObject photoCard = Instantiate(photoCardPrefab, photoCards);

            // Add Event to each photoCard.
            photoCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                LeanTween.moveLocalX(photoCard, -Screen.width, .2f);
            });

            if (i % 2 != 0)
            {
                photoCard.transform.Rotate(new Vector3(0f, 0f, -rotation));
            }
            else
            {
                photoCard.transform.Rotate(new Vector3(0f, 0f, rotation));
            }
        }

        this.closeAlbumBtn.onClick.AddListener(() => SceneManager.UnloadSceneAsync("Delivery Info Scene"));

        this.StartShowingInformation();
    }

    public void StartShowingInformation()
    {
        this.story = new Story(AlbumManager.Instance.itemGivers[this.photoCardCurrentIndex].itemsToGive[0].informationLink);

        if (story.canContinue)
        {
            string storyText = story.Continue();

            if (storyText == "")
            {
                this.ExitTopicInformationPanel();
            }
            print(storyText);
        }
        else
        {
            print("END OF STORY");
        }
    }

    public void ContinueShowingDialogue()
    {
        if (story.canContinue)
        {
            string storyText = story.Continue();

            if (storyText == "")
            {
                this.ExitTopicInformationPanel();
            }
            print(storyText);
        }
        else
        {
            this.ExitTopicInformationPanel();
        }
    }


    public void ExitTopicInformationPanel()
    {
        print("INK File is done");
    }

    public void Next()
    {
        try
        {
            this.story.ChooseChoiceIndex(0);
            this.ContinueShowingDialogue();
        }
        catch (System.Exception e)
        {
            this.ExitTopicInformationPanel();
        }
    }
}
