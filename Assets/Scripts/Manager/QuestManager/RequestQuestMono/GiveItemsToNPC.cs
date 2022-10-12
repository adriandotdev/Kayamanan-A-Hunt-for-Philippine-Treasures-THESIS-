using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GiveItemsToNPC : MonoBehaviour
{
    public Button showInfoOfItemsButton;
    public string npcName;

    private void Start()
    {
        this.showInfoOfItemsButton = transform.GetChild(0).GetChild(3).GetComponent<Button>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ItemGiver[] itemGiverFromRequestRequesterComponent = GetComponent<RequestRequester>().requestQuest.requestGoal.itemGivers;

        if (GetComponent<RequestRequester>() != null && npcName == gameObject.name)
        {
            Quest requestQuest = GetComponent<RequestRequester>().requestQuest;

            if (collision.gameObject.CompareTag("Player")
                && requestQuest != null && requestQuest.questID.Length > 0
                && GetComponent<RequestRequester>().isAllItemsGathered() && requestQuest.requestGoal.isItemReceivedOfNpc == false)
                {
                this.showInfoOfItemsButton.onClick.AddListener(() =>
                    {
                        for (int i = 0; i < itemGiverFromRequestRequesterComponent.Length; i++)
                        {
                            for (int j = 0; j < itemGiverFromRequestRequesterComponent[i].itemsToGive.Count; j++)
                            {
                                InventoryManager.instance.DeleteItem(itemGiverFromRequestRequesterComponent[i].itemsToGive[j]);
                            }
                        }
                        InventoryManager.instance.DisplayInventoryItems();
                        GetComponent<RequestRequester>().giveItemToNpcBtn.gameObject.SetActive(false);
                        GetComponent<RequestRequester>().requestQuest.requestGoal.isItemReceivedOfNpc = true;
                        QuestManager.instance.FindRequestQuest(GetComponent<RequestRequester>().requestQuest.questID);
                        QuestManager.instance.SetRequestItemsAreGiven(GetComponent<RequestRequester>().requestQuest.questID);
                    });
                }
                else if (collision.gameObject.CompareTag("Player") && requestQuest != null
                    && requestQuest.questID.Length > 0 && GetComponent<RequestRequester>().isAllItemsGathered() 
                    && requestQuest.requestGoal.isItemReceivedOfNpc == true)
                {
                    this.showInfoOfItemsButton.onClick.RemoveAllListeners();
                    this.showInfoOfItemsButton.onClick.AddListener(() =>
                    {
                        AlbumManager.Instance.itemGivers = GetComponent<RequestRequester>().requestQuest.requestGoal.itemGivers;
                        SceneManager.LoadScene("Delivery Info Scene", LoadSceneMode.Additive);
                    });
                }
            }
        }
}
