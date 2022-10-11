using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour, IDataPersistence
{
    public static TimeManager instance;
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnTimeToSleep;
    public static Action<bool> OpenLights;

    public static int ActualHourInRealLife { get; private set; }
    public static float NoOfSecondsPerTwoAndHalfMinutes { get; private set; }

    public static bool IsDaytime { get; private set; }

    public bool m_IsAllEstablishmentsOpen;
    public bool m_IsPaused;

    public PlayerData playerData;

    public GameObject globalLight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        /** 
         <summary>
            24 hrs in real life. In this game, 1 hour is equivalent to 1 day.
            So 30 mins for daytime and 30 mins for night.

            The 30 mins is divided by 12 so equals ito sa 2.5 mins for kada oras in real time.

            All Establishments are open 8AM to 5PM
            Mystery Doors are open 5PM to 8PM
            
            NPC pag gabi na.
        </summary>
         */
        //print(Application.persistentDataPath);

        //PlayerDataHandler playerDataHandler = new PlayerDataHandler("Time");

        //try
        //{
        //    playerData = playerDataHandler.Load();
        //}
        //catch (System.Exception e)
        //{
        //    playerData = null;
        //}

        //if (playerData == null)
        //{
        //    playerDataHandler.Save(new PlayerData());
        //    playerData = playerDataHandler.Load();
        //    print("TIME DATA IS NULL");
        //}
        //else
        //{
        //    print("TIME DATA IS NOT NULL");
        //}
        // All code above is for testing purposes only.

        ActualHourInRealLife = playerData.playerTime.m_ActualHourInRealLife;
        NoOfSecondsPerTwoAndHalfMinutes = playerData.playerTime.m_NoOfSecondsPerTwoAndHalfMinutes; // MUST BE 150
        IsDaytime = playerData.playerTime.m_IsDaytime;
        m_IsAllEstablishmentsOpen = playerData.playerTime.m_IsAllEstablishmentsOpen;
        OnHourChanged?.Invoke();
        InterpolateColor(this.playerData.playerTime.m_ActualHourInRealLife);
        this.OpenAllEstablishments();
    }

    void Update()
    {
        if (this.m_IsPaused) return;

        // For each frame, deduct the seconds.
        playerData.playerTime.m_NoOfSecondsPerMinute -= (0.15f * Time.deltaTime); // it must be 0.15f * Time.deltaTime

        if (playerData.playerTime.m_NoOfSecondsPerMinute <= 0)
        {
            playerData.playerTime.m_NoOfSecondsPerMinute = 0.15f;
            playerData.playerTime.m_ActualMinuteInRealLife++;
            OnMinuteChanged?.Invoke();
        }

        if (this.playerData.playerTime.m_ActualHourInRealLife >= 20)
        {
            OnTimeToSleep?.Invoke();
        }

        /** If 60 minutes na then increment the Hour. */
        if (playerData.playerTime.m_ActualMinuteInRealLife >= 60)
        {
            playerData.playerTime.m_NoOfSecondsPerMinute = playerData.playerTime.SECONDS_PER_MINUTE;

            this.playerData.playerTime.m_ActualHourInRealLife++;

            if (this.playerData.playerTime.m_ActualHourInRealLife >= 25)
            {
                this.playerData.playerTime.m_ActualHourInRealLife = 1;
            }

            this.playerData.playerTime.m_ActualMinuteInRealLife = 0;

            this.SetIsDaytime();

            OnHourChanged?.Invoke(); // Invoke the event so that the other delegates will be called.

            InterpolateColor(this.playerData.playerTime.m_ActualHourInRealLife);

            if (this.playerData.playerTime.m_ActualHourInRealLife >= 24)
            {
                this.playerData.playerTime.m_DayEvent++;

                OnHourChanged?.Invoke(); // Invoke the event so that the other delegates will be called.
            }
        }
        this.OpenAllEstablishments();
    }

    void InterpolateColor(int hourTime)
    {
        try
        {
            /** <summary>
             *  7 pm to 4am -> Color black
             *  5 am to 5pm -> Color
             *  5pm to 7pm -> Color
             * </summary> */
            Color haponColor;
            ColorUtility.TryParseHtmlString("#B5B5B5", out haponColor);

            Color madalingArawColor;
            ColorUtility.TryParseHtmlString("#434343", out madalingArawColor);

            Color gabiColor;
            ColorUtility.TryParseHtmlString("#3F5695", out gabiColor);

            if (hourTime >= 19 && hourTime <= 24 || hourTime >= 1 && hourTime <= 3)
            {
                OpenLights?.Invoke(true);
            }
            else
            {
                OpenLights?.Invoke(false);
            }

            // Color of global light when the game time is 8 am to 2 pm.
            if (hourTime >= 8 && hourTime < 15)
            {
                globalLight.GetComponent<Light2D>().color = Color.white;
            }
            // Color of the global light when the game time is 3 pm to 4 pm
            else if (hourTime >= 15 && hourTime <= 18)
            {
                globalLight.GetComponent<Light2D>().color = Color.Lerp(Color.white, haponColor, (hourTime / 18f));
            }
            // Color of global light when it is 5 pm to 12 am.
            else if (hourTime > 18 && hourTime <= 24)
            {
                globalLight.GetComponent<Light2D>().color = Color.Lerp(Color.white, gabiColor, (hourTime / 24f));
            }
            // When it is 1 am.
            else if (hourTime >= 1 && hourTime <= 1)
            {
                globalLight.GetComponent<Light2D>().color = gabiColor;
            }
            // 1 am to 8 am
            else if (hourTime >= 1 && hourTime <= 8)
            {
                globalLight.GetComponent<Light2D>().color = Color.Lerp(gabiColor, Color.white, (hourTime / 8f));
            }
        }
        catch (System.Exception e)
        {

        }
    }

    void OpenAllEstablishments()
    {
        // If it is NOT daytime, then it must be 5 pm because it is fixed closing hours of establishment.
        if (this.playerData.playerTime.m_ActualHourInRealLife >= 17 && this.playerData.playerTime.m_IsDaytime == false)
        {
            this.playerData.playerTime.m_IsAllEstablishmentsOpen = false;
            DataPersistenceManager.instance?.SaveGame();
        }

        // If it is daytime, then it must be 8 am because it is the fixed opening hours of establishment.
        if (this.playerData.playerTime.m_ActualHourInRealLife >= 8
            && this.playerData.playerTime.m_ActualHourInRealLife != 24
            && this.playerData.playerTime.m_IsDaytime == true)
        {
            this.playerData.playerTime.m_IsAllEstablishmentsOpen = true;
            DataPersistenceManager.instance?.SaveGame();
        }
    }

    void SetIsDaytime()
    {
        // Check if it is 12 am, then day time is set to true.
        if (this.playerData.playerTime.m_ActualHourInRealLife == 24)
        {
            this.playerData.playerTime.m_IsDaytime = true;
        }
        // Check if it is afternoon.
        else if (this.playerData.playerTime.m_ActualHourInRealLife >= 12) 
        {
            this.playerData.playerTime.m_IsDaytime = false;
        }
        else
        {
            this.playerData.playerTime.m_IsDaytime = true;
        }
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public void LoadSlotsData(Slots slots)
    {
        throw new NotImplementedException();
    }

    public void SaveSlotsData(ref Slots slots)
    {
        throw new NotImplementedException();
    }
}
