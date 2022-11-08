using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequestRequester : MonoBehaviour
{
    public Transform requestPanel;
    public TextMeshProUGUI requestMessageTxt;
    public Quest completedQuestToGainInfo;

    // Buttons of Request Panel.
    Button acceptRequestBtn;
    Button cancelRequestBtn;

    public Quest requestQuest; // requestQuest

    // NPC Buttons
    public Button talkButton;
    public Button requestBtn;
    public Button giveItemToNpcBtn;
    public Button giveItemToPlayerBtn;

    // UI to Hide when Request Panel is opened.
    public CanvasGroup canvasGroup;
    public GameObject joystick;
    public GameObject inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all the UI to hide when the request panel is opened.
        this.canvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.joystick = GameObject.Find("Fixed Joystick");
        this.inventoryPanel = GameObject.Find("Inventory Panel");

        // Initialize NPC Buttons
        this.talkButton = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        this.requestBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        this.giveItemToNpcBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
        this.giveItemToPlayerBtn = transform.GetChild(0).GetChild(4).GetComponent<Button>();

        // Initialize Request Panel Components
        this.requestPanel = GameObject.Find("Request Panel").transform;
        this.requestMessageTxt = this.requestPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        this.acceptRequestBtn = this.requestPanel.GetChild(1).GetChild(0).GetComponent<Button>();
        this.cancelRequestBtn = this.requestPanel.GetChild(1).GetChild(1).GetComponent<Button>();

        // Add event to requestBtn

        if (this.requestQuest != null)
            this.requestBtn.onClick.AddListener(OpenRequestPanel);

        // Add event to cancelRequestBtn
        cancelRequestBtn.onClick.AddListener(CloseRequestPanel);
    }

    public void OpenRequestPanel()
    {
        /** Every bukas ng panel, is doon lang mag aadd ng event para sa acceptRequestBtn sa panel.*/
        if (this.requestQuest.requestGoal.receiver == gameObject.name)
        {
            this.DisableOtherUIsWhenRequestPanelIsOpen(false); // Disable other UIs when request panel is open.

            this.requestMessageTxt.text = this.requestQuest.requestGoal.msgRequest; // set the message to textmeshpro.

            this.acceptRequestBtn.onClick.RemoveAllListeners();
            this.acceptRequestBtn.onClick.AddListener(AcceptRequest);

            this.DisableNPCButtons(false); // Disable the NPC Buttons

            LeanTween.scale(this.requestPanel.gameObject, Vector2.one, .1f)
                .setEaseSpring();
        }
    }

    public void AcceptRequest()
    {
        // Accept the request
        this.requestBtn.gameObject.SetActive(false);

        print(this.requestQuest.questID); // For Debugging Only

        // so the main problem is 'this.request.questID' is not CHANGING whenever we set again the EventListener. SO WHY? HAHA.
        QuestManager.instance?.SetRequestAccepted(this.requestQuest.questID, false);
        QuestManager.instance?.OpenPlainAlertBox(this.requestQuest.requestGoal.receiver + " request is accepted!");

        this.requestQuest.requestGoal.isRequestFromNPCGained = true;

        this.CloseRequestPanel();
    }

    public void CloseRequestPanel()
    {
        this.DisableOtherUIsWhenRequestPanelIsOpen(true);
        this.DisableNPCButtons(true); // Enable the NPC Buttons

        LeanTween.scale(this.requestPanel.gameObject, Vector2.zero, .1f)
            .setEaseSpring();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && this.requestQuest != null && this.requestQuest.questID.Length > 0
            && this.requestQuest.requestGoal.isRequestFromNPCGained == false)
        {
            this.requestBtn.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Player")
            && this.requestQuest != null && this.requestQuest.questID.Length > 0
            && this.isAllItemsGathered() && this.requestQuest.requestGoal.isItemReceivedOfNpc == false)
        {
            this.giveItemToNpcBtn.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Player") && requestQuest != null
                    && requestQuest.questID.Length > 0 && isAllItemsGathered()
                    && requestQuest.requestGoal.isItemReceivedOfNpc == true 
                    && requestQuest.region.ToUpper() != "PRE-QUEST")
        {
            this.giveItemToNpcBtn.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Player") && this.completedQuestToGainInfo.questID.Length > 0)
        {
            this.giveItemToNpcBtn.gameObject.SetActive(true);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.HideAllButtons();
            this.acceptRequestBtn.onClick.RemoveAllListeners();
        }
    }

    // Hide All NPC Buttons
    void HideAllButtons()
    {
        this.talkButton.gameObject.SetActive(false);
        this.requestBtn.gameObject.SetActive(false);
        this.giveItemToNpcBtn.gameObject.SetActive(false);
        this.giveItemToPlayerBtn.gameObject.SetActive(false);
    }

    // Disable the interactability of NPC Buttons
    void DisableNPCButtons(bool isInteractable)
    {
        this.talkButton.enabled = isInteractable;
        this.requestBtn.enabled = isInteractable;
        this.giveItemToNpcBtn.enabled = isInteractable;
        this.giveItemToPlayerBtn.enabled = isInteractable;
    }

    // Disable other UIs when the request panel is open.
    public void DisableOtherUIsWhenRequestPanelIsOpen(bool isEnable)
    {
        this.canvasGroup.interactable = isEnable;
        this.joystick.SetActive(isEnable);
        this.inventoryPanel.SetActive(isEnable);
    }

    // Check if all required items are get of the player.
    public bool isAllItemsGathered()
    {
        foreach (ItemGiver itemGiver in this.requestQuest.requestGoal.itemGivers)
        {
            if (itemGiver.isItemsGiven == false) return false;
        }

        return true;
    }
}
