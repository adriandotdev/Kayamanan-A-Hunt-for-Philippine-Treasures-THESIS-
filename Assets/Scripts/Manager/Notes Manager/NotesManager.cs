using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NotesManager : MonoBehaviour
{
    public static NotesManager instance;

    public Button notesBtn;
    public RectTransform notesPanel;
    public Button closeNotesPanelBtn;
    public GameObject notePrefab;
    public Transform notesPanelContainer;

    // UI Elements to Hide/Disabled
    public CanvasGroup houseCanvasGroup;
    public GameObject inventoryPanel;
    public GameObject joyStick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // INIT
        this.notesPanel = GameObject.Find("Notes Panel").GetComponent<RectTransform>();
        this.notesBtn = GameObject.Find("Notes Button").GetComponent<Button>();
        this.closeNotesPanelBtn = GameObject.Find("Close Notes Panel BTN").GetComponent<Button>();
        this.notesPanelContainer = this.notesPanel.GetChild(2).GetChild(0).GetChild(0);
        this.notesPanel.localScale = Vector2.zero;
        this.houseCanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.joyStick = GameObject.Find("Fixed Joystick");
        this.inventoryPanel = GameObject.Find("Inventory Panel");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House") && 
            DataPersistenceManager.instance.playerData.isTutorialDone == false)
            return;

        this.notesBtn.onClick.AddListener(() => {
            SoundManager.instance?.PlaySound("Button Click 1");
            this.OpenNotesPanel();
            this.notesBtn.transform.GetChild(0).gameObject.SetActive(false);
        });
        this.closeNotesPanelBtn.onClick.AddListener(this.CloseNotesPanel);
    }

    public void OpenNotesPanel()
    {
        this.houseCanvasGroup.interactable = false;
        this.inventoryPanel.SetActive(false);
        this.joyStick.SetActive(false);

        if (DataPersistenceManager.instance != null)
            DataPersistenceManager.instance.playerData.notesNewIcon = false;

        LeanTween.scale(this.notesPanel.gameObject, new Vector2(334.8216f, 334.8216f), .2f)
            .setEaseSpring();

        foreach (Quest quest in DataPersistenceManager.instance.playerData.notesInfos)
        {
            GameObject note = Instantiate(this.notePrefab, this.notesPanelContainer);
            note.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.note;
        }
    }

    public void CloseNotesPanel()
    {
        SoundManager.instance?.PlaySound("Button Click 1");
        this.houseCanvasGroup.interactable = true;
        this.inventoryPanel.SetActive(true);
        this.joyStick.SetActive(true);

        LeanTween.scale(this.notesPanel.gameObject, Vector2.zero, .2f)
           .setEaseSpring();

        foreach (Transform notePrefab in this.notesPanelContainer)
        {
            Destroy(notePrefab.gameObject);
        }
    }
}