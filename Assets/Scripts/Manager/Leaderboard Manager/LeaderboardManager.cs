using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LeaderboardManager : MonoBehaviour
{
    // SINCE THIS CLASS ONLY RUNS INSIDE THE Leaderboard Scene, we only drag and drop the needed components.
    [SerializeField] private Transform leaderboardItemContainer;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    [Header("Leaderboard Item Prefab")]
    [SerializeField] private Transform leaderboardItemPrefab;

    private Slots slots;
    public List<LeaderboardData> leaderboardData;

    /** <summary>
     *  The leaderboard item consists of
     *  
     *  RANK (int)
     *  Profile (string)
     *  Region (int)
     *  Heroes (int)
     *  Festivals (int)
     *  Tourist Attractions (int)
     *  General Knowledge (int)
     *  Total (int)
     * </summary>*/
    [System.Serializable]
    public class LeaderboardData : IComparable<LeaderboardData>
    {
        public PlayerData PlayerData { get; private set; }

        public string Profile { get; private set; }
        public int Region { get; private set; }
        public int Heroes { get; private set; }
        public int Festivals { get; private set; }
        public int TouristAtt { get; private set; }
        public int GeneralKnowledge { get; private set; }
        public int Total { get; private set; }

        public LeaderboardData(PlayerData playerData)
        {
            this.PlayerData = playerData;

            this.Heroes = this.PlayerData.NumberOfCollectedCollectiblesFor("National Heroes");
            this.Festivals = this.PlayerData.NumberOfCollectedCollectiblesFor("National Festivals");
            this.TouristAtt = this.PlayerData.NumberOfCollectedCollectiblesFor("Tourist Attractions");
            this.GeneralKnowledge = this.PlayerData.NumberOfCollectedCollectiblesFor("General Knowledge");
            this.Region = this.PlayerData.TotalNumberOfOpenRegions();
            this.Total = this.PlayerData.TotalOfCollectibles();
            this.Profile = this.PlayerData.name;
        }

        public int CompareTo(LeaderboardData other)
        {
            //if (this.PlayerData.TotalOfCollectibles() > other.PlayerData.TotalOfCollectibles()) return 1;
            //else if (this.PlayerData.TotalOfCollectibles() < other.PlayerData.TotalOfCollectibles())
            //return -1;

            //return 0;

            if (this.PlayerData.TotalOfCollectibles() == other.PlayerData.TotalOfCollectibles())
            {
                print(this.PlayerData.name + " and " + other.PlayerData.name + " is comparing");
                if (this.PlayerData.playerTime.m_GameTimeHour != other.PlayerData.playerTime.m_GameTimeHour)
                {
                    if (this.PlayerData.playerTime.m_GameTimeHour > other.PlayerData.playerTime.m_GameTimeHour) return 1;
                    else if (this.PlayerData.playerTime.m_GameTimeHour < other.PlayerData.playerTime.m_GameTimeHour) return -1;
                    else return 0;
                }
                else if (this.PlayerData.playerTime.m_GameTimeMinute != other.PlayerData.playerTime.m_GameTimeMinute)
                {
                    if (this.PlayerData.playerTime.m_GameTimeMinute > other.PlayerData.playerTime.m_GameTimeMinute) return 1;
                    else if (this.PlayerData.playerTime.m_GameTimeMinute < other.PlayerData.playerTime.m_GameTimeMinute) return -1;
                    else return 0;
                }
                else if (this.PlayerData.playerTime.m_GameTimeSeconds != other.PlayerData.playerTime.m_GameTimeSeconds)
                {
                    if (this.PlayerData.playerTime.m_GameTimeSeconds > other.PlayerData.playerTime.m_GameTimeSeconds) return 1;
                    else if (this.PlayerData.playerTime.m_GameTimeSeconds < other.PlayerData.playerTime.m_GameTimeSeconds) return -1;
                    else return 0;
                }
            }
            else 
            {
                print(this.PlayerData.name + " and " + other.PlayerData.name + " is comparing 2");

                if (this.PlayerData.TotalOfCollectibles() < other.PlayerData.TotalOfCollectibles()) return 1;
                else if (this.PlayerData.TotalOfCollectibles() > other.PlayerData.TotalOfCollectibles()) return -1;
                else return 0;
            }
            return 0;
        }
    }

    private void Start()
    {
        PlayerDataHandler playerDataHandler = null;

        SlotsFileHandler slotsFileHandler = new SlotsFileHandler();
        this.slots = slotsFileHandler.Load();

        this.leaderboardData = new List<LeaderboardData>(this.slots.ids.Count);

        for (int i = 0; i < this.slots.ids.Count; i++)
        {
            playerDataHandler = new PlayerDataHandler(this.slots.ids[i]);

            PlayerData currentPlayerData = playerDataHandler.Load();

            this.leaderboardData.Add(new LeaderboardData(currentPlayerData));
            
            playerDataHandler = null;
        }

        this.DisplayLeaderboards();
        this.closeButton.onClick.AddListener(CloseLeaderboardScene);
    }


    void DisplayLeaderboards()
    {
        /**Sort the data based on the total number of collectibles collected.*/
        this.leaderboardData.Sort();
        //this.leaderboardData.Reverse();

        int rank = 1;

        foreach (LeaderboardData ld in this.leaderboardData)
        {
            Transform leaderboardItem = Instantiate(this.leaderboardItemPrefab.gameObject, this.leaderboardItemContainer).transform;

            string time = ld.PlayerData.playerTime.m_GameTimeHour + ":" + ld.PlayerData.playerTime.m_GameTimeMinute + ":" + ((int)ld.PlayerData.playerTime.m_GameTimeSeconds);

            leaderboardItem.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = rank.ToString();
            leaderboardItem.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ld.PlayerData.name;
            leaderboardItem.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = time;
            leaderboardItem.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Region.ToString(); // Number of regions.
            leaderboardItem.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Heroes.ToString(); // Number of Heroes Collectibles
            leaderboardItem.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Festivals.ToString(); // Number of Festivals Collectibles
            leaderboardItem.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = ld.TouristAtt.ToString(); // Number of Tourist Attractions
            leaderboardItem.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>().text = ld.GeneralKnowledge.ToString(); // Number of General Knowledge
            leaderboardItem.GetChild(8).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Total.ToString(); // Total Colectibles
            rank++;
        }
    }

    private int CompareLeaderboard2(LeaderboardData px, LeaderboardData py)
    {
        if (px.PlayerData.TotalOfCollectibles() == py.PlayerData.TotalOfCollectibles())
        {
            if (px.PlayerData.playerTime.m_GameTimeHour != py.PlayerData.playerTime.m_GameTimeHour)
            {
                if (px.PlayerData.playerTime.m_GameTimeHour > py.PlayerData.playerTime.m_GameTimeHour) return 1;
                else if (px.PlayerData.playerTime.m_GameTimeHour < py.PlayerData.playerTime.m_GameTimeHour) return -1;
                else return 0;
            }
            else if (px.PlayerData.playerTime.m_GameTimeMinute != py.PlayerData.playerTime.m_GameTimeMinute)
            {
                if (px.PlayerData.playerTime.m_GameTimeMinute > py.PlayerData.playerTime.m_GameTimeMinute) return 1;
                else if (px.PlayerData.playerTime.m_GameTimeMinute < py.PlayerData.playerTime.m_GameTimeMinute) return -1;
                else return 0;
            }
            else if (px.PlayerData.playerTime.m_GameTimeSeconds != py.PlayerData.playerTime.m_GameTimeSeconds)
            {
                if (px.PlayerData.playerTime.m_GameTimeSeconds > py.PlayerData.playerTime.m_GameTimeSeconds) return 1;
                else if (px.PlayerData.playerTime.m_GameTimeSeconds < py.PlayerData.playerTime.m_GameTimeSeconds) return -1;
                else return 0;
            }
        }
        else
        {
            if (px.PlayerData.TotalOfCollectibles() > py.PlayerData.TotalOfCollectibles()) return 1;
            else if (px.PlayerData.TotalOfCollectibles() < py.PlayerData.TotalOfCollectibles()) return -1;
            else return 0;
        }
        return 0;
    }

    /** <summary>
     *  IMPLEMENTATION of compararer to compare two leaderboard instance.
     * </summary>*/
    private class CompareLeaderboard : IComparer
    {
        public int Compare(object x, object y)
        {
            LeaderboardData px = x as LeaderboardData;
            LeaderboardData py = y as LeaderboardData;

            print(px.PlayerData.TotalOfCollectibles() + " : " + py.PlayerData.TotalOfCollectibles());

            if (px.PlayerData.TotalOfCollectibles() > py.PlayerData.TotalOfCollectibles())
                return -1;
            else if ((px.PlayerData.playerTime.m_GameTimeHour < py.PlayerData.playerTime.m_GameTimeHour)
                && (px.PlayerData.playerTime.m_GameTimeMinute < py.PlayerData.playerTime.m_GameTimeMinute)
                && (px.PlayerData.playerTime.m_GameTimeSeconds < py.PlayerData.playerTime.m_GameTimeSeconds))
                return 0;

            return 0;
        }
    } 

    public static IComparer ComparePlayerData()
    {
        return new  CompareLeaderboard();
    }

    private void CloseLeaderboardScene()
    {
        SoundManager.instance?.PlaySound("Button Click 1");
        TransitionLoader.instance?.StartAnimation("Menu");
    }
}
