using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [HideInInspector] 
    public enum ESTABLISHMENTS_TYPE { HOUSE, PUBLIC_PLACES_ENTER }
    public string sceneToLoad;
    public string nameOfExit;

    public ESTABLISHMENTS_TYPE m_EstablishmentType;

    public string nameOfBuilding;
    public RectTransform closeBuildingPanel;
    public Coroutine openingCloseBuildingPanel;

    private void Start()
    {
        try
        {
            this.closeBuildingPanel = GameObject.Find("Close Building Panel").GetComponent<RectTransform>();
            this.closeBuildingPanel.transform.localScale = Vector2.zero;
        }
        catch (System.Exception e) {  }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && SceneTransitionManager.instance.fromEnter == false)
        {
            // Check if it is PUBLIC PLACES and if it is opening hours.
            if (ESTABLISHMENTS_TYPE.PUBLIC_PLACES_ENTER == m_EstablishmentType && !TimeManager.instance.playerData.playerTime.m_IsAllEstablishmentsOpen)
            {
                if (this.openingCloseBuildingPanel != null)
                    StopCoroutine(openingCloseBuildingPanel);

                this.closeBuildingPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.nameOfBuilding + " is closed.";
                LeanTween.scale(this.closeBuildingPanel.gameObject, Vector2.one, .2f)
                    .setEaseSpring();
                this.openingCloseBuildingPanel = StartCoroutine(this.Close());
                return;
            }

            // Check if not null 
            if (SceneTransitionManager.instance != null)
            {
                SceneTransitionManager.instance.nameOfExit = nameOfExit;
                SceneTransitionManager.instance.fromEnter = true;
            }
            //SceneManager.LoadScene(this.sceneToLoad);
            TransitionLoader.instance?.StartAnimation(this.sceneToLoad);
        }
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);

        LeanTween.scale(this.closeBuildingPanel.gameObject, Vector2.zero, .2f)
                    .setEaseSpring();
    }
}
