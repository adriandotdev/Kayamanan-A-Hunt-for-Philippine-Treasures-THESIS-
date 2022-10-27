using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryGoalReceiver : MonoBehaviour
{
    private Button giveBtn;
    public Quest quest; // reason ay para ma set ang quest as completed by questID

    private void Start()
    {
        this.giveBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();

        if (this.quest != null)
            this.giveBtn.onClick.AddListener(this.GiveItemToNPC);
    }

    void GiveItemToNPC()
    {
        if (this.quest != null && this.quest.questID.Length > 0 && this.quest.isCompleted
            && this.quest.deliveryGoal.wayOfInfo.ToUpper() == "text".ToUpper())
        {
            DialogueManager._instance?.StartDialogue(Resources.Load<TextAsset>(this.quest.deliveryGoal.informationLinkWhenInfoHasBeenSeen));
            return;
        }

        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            if (quest.questID == this.quest.questID && quest.questType == Quest.QUEST_TYPE.DELIVERY)
            {
                // If the WAY OF INFO IS ONLY PURE DIALOGUE.
                if (this.quest.deliveryGoal.wayOfInfo.ToUpper() == "text".ToUpper())
                {
                    DialogueManager._instance.actorField.text= this.quest.deliveryGoal.receiverName;
                    DialogueManager._instance.npcName = this.quest.deliveryGoal.receiverName;
                    DialogueManager._instance?.StartDialogue(Resources.Load<TextAsset>(this.quest.deliveryGoal.informationLink));
                }
                else
                {
                    // IF THE WAY OF INFO IS DIALOGUE WITH PHOTO ALBUM
                    DialogueManager._instance.deliveryGoal = this.quest.deliveryGoal.Copy();
                    DialogueManager._instance.actorField.text = this.quest.deliveryGoal.receiverName;
                    DialogueManager._instance.npcName = this.quest.deliveryGoal.receiverName;
                    DialogueManager._instance?.StartDialogue(Resources.Load<TextAsset>(this.quest.deliveryGoal.informationLink));


                }
                //// Loop through each item at inventory
                foreach (Item item in DataPersistenceManager.instance.playerData.inventory.items)
                {
                    // If the quest item is equal to the 'current item' pointer
                    if (item.itemName.ToUpper() == this.quest.deliveryGoal.item.itemName.ToUpper() && 
                        quest.deliveryGoal.deliverGoalId == this.quest.deliveryGoal.deliverGoalId && quest.questType == Quest.QUEST_TYPE.DELIVERY
                        && this.quest.questType == Quest.QUEST_TYPE.DELIVERY)
                    {
                        item.quantity -= this.quest.deliveryGoal.item.quantity;

                        if (item.quantity <= 0)
                            DataPersistenceManager.instance.playerData.inventory.items.RemoveAll(toRemove => toRemove.itemName.ToUpper()
                            == item.itemName.ToUpper());
                        break;
                    }
                }

                this.quest.isCompleted = true;

                this.giveBtn.gameObject.SetActive(false);
                // Find the Delivery Quest with a specific ID.
                QuestManager.instance.FindDeliveryQuestGoal(this.quest.questID);

                // Reset all gameobjects holding a DeliveryGoal Giver and Receiver.
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
        if (this.quest == null) return;

        if (collision.gameObject.CompareTag("Player") 
            && this.quest.deliveryGoal.itemReceivedFromGiver 
            && !this.quest.isCompleted)
        {
            //this.giveBtn.transform.GetChild(0).GetComponent<Image>().sprite 
            //    = Resources.Load<Sprite>("Collectibles/Items/" + this.quest.deliveryGoal.item.itemName);
            this.giveBtn.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Player")
            && this.quest.isCompleted)
        {
            this.giveBtn.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.quest == null) return;

        if (collision.gameObject.CompareTag("Player")
            && this.quest.isCompleted)
        {
            this.giveBtn.onClick.RemoveAllListeners();
            this.giveBtn.onClick.AddListener(this.GiveItemToNPC);
        } 
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.giveBtn.gameObject.SetActive(false);
        }
    }
}
