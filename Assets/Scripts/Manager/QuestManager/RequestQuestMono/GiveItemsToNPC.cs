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
        if (GetComponent<RequestRequester>().requestQuest != null)
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
                this.showInfoOfItemsButton.onClick.RemoveAllListeners();
                this.showInfoOfItemsButton.onClick.AddListener(() =>
                    {
                        print("GIVE ITEMS TO NPC");
                        for (int i = 0; i < itemGiverFromRequestRequesterComponent.Length; i++)
                        {
                            for (int j = 0; j < itemGiverFromRequestRequesterComponent[i].itemsToGive.Count; j++)
                            {
                                InventoryManager.instance.DeleteItem(itemGiverFromRequestRequesterComponent[i].itemsToGive[j]
                                    , itemGiverFromRequestRequesterComponent[i].itemsToGive[j].quantity);
                            }
                        }
                        InventoryManager.instance.DisplayInventoryItems();
                        GetComponent<RequestRequester>().giveItemToNpcBtn.gameObject.SetActive(false);
                        GetComponent<RequestRequester>().requestQuest.requestGoal.isItemReceivedOfNpc = true;
                        QuestManager.instance.FindRequestQuest(GetComponent<RequestRequester>().requestQuest.questID);
                        QuestManager.instance.SetRequestItemsAreGiven(GetComponent<RequestRequester>().requestQuest.questID);
                        AlbumManager.Instance.itemGivers = GetComponent<RequestRequester>().completedQuestToGainInfo.requestGoal.itemGivers;

                        if (requestQuest.region.ToUpper() != "PRE-QUEST")
                        {
                            AlbumManager.Instance.isFirstAlbum = true;
                            SceneManager.LoadScene("Delivery Info Scene", LoadSceneMode.Additive);
                        }
                    });
                }
                else if (collision.gameObject.CompareTag("Player") && GetComponent<RequestRequester>().completedQuestToGainInfo.questID.Length > 0)
                {
                    this.showInfoOfItemsButton.onClick.RemoveAllListeners();
                    this.showInfoOfItemsButton.onClick.AddListener(() =>
                    {
                        print("GIVE ITEMS TO NPC 2");

                        AlbumManager.Instance.itemGivers = GetComponent<RequestRequester>().completedQuestToGainInfo.requestGoal.itemGivers;
                        AlbumManager.Instance.isFirstAlbum = true;
                        SceneManager.LoadScene("Delivery Info Scene", LoadSceneMode.Additive);
                    });
                }
            }
        }
}
