using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationObserver : MonoBehaviour
{
    public int iconIndex = 0;

    private void Start()
    {
        // Quest Icon
        if (iconIndex == 0)
        {
            if (DataPersistenceManager.instance.playerData.questNewIcon)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        // Achievements Icon
        else if (iconIndex == 1)
        {
            if (DataPersistenceManager.instance.playerData.achievementsNewIcon)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (DataPersistenceManager.instance.playerData.notesNewIcon)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
