using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeTracker : MonoBehaviour
{
    public float Hour { get; private set; }
    public float Minute { get; private set; }
    public float Seconds { get; private set; }

    private void OnDisable()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
        }    
    }

    private void Start()
    {
        if (DataPersistenceManager.instance != null)
        {
            this.Hour = DataPersistenceManager.instance.playerData.playerTime.m_GameTimeHour;
            this.Minute = DataPersistenceManager.instance.playerData.playerTime.m_GameTimeMinute;
            this.Seconds = DataPersistenceManager.instance.playerData.playerTime.m_GameTimeSeconds;
        }
    }

    private void Update()
    {
        this.Seconds += 1 * Time.deltaTime;
        DataPersistenceManager.instance.playerData.playerTime.m_GameTimeSeconds = this.Seconds;

        if (this.Seconds >= 60)
        {
            this.Seconds = 0f;
            this.Minute++;
            DataPersistenceManager.instance.playerData.playerTime.m_GameTimeMinute = this.Minute;

            if (this.Minute >= 60)
            {
                this.Minute = 0f;
                this.Hour++;
                DataPersistenceManager.instance.playerData.playerTime.m_GameTimeHour = this.Hour;
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
        }
    }
}
