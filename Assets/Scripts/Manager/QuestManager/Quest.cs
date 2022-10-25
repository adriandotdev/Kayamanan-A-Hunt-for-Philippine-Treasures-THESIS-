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
    public string note;
    public string hint;
    public int dunongPointsRewards;
    public string region;
    public int regionNum;
    public TalkGoal talkGoal;
    public DeliveryGoal deliveryGoal;
    public NumberGoal numberGoal;
    public RequestGoal requestGoal;
    public ShowPhotoAlbumGoal showPhotoAlbumGoal;
    public SearchGoal searchGoal;

    public enum QUEST_TYPE { TALK, DELIVERY, NUMBER, REQUEST, SHOW_PHOTO_ALBUM, SEARCH }

    public QUEST_TYPE questType;

    public Quest(string title, string note, string description, int dunongPointsRewards, string region, int regionNum, TalkGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.note = note;
        this.dunongPointsRewards = dunongPointsRewards;
        this.talkGoal = goal;
        this.deliveryGoal = null;
        this.requestGoal = null;
        this.region = region;
        this.regionNum = regionNum;
        this.questType = QUEST_TYPE.TALK;
    }

    public Quest(string title, string note, string description, int dunongPointsRewards, string region, int regionNum, DeliveryGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.note = note;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.deliveryGoal = goal;
        this.talkGoal = null;
        this.requestGoal = null;
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

    public Quest(string title, string note, string description, int dunongPointsRewards, string region, int regionNum, RequestGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.note = note;
        this.region = region;
        this.regionNum = regionNum;
        this.dunongPointsRewards = dunongPointsRewards;
        this.requestGoal = goal;

        this.questType = QUEST_TYPE.REQUEST;
    }

    public Quest(string title, string note, string description, int dunongPointsRewards, string region, int regionNum, ShowPhotoAlbumGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.note = note;
        this.dunongPointsRewards = dunongPointsRewards;
        this.region = region;
        this.regionNum = regionNum;
        this.showPhotoAlbumGoal = goal;

        this.questType = QUEST_TYPE.SHOW_PHOTO_ALBUM;
    }

    public Quest(string title, string note, string description, string hint, int dunongPointsRewards, string region, int regionNum, SearchGoal searchGoal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.hint = hint;
        this.dunongPointsRewards = dunongPointsRewards;
        this.region = region;
        this.regionNum = regionNum;
        this.searchGoal = searchGoal;
        this.note = note;
        this.questType = QUEST_TYPE.SEARCH;
    }

    private void SetQuestID(string questID)
    {
        this.questID = questID;
    }

    public Quest CopyTalkQuestGoal()
    {
        Quest questCopy = new Quest(this.title, this.note, this.description, this.dunongPointsRewards, this.region, this.regionNum, this.talkGoal.CopyTalkGoal());

        questCopy.SetQuestID(this.questID);

        return questCopy;
    }

    public Quest CopyQuestDeliveryGoal()
    {
        Quest questCopy = new Quest(this.title, this.note, this.description, this.dunongPointsRewards, this.region, this.regionNum, this.deliveryGoal.Copy());

        questCopy.isCompleted = this.isCompleted;
        questCopy.SetQuestID(this.questID);

        return questCopy;
    }

    public Quest CopyRequestGoal()
    {
        Quest copy = new Quest(this.title, this.note, this.description, this.dunongPointsRewards, this.region, this.regionNum, this.requestGoal.Copy());

        copy.SetQuestID(this.questID);

        return copy;
    }

    public Quest CopyShowAlbumQuest()
    {
        Quest copy = new Quest(this.title, this.note, this.description, this.dunongPointsRewards, this.region, this.regionNum, this.showPhotoAlbumGoal.Copy());

        copy.SetQuestID(this.questID);

        return copy;
    }

    public Quest CopySearchQuest()
    {
        // string title, string note, string description, string hint, int dunongPointsRewards, string region, int regionNum, SearchGoal searchGoal
        Quest copy = new Quest(this.title, this.note, this.description, this.hint, this.dunongPointsRewards, this.region, this.regionNum, this.searchGoal.Copy());
        return copy;
    }
}

