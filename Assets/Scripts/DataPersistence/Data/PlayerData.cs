using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData 
{
    public string id;
    public bool isNewlyCreated;
    public bool isTutorialDone;
    public bool isIntroductionDone;
    public bool isPreQuestIntroductionDone;
    public bool isNPCIntroductionPanelDone;
    public bool isFromSleeping;
    public string name;
    public string gender;
    public int dunongPoints;
    public int requiredDunongPointsToPlay;
    public int remainingTime;

    public string sceneToLoad;
    public float xPos;
    public float yPos;

    public Notebook notebook;

    public List<RegionData> regionsData;

    public List<Quest> quests;
    public List<Quest> currentQuests;
    public List<Quest> completedQuests;
    public List<NPC_INFO> npcInfos;
    public Inventory inventory;
    public PlayerTime playerTime;

    // LIST OF REGIONS FOR LUZON
    const string ILOCOS_REGION = "Ilocos Region";
    const string CAGAYAN_VALLEY = "Cagayan Valley";
    const string CAR = "Cordillera Administrative Region";
    const string CENTRAL_LUZON = "Central Luzon";
    const string CALABARZON = "CALABARZON";
    const string MIMAROPA = "MIMAROPA";
    const string BICOL_REGION = "Bicol Region";
    const string NCR = "National Capital Region";

    // LIST OF REGIONS FOR VISAYAS
    const string WESTERN_VISAYAS = "Western Visayas";
    const string CENTRAL_VISAYAS = "Central Visayas";
    const string EASTERN_VISAYAS = "Eastern Visayas";

    // LIST OF REGIONS FOR MINDANAO
    const string ZAMBOANGA_PENINSULA = "Zamboanga Peninsula";
    const string NORTHERN_MINDANAO = "Northern Mindanao";
    const string DAVAO_REGION = "Davao Region";
    const string SOCCSKSARGEN = "SOCCSKSARGEN";
    const string CARAGA_REGION = "Caraga Region";
    const string BARMM = "BARMM";

    // LIST OF CATEGORIES
    const string HEROES = "National Heroes";
    const string FESTIVALS = "National Festivals";
    const string TOURIST_ATTRACTIONS = "Tourist Attractions";
    const string GENERAL_KNOWLEDGE = "General Knowledge";

    public PlayerData()
    {
        this.isNewlyCreated = true;
        this.isTutorialDone = false; // This is to track if the tutorial for UI is done.
        this.isIntroductionDone = false; // This is to track if the introduction of the game is already viewed or done.
        this.isPreQuestIntroductionDone = false; 
        this.isNPCIntroductionPanelDone = false;
        this.id = null;
        this.name = null;
        this.gender = "male";
        this.dunongPoints = 0;
        this.requiredDunongPointsToPlay = 65;
        this.remainingTime = 18000;
        this.sceneToLoad = "House";
        this.xPos = 0;
        this.yPos = 0;
        this.regionsData = new List<RegionData>();
        this.notebook = new Notebook();
        this.quests = new List<Quest>();
        this.currentQuests = new List<Quest>();
        this.completedQuests = new List<Quest>();
        this.npcInfos = new List<NPC_INFO>();
        this.inventory = new Inventory();
        this.playerTime = new PlayerTime();

        this.AddRegionsForLuzon();
        this.AddRegionsForVisayas();
        this.AddRegionsForMindanao();

        this.quests.Add(new Quest("A Day with Friendliness", "Meet all the villagers.", "PRE-QUEST", new NumberGoal(2, NumberGoal.CORRESPONDING_OBJECT_TO_COUNT.TALK_NPC)));
        this.quests.Add(new Quest("A Simple Thanks", "Get the Mango from Mang Esterlito and give it to Aling Nena.", "PRE-QUEST", 
            new DeliveryGoal("Mang Esterlito", "Aling Nena", "Can you give this to Aling Nena?", new Item("Mango", 1, "", true))));

        // Quest for Luzon
        this.quests.Add(new Quest("Ilocos Region Quest!", "Get the Mango from Aling Marites and give it to Mang Esterlito", 25, ILOCOS_REGION, 1,
            new DeliveryGoal("Aling Marites", "Mang Esterlito", "Can you give this Mango to Mang Esterlito?",
            new Item("Mango", 1, "", false))));
        this.quests.Add(new Quest("Ilocos Region Quest!", "Get the Mango from Aling Julia and give it to Aling Marites", 25, ILOCOS_REGION, 1,
            new DeliveryGoal("Aling Julia", "Aling Marites", "Can you give this Mango to Aling Marites?",
            new Item("Mango", 2, "", false))));
        this.quests.Add(new Quest("Ilocos Region Quest!", "Talk to Mang Esterlito", 15, ILOCOS_REGION, 1, new TalkGoal("Mang Esterlito")));

        this.quests.Add(new Quest("Cagayan Valley Quest!", "Talk to Aling Nena", 10, CAGAYAN_VALLEY, 2, new TalkGoal("Aling Nena")));

        this.quests.Add(new Quest("CAR Quest!", "Get the Mango from Aling Marites and give it to Mang Esterlito", 25, CAR, 3,
            new DeliveryGoal("Aling Marites", "Mang Esterlito", "Can you give this Mango to Mang Esterlito?",
            new Item("Mango", 1, "", false))));

        this.quests.Add(new Quest("Central Luzon Quest!", "Help Aling Julia to give Aling Marites a Mango", 15, CENTRAL_LUZON, 4,
            new DeliveryGoal("Aling Julia", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Mango", 1, "", false))));

        this.quests.Add(new Quest("NCR Quest!", "Help Mang Esterlito to give Aling Marites a Mango", 5, NCR, 5,
            new DeliveryGoal("Mang Esterlito", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Mango", 3, "", false))));

        this.quests.Add(new Quest("CALABARZON Quest!", "Talk to Aling Julia", 35, CALABARZON, 6, new TalkGoal("Aling Julia")));

        this.quests.Add(new Quest("MIMAROPA Quest!", "Talk to Aling Marites", 15, MIMAROPA, 7, new TalkGoal("Aling Marites")));

        this.quests.Add(new Quest("Bicol Region Quest!", "Help Aling Julia to give Aling Marites a Banana", 5, BICOL_REGION, 8,
            new DeliveryGoal("Aling Julia", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Banana", 2, "", false))));

        
        // Quest for Visayas.
        this.quests.Add(new Quest("Western Visayas Quest!", "Talk to Aling Julia", 35, WESTERN_VISAYAS, 9, new TalkGoal("Aling Julia")));
        this.quests.Add(new Quest("Central Visayas Quest!", "Talk to Aling Marites", 25, WESTERN_VISAYAS, 10, new TalkGoal("Aling Marites")));
        this.quests.Add(new Quest("Eastern Visayas Quest!", "Talk to Aling Marites", 75, WESTERN_VISAYAS, 11, new TalkGoal("Aling Marites")));

        // Quest for Mindanao
        this.quests.Add(new Quest("Zamboanga Peninsula Quest!", "Talk to Aling Julia", 35, ZAMBOANGA_PENINSULA, 12, new TalkGoal("Aling Julia")));
        this.quests.Add(new Quest("Northern Mindanao Quest!", "Talk to Aling Marites", 10, NORTHERN_MINDANAO, 13, new TalkGoal("Aling Marites")));
        this.quests.Add(new Quest("Davao Region Quest!", "Talk to Aling Nena", 35, DAVAO_REGION, 14, new TalkGoal("Aling Nena")));
        this.quests.Add(new Quest("SOCCSKSARGEN Quest!", "Talk to Mang Esterlito", 15, SOCCSKSARGEN, 15, new TalkGoal("Mang Esterlito")));
        this.quests.Add(new Quest("Caraga Region Quest!", "Talk to Aling Marites", 45, CARAGA_REGION, 16, new TalkGoal("Aling Marites")));
        this.quests.Add(new Quest("BARMM Quest!", "Talk to Aling Julia", 325, BARMM, 17, new TalkGoal("Aling Julia")));

        this.AddNPCsInfo();
    }

    /// <summary>
    /// A function that adds all the regions of Luzon.
    /// </summary>
    void AddRegionsForLuzon()
    {
        this.regionsData.Add(
            new RegionData(
                1,
                true,
                ILOCOS_REGION,
                "Ilocos is a region in the Philippines, encompassing the northwestern coast of Luzon island. It’s " +
                "known for its historic sites, beaches and the well-preserved Spanish colonial city of Vigan. " +
                "Dating from the 16th century, Vigan’s Mestizo district is characterized by cobblestone streets and " +
                "mansions with wrought-iron balconies. Farther north, Laoag City is a jumping-off point for the huge La Paz Sand Dunes.",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                2,
                false,
                CAGAYAN_VALLEY,
                "Cagayan Valley, designated as Region II, is an administrative region in the Philippines, " +
                "located in the northeastern section of Luzon Island. It is composed of five Philippine " +
                "provinces: Batanes, Cagayan, Isabela, Nueva Vizcaya, and Quirino.",
                new Category[1] { new Category(TOURIST_ATTRACTIONS) }));

        this.regionsData.Add(new RegionData(
                3,
                false,
                CAR,
                "The Cordillera Administrative Region, also known as the Cordillera Region, or simply, Cordillera, " +
                "is an administrative region in the Philippines, situated within the island of Luzon.",
                new Category[3] { new Category(HEROES), new Category(FESTIVALS), new Category(GENERAL_KNOWLEDGE) }));

        this.regionsData.Add(new RegionData(
                4,
                false,
                CENTRAL_LUZON,
                "",
                new Category[2] { new Category(FESTIVALS), new Category(GENERAL_KNOWLEDGE) }));

        this.regionsData.Add(new RegionData(
                5,
                false,
                NCR,
                "",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                6,
                false,
                CALABARZON,
                "",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                7,
                false,
                MIMAROPA,
                "",
                new Category[2] { new Category(FESTIVALS), new Category(TOURIST_ATTRACTIONS) }));

        this.regionsData.Add(new RegionData(
                8,
                false,
                BICOL_REGION,
                "",
                new Category[1] { new Category(HEROES) }));


    }

    void AddRegionsForVisayas()
    {
        // It should be 9, 10, 11, for testing purposes only.
        this.regionsData.Add(new RegionData(9, false, WESTERN_VISAYAS, "Western Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(10, false, EASTERN_VISAYAS, "Eastern Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(11, false, CENTRAL_VISAYAS, "Central Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
    }

    void AddRegionsForMindanao()
    {
        this.regionsData.Add(new RegionData(12, false, ZAMBOANGA_PENINSULA, "Western Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(13, false, NORTHERN_MINDANAO, "Central Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(14, false, DAVAO_REGION, "Eastern Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) })); 
        this.regionsData.Add(new RegionData(15, false, SOCCSKSARGEN, "Western Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(16, false, CARAGA_REGION, "Central Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
        this.regionsData.Add(new RegionData(17, false, BARMM, "Eastern Visayas is the first region of Visayas major island.", new Category[] { new Category(HEROES) }));
    }

    void AddNPCsInfo()
    {
        this.npcInfos.Add(new NPC_INFO("Mang Esterlito", ""));
        this.npcInfos.Add(new NPC_INFO("Aling Marites", ""));
        this.npcInfos.Add(new NPC_INFO("Aling Julia", ""));
        this.npcInfos.Add(new NPC_INFO("Ramon Villegas", ""));
        this.npcInfos.Add(new NPC_INFO("Gregorio Zaide", ""));
        this.npcInfos.Add(new NPC_INFO("Mikaela Fudolig", ""));
        this.npcInfos.Add(new NPC_INFO("Ivan Henares", ""));
    }

    /**<summary>
     *  A method that counts the number of collectibles of the specified
     *  category.
     *  @param categoryName
     *  - name of the category that is going to be count.
     * </summary> */
     public int NumberOfCollectedCollectiblesFor(string categoryName)
    {
        int count = 0;

        foreach (Collectible collectible in this.notebook.collectibles)
        {
            if (collectible.isCollected && collectible.categoryName.ToUpper() == categoryName.ToUpper())
            {
                count += 1;
            }
        }

        return count;
    }

    /**<summary>
     * Get the total number of collectibles collected.
     * </summary> */
    public int TotalOfCollectibles()
    {
        return this.NumberOfCollectedCollectiblesFor("National Heroes") + this.NumberOfCollectedCollectiblesFor("National Festivals")
            + this.NumberOfCollectedCollectiblesFor("Tourist Attractions") + this.NumberOfCollectedCollectiblesFor("General Knowledge");
    }

    public int TotalNumberOfOpenRegions()
    {
        int count = 0;

        foreach (RegionData regionData in this.regionsData)
        {
            if (regionData.isOpen) count++;
        }

        return count;
    }
}

[System.Serializable]
public class Notebook
{
    public List<Collectible> collectibles;

    const string LUZON = "Luzon";
    const string VISAYAS = "VISAYAS";
    const string MINDANAO = "MINDANAO";

    const string HEROES = "National Heroes";
    const string FESTIVALS = "National Festivals";
    const string TOURIST_ATTRACTIONS = "Tourist Attractions";
    const string GENERAL_KNOWLEDGE = "General Knowledge";

    // LIST OF REGIONS FOR LUZON
    const string REGION_1 = "Ilocos Region";
    const string REGION_2 = "Cagayan Valley";
    const string CAR = "Cordillera Administrative Region";
    const string REGION_3 = "Central Luzon";
    const string REGION_4A = "CALABARZON";
    const string MIMAROPA = "MIMAROPA";
    const string REGION_5 = "Bicol Region";
    const string NCR = "National Capital Region";

    // LIST OF REGIONS FOR VISAYAS
    const string WESTERN_VISAYAS = "Western Visayas";
    const string CENTRAL_VISAYAS = "Central Visayas";
    const string EASTERN_VISAYAS = "Eastern Visayas";

    // LIST OF REGIONS FOR MINDANAO
    const string ZAMBOANGA_PENINSULA = "Zamboanga Peninsula";
    const string NORTHERN_MINDANAO = "Northern Mindanao";
    const string DAVAO_REGION = "Davao Region";
    const string SOCCSKSARGEN = "SOCCSKSARGEN";
    const string CARAGA_REGION = "Caraga Region";
    const string BARMM = "BARMM";

    // PATH
    const string HEROES_FILEPATH = "Collectibles/Heroes/";
    const string FESTIVALS_FILEPATH = "Collectibles/Festivals/";
    const string TOURIST_ATT_FILEPATH = "Collectibles/Tourist Attractions/";

    public Notebook()
    {
        this.collectibles = new List<Collectible>();

        // Collectibles for REGION 1
        this.collectibles.Add(new Collectible("Diego Silang", HEROES_FILEPATH + "Diego Silang", HEROES, LUZON, REGION_1));
        this.collectibles.Add(new Collectible("Cape Bojeador", TOURIST_ATT_FILEPATH + "Cape Bojeador", TOURIST_ATTRACTIONS, LUZON, REGION_1));

        // Collectibles for REGION 2
        this.collectibles.Add(new Collectible("Ivatan Houses", TOURIST_ATT_FILEPATH + "Ivatan House", TOURIST_ATTRACTIONS, LUZON, REGION_2));

        // Collectibles for CAR
        this.collectibles.Add(new Collectible("Panagbenga Festival", FESTIVALS_FILEPATH + "Panagbenga Festival", FESTIVALS, LUZON, CAR));
        this.collectibles.Add(new Collectible("Diplomat Hotel", TOURIST_ATT_FILEPATH + "Diplomat Hotel", TOURIST_ATTRACTIONS, LUZON, CAR));

        // Collectibles for REGION 3
        this.collectibles.Add(new Collectible("Marcelo H. Del Pilar", HEROES_FILEPATH + "Marcelo H. Del Pilar", HEROES, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Gregorio Del Pilar", HEROES_FILEPATH + "Gregorio Del Pilar", HEROES, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Carabao Festival", FESTIVALS_FILEPATH + "Carabao Festival", FESTIVALS, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Hot Air Balloon Festival", FESTIVALS_FILEPATH + "Hot Air Balloon Festival", FESTIVALS, LUZON, REGION_3));

        // Collectibles for CALABARZON (REGION 4A)
        this.collectibles.Add(new Collectible("Batingaw Festival", FESTIVALS_FILEPATH + "Batingaw Festival", FESTIVALS, LUZON, REGION_4A));
        
        // Collectibles for MIMAROPA (REGION 4B) 
        this.collectibles.Add(new Collectible("Moriones Festival", FESTIVALS + "Moriones Festival", HEROES, LUZON, MIMAROPA));

    }
}

[System.Serializable]
public class Collectible
{
    public string name;
    public string imagePath;
    public bool isCollected;
    public string categoryName;
    public string regionName;
    public string majorIsland;

    public Collectible(string name, string imagePath, string categoryName, string majorIsland, string regionName)
    {
        this.name = name;
        this.imagePath = imagePath;
        this.categoryName = categoryName;
        this.majorIsland = majorIsland;
        this.regionName = regionName;
        this.isCollected = false;
    }
}

[System.Serializable]
public class RegionData
{
    public int regionNumber;
    public bool isOpen;
    public string regionName;
    public string information;
    public int currentScore;
    public int highestScore;
    public int noOfStars;

    public List<Category> categories;

    public RegionData(int regionNumber, bool isOpen, string regionName, string information, Category[] categories)
    {
        this.regionNumber = regionNumber;
        this.isOpen = isOpen;
        this.regionName = regionName;
        this.information = information;
        this.currentScore = 0;
        this.highestScore = 0;
        this.categories = new List<Category>(categories);
    }
}

[System.Serializable]
public class Category
{
    public string categoryName;
    public int highestScore;
    public int noOfStars;

    public Category(string categoryName)
    {
        this.categoryName = categoryName;
        this.highestScore = 0;
        this.noOfStars = 0;
    }
}

[System.Serializable]
public class Inventory
{
    public List<Item> items;
    private int MAX_SLOT = 7;
    
    public Inventory()
    {
        this.items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (IsItemExisting(item))
        {
            this.UpdateItem(item);
        }
        else
        {
            if (items.Count != MAX_SLOT)
                this.items.Add(item);
        }
    }

    public void UpdateItem(Item item)
    {
        foreach (Item itemLoop in this.items)
        {
            if (itemLoop.itemName.ToUpper() == item.itemName.ToUpper())
            {
                itemLoop.quantity += item.quantity;
                return;
            }
        }
    }

    public bool IsItemExisting(Item item)
    {
        foreach (Item itemLoop in this.items)
        {
            if (itemLoop.itemName.ToUpper() == item.itemName.ToUpper())
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class PlayerTime 
{
    public int m_ActualHourInRealLife;
    public int m_ActualMinuteInRealLife;
    public bool m_IsDaytime;
    public bool m_IsAllEstablishmentsOpen; // validates if the player is valid to enter in the establishment.
    public int m_DayEvent; // Maximum day event = 5 days.
    public float m_NoOfSecondsPerTwoAndHalfMinutes;
    public float m_NoOfSecondsPerMinute;

    public float m_GameTimeHour;
    public float m_GameTimeMinute;
    public float m_GameTimeSeconds;

    public float SECONDS_PER_HOUR { get; set; } // It must be 150 seconds = 2.5 minutes.
    public float SECONDS_PER_MINUTE { get; set; } // 
    public PlayerTime()
    {
        this.m_ActualHourInRealLife = 8;
        this.m_IsDaytime = true;
        this.m_IsAllEstablishmentsOpen = true;
        this.m_DayEvent = 1;
        SECONDS_PER_HOUR = 1f;
        SECONDS_PER_MINUTE = 0.15f;
        this.m_NoOfSecondsPerTwoAndHalfMinutes = SECONDS_PER_HOUR;
        this.m_NoOfSecondsPerMinute = SECONDS_PER_MINUTE;

        // Game Time
        this.m_GameTimeHour = 0f;
        this.m_GameTimeMinute = 0f;
        this.m_GameTimeSeconds = 0f;
    }
}

[System.Serializable]
public class NPC_INFO
{
    public string name;
    public string info;
    public bool isMet;
    public float xPos;
    public float yPos;

    public NPC_INFO(string name, string info)
    {
        this.name = name;
        this.info = info;
        this.isMet = false;
    }

    public NPC_INFO(string name, string info, bool isMet)
    {
        this.name = name;
        this.info = info;
        this.isMet = isMet;
    }

    public NPC_INFO Copy( )
    {
        NPC_INFO copy = new NPC_INFO(this.name, this.info, this.isMet);
        copy.xPos = this.xPos;
        copy.yPos = this.yPos;

        return copy;
    }
}