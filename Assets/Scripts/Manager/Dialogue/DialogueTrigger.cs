using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset[] inks;
    public TMPro.TextMeshProUGUI actorName;
    private Button talkButton; // Talk Button
    private Button deliveryReqButton; // Delivery Request Button.
    private Button giveItemButton; // Give Item Button
    [SerializeField]private string NPC_NAME;

    private void OnEnable()
    {
        DialogueManager.OnDialogueRunning += HideButtons;
        // First child of this gameobject.
        Transform firstChild = transform.GetChild(0);

        this.NPC_NAME = transform.gameObject.name;

        this.actorName = firstChild.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        this.talkButton = firstChild.GetChild(1).GetComponent<Button>();
        this.deliveryReqButton = firstChild.GetChild(2).GetComponent<Button>();
        this.giveItemButton = firstChild.GetChild(3).GetComponent<Button>();

        // Add Event to Talk Button
        this.talkButton.onClick.AddListener(() =>
        {
            this.StartDialogue();
        });
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueRunning -= HideButtons;
    }

    private void Start()
    {
        this.NPC_NAME = transform.gameObject.name;
        this.actorName.text = transform.name;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print("HELLO FROM SPACE BUTTON");
            DialogueManager._instance.ContinueDialogue();
        }
    }

    public int CurrentOpenRegion()
    {
        int regionsData = 0;

        foreach (RegionData regionData in DataPersistenceManager.instance.playerData.regionsData)
        {
            if (regionData.isOpen)
                regionsData = regionData.regionNumber - 1;
        }
        return regionsData;
    }


    public void StartDialogue()
    {
        DialogueManager._instance.StartDialogue(inks[this.CurrentOpenRegion()]);
        DialogueManager._instance.actorField.text = this.NPC_NAME;

        QuestManager.instance.FindTalkQuestGoal(this.NPC_NAME);
    }

    public void HideButtons(bool toHide)
    {
        if (toHide)
        {
            this.talkButton.enabled = false;
            this.deliveryReqButton.enabled = false;
            this.giveItemButton.enabled = false;
        }
        else
        {
            this.talkButton.enabled = true;
            this.deliveryReqButton.enabled = true;
            this.giveItemButton.enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //actorName.gameObject.SetActive(false);
            this.talkButton.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        talkButton.gameObject.SetActive(false);
    }
}
