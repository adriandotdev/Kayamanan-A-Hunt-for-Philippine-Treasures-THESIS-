using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 *  This script ay responsible para sa Delivery Quest
 *  which is specific sa Giver ng item kay player.
 * </summary> 
 */
public class DeliveryGoalGiver : MonoBehaviour
{
    public Quest quest;

    private CanvasGroup UICanvasGroup;
    private GameObject joystick; // joystick UI
    private GameObject inventoryPanel; // Inventory Panel UI

    private Button talkButton; // Talk Button
    private Button deliveryReqButton; // Delivery Request Button
    private Button giveItemButton; // Give Item Button

    private RectTransform fedexPanel;
    private TMPro.TextMeshProUGUI requestMessage;
    private Image collectibleImage;
    private TMPro.TextMeshProUGUI itemName;
    private TMPro.TextMeshProUGUI itemQuantity;
    private Button accept;
    private Button cancel;

    private void OnEnable()
    {
        this.UICanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.joystick = GameObject.Find("Fixed Joystick");
        this.inventoryPanel = GameObject.Find("Inventory Panel");

        // Fedex Panel
        this.fedexPanel = GameObject.Find("Fedex Quest Panel").GetComponent<RectTransform>();
        this.requestMessage = this.fedexPanel.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        this.talkButton = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        this.deliveryReqButton = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        this.giveItemButton = transform.GetChild(0).GetChild(3).GetComponent<Button>();

        // Buttons for accepting or declining the quest.
        this.accept = GameObject.Find("Delivery Quest Buttons").transform.GetChild(0).GetComponent<Button>();
        this.cancel = GameObject.Find("Delivery Quest Buttons").transform.GetChild(1).GetComponent<Button>();

        // Item Information for Fedex Panel
        this.collectibleImage = GameObject.Find("Delivery Quest Item").transform.GetChild(1).GetComponent<Image>();
        this.itemName = GameObject.Find("Delivery Quest Item").transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        this.itemQuantity = GameObject.Find("Delivery Quest Item").transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>();

        this.deliveryReqButton.onClick.AddListener(this.OpenDeliveryPanel);
        this.fedexPanel.localScale = Vector2.zero;
    }

    void OpenDeliveryPanel()
    {
        // Hide joystick and inventory panel, and disable the interactable property of canvas group.
        this.UICanvasGroup.interactable = false;
        this.joystick.SetActive(false);
        this.inventoryPanel.SetActive(false);

        // Disable All Buttons
        this.talkButton.enabled = false;
        this.deliveryReqButton.enabled = false;
        this.giveItemButton.enabled = false;

        LeanTween.scale(this.fedexPanel.gameObject, Vector2.one, .2f)
        .setEaseSpring()
        .setOnComplete(() => {
            this.accept.onClick.AddListener(this.AcceptDeliverableItem);
            this.cancel.onClick.AddListener(this.CloseDeliveryPanel);
            this.requestMessage.text = this.quest.deliveryGoal.deliveryMessage;
            this.collectibleImage.sprite = Resources.Load<Sprite>("Collectibles/Items/" + this.quest.deliveryGoal.item.itemName);
            this.itemName.text = this.quest.deliveryGoal.item.itemName;
            this.itemQuantity.text = "x" + this.quest.deliveryGoal.item.quantity;
        });
    }

    void CloseDeliveryPanel()
    {
        // Show joystick and inventory panel
        this.UICanvasGroup.interactable = true;
        this.joystick.SetActive(true);
        this.inventoryPanel.SetActive(true);
        // Enable again the button
        this.talkButton.enabled = true;
        this.deliveryReqButton.enabled = true;
        this.giveItemButton.enabled = true;

        LeanTween.scale(this.fedexPanel.gameObject, Vector2.zero, .2f)
        .setEaseSpring()
        .setOnComplete(() =>
        {
            this.accept.onClick.RemoveAllListeners();
            this.cancel.onClick.RemoveAllListeners();
        });
    }

    void AcceptDeliverableItem()
    {
        // Show joystick and inventory panel
        this.UICanvasGroup.interactable = true;
        this.joystick.SetActive(true);
        this.inventoryPanel.SetActive(true);

        this.talkButton.enabled = true;
        this.deliveryReqButton.enabled = true;
        this.giveItemButton.enabled = true;

        LeanTween.scale(this.fedexPanel.gameObject, Vector2.zero, .2f)
        .setEaseSpring()
        .setOnComplete(() =>
        {
            this.SetItemAsReceived();
            this.CloseDeliveryPanel();
            this.deliveryReqButton.gameObject.SetActive(false);
        });
    }
    
    void SetItemAsReceived()
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            // check if ang quest ay Delivery Quest based sa pagcheck if existing ang deliverGoalId.
            if (quest.questID == this.quest.questID && quest.deliveryGoal.deliverGoalId.Length > 0)
            {
                quest.deliveryGoal.itemReceivedFromGiver = true; // set to true
                // reset ulit ang lahat ng gameobject na may handle ng DeliveryGoalGiver and DeliveryGoalReceiver.
                QuestManager.instance.SetupScriptsForDeliveryQuestToNPCs();
                DataPersistenceManager.instance.playerData.inventory.AddItem(quest.deliveryGoal.item.CopyItem());
                InventoryManager.instance.DisplayInventoryItems();
                return;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /** All NPC ay pwede maging Giver and Receiver ng item,
         * so if the quest is null, it means na hindi kabilang ang
         NPC gameobject na ito sa kahit anong quest na makikita sa pending
        quest ni player. */
        if (this.quest is null) return;

        // Only show the help button if the quest item is not yet given.
        if (collision.gameObject.CompareTag("Player") 
             && this.quest != null
             && !this.quest.deliveryGoal.itemReceivedFromGiver)
        {
            this.deliveryReqButton.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.deliveryReqButton.gameObject.SetActive(false);
        }
    }
}
