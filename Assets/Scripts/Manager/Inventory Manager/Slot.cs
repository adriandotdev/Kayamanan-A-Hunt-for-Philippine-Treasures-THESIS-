using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            /** If this game object index is >= to the number of items in the List then we set the 
             SlotItem property 'isCorrectlyDropped' to false and terminate execution of program flow. */
            if (gameObject.transform.GetSiblingIndex() >= 
                DataPersistenceManager.instance.playerData.inventory.items.Count)
            {
                print("SLOT ITEM IS NULL: " + eventData.pointerDrag.GetComponent<SlotItem>() is null);
                eventData.pointerDrag.GetComponent<SlotItem>().isCorrectlyDropped = false;
                return;
            }

            // Otherwise, we set it to True.
            eventData.pointerDrag.GetComponent<SlotItem>().isCorrectlyDropped = true;

            // Check if a slot is greater than to the number of items in the
            if (gameObject.transform.childCount < 2)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.SetAsFirstSibling();
                eventData.pointerDrag.transform.localPosition = Vector3.zero;
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.SetAsFirstSibling();
                eventData.pointerDrag.transform.localPosition = Vector3.zero;

                transform.GetChild(1).SetParent(eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform);

                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition
                    = eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).parent.GetComponent<RectTransform>().anchoredPosition;

                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).localPosition = Vector3.zero;
                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).SetAsFirstSibling();
            }
            
        }
        
    }
}
