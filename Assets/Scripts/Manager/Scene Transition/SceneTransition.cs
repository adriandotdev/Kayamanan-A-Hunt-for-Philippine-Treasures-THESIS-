using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [HideInInspector] 
    public enum ESTABLISHMENTS_TYPE { HOUSE, PUBLIC_PLACES_ENTER }
    public string sceneToLoad;
    public string nameOfExit;

    public ESTABLISHMENTS_TYPE m_EstablishmentType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && SceneTransitionManager.instance.fromEnter == false)
        {
            // Check if it is PUBLIC PLACES and if it is opening hours.
            if (ESTABLISHMENTS_TYPE.PUBLIC_PLACES_ENTER == m_EstablishmentType && !TimeManager.instance.playerData.playerTime.m_IsAllEstablishmentsOpen)
            {
                print(gameObject.name + " is closed.");
                return;
            }

            // Check if not null 
            if (SceneTransitionManager.instance != null)
            {
                SceneTransitionManager.instance.nameOfExit = nameOfExit;
                SceneTransitionManager.instance.fromEnter = true;
            }
            SceneManager.LoadScene(this.sceneToLoad);
        }
    }
}
