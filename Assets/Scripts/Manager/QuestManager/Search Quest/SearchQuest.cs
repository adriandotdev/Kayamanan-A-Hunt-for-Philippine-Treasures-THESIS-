using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchQuest : MonoBehaviour
{
    public Quest searchQuest;
    public Button giveItemToNPCBtn;
    public Button requestBtn;

    private void Start()
    {
        this.giveItemToNPCBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();
        this.requestBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();

        this.requestBtn.onClick.AddListener(() => Debug.Log("Request Type of Search"));
        this.giveItemToNPCBtn.onClick.AddListener(GiveItem);
    }

    public void GiveItem()
    {
        InventoryManager.instance?.DeleteItem(this.searchQuest.searchGoal.itemToFind.CopyItem()
            , this.searchQuest.searchGoal.itemToFind.quantity);

        InventoryManager.instance?.DisplayInventoryItems();
    }

    public bool IsPlayerHasItemNeeded()
    {
        foreach (Item item in DataPersistenceManager.instance.playerData.inventory.items)
        {
            if (item.itemName.ToUpper() == this.searchQuest.searchGoal.itemToFind.itemName.ToUpper()) return true;
        }
        return false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && searchQuest != null && IsPlayerHasItemNeeded() && searchQuest.searchGoal.typesOfSearch == "clue")
        {
            this.giveItemToNPCBtn.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Player")
            && searchQuest != null && IsPlayerHasItemNeeded() == false
            && searchQuest.searchGoal.typesOfSearch == "request")
        {
            this.requestBtn.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.giveItemToNPCBtn.gameObject.SetActive(false);
            this.requestBtn.gameObject.SetActive(false);
        }
    }
}
