using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex;

    private int firstPopupIndex = 0;
    private int dunongPointsPopupIndex = 0;
    public Joystick joystick;

    private void Start()
    {
        if (DataPersistenceManager.instance.playerData.isTutorialDone == false)
        {
            GameObject.Find("Quest Panel").SetActive(false);
            GameObject.Find("Alert Box").SetActive(false);
            this.popups[this.popupIndex].SetActive(true);
            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).gameObject, Vector2.one, .2f)
                .setDelay(1f).setEaseSpring();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DataPersistenceManager.instance.playerData.isTutorialDone == true)
        {
            for (int i = 0; i < popups.Length; i++)
            {
                popups[i].SetActive(false);
            }
            return;
        }

        this.joystick.gameObject.SetActive(false);

        // Hides the popups that is not equal to the current
        // value of index.
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == this.popupIndex)
            {
                popups[i].SetActive(true);
            }
            else
            {
                popups[i].SetActive(false);
            }
        }

        if (this.popupIndex < 1)
        {
            if (Input.touchCount == 1)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selected = EventSystem.current.currentSelectedGameObject;

                    if (selected != null &&  selected.name == "Next Step")
                    {

                        this.firstPopupIndex++;

                        if (this.firstPopupIndex == 1)
                        {
                            GameObject obj = this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;

                            TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();

                            text.text = "NOW LET'S START!";
                        }

                        else if (this.firstPopupIndex == 2)
                        {
                            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).gameObject, Vector2.zero, .5f)
                                .setEaseSpring()
                                .setOnComplete(() => {

                                    this.popups[this.popupIndex].SetActive(false);

                                   
                                });
                            this.popupIndex++;
                        }
                    }
                    selected = null;
                }
            }
        }

        // Quest Button Tutorial
        else if (this.popupIndex == 1)
        {
            NextPopup(null, true);
        }
        // Inventory Panel
        else if (this.popupIndex == 2)
        {
            NextPopup(null, true);
        }
        else if (this.popupIndex == 3)
        {
            NextPopup(null, true);
        }
        else if (this.popupIndex == 4)
        {
            NextPopup(DunongPointsPopup, false);
        }
        else if (this.popupIndex == 5)
        {
            NextPopup(FinishTutorial, true);
        }
    }

    public void DunongPointsPopup()
    {
        this.dunongPointsPopupIndex++;

        if (this.dunongPointsPopupIndex == 1)
        {
            GameObject obj = this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).gameObject;

            TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();

            text.text = "You must complete all the quests in order to gain dunong points . This will be used to take assessments at the  school event.";
        }

        else if (this.dunongPointsPopupIndex == 2)
        {
            //LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).gameObject, Vector2.zero, .5f)
            //    .setEaseSpring()
            //    .setOnComplete(() =>
            //    {

                    this.popups[this.popupIndex].SetActive(false);
                    this.popupIndex++;
                //});
        }
    }

    public void FinishTutorial()
    {
        DataPersistenceManager.instance.playerData.isTutorialDone = true;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("House");
    }

    /** THIS FUNCTION is for setting up the popups events. Since some the popups has the same behaviour
     we only set new popup.*/
    public void NextPopup(Action popupEventToExecute, bool isOnceToShow)
    {
        LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0)
           .GetChild(0).GetChild(1).gameObject, Vector2.one, .5f)
           .setEaseSpring();

        if (Input.touchCount == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;

                if (selected != null && selected.name == "Next Step")
                {
                    if (isOnceToShow)
                    {
                        this.popups[this.popupIndex].SetActive(false);
                        this.popupIndex++;
                    }
                    popupEventToExecute?.Invoke();
                }
                selected = null;
            }
        }
    }
}
