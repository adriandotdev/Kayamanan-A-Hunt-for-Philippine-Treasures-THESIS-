using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;
    public bool isCompleted;
    public bool isClaimed;

    public string title;
    public string description;
    public int dunongPointsRewards;
    public string region;
    public int regionNum;
    public TalkGoal talkGoal;
    public DeliveryGoal deliveryGoal;
    public NumberGoal numberGoal;


    public enum QUEST_TYPE { TALK, DELIVERY, NUMBER }

    public QUEST_TYPE questType;

    public Quest(string title, string description, int dunongPointsRewards, string region, int regionNum, TalkGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.talkGoal = goal;
        this.deliveryGoal = null;
        this.region = region;
        this.regionNum = regionNum;
        this.questType = QUEST_TYPE.TALK;
    }

    public Quest(string title, string description, int dunongPointsRewards, string region, int regionNum, DeliveryGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.deliveryGoal = goal;
        this.talkGoal = null;
        this.region = region;
        this.regionNum = regionNum;
        this.questType = QUEST_TYPE.DELIVERY;
    }

    public Quest(string title, string description, string region, NumberGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.region = region;
        this.numberGoal = goal;
        this.questType = QUEST_TYPE.NUMBER;
    }

    public Quest(string title, string description, string region, DeliveryGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.region = region;
        this.deliveryGoal = goal;
        this.questType = QUEST_TYPE.DELIVERY;
    }

    private void SetQuestID(string questID)
    {
        this.questID = questID;
    }

    public Quest CopyTalkQuestGoal()
    {
        Quest questCopy = new Quest(this.title, this.description, this.dunongPointsRewards, this.region, this.regionNum, this.talkGoal.CopyTalkGoal());

        questCopy.SetQuestID(this.questID);

        return questCopy;
    }

    public Quest CopyQuestDeliveryGoal()
    {
        Quest questCopy = new Quest(this.title, this.description, this.dunongPointsRewards, this.region, this.regionNum,
            this.deliveryGoal.Copy());

        questCopy.SetQuestID(this.questID);

        return questCopy;
    }
}

