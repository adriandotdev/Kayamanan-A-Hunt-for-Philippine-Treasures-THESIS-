using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewState : MonoBehaviour
{
    public Transform readyStateQueueVisual;
    public Transform runStateQueueVisual;
    public Transform suspendReadyQueueVisual;
    public Transform runningQueueVisual;
    public Transform waitBlockQueueVisual;
    public Transform suspendWaitQueueVisual;
    public Transform newProcess;

    public void CreateNewStateMethod()
    {
        if (readyStateQueueVisual.childCount == 3)
        {
            Transform process = Instantiate(newProcess, suspendReadyQueueVisual);
            process.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "P" + ProcessManager.instance.processNumber;
        }
        else
        {
            Transform process = Instantiate(newProcess, readyStateQueueVisual);
            process.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "P" + ProcessManager.instance.processNumber;
        }
        ProcessManager.instance.processNumber++;
    }

    public void DispatchAndPutToRunStateQueue()
    {
        Transform toRemove = readyStateQueueVisual.GetChild(0);
        toRemove.SetParent(runStateQueueVisual);
        toRemove.localPosition = Vector2.zero;
    }

    public void ResumeSuspendedStateToReady()
    {
        if (readyStateQueueVisual.childCount < 3)
        {
            Transform toRemove = suspendReadyQueueVisual.GetChild(0);
            toRemove.SetParent(readyStateQueueVisual);
            toRemove.localPosition = Vector2.zero;
        }
    }

    public void CompletedTermination()
    {
        if (runningQueueVisual.childCount > 0)
        {
            Transform toRemove = runningQueueVisual.GetChild(0);
            toRemove.SetParent(null);
            Destroy(toRemove);
        }
    }

    public void IORequest()
    {
        try
        {


            if (waitBlockQueueVisual.childCount == 3)
            {
                Transform toRemove = runningQueueVisual.GetChild(0);
                toRemove.SetParent(suspendWaitQueueVisual);
                toRemove.localPosition = Vector2.zero;
            }
            else
            {
                Transform toRemove = runningQueueVisual.GetChild(0);
                toRemove.SetParent(waitBlockQueueVisual);
                toRemove.localPosition = Vector2.zero;
            }
        }
        catch (System.Exception e) { print(e.Message); }
    }

    public void ResumeToWaitBlock()
    {
        if (suspendWaitQueueVisual.childCount > 0 && waitBlockQueueVisual.childCount < 3)
        {
            Transform toRemove = suspendWaitQueueVisual.GetChild(0);
            toRemove.SetParent(waitBlockQueueVisual);
            Destroy(toRemove);
        }
    }
    public void IOCompletion()
    {
        try
        {
            if (readyStateQueueVisual.childCount == 3)
            {
                Transform toRemove = waitBlockQueueVisual.GetChild(0);
                toRemove.SetParent(suspendReadyQueueVisual);
                toRemove.localPosition = Vector2.zero;
            }
            else
            {
                Transform toRemove = waitBlockQueueVisual.GetChild(0);
                toRemove.SetParent(readyStateQueueVisual);
                toRemove.localPosition = Vector2.zero;
            }
        } catch (System.Exception e) { print(e.Message); }
    }

    public void CompletedButInSuspend()
    {
        if (this.suspendWaitQueueVisual.childCount > 0)
        {
            Transform toRemove = suspendWaitQueueVisual.GetChild(0);
            toRemove.SetParent(suspendReadyQueueVisual);
            toRemove.localPosition = Vector2.zero;
        }
    }

    public void PriorityQuantum()
    {
        if (runningQueueVisual.childCount > 0)
        {
            Transform toRemove = runningQueueVisual.GetChild(0);
            Transform toRemove2 = readyStateQueueVisual.GetChild(0);

            toRemove.SetParent(readyStateQueueVisual);
            toRemove2.SetParent(runningQueueVisual);
        }
    }
}
