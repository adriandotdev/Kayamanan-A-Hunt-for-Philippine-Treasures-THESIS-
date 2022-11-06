using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SchoolEventIntroduction : MonoBehaviour
{
    [TextArea]
    public string[] introductions;
    [TextArea]
    public string[] winnerGame;
    // Needed UI
    public Transform schoolEventTutorialPanel;
    public Transform man;
    public TextMeshProUGUI dialogueText;
    public Button nextBtn;

    // UI TO HIDE
    public GameObject minimap;
    public GameObject clock;
    public GameObject dunongPts;
    public GameObject inventoryPanel;
    public CanvasGroup houseCG;
    public GameObject fixedJoystick;

    int dialogueIndex = 0;

    private void Start()
    {
        if (DataPersistenceManager.instance.playerData.isGameCompleted)
        {
            this.dialogueIndex = 0;

            this.InitializeNotificationComponent();

            dialogueText.text = winnerGame[dialogueIndex];

            nextBtn.onClick.AddListener(NextDialogueWinner);
        }
        else if (DataPersistenceManager.instance.playerData.isFirstTimeGoingToSchool)
        {
            this.dialogueIndex = 0;

            this.InitializeNotificationComponent();

            dialogueText.text = introductions[dialogueIndex];

            nextBtn.onClick.AddListener(NextDialogue);

            DataPersistenceManager.instance.playerData.isFirstTimeGoingToSchool = false;
        }
        else if (DataPersistenceManager.instance.playerData.isQuestReset)
        {
            this.InitializeNotificationComponent();

            dialogueText.text = "OOOPS! It seems like you didn't ace the assessments! Complete the quests again and review all the information.";

            nextBtn.onClick.AddListener(() =>
            {
                LeanTween.moveLocalX(man.gameObject, -Screen.width, .3f)
                .setEaseInBounce();

                LeanTween.scale(schoolEventTutorialPanel.gameObject, Vector2.zero, .2f)
                        .setEaseSpring();
                this.ShowUIElements(true);
            });

            DataPersistenceManager.instance.playerData.isQuestReset = false;
        }
        else if (DataPersistenceManager.instance.playerData.hasNewOpenRegion)
        {
            this.InitializeNotificationComponent();

            dialogueText.text = "Congratulations! You've opened a new region in the Luzon booth!";

            nextBtn.onClick.AddListener(() =>
            {
                LeanTween.moveLocalX(man.gameObject, -Screen.width, .3f)
                .setEaseInBounce();

                LeanTween.scale(schoolEventTutorialPanel.gameObject, Vector2.zero, .2f)
                        .setEaseSpring();
                this.ShowUIElements(true);
            });

            DataPersistenceManager.instance.playerData.hasNewOpenRegion = false; ;
        }
    }

    public void InitializeNotificationComponent()
    {
        this.InitializeUIToHide();

        schoolEventTutorialPanel = GameObject.Find("School Event Tutorial").transform;
        man = schoolEventTutorialPanel.GetChild(0);
        man.localPosition = new Vector2(-Screen.width, man.localPosition.y);

        LeanTween.scale(schoolEventTutorialPanel.gameObject, new Vector2(194.3792f, 194.3792f), .2f)
            .setEaseInBounce()
            .setOnComplete(() =>
            {
                LeanTween.moveLocalX(man.gameObject, -3.149976f, .3f);
            });

        schoolEventTutorialPanel.localScale = new Vector2(194.3792f, 194.3792f);

        dialogueText = schoolEventTutorialPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        nextBtn = schoolEventTutorialPanel.GetChild(2).GetComponent<Button>();
    }

    public void InitializeUIToHide()
    {
        this.minimap = GameObject.Find("Minimap with Frame");
        this.houseCG = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.fixedJoystick = GameObject.Find("Fixed Joystick");

        this.ShowUIElements(false);
    }

    public void ShowUIElements(bool show)
    {
        this.minimap.SetActive(show);
        this.fixedJoystick.GetComponent<Joystick>().enabled = show;
        
        if (show)
        {
            this.houseCG.alpha = 1;
            this.houseCG.interactable = true;
        }
        else
        {
            this.houseCG.alpha = 0;
            this.houseCG.interactable = false;
        }
    }
    public void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < this.introductions.Length)
        {
            dialogueText.text = introductions[dialogueIndex];
        }
        else
        {
            LeanTween.moveLocalX(man.gameObject, -Screen.width, .3f)
                .setEaseInBounce();

            LeanTween.scale(schoolEventTutorialPanel.gameObject, Vector2.zero, .2f)
                    .setEaseSpring();

            this.ShowUIElements(true);
        }
    }

    public void NextDialogueWinner()
    {
        dialogueIndex++;

        if (dialogueIndex < this.winnerGame.Length)
        {
            dialogueText.text = winnerGame[dialogueIndex];
        }
        else
        {
            LeanTween.moveLocalX(man.gameObject, -Screen.width, .3f)
                .setEaseInBounce();

            LeanTween.scale(schoolEventTutorialPanel.gameObject, Vector2.zero, .2f)
                    .setEaseSpring();

            SceneManager.LoadScene("E First Sequence");
        }
    }
}
