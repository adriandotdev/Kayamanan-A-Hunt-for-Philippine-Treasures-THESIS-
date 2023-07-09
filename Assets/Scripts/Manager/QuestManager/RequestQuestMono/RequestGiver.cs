using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RequestGiver : MonoBehaviour
{
    public Quest requestQuest;
    public ItemGiver itemGiver;

    // Request Panel Components
    public Transform requestPanel;
    public TextMeshProUGUI requestMessageTxt;
    public Button acceptRequestBtn;
    public Button cancelRequestBtn;

    // NPC Buttons
    public Button talkBtn;
    public Button requestBtn;
    public Button giveItemToNpcBtn;
    public Button giveItemToPlayerBtn;

    public CanvasGroup canvasGroup;
    public GameObject joystick;
    public GameObject inventoryPanel;

    void Start()
    {
        try
        {
            // Initialize all the UI to hide when the request panel is opened.
            this.canvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
            this.joystick = GameObject.Find("Fixed Joystick");
            this.inventoryPanel = GameObject.Find("Inventory Panel");

            // Initialize NPC Buttons
            if (CheckIfStore() == true)
            {
                // When the game object is just a store.
                this.giveItemToPlayerBtn = transform.GetChild(1).GetChild(0).GetComponent<Button>();
            }
            else
            {
                this.talkBtn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
                this.requestBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
                this.giveItemToNpcBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
                this.giveItemToPlayerBtn = transform.GetChild(0).GetChild(4).GetComponent<Button>();
            }

            // Initialize Request Panel Components
            this.requestPanel = GameObject.Find("Request Panel").transform;
            this.requestMessageTxt = this.requestPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
            this.acceptRequestBtn = this.requestPanel.GetChild(1).GetChild(0).GetComponent<Button>();
            this.cancelRequestBtn = this.requestPanel.GetChild(1).GetChild(1).GetComponent<Button>();

            if (gameObject.tag == "Store")
            {
                transform.GetChild(2).GetComponent<BuyTrigger>().acceptRequestBtn = acceptRequestBtn;
                transform.GetChild(2).GetComponent<BuyTrigger>().giveItemToPlayerBtn = giveItemToPlayerBtn;
            }

            // Add event to requestBtn
            if (this.requestQuest != null)
                this.giveItemToPlayerBtn.onClick.AddListener(OpenRequestPanel);

            // Add event to cancelRequestBtn
            cancelRequestBtn.onClick.AddListener(CloseRequestPanel);
        }
        catch (System.Exception e) { }
    }

    public void OpenRequestPanel()
    {
        if (this.itemGiver.giverName == gameObject.name)
        {
            this.acceptRequestBtn.onClick.RemoveAllListeners();
            this.acceptRequestBtn.onClick.AddListener(GiveItem);

            this.DisableOtherUIsWhenRequestPanelIsOpen(false);
            this.DisableNPCButtons(false);

            this.requestMessageTxt.text = this.itemGiver.message; // set the message to textmeshpro

            LeanTween.scale(this.requestPanel.gameObject, Vector2.one, .1f)
                .setEaseSpring();
        }
    }

    public void GiveItem()
    {
        // Set the given items as received
        this.itemGiver.isItemsGiven = true;

        this.giveItemToPlayerBtn.gameObject.SetActive(false);

        SoundManager.instance?.PlaySound("Item Received");

        // We set to false since the request is not yet accepted.
        QuestManager.instance?.SetRequestAccepted(this.requestQuest.questID, false);
        QuestManager.instance?.OpenPlainAlertBox("You've received an items!");

        // ADD ALL ITEMS from itemGiver.
        foreach (Item item in this.itemGiver.itemsToGive)
        {
            if (item.itemName != "None")
                DataPersistenceManager.instance.playerData.inventory.AddItem(item);
        }
        InventoryManager.instance.DisplayInventoryItems();

        this.CloseRequestPanel();
        
    }

    public void CloseRequestPanel()
    {
        this.DisableOtherUIsWhenRequestPanelIsOpen(true);
        this.DisableNPCButtons(true);

        this.acceptRequestBtn.onClick.RemoveAllListeners();

        LeanTween.scale(this.requestPanel.gameObject, Vector2.zero, .1f)
            .setEaseSpring();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.tag != "Store"
            && this.requestQuest != null && this.requestQuest.questID.Length > 0 && this.requestQuest.requestGoal.isRequestFromNPCGained
            && this.itemGiver.isItemsGiven == false)
        {
            this.giveItemToPlayerBtn.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.tag != "Store"
            && requestQuest != null && this.requestQuest.questID.Length > 0)
        {
            this.HideAllButtons();
            this.acceptRequestBtn.onClick.RemoveAllListeners();
        }
    }

    // Disable the NPC Buttons
    void DisableNPCButtons(bool isInteractable)
    {
        if (CheckIfStore() == true)
        {
            this.giveItemToPlayerBtn.enabled = isInteractable;
        }
        else
        {
            this.talkBtn.enabled = isInteractable;
            this.requestBtn.enabled = isInteractable;
            this.giveItemToNpcBtn.enabled = isInteractable;
        }
    }


    public void DisableOtherUIsWhenRequestPanelIsOpen(bool isEnable)
    {
        this.canvasGroup.interactable = isEnable;
        this.joystick.SetActive(isEnable);
        this.inventoryPanel.SetActive(isEnable);
    }

    // Hide All NPC Buttons
    public void HideAllButtons()
    {
        if (CheckIfStore() == false)
        {
            this.talkBtn.gameObject.SetActive(false);
            this.requestBtn.gameObject.SetActive(false);
            this.giveItemToNpcBtn.gameObject.SetActive(false);
            this.giveItemToPlayerBtn.gameObject.SetActive(false);
        }
        else
        {
            this.giveItemToPlayerBtn.gameObject.SetActive(false);
        }       
    }

    public bool CheckIfStore()
    {
        if (gameObject.name == "Store in Market" || gameObject.name == "Karinderya"
            || gameObject.name == "Flower Shop" || gameObject.name == "Strawberry Jam Shop"
            || gameObject.name == "Karinderya")
        {
            return true;
        }
        return false;
    }
}
