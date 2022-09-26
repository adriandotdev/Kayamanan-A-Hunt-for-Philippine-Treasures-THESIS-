using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;

public class TopicInfoLoader : MonoBehaviour
{
    // All of the panels
    private GameObject comicStrip1;
    private GameObject comicStrip2;
    private GameObject comicStrip3;

    // For COMIC STRIP - 1
    private Transform bubbleInfo;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI infoText;
    public Transform cs1ImageContainer;

    // For COMIC STRIP - 2 
    public Transform cs2TitleContainer;
    public Transform cs2Stars;
    public Transform cs2ImagePanel;
    public Transform cs2InformationPanel;

    // Canvas Group to disable.
    private GameObject canvasOfMuseum;

    // story.
    public Story story;

    private void Start()
    {
        this.InitializeComicStrips(); // Initialize Comic Strips

        if (TopicInfoManager.Instance.panelNumber == TopicInfoManager.PANEL_NUMBER.ONE)
        {
            CanvasGroup canvasGroup1 = this.comicStrip1.GetComponent<CanvasGroup>();
            this.comicStrip1.SetActive(true);

            this.ModifyCanvasGroup(canvasGroup1, true);

            this.bubbleInfo = GameObject.Find("Bubble Info").transform;
            this.title = GameObject.Find("Subject Title").GetComponent<TMPro.TextMeshProUGUI>();
            this.infoText = this.bubbleInfo.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            this.cs1ImageContainer = GameObject.Find("CS1 Image Container").transform;
            this.cs1ImageContainer.GetChild(2).GetComponent<Image>().sprite = TopicInfoManager.Instance.TopicImage;

            this.title.text = TopicInfoManager.Instance.Topic;

            Transform infoTitle = GameObject.Find("Info Title").transform;

            this.canvasOfMuseum = GameObject.Find("Canvas");

            StartShowingInformation();
            TweeningManager.instance.ScaleTitleElementsInPanelOne(this.cs1ImageContainer.GetChild(0).gameObject);
            TweeningManager.instance.ScaleTitleElementsInPanelOne(this.cs1ImageContainer.GetChild(1).gameObject);
            TweeningManager.instance.RotateStarInPanelOne(GameObject.Find("Stars").transform.GetChild(0).gameObject);
            TweeningManager.instance.RotateStarInPanelOne(GameObject.Find("Stars").transform.GetChild(1).gameObject);
        }
        else if (TopicInfoManager.Instance.panelNumber == TopicInfoManager.PANEL_NUMBER.TWO)
        {
            CanvasGroup canvasGroup2 = this.comicStrip2.GetComponent<CanvasGroup>();
            this.comicStrip2.SetActive(true);

            this.ModifyCanvasGroup(canvasGroup2, true);

            this.cs2TitleContainer = GameObject.Find("CS2 Title Container").transform;
            this.cs2Stars = GameObject.Find("CS2 Stars").transform;
            this.cs2ImagePanel = GameObject.Find("CS2 Image Panel").transform;
            this.cs2InformationPanel = GameObject.Find("CS2 Information Panel").transform;

            this.cs2ImagePanel.GetChild(0).GetComponent<Image>().sprite = TopicInfoManager.Instance.TopicImage;
            this.cs2TitleContainer.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = TopicInfoManager.Instance.Topic;

            this.canvasOfMuseum = GameObject.Find("Canvas");

            StartShowingInformation();
            TweeningManager.instance.ScaleTitleElementsInPanelOne(this.cs2Stars.GetChild(0).gameObject);
            TweeningManager.instance.ScaleTitleElementsInPanelOne(this.cs2Stars.GetChild(1).gameObject);
            TweeningManager.instance.ScaleTitleElementsInPanelOne(this.cs2TitleContainer.GetChild(0).gameObject);
        }
    }
    
    void InitializeComicStrips()
    {
        this.comicStrip1 = GameObject.Find("Comic Strip 1");
        this.comicStrip2 = GameObject.Find("Comic Strip 2");

        CanvasGroup canvasGroup1 = this.comicStrip1.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup2 = this.comicStrip2.GetComponent<CanvasGroup>();

        this.ModifyCanvasGroup(canvasGroup1, false);
        this.ModifyCanvasGroup(canvasGroup2, false);

        this.comicStrip1.SetActive(false);
        this.comicStrip2.SetActive(false);
    }

    void ModifyCanvasGroup(CanvasGroup cg, bool value)
    {
        cg.blocksRaycasts = value;
        cg.interactable = value;
    }

    public void SetTextMeshToUse(string text)
    {
        if (this.infoText != null)
        {
            this.infoText.text = text;
        }
        else if (this.cs2InformationPanel != null)
        {
            this.cs2InformationPanel.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = text;
        }
    }

    public void StartShowingInformation()
    {
        this.canvasOfMuseum.SetActive(false);

        story = new Story(TopicInfoManager.Instance.Info.text);

        if (story.canContinue)
        {
            string storyText = story.Continue();

            if (storyText == "")
            {
                this.ExitTopicInformationPanel();
                return;
            }
            this.SetTextMeshToUse(storyText);
        }
        else {
            this.ExitTopicInformationPanel();
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
            this.SetTextMeshToUse(storyText);
        }
        else
        {
            this.ExitTopicInformationPanel();
        }
    }


    public void ExitTopicInformationPanel()
    { 
        this.canvasOfMuseum.SetActive(true);
        SceneManager.UnloadSceneAsync("Topic Info Scene");
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
