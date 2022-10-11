using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCIntroduction : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject npcIntroductionPanel;
    [SerializeField] private GameObject[] npcIntroPopups;
    [SerializeField] private Button nextPopupBTN;

    // Canvas Group
    private CanvasGroup canvasGroupOfUI;

    // Inventory Panel && Joystick
    private GameObject inventoryPanel;
    private GameObject joystick;

    // PlayerData
    private PlayerData playerData;

    // INDEX
    private int currentIndex;

    private void Start()
    {
        if (this.playerData.isPreQuestIntroductionDone || this.playerData.isNPCIntroductionPanelDone) return;

        this.currentIndex = 0;

        this.nextPopupBTN.onClick.AddListener(NextPopup);

        this.canvasGroupOfUI = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.inventoryPanel = GameObject.Find("Inventory Panel");
        this.joystick = GameObject.Find("Fixed Joystick");

        LeanTween.scale(this.npcIntroductionPanel, new Vector2(178.5917f, 178.5917f), .2f)
            .setEaseSpring().setOnComplete(() => {
                this.inventoryPanel.SetActive(false);
                this.joystick.SetActive(false);
            });

        this.canvasGroupOfUI.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerData.isPreQuestIntroductionDone || this.currentIndex > this.npcIntroPopups.Length - 1) return;

        for (int i = 0; i < this.npcIntroPopups.Length; i++)
        {
            // IF the currentIndex is NOT EQUAL to variable 'i', then we hide it.
            if (i != currentIndex)
            {
                this.npcIntroPopups[i].SetActive(false);
            }
        }    

        /** NOTE: number of cases is the sequence of popup */
        switch (this.currentIndex)
        {
            case 1:
                this.npcIntroPopups[currentIndex].SetActive(true);
                break;
            case 2:
                this.npcIntroPopups[currentIndex].SetActive(true);
                break;
            case 3:
                this.npcIntroPopups[currentIndex].SetActive(true);
                break;
            case 4:
                this.npcIntroPopups[currentIndex].SetActive(true);
                break;
        }
    }

    private void NextPopup()
    {
        this.currentIndex++; // Increment

        // IF the currentIndex is GREATER THAN the length of popups, then we close the panel.
        if (this.currentIndex > this.npcIntroPopups.Length - 1)
        {
            LeanTween.scale(this.npcIntroductionPanel, Vector2.zero, .2f)
             .setEaseSpring();

            this.canvasGroupOfUI.interactable = true;
            this.inventoryPanel.SetActive(true);
            this.joystick.SetActive(true);
            this.playerData.isNPCIntroductionPanelDone = true;
        }
    }
    public void LoadPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }

    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }
}
