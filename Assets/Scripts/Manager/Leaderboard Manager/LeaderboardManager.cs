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
    private LeaderboardData[] leaderboardData;

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
    class LeaderboardData
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
        }
    }

    private void Start()
    {
        PlayerDataHandler playerDataHandler = null;

        SlotsFileHandler slotsFileHandler = new SlotsFileHandler();
        this.slots = slotsFileHandler.Load();

        this.leaderboardData = new LeaderboardData[this.slots.ids.Count];

        for (int i = 0; i < this.slots.ids.Count; i++)
        {
            playerDataHandler = new PlayerDataHandler(this.slots.ids[i]);

            PlayerData currentPlayerData = playerDataHandler.Load();

            this.leaderboardData[i] = new LeaderboardData(currentPlayerData);

            playerDataHandler = null;
        }

        this.DisplayLeaderboards();

        this.closeButton.onClick.AddListener(CloseLeaderboardScene);
    }

    void DisplayLeaderboards()
    {
        /**Sort the data based on the total number of collectibles collected.*/
        Array.Sort(this.leaderboardData, ComparePlayerData()); 

        int rank = 1;

        foreach (LeaderboardData ld in this.leaderboardData)
        {
            Transform leaderboardItem = Instantiate(this.leaderboardItemPrefab.gameObject, this.leaderboardItemContainer).transform;

            leaderboardItem.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = rank.ToString();
            leaderboardItem.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ld.PlayerData.name;
            leaderboardItem.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Region.ToString(); // Number of regions.
            leaderboardItem.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Heroes.ToString(); // Number of Heroes Collectibles
            leaderboardItem.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Festivals.ToString(); // Number of Festivals Collectibles
            leaderboardItem.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = ld.TouristAtt.ToString(); // Number of Tourist Attractions
            leaderboardItem.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = ld.GeneralKnowledge.ToString(); // Number of General Knowledge
            leaderboardItem.GetChild(7).GetComponent<TMPro.TextMeshProUGUI>().text = ld.Total.ToString(); // Total Colectibles
            rank++;
        }
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

            if (px.PlayerData.TotalOfCollectibles() > py.PlayerData.TotalOfCollectibles())
                return -1;
            return 0;
        }
    }

    public static IComparer ComparePlayerData()
    {
        return (IComparer)new CompareLeaderboard();
    }

    private void CloseLeaderboardScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
