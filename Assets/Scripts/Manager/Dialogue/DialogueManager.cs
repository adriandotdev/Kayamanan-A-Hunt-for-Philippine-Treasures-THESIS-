using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DialogueManager : MonoBehaviour
{
    public static Action<bool> OnDialogueRunning;
    public static DialogueManager _instance;

    [Header("UI Elements")]
    // Panel or the dialogue box.
    [SerializeField] private RectTransform panel;
    [SerializeField] public TMPro.TextMeshProUGUI actorField;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueField;
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform inventoryPanel;

    [Header("Canvas Group")]
    public CanvasGroup houseGroup;

    private Story currentStory;

    public GameObject[] choicesBtn;

    private float typingSpeed = 0.001f;

    public Coroutine coroutine;
    public int lengthOfText;
    // the name of the current talking npc.
    public string npcName;
    /** Check if the npc is talking */
    public bool isTalking;

    public bool isNotWantToRecap;

    [SerializeField] public ShowPhotoAlbumGoal showAlbumGoal;
    [SerializeField] public DeliveryGoal deliveryGoal;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneRequiresDialogueLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneRequiresDialogueLoaded;
    }

    private void Start()
    {
        if (_instance == null)
            _instance = this;

        this.isTalking = false;
    }

    // Run this function when house scene is loaded.
    public void OnSceneRequiresDialogueLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Museum")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Church"))
        {
            this.houseGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();

            try
            {
                this.panel.GetComponent<Button>().onClick.AddListener(() =>
                {
                    print("WANT TO SKIP THE TYPING EFFECT");

                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                        this.dialogueField.maxVisibleCharacters = lengthOfText;
                        this.ShowChoices();
                    }
                });
            }
            catch (System.Exception e) { }
        }
    }

    public void StartDialogue(TextAsset ink)
    {
        isTalking = true;
        isNotWantToRecap = false;

        currentStory = new Story(ink.text);
        panel.gameObject.SetActive(true);
        this.joystick.gameObject.SetActive(false);
        this.inventoryPanel.gameObject.SetActive(false);
        this.houseGroup.interactable = false;
        this.houseGroup.blocksRaycasts = false;

        OnDialogueRunning?.Invoke(true);

        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();

            if (text == "")
            { 
                panel.gameObject.SetActive(false);
                OnDialogueRunning?.Invoke(false);
                isTalking = false;
                return;
            }
            lengthOfText = text.Length;
            coroutine = StartCoroutine(this.DisplayLine(text));
        }
        else
        {
            ExitDialogue();
        }
    }

    IEnumerator DisplayLine(string line)
    {
        this.dialogueField.text = line;
        this.dialogueField.maxVisibleCharacters = 0;

        this.HideChoices();

        if (currentStory.currentTags.Count > 0 && currentStory.currentTags[0].Contains(":") == false)
        {
            actorField.text = currentStory.currentTags[0];
        }
        else
        {
            actorField.text = this.npcName;
        }

        foreach (char c in line.ToCharArray())
        {
            this.dialogueField.maxVisibleCharacters++;
            yield return new WaitForSeconds(this.typingSpeed);
        }

        ShowChoices();
        if (currentStory.currentTags.Count > 0 && currentStory.currentTags[0].Contains(":"))
        {
            isNotWantToRecap = true;
        }
    }

    public void HideChoices()
    {
        foreach (GameObject choice in this.choicesBtn)
        {
            choice.SetActive(false);
        }
    }

    public void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();

            if (text == "")
            {
                OnDialogueRunning?.Invoke(false);
                panel.gameObject.SetActive(false);
                this.houseGroup.interactable = true;
                this.houseGroup.blocksRaycasts = true;
                this.joystick.gameObject.SetActive(true);
                this.inventoryPanel.gameObject.SetActive(true);
                isTalking = false;

                // If the showAlbumGoal is not null, it means that it requires to show the album for 
                // the requested by player.
                if (this.showAlbumGoal != null && this.showAlbumGoal.giverOfInfo.Length > 0 && this.showAlbumGoal.giverOfInfo.Length > 0)
                {
                    AlbumManager.Instance.isFirstAlbum = false;
                    AlbumManager.Instance.items = this.showAlbumGoal.items;
                    SceneManager.LoadScene("Showing Album", LoadSceneMode.Additive);
                    this.showAlbumGoal = null;
                }
                else if (this.deliveryGoal != null && this.deliveryGoal.wayOfInfo.ToLower() == "not text" && this.deliveryGoal.giverName.Length > 0)
                {
                    if (isNotWantToRecap == false)
                    {

                        AlbumManager.Instance.isFirstAlbum = false;
                        AlbumManager.Instance.items = this.deliveryGoal.itemsToShow;
                        SceneManager.LoadScene("Showing Album", LoadSceneMode.Additive);
                        this.deliveryGoal = null;
                    }
                    this.deliveryGoal = null;
                }
                return;
            }
            lengthOfText = text.Length;
            coroutine = StartCoroutine(this.DisplayLine(text));
        }
        else
        {
            ExitDialogue();
        }
    }


    public void ExitDialogue()
    {
        OnDialogueRunning?.Invoke(false);
        panel.gameObject.SetActive(false);
        this.joystick.gameObject.SetActive(true);
        this.inventoryPanel.gameObject.SetActive(true);
        dialogueField.text = "";
        isTalking = false;
    }

    public void ShowChoices()
    {
        List<Choice> choices = currentStory.currentChoices;

        if (choicesBtn.Length >= choices.Count)
        {
            int index = 0;

            foreach (Choice choice in choices)
            {
                choicesBtn[index].SetActive(true);
                
                choicesBtn[index].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choice.text;
                index++;
            }

            for (int i = index; i < choicesBtn.Length; i++)
            {
                choicesBtn[i].gameObject.SetActive(false);
            }
        }

    }

    public void MakeChoice(int choiceIndex)
    {

        currentStory.ChooseChoiceIndex(choiceIndex);
        this.ContinueDialogue();
    }
}

