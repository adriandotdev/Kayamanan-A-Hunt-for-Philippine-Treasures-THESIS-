using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    // Inventory
    [HideInInspector] public RectTransform inventoryPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnPlaySceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnPlaySceneLoaded;
    }

    void OnPlaySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House") 
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Museum")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Church")
            && DataPersistenceManager.instance.playerData.isTutorialDone == true)
        {
            this.inventoryPanel = GameObject.Find("Inventory Panel").GetComponent<RectTransform>();
            this.DisplayInventoryItems();
        }
    }

    public void DisplayInventoryItems()
    {
        this.ResetInventoryItems();
        
        List<Item> inventory = DataPersistenceManager.instance.playerData.inventory.items;

        for (int i = 0; i < inventory.Count; i++)
        {
            // first child is the image
            // second child is the quantity text
            Transform slot = this.inventoryPanel.GetChild(i);

            slot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + inventory[i].itemName);

            // Needed to update the Inventory UI when the inventory item gets swapped.
            slot.GetChild(0).GetComponent<SlotItem>().itemName = inventory[i].itemName;

            slot.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = inventory[i].quantity.ToString();
        }
    }

    public void DeleteItem(Item itemToRemove, int quantityToDeduct)
    {
        List<Item> inventory = DataPersistenceManager.instance.playerData.inventory.items;

        Item itemFound = inventory.Find(item => item.itemName == itemToRemove.itemName);

        if (itemFound != null)
        {
            itemFound.quantity -= quantityToDeduct;

            if (itemFound.quantity <= 0)
                inventory.RemoveAll(itemToRemoveInInventory => itemToRemoveInInventory.itemName == itemToRemove.itemName);
        }
    }

    public void ResetInventoryItems()
    { 
        foreach (Transform slot in this.inventoryPanel.transform)
        {
            slot.GetChild(0).GetComponent<Image>().sprite = null;
            slot.GetChild(0).GetComponent<SlotItem>().itemName = "";
            slot.GetChild(0).GetComponent<SlotItem>().isCorrectlyDropped = false;
            slot.GetChild(0).GetComponent<SlotItem>().parentOfSlot = null;
            slot.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "0";
        }
    }
}
