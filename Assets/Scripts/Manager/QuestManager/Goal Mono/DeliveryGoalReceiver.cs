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

        this.giveBtn.onClick.AddListener(this.GiveItem);
    }

    void GiveItem()
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            if (quest.questID == this.quest.questID && quest.questType == Quest.QUEST_TYPE.DELIVERY)
            {
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
                QuestManager.instance.SetupScriptsForDeliveryQuestToNPCs();
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
            this.giveBtn.transform.GetChild(0).GetComponent<Image>().sprite 
                = Resources.Load<Sprite>("Collectibles/Items/" + this.quest.deliveryGoal.item.itemName);
            this.giveBtn.gameObject.SetActive(true);
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
