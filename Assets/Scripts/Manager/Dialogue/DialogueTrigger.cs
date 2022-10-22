using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class DialogueTrigger : MonoBehaviour
{
    public TextAsset npcIntroduction;
    public TextAsset[] inks;
    public TMPro.TextMeshProUGUI actorName;
    private Button talkButton; // Talk Button
    private Button deliveryReqButton; // Delivery Request Button.
    private Button giveItemToNPCBtn; // Give Item to NPC Button
    private Button giveItemToPlayer; // Give Item to Player Button

    [SerializeField] public string NPC_NAME;

    [SerializeField] public Quest showAlbumQuest;

    private void OnEnable()
    {
        DialogueManager.OnDialogueRunning += HideButtons;
        // First child of this gameobject.
        Transform firstChild = transform.GetChild(0);

        this.NPC_NAME = transform.gameObject.name;

        this.actorName = firstChild.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        this.talkButton = firstChild.GetChild(1).GetComponent<Button>();
        this.deliveryReqButton = firstChild.GetChild(2).GetComponent<Button>();
        this.giveItemToNPCBtn = firstChild.GetChild(3).GetComponent<Button>();
        this.giveItemToPlayer = firstChild.GetChild(4).GetComponent<Button>();

        // Add Event to Talk Button
        this.talkButton.onClick.AddListener(() =>
        {
            this.StartDialogue();
        });
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueRunning -= HideButtons;
    }

    private void Start()
    {
        this.NPC_NAME = transform.gameObject.name;
        this.actorName.text = transform.name;
    }

    public int CurrentOpenRegion()
    {
        int regionsData = 0;

        foreach (RegionData regionData in DataPersistenceManager.instance.playerData.regionsData)
        {
            if (regionData.isOpen)
                regionsData = regionData.regionNumber - 1;
        }
        return regionsData;
    }


    public void StartDialogue()
    {
        /**  Kapag ang isPreQuestIntroductionDone is false pa, and if ang NPC na gameobject na ito ay
         first time palang kakausapin ni player, then we need to go inside this if block
         and execute ang instruction. */
        if (DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone == false && CheckIfFirstTimeTalking(this.NPC_NAME))
        {
            foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
            {
                if (quest.questType == Quest.QUEST_TYPE.NUMBER && quest.numberGoal.correspondingCount == NumberGoal.CORRESPONDING_OBJECT_TO_COUNT.TALK_NPC)
                { 
                    quest.numberGoal.currentNumber++;

                    if (quest.numberGoal.currentNumber == quest.numberGoal.targetNumber)
                    {
                        QuestManager.instance?.OpenPlainAlertBoxAndAlertBox("You met a new villager");

                        DataPersistenceManager.instance.playerData.quests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                        DataPersistenceManager.instance.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                        
                        // CHECK IF THE INTRODUCTION OR PRE QUEST IS  DONE THEN WE ONLY SET THE isAllNPCMet to true.
                        if (QuestManager.instance.CheckIfPreQuestsDone() == true)
                        {
                            DataPersistenceManager.instance.playerData.isPreQuestIntroductionDone = true;
                            QuestManager.instance?.GetListOfQuests();
                            QuestManager.instance?.SetupScriptsForRequestQuest();
                        }

                        break;
                    }
                    else
                    {
                        QuestManager.instance?.OpenPlainAlertBox("You met new villager");
                    }
                }
            }
            DataPersistenceManager.instance?.SaveGame();
            DialogueManager._instance?.StartDialogue(this.npcIntroduction);
            DialogueManager._instance.actorField.text = this.NPC_NAME;
        }
        else
        {
            // If the quest is having a showing of album.
            if (this.showAlbumQuest != null && this.showAlbumQuest.questID.Length > 0)
            {
                DialogueManager._instance.showAlbumGoal = this.showAlbumQuest.showPhotoAlbumGoal.Copy();
                DialogueManager._instance?.StartDialogue(inks[this.CurrentOpenRegion()]);
                DialogueManager._instance.actorField.text = this.NPC_NAME;
                QuestManager.instance.FindTalkWithShowPhotoAlbum(this.showAlbumQuest.questID);
            }
            else
            {
                DialogueManager._instance?.StartDialogue(inks[this.CurrentOpenRegion()]);
                DialogueManager._instance.actorField.text = this.NPC_NAME;
                QuestManager.instance.FindTalkQuestGoal(this.NPC_NAME);
            }
        }
    }

    IEnumerator DestroyPopup(GameObject popup)
    {
        yield return new WaitForSeconds(.5f);

        Destroy(popup);
    }

    bool CheckIfFirstTimeTalking(string NPC_NAME)
    {
        foreach (NPC_INFO npcInfo in DataPersistenceManager.instance.playerData.npcInfos)
        {
            if (npcInfo.name.ToUpper() == NPC_NAME.ToUpper())
            {
                if (!npcInfo.isMet)
                {
                    npcInfo.isMet = true;
                    return true;
                }
            }
        }
        return false;
    }

    public void HideButtons(bool toDisable)
    {
        if (toDisable)
        {
            this.talkButton.enabled = false;
            this.deliveryReqButton.enabled = false;
            this.giveItemToNPCBtn.enabled = false;
            this.giveItemToPlayer.enabled = false;
        }
        else
        {
            this.talkButton.enabled = true;
            this.deliveryReqButton.enabled = true;
            this.giveItemToNPCBtn.enabled = true;
            this.giveItemToPlayer.enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.talkButton.gameObject.SetActive(true);
            DialogueManager._instance.npcName = this.NPC_NAME;
            DialogueManager._instance.isTalking = true;
            GetComponent<NPC>()?.animator.SetFloat("Speed", 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        talkButton.gameObject.SetActive(false);
        DialogueManager._instance.npcName = "";
        DialogueManager._instance.isTalking = false;
    }
}
