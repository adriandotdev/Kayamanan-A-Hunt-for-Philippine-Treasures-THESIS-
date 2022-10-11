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
    public enum CORRESPONDING_OBJECT_TO_COUNT { TALK_NPC, READING }
    public CORRESPONDING_OBJECT_TO_COUNT correspondingCount;
    
    public NumberGoal( int targetNumber, CORRESPONDING_OBJECT_TO_COUNT count)
    {
        this.targetNumber = targetNumber;
        this.correspondingCount = count;
    }
}

/**
 * NEED NG SCRIPT FOR REQUESTGIVER AND REQUESTRECEIVER 
 */
[System.Serializable]
public class RequestGoal
{
    public bool isRequestFromNPCGained;
    public bool isItemReceivedOfNpc;

    public string requestGoalID;

    // RECEIVER and GIVER
    public string receiver;
    public ItemGiver[] itemGivers;

    // MESSAGE
    public string msgRequest;
    public string msgOfGiver;

    public RequestGoal(ItemGiver[] itemGivers, string receiver, string messageRequest, string msgOfGiver)
    {
        this.requestGoalID = Guid.NewGuid().ToString();

        this.isRequestFromNPCGained = false;
        this.isItemReceivedOfNpc = false;

        this.itemGivers = itemGivers;
        this.receiver = receiver;
        this.msgRequest = messageRequest;
        this.msgOfGiver = msgOfGiver;
    }

    public RequestGoal Copy()
    {
        RequestGoal rg = new RequestGoal(this.itemGivers, this.receiver, this.msgRequest, this.msgOfGiver);

        rg.requestGoalID = this.requestGoalID;
        rg.isRequestFromNPCGained = this.isRequestFromNPCGained;
        rg.isItemReceivedOfNpc = this.isItemReceivedOfNpc;

        return rg;
    }
}

[System.Serializable]
public class ItemGiver {

    public string giverName;
    public List<Item> itemsToGive;
    public bool isItemsGiven;

    public ItemGiver(string giverName, List<Item> itemsToGive)
    {
        this.giverName = giverName;
        this.itemsToGive = itemsToGive;
        this.isItemsGiven = false;
    }
}
//[System.Serializable]
//public class RequestGoal
//{
//    public bool isRequestFromNPCGained;
//    public bool isItemReceived;

//    public string deliverGoalId;

//    // RECEIVER and GIVER
//    public string receiver;
//    public string giver;

//    // MESSAGE
//    public string msgRequest;
//    public string msgOfGiver;

//    // Item
//    public Item item;

//    public RequestGoal(string giver, string receiver, string messageRequest, string msgOfGiver, Item item)
//    {
//        this.deliverGoalId = Guid.NewGuid().ToString();

//        this.isRequestFromNPCGained = false;
//        this.isItemReceived = false;

//        this.giver = giver;
//        this.receiver = receiver;
//        this.msgRequest = messageRequest;
//        this.msgOfGiver = msgOfGiver;

//        this.item = item;
//    }

//    public RequestGoal Copy()
//    {
//        RequestGoal rg = new RequestGoal(this.giver, this.receiver, this.msgRequest, this.msgOfGiver, this.item);

//        return rg;
//    }
//}

[System.Serializable]
public class MsgRequest
{
    public string Message { get; private set; }
    public string Response { get; private set; }

    public MsgRequest(string message, string response)
    {
        this.Message = message;
        this.Response = response;
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