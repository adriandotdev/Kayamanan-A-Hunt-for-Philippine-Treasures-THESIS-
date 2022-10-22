using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepCutsceneEndTrigger : MonoBehaviour
{
    void Start()
    {
        //DataPersistenceManager.instance.playerData.isFromSleeping = true;
        //DataPersistenceManager.instance.playerData.playerTime.m_IsDaytime = true;
        //DataPersistenceManager.instance.playerData.playerTime.m_ActualHourInRealLife = 8;
        //DataPersistenceManager.instance.playerData.playerTime.m_ActualMinuteInRealLife = 0;
        //DataPersistenceManager.instance.playerData.playerTime.m_DayEvent += 1;
        SceneManager.LoadScene(DataPersistenceManager.instance.playerData.sceneToLoad);        
    }
}
