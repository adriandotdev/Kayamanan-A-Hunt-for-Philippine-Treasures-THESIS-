using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMono : MonoBehaviour
{
    public Item item;

    private void Start()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            DataPersistenceManager.instance.playerData.inventory.AddItem(this.item.CopyItem());

            InventoryManager.instance.DisplayInventoryItems();

            Destroy(gameObject);

            QuestManager.instance.OpenPlainAlertBox("You've found an item!");
        });      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }
}
