using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTrigger : MonoBehaviour
{
    public Quest requestQuest;
    public ItemGiver itemGiver;
    public Button giveItemToPlayerBtn;
    public Button acceptRequestBtn;

    private void Start()
    {
        this.requestQuest = transform.parent.GetComponent<RequestGiver>().requestQuest;
        this.itemGiver = transform.parent.GetComponent<RequestGiver>().itemGiver;
        this.giveItemToPlayerBtn = transform.parent.GetComponent<RequestGiver>().giveItemToPlayerBtn;
        this.acceptRequestBtn = transform.parent.GetComponent<RequestGiver>().acceptRequestBtn;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && this.requestQuest != null && this.requestQuest.questID.Length > 0 && this.requestQuest.requestGoal.isRequestFromNPCGained
            && this.itemGiver.isItemsGiven == false)
        {
            this.giveItemToPlayerBtn.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
           && requestQuest != null && this.requestQuest.questID.Length > 0)
        {
            gameObject.transform.parent.GetComponent<RequestGiver>().HideAllButtons();
            this.acceptRequestBtn.onClick.RemoveAllListeners();
        }
    }
}
