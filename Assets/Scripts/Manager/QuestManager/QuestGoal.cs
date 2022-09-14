using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class QuestGoal
{
    public bool isFinished;

    public virtual void Finish()
    {
        this.isFinished = true;
    }
}

[System.Serializable]
public class TalkGoal : QuestGoal
{
    [SerializeField] private string npcName;

    public TalkGoal(string npcName)
    {
        this.npcName = npcName;
    }

    public string GetNPCName()
    {
        return this.npcName;
    }

    public override void Finish()
    {
        base.Finish();
    }

    public TalkGoal CopyTalkGoal()
    {
        return new TalkGoal(this.npcName);
    }
}


[System.Serializable]
public class DeliveryGoal : QuestGoal
{
    public string deliverGoalId;
    public string deliveryMessage;
    public string giverName;
    public string receiverName;
    public bool itemReceivedFromGiver;
    public Item item;

    public DeliveryGoal(string giverName, string receiverName,  string deliveryMessage, Item item)
    {
        this.deliverGoalId = Guid.NewGuid().ToString();
        this.giverName = giverName;
        this.receiverName = receiverName;
        this.deliveryMessage = deliveryMessage;
        this.item = item;
    }

    public DeliveryGoal Copy()
    {
        DeliveryGoal deliveryGoalCopy =  new DeliveryGoal(this.giverName, this.receiverName, 
            this.deliveryMessage, 
            this.item.CopyItem());

        deliveryGoalCopy.itemReceivedFromGiver = this.itemReceivedFromGiver;
        deliveryGoalCopy.deliverGoalId = this.deliverGoalId;

        return deliveryGoalCopy;
    }
}

[System.Serializable]
public class NumberGoal
{
    public int targetNumber;
    public int currentNumber;
    public enum CORRESPONDING_OBJECT_TO_COUNT { TALK_NPC, FEEDING_CHICKEN }
    public CORRESPONDING_OBJECT_TO_COUNT correspondingCount;
     
    public NumberGoal( int targetNumber, CORRESPONDING_OBJECT_TO_COUNT count)
    {
        this.targetNumber = targetNumber;
        this.correspondingCount = count;
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public string information;
    public int quantity;
    public bool stackable;

    public Item(string itemName, int quantity, string information, bool stackable)
    {
        this.itemName = itemName;
        this.information = information;
        this.stackable = stackable;
        this.quantity = quantity;
    }

    public Item CopyItem()
    {
        Item clonedItem = new Item(this.itemName, this.quantity, this.information, this.stackable);

        return clonedItem;
    }
}