using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Rendering.Universal;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    // Events
    public static Action<string> OnAllQuestCompleted;

    public static QuestManager instance;
    //public List<Quest> quests;

    // Events
    public static Action UpdateDPValueUI;
    public static Action ShowItemReceivedPopup;

    public GameObject questPrefab;
    
    [Header("Number Quest Prefab")]
    public GameObject questNumberGoalPrefab;

    [Header("Request Prefab")]
    public GameObject requestPrefabWithNoHint;
    public GameObject requestPrefabWithHint;
    public GameObject requestItemImagePrefab;

    private GameObject questAlertBox;
    private GameObject plainAlertBox;

    Coroutine questAlertCoroutine;
    Coroutine plainAlertCoroutine;

    public PlayerData playerData;

    // Buttons for Showing Icons
    public Transform notesIcon;

    public TextMeshProUGUI questDoneNoteInPanel;

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
        SceneManager.sceneLoaded += OnSceneRequiresQuestDataLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneRequiresQuestDataLoaded;
    }

    void OnSceneRequiresQuestDataLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Museum")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Church"))
        {

            if (scene.name == "House")
            {
                if (DataPersistenceManager.instance.playerData.isTutorialDone == false)
                    return;
            }

            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
            this.SetupScriptsForDeliveryQuestToNPCs();
            this.SetupScriptsForRequestQuest();
            this.SetupShowPhotoAlbumQuest();
            this.SetupSearchQuest();
            this.SetupAllNotes();
        }
    }

    /** Gets all the quest that is related to the 
     current open region. */
    public void GetListOfQuests()
    {
        // This is the first quest for Day 1.
        if (this.playerData.isPreQuestIntroductionDone == false)
        {
            foreach (Quest quest in this.playerData.quests)
            {
                if (quest.region.ToUpper() == "PRE-QUEST".ToUpper())
                {
                    // Check if the current 'quest' is not yet in the 'currentQuest' list.
                    Quest foundQuest = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID);

                    if (foundQuest == null)
                    {
                        this.playerData.currentQuests.Add(quest);
                    }
                }
            }
            return;
        }

        // Get the current open region.
        string currentRegion = this.GetCurrentOpenRegion();

        // Loop to the list of quest.
        foreach (Quest quest in this.playerData.quests)
        {
            if (quest.region.ToUpper() == currentRegion.ToUpper())
            {
                // Check if the current 'quest' is not yet in the 'currentQuest' list.
                Quest foundQuest = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID);

                if (foundQuest == null)
                {
                    this.playerData.currentQuests.Add(quest);
                }
            }
        }
    }

    /** Get the last open region */
    public string GetCurrentOpenRegion()
    {
        string regionNameOpened = "Ilocos Region";

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.isOpen)
            {
                regionNameOpened = regionData.regionName;
            }
        }
        return regionNameOpened;
    }

    public void GetAllNecessaryGameObjects()
    {
        try
        {

            GameObject inventoryPanel = GameObject.Find("Inventory Panel");
            GameObject fixedJoystick = GameObject.Find("Fixed Joystick");

            RectTransform questContentScrollView = GameObject.Find("Quest Content Scroll View").GetComponent<RectTransform>();

            Transform questPanel = GameObject.Find("Quest Panel").transform;

            CanvasGroup houseCanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
            Button openQuestPanel = GameObject.Find("Quest Button").GetComponent<Button>();
            Button pendingBtn = GameObject.Find("Pending").GetComponent<Button>();
            Button completedBtn = GameObject.Find("Completed").GetComponent<Button>();
            Button closeQuestPanel = questPanel.GetChild(3).GetComponent<Button>();

            this.questAlertBox = GameObject.Find("Alert Box");
            this.plainAlertBox = GameObject.Find("Plain Alert Box");
            this.notesIcon = GameObject.Find("Notes Button").transform;
            this.questDoneNoteInPanel = GameObject.Find("Completed QUEST NOTE").GetComponent<TextMeshProUGUI>();
            // Add event to pending btn. IT IS LOCATED INSIDE THE QUEST PANEL.
            pendingBtn.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 2");
                this.RemoveAllQuest(questContentScrollView); // reset
                this.DisplayQuestPrefabAtQuestPanel(questContentScrollView);

                this.ChangeButtonColor(pendingBtn, completedBtn);
            });

            // Add event to the completed button. IT IS LOCATED INSIDE THE QUEST PANEL.
            completedBtn.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 2");
                this.RemoveAllQuest(questContentScrollView);
                this.GetAllCompletedQuest(questContentScrollView);

                this.ChangeButtonColor(completedBtn, pendingBtn);
            });

            closeQuestPanel.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                LeanTween.scale(questPanel.gameObject, Vector2.zero, .2f)
                .setEaseSpring()
                .setOnComplete(() =>
                {
                    this.RemoveAllQuest(questContentScrollView);
                    inventoryPanel.SetActive(true);
                    fixedJoystick.SetActive(true);
                    houseCanvasGroup.interactable = true;
                    houseCanvasGroup.blocksRaycasts = true;
                });
                this.ChangeButtonColor(pendingBtn, completedBtn);
            });

            openQuestPanel.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                DataPersistenceManager.instance.playerData.questNewIcon = false; // set the questNewIcon property to false to hide the icon.
                openQuestPanel.transform.GetChild(0).gameObject.SetActive(false); // Hide the 'New' TEXT.

                this.RemoveAllQuest(questContentScrollView);
                this.DisplayQuestPrefabAtQuestPanel(questContentScrollView);
                LeanTween.scale(questPanel.gameObject, new Vector2(296.8722f, 296.8722f), .2f)
                    .setEaseSpring();
                inventoryPanel.SetActive(false);
                fixedJoystick.SetActive(false);
                houseCanvasGroup.interactable = false;
                houseCanvasGroup.blocksRaycasts = false;

                if (DataPersistenceManager.instance.playerData.currentQuests.Count < 1)
                {
                    this.questDoneNoteInPanel.gameObject.SetActive(true);
                }
                else
                {
                    this.questDoneNoteInPanel.gameObject.SetActive(false);
                }
            });

            this.ChangeButtonColor(pendingBtn, completedBtn);
            this.questAlertBox.transform.localScale = Vector2.zero;
            this.plainAlertBox.transform.localScale = Vector2.zero;
            questPanel.transform.localScale = Vector2.zero;


        }
        catch (System.Exception e) { Debug.Log(e.Message); }
    }

    /** This function is responsible for changing the color
     of 'Pending' and 'Completed' buttons in Quest Panel. */
    void ChangeButtonColor(Button buttonToChange, Button buttonToRevert)
    {
        // Setup the color for selecting button.
        Color selectedColor;
        ColorUtility.TryParseHtmlString("#331313", out selectedColor);

        buttonToChange.GetComponent<Image>().color = selectedColor;
        buttonToChange.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        buttonToRevert.GetComponent<Image>().color = Color.white;
        buttonToRevert.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = selectedColor;
    }

    /**
     * <summary>
     *  Displays the quest prefab at request panel.
     *  
     *  <paramref name="content"/> The RectTransform that holds all of the quest prefabs.
     * </summary> 
     */
    public void DisplayQuestPrefabAtQuestPanel(RectTransform content)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            GameObject questSlot;

            switch (quest.questType)
            {

                case Quest.QUEST_TYPE.NUMBER: 

                    questSlot = Instantiate(questNumberGoalPrefab, content.transform);

                    questSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                        .text = quest.title;
                    questSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.description;
                    questSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>()
                        .text = quest.numberGoal.currentNumber + "/" + quest.numberGoal.targetNumber;
                    questSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();
                    break;

                case Quest.QUEST_TYPE.REQUEST:

                    if (quest.hint.Length > 0)
                        questSlot = Instantiate(this.requestPrefabWithHint, content.transform);
                    else
                        questSlot = Instantiate(this.requestPrefabWithNoHint, content.transform);

                    questSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                        .text = quest.title;
                    questSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.description;
                    questSlot.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();
                    questSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.requestGoal.isRequestFromNPCGained == false ? "Pending"
                        : this.isAllItemsGatheredForRequestQuest(quest.requestGoal.itemGivers) ? "Items Received" : "Request Accepted";
                    foreach (ItemGiver giver in quest.requestGoal.itemGivers)
                    {
                        foreach (Item item in giver.itemsToGive)
                        {
                            if (item.itemName != "None")
                            {
                                GameObject itemImage = Instantiate(this.requestItemImagePrefab, questSlot.transform.GetChild(4).GetChild(1));
                                itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + item.itemName);
                            }
                        }
                    }

                    if (quest.hint.Length > 0)
                        questSlot.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;

                    break;

                case Quest.QUEST_TYPE.DELIVERY:
                    if (quest.hint.Length > 0)
                        questSlot = Instantiate(this.requestPrefabWithHint, content.transform);
                    else
                        questSlot = Instantiate(this.requestPrefabWithNoHint, content.transform);

                    questSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                        .text = quest.title;

                    questSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.description;

                    questSlot.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();

                    questSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>()
                       .text = quest.deliveryGoal.itemReceivedFromGiver == false ? "Pending" : "Items Received";

                    foreach (Item item in quest.deliveryGoal.items)
                    {
                        GameObject itemImage = Instantiate(this.requestItemImagePrefab, questSlot.transform.GetChild(4).GetChild(1));
                        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + item.itemName);
                    }

                    if (quest.hint.Length > 0)
                        questSlot.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;
                    break;
                default:
                    questSlot = Instantiate(questPrefab, content.transform);

                    // Title of Quest
                    questSlot.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.title;
                    // Description of Quest
                    questSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.description;

                    questSlot.transform.GetChild(2).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();

                    if (quest.hint.Length > 0)
                    {
                        questSlot.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;
                    }
                    else
                    {
                        questSlot.transform.GetChild(3).gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    public bool isAllItemsGatheredForRequestQuest(ItemGiver[] itemGivers)
    {
        foreach (ItemGiver itemGiver in itemGivers)
        {
            if (itemGiver.isItemsGiven == false) return false;
        }

        return true;
    }

    /** Display all the complete quest at Quest Panel. */
    public void GetAllCompletedQuest(RectTransform content)
    {
        GameObject questSlot;

        foreach (Quest quest in this.playerData.completedQuests)
        {
            switch (quest.questType)
            {
                case Quest.QUEST_TYPE.REQUEST:

                    if (quest.hint != null && quest.hint.Length > 0)
                        questSlot = Instantiate(this.requestPrefabWithHint, content.transform);
                    else
                        questSlot = Instantiate(this.requestPrefabWithNoHint, content.transform);

                    questSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                        .text = quest.title;
                    questSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.description;
                    questSlot.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();
                    questSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = "COMPLETED";

                    foreach (ItemGiver giver in quest.requestGoal.itemGivers)
                    {
                        foreach (Item item in giver.itemsToGive)
                        {
                            if (item.itemName != "None")
                            {
                                GameObject itemImage = Instantiate(this.requestItemImagePrefab, questSlot.transform.GetChild(4).GetChild(1));
                                itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + item.itemName);
                            }
                        }
                    }

                    if (quest.hint != null && quest.hint.Length > 0)
                        questSlot.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;
                    break;

                case Quest.QUEST_TYPE.DELIVERY:

                    if (quest.hint != null && quest.hint.Length > 0)
                        questSlot = Instantiate(this.requestPrefabWithHint, content.transform);
                    else
                        questSlot = Instantiate(this.requestPrefabWithNoHint, content.transform);

                    questSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                        .text = quest.title;

                    questSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.description;

                    questSlot.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();

                    questSlot.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>()
                       .text = quest.deliveryGoal.itemReceivedFromGiver == false ? "Pending" : "Items Received";

                    foreach (Item item in quest.deliveryGoal.items)
                    {
                        GameObject itemImage = Instantiate(this.requestItemImagePrefab, questSlot.transform.GetChild(4).GetChild(1));
                        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + item.itemName);
                    }

                    if (quest.hint != null && quest.hint.Length > 0)
                        questSlot.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;
                    break;

                default:

                    // Instantiate the Quest Prefab
                    questSlot = Instantiate(questPrefab, content.transform);

                    // Quest Title
                    questSlot.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.title;

                    // Quest Description
                    questSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.description;

                    // Dunong Points Rewards
                    questSlot.transform.GetChild(2).transform.GetChild(1)
                        .GetComponent<TMPro.TextMeshProUGUI>()
                        .text = quest.dunongPointsRewards.ToString();

                    if (quest.hint != null && quest.hint.Length > 0)
                    {
                        questSlot.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.hint;
                    }
                    else
                    {
                        questSlot.transform.GetChild(3).gameObject.SetActive(false);
                    }
                    break;
            }

        }
    }

    public void SetupAllNotes()
    {
        foreach (Quest quest in this.playerData.notesInfos)
        {
            if (quest.questType == Quest.QUEST_TYPE.REQUEST)
            {
                GameObject requester = GameObject.Find(quest.requestGoal.receiver);

                if (requester != null)
                {
                    requester.GetComponent<RequestRequester>().completedQuestToGainInfo = quest;
                    requester.GetComponent<GiveItemsToNPC>().npcName = quest.requestGoal.receiver;
                }
            }
            else if (quest.questType == Quest.QUEST_TYPE.SHOW_PHOTO_ALBUM)
            {
                GameObject npc = GameObject.Find(quest.showPhotoAlbumGoal.giverOfInfo);
                if (npc != null)
                {
                    npc.GetComponent<DialogueTrigger>().showAlbumQuest = quest;
                }
            }
            else if (quest.questType == Quest.QUEST_TYPE.DELIVERY)
            {
                print(quest.deliveryGoal.receiverName);
                GameObject receiver = GameObject.Find(quest.deliveryGoal.receiverName);

                if (receiver != null)
                {
                    receiver.GetComponent<DeliveryGoalReceiver>().quest = quest.CopyQuestDeliveryGoal();
                }
            }
        }
    }

    public void RemoveAllNotes()
    {
        this.playerData.notesInfos = new List<Quest>();
    }

    /** Remove all quest at Quest Panel */
    public void RemoveAllQuest(RectTransform content)
    {
        foreach (Transform questSlot in content)
        {
            Destroy(questSlot.gameObject);
        }
    }

    public void SetupShowPhotoAlbumQuest()
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questType == Quest.QUEST_TYPE.SHOW_PHOTO_ALBUM && quest.questID.Length > 0)
            {
                GameObject npc = GameObject.Find(quest.showPhotoAlbumGoal.giverOfInfo);

                if (npc != null)
                {
                    npc.GetComponent<DialogueTrigger>().showAlbumQuest = quest;
                }
            }
        }
    }

    /**<summary>
     *  Setups to all the gameobjects that has a request goal/quest.
     * </summary> */
    public void SetupScriptsForRequestQuest()
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.requestGoal != null && quest.questID.Length > 0)
            {
                // Find the gameobject that made a request.
                GameObject requester = GameObject.Find(quest.requestGoal.receiver);

                Quest questCopy = quest; 

                if (requester != null)
                {
                    requester.GetComponent<RequestRequester>().requestQuest = questCopy;
                    //requester.GetComponent<RequestRequester>().completedQuestToGainInfo = questCopy;
                    requester.GetComponent<GiveItemsToNPC>().npcName = questCopy.requestGoal.receiver;
                }

                try
                {
                    // Notify all of the gameobject that has RequestGiver script attached to it.
                    foreach (ItemGiver itemGiver in questCopy.requestGoal.itemGivers)
                    {
                        GameObject giver = GameObject.Find(itemGiver.giverName);

                        if (giver.tag == "Store")
                        {
                            giver.transform.GetChild(2).GetComponent<Light2D>().enabled = true;
                            giver.transform.GetChild(2).GetComponent<BuyTrigger>().requestQuest = questCopy;
                            giver.transform.GetChild(2).GetComponent<BuyTrigger>().itemGiver = itemGiver;
                        }
                        giver.GetComponent<RequestGiver>().requestQuest = questCopy;
                        giver.GetComponent<RequestGiver>().itemGiver = itemGiver;
                    }
                }
                catch (System.Exception e) { }
            }
        }
    }

    /** Setups all the gameobject who handles DeliveryGoalGiver and DeliveryGoalReceiver Quest. */
    public void SetupScriptsForDeliveryQuestToNPCs()
    {
        //DeliveryGoalGiver[] dgg = GameObject.FindObjectsOfType<DeliveryGoalGiver>();
        //DeliveryGoalReceiver[] dgr = GameObject.FindObjectsOfType<DeliveryGoalReceiver>();

        //foreach (DeliveryGoalGiver dg in dgg)
        //{
        //    dg.quest = null;
        //}

        //foreach (DeliveryGoalReceiver dr in dgr)
        //{
        //    dr.quest = null;
        //}

        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.deliveryGoal != null)
            {
                GameObject giver = GameObject.Find(quest.deliveryGoal.giverName);
                GameObject receiver = GameObject.Find(quest.deliveryGoal.receiverName);

                if (giver != null)
                {
                    print(giver.name);
                    giver.GetComponent<DeliveryGoalGiver>().quest = quest.CopyQuestDeliveryGoal();
                }

                if (receiver != null)
                {
                    print(receiver.name);
                    receiver.GetComponent<DeliveryGoalReceiver>().quest = quest.CopyQuestDeliveryGoal();
                }
            }
        }
    }

    public void SetupSearchQuest()
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questType == Quest.QUEST_TYPE.SEARCH)
            {
                GameObject receiverOfFoundItem = GameObject.Find(quest.searchGoal.npcToReceived);

                if (receiverOfFoundItem != null)
                {
                    receiverOfFoundItem.GetComponent<SearchQuest>().searchQuest = quest.CopySearchQuest();
                }

                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(quest.searchGoal.sceneToSpawn)
                    && quest.searchGoal.typesOfSearch.ToLower() == "clue")
                {
                    Transform positionToSpawn = GameObject.Find(quest.searchGoal.locationToSpawn).transform;
                    GameObject itemToFind = Instantiate(Resources.Load<GameObject>("Prefabs/Item Prefab"));
                    itemToFind.GetComponent<ItemMono>().item = quest.searchGoal.itemToFind.CopyItem();
                    itemToFind.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Collectibles/Items/" + quest.searchGoal.itemToFind.itemName);
                    itemToFind.transform.position = positionToSpawn.position;
                }
            }
        }
    }

    public void FindTalkWithShowPhotoAlbum(string questID)
    {
        print("IT IS GOING HERE IN SHOW PHOTO ALBUM");

        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questID == questID)
            {
                this.OpenAlertBox();

                SoundManager.instance?.PlaySound("Quest Notification");

                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID).CopyShowAlbumQuest();

                questFound.isCompleted = true;

                if (this.CheckIfPreQuestsDone())
                {
                    this.playerData.completedQuests.Add(questFound);
                    this.playerData.notesInfos.Add(questFound);
                    this.notesIcon.GetChild(0).gameObject.SetActive(true);
                    DataPersistenceManager.instance.playerData.notesNewIcon = true;
                    this.SetupAllNotes();

                    DataPersistenceManager.instance.playerData.dunongPoints += quest.dunongPointsRewards;

                    UpdateDPValueUI?.Invoke();
                }

                this.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                this.playerData.quests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);

                if (this.CheckIfPreQuestsDone() == true)
                {
                    DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone = true;
                    this.GetListOfQuests();
                    this.SetupShowPhotoAlbumQuest();
                }
                this.GetListOfQuests();

                if (this.playerData.currentQuests.Count < 1)
                {
                    OnAllQuestCompleted?.Invoke("Now you can take the assessment at the school event!");
                }
                DataPersistenceManager.instance?.SaveGame();

                return;
            }
        }
    }

    public void FindRequestQuest(string questID)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questID == questID)
            {
                this.OpenPlainAlertBoxAndAlertBox("Item succesfully given to " + quest.requestGoal.receiver);

                SoundManager.instance?.PlaySound("Quest Notification");

                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID).CopyRequestGoal();

                questFound.isCompleted = true;

                // CHECK IF THE PRE-QUEST IS DONE.
                if (this.CheckIfPreQuestsDone())
                {
                    // ADD TO COMPLETED QUEST
                    this.playerData.completedQuests.Add(questFound);

                    // ADD TO NOTES
                    this.playerData.notesInfos.Add(questFound);

                    // SHOW THE NEW ICON IN THE NOTES BUTTON
                    DataPersistenceManager.instance.playerData.notesNewIcon = true;
                    this.notesIcon.GetChild(0).gameObject.SetActive(true);
                    this.SetupAllNotes(); // SETUP ALL NOTES.

                    // ADD ADITIONAL DUNONG POINTS
                    DataPersistenceManager.instance.playerData.dunongPoints += quest.dunongPointsRewards;

                    // UPDATE DUNONG POINTS VALUE IN UI.
                    UpdateDPValueUI?.Invoke();
                }

                this.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                this.playerData.quests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                
                if (this.CheckIfPreQuestsDone() == true)
                {
                    if (DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone == false)
                    {
                        OnAllQuestCompleted?.Invoke("<b>Pre-Quest is done</b>. You can check now the new quests for <b>Ilocos Region</b>.");
                    }
                    DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone = true;
                    this.GetListOfQuests();
                    this.SetupScriptsForRequestQuest();
                }
                else
                {
                    this.GetListOfQuests();
                    this.SetupScriptsForRequestQuest();
                }
                //this.GetListOfQuests();

                if (this.playerData.currentQuests.Count < 1)
                {
                    OnAllQuestCompleted?.Invoke("Now you can take the assessment at the school event!");
                }

                DataPersistenceManager.instance?.SaveGame();

                return;
            }
        }
    }

    /** Find the delivery quest based on ID and set it as completed. */
    public void FindDeliveryQuestGoal(string deliveryQuestID)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questID == deliveryQuestID)
            {
                this.OpenPlainAlertBoxAndAlertBox("Item succesfully given to " + quest.deliveryGoal.receiverName);

                SoundManager.instance?.PlaySound("Quest Notification");

                // Find and Copy the Delivery Quest
                // Copy the instance so that we don't directly modify the object.
                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID).CopyQuestDeliveryGoal();

                questFound.isCompleted = true;

                if (this.CheckIfPreQuestsDone() == true)
                {
                    DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone = true;
                    this.GetListOfQuests();
                    this.SetupScriptsForDeliveryQuestToNPCs();
                }

                if (this.CheckIfPreQuestsDone())
                {
                    this.playerData.notesInfos.Add(questFound);
                    this.playerData.completedQuests.Add(questFound);

                    // SHOW THE NEW icon in the notes button
                    DataPersistenceManager.instance.playerData.notesNewIcon = true;
                    this.notesIcon.GetChild(0).gameObject.SetActive(true);
                    this.SetupAllNotes();

                    DataPersistenceManager.instance.playerData.dunongPoints += quest.dunongPointsRewards;

                    UpdateDPValueUI?.Invoke(); // Update UI.
                }
                this.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                this.playerData.quests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);

                this.GetListOfQuests();

                if (this.playerData.currentQuests.Count < 1)
                {
                    OnAllQuestCompleted?.Invoke("Now you can take the assessment at the school event!");
                }

                DataPersistenceManager.instance?.SaveGame();

                return;
            }
        }
    }

    /** Find the talk quest and set it as completed. */
    public void FindTalkQuestGoal(string npcName)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questType == Quest.QUEST_TYPE.TALK
                && !quest.isCompleted
                && quest.talkGoal.GetNPCName().ToUpper() == npcName.ToUpper())
            {
                quest.isCompleted = true;

                this.OpenAlertBox();

                DataPersistenceManager.instance.playerData.dunongPoints += quest.dunongPointsRewards;

                UpdateDPValueUI?.Invoke(); // Update UI.

                SoundManager.instance?.PlaySound("Quest Notification");

                Quest questFoundInCurrentQuest = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID).CopyTalkQuestGoal();

                this.playerData.notesInfos.Add(questFoundInCurrentQuest);

                // SHOW the New Icon at notes button.
                DataPersistenceManager.instance.playerData.notesNewIcon = true;
                this.notesIcon.GetChild(0).gameObject.SetActive(true);

                this.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID.ToLower() == questFoundInCurrentQuest.questID.ToLower());
                this.playerData.quests.RemoveAll(questToRemove => questToRemove.questID.ToLower() == questFoundInCurrentQuest.questID.ToLower());

                questFoundInCurrentQuest.isCompleted = true;

                this.playerData.completedQuests.Add(questFoundInCurrentQuest);

                this.GetListOfQuests();

                if (this.playerData.currentQuests.Count < 1)
                {
                    OnAllQuestCompleted?.Invoke("Now you can take the assessment at the school event!");
                }

                DataPersistenceManager.instance.SaveGame();

                return;
            }
        }
    }

    /** Find the request quest and set the 'isRequestFromNPCGained' to true. */
    public void SetRequestAccepted(string questID, bool isRequestAccepted)
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            if (quest.questID == questID)
            {
                if (isRequestAccepted == false)
                {
                    quest.requestGoal.isRequestFromNPCGained = true;
                }
            }
            return;
        }
    }

    public void SetRequestItemsAreGiven(string questID)
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            if (quest.questID == questID)
            {
                quest.requestGoal.isItemReceivedOfNpc = true;
            }
            return;
        }
    }

    public bool CheckIfPreQuestsDone()
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.quests)
        {
            if (quest.region.ToUpper() == "PRE-QUEST".ToUpper())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// @called at MainGame class.
    /// </summary>
    /// <param name="regionName"></param>
    public void ResetAllCompletedQuests(string regionName)
    {
        DataPersistenceManager.instance.playerData.isQuestReset = true;
        DataPersistenceManager.instance.playerData.questNewIcon = true;

        // Add all the completed quests to the current quest based on region
        foreach (Quest quest in DataPersistenceManager.instance.playerData.completedQuests)
        {
            if (quest.region.ToUpper() == regionName.ToUpper())
            {
                quest.isCompleted = false;

                // IF THE Quest Type is Delivery
                if (quest.questType == Quest.QUEST_TYPE.DELIVERY)
                {
                    quest.deliveryGoal.isFinished = false;
                    quest.deliveryGoal.itemReceivedFromGiver = false;
                    this.playerData.quests.Add(quest.CopyQuestDeliveryGoal());
                }
                // IF the Quest
                else if (quest.questType == Quest.QUEST_TYPE.REQUEST)
                {
                    quest.requestGoal.isRequestFromNPCGained = false;
                    quest.requestGoal.isItemReceivedOfNpc = false;

                    // Also reset the itemGiver object.
                    foreach (ItemGiver itemGiver in quest.requestGoal.itemGivers)
                    {
                        itemGiver.isItemsGiven = false;
                    }

                    this.playerData.quests.Add(quest.CopyRequestGoal());
                }
                else if (quest.questType == Quest.QUEST_TYPE.TALK)
                {
                    this.playerData.quests.Add(quest.CopyTalkQuestGoal());
                }
                else if (quest.questType == Quest.QUEST_TYPE.SHOW_PHOTO_ALBUM)
                {
                    this.playerData.quests.Add(quest.CopyShowAlbumQuest());
                }
            }
        }

        this.playerData.completedQuests.RemoveAll(questToRemove => questToRemove.region.ToUpper() == regionName.ToUpper());
        this.RemoveAllNotes();
    }

    public void OpenAlertBox()
    {
        if (this.questAlertCoroutine != null)
        {
            this.questAlertBox.transform.localScale = Vector2.zero;
            StopCoroutine(this.questAlertCoroutine);
        }

        LeanTween.scale(this.questAlertBox.gameObject, Vector2.one, .2f)
        .setEaseSpring()
        .setOnComplete(() =>
        {
            this.questAlertCoroutine = StartCoroutine(HideQuestAlertBox(this.questAlertBox, 1f));
        });
    }

    public void OpenPlainAlertBox(string message)
    {
        if (this.plainAlertCoroutine != null)
        {
            this.plainAlertBox.transform.localScale = Vector2.zero;
            StopCoroutine(this.plainAlertCoroutine);
        }

        this.plainAlertBox.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = message;

        LeanTween.scale(this.plainAlertBox.gameObject, Vector2.one, .2f)
        .setEaseSpring()
        .setOnComplete(() =>
        {
            this.plainAlertCoroutine = StartCoroutine(HideQuestAlertBox(this.plainAlertBox, 1f));
        });
    }


    public void OpenPlainAlertBoxAndAlertBox(string message)
    {
        if (this.plainAlertCoroutine != null)
        {
            this.plainAlertBox.transform.localScale = Vector2.zero;
            StopCoroutine(this.plainAlertCoroutine);
        }

        if (this.questAlertCoroutine != null)
        {
            this.questAlertBox.transform.localScale = Vector2.zero;
            StopCoroutine(this.questAlertCoroutine);
        }

        this.plainAlertBox.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = message;

        LeanTween.scale(this.plainAlertBox.gameObject, Vector2.one, .2f)
            .setEaseSpring()
            .setOnComplete(() =>
            {
                this.plainAlertCoroutine = StartCoroutine(HideQuestAlertBox(this.plainAlertBox, 1f));

                LeanTween.scale(this.questAlertBox.gameObject, Vector2.one, .2f)
                .setEaseSpring()
                .setOnComplete(() =>
                {
                    this.questAlertCoroutine = StartCoroutine(HideQuestAlertBox(this.questAlertBox, 1f));
                })
                .setDelay(1f);
            });
    }

    // Hides Alert Box.
    IEnumerator HideQuestAlertBox(GameObject alertBox, float timeToWaitBeforeClosingThePanel)
    {
        yield return new WaitForSeconds(timeToWaitBeforeClosingThePanel);

        LeanTween.scale(alertBox, Vector2.zero, .2f)
            .setEaseSpring();
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
