using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public PlayerData previousData;
    public string id;
    public bool isGameCompleted;
    public bool isNewlyCreated;
    public bool isTutorialDone;
    public bool isIntroductionDone;
    public bool isPreQuestIntroductionDone;
    public bool isNPCIntroductionPanelDone;
    public bool isFromSleeping;
    public bool isSleepPanelNotShown;
    public bool isFirstTimeGoingToSchool;
    public bool isQuestReset;
    public bool hasNewOpenRegion;
    public string versionNumber;

    // Icons Notification Booleans
    public bool questNewIcon;
    public bool notesNewIcon;
    public bool achievementsNewIcon;

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
    public List<Quest> notesInfos;

    public Inventory inventory;
    public PlayerTime playerTime;
    public MemoryGameData memoryGameData;

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

    const string INFO_PATH = "Delivery Ink Informations";
    const string THANKYOU_PATH = "Delivery Ink Informations/Thank You Inks/";

    public PlayerData()
    {
        this.isGameCompleted = false;
        this.isNewlyCreated = true;
        this.isTutorialDone = false; // This is to track if the tutorial for UI is done.
        this.isIntroductionDone = false; // This is to track if the introduction of the game is already viewed or done.
        this.isPreQuestIntroductionDone = false;
        this.isNPCIntroductionPanelDone = false;
        this.isSleepPanelNotShown = true;
        this.isFirstTimeGoingToSchool = true;
        this.isQuestReset = false;
        this.hasNewOpenRegion = false;
        this.versionNumber = "4.0";

        this.questNewIcon = false;
        this.achievementsNewIcon = false;
        this.notesNewIcon = false;
         
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
        this.notesInfos = new List<Quest>();
        this.inventory = new Inventory();
        this.playerTime = new PlayerTime();
        this.memoryGameData = new MemoryGameData();

        this.AddRegionsForLuzon(); 
        this.AddRegionsForVisayas();
        this.AddRegionsForMindanao();

        // PRE-QUEST
        this.PreQuest();

        ////// Regions Quest
        this.QuestForIlocosRegion();
        this.QuestForCagayanValley();
        this.QuestForCAR();
        this.QuestForCentralLuzon();
        this.QuestForNCR();
        this.QuestForCALABARZON();
        this.AddQuestToMIMAROPA();
        this.QuestForBicolRegion();
        this.QuestForWesternVisayas();
        this.QuestForCentralVisayas();
        this.QuestForEasternVisayas();
        this.QuestForZamboangaPeninsula();
        this.QuestForNorthernMindanao();
        this.QuestForDavaoRegion();
        this.QuestForSOCCSKSARGEN();
        this.QuestForCARAGARegion();
        this.QuestForBARMM();

        this.AddNPCsInfo();
        this.InitializeRequiredDunongPointsToPlay(1);
    }

    public void InitializeRequiredDunongPointsToPlay(int regionNum)
    {
        int totalRequiredDP = 0;

        foreach (Quest quest in this.quests)
        {
            // By default is Ilocos Region.
            if (quest.regionNum == regionNum)
            {
                totalRequiredDP += quest.dunongPointsRewards;
            }
        }

        this.requiredDunongPointsToPlay = totalRequiredDP;
    }

    public void PreQuest()
    {
        this.quests.Add(new Quest("A Day with Friendliness", "Meet all the villagers.", "PRE-QUEST", new NumberGoal(12, NumberGoal.CORRESPONDING_OBJECT_TO_COUNT.TALK_NPC)));
        this.quests.Add(new Quest("Preparation for Lunch!", "Find a market store that has blue roof.", "Buy your mother a malunggay from the market nearby.", 25, "PRE-QUEST", 0,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Store in Market", "Buy a Malunggay?", new List<Item> {new Item("Malunggay", 1, false, "Malunggay", "Delivery Ink Informations/Malunggay", "PCP/Malunggay")})
                },
                "Aling Nena",
                "Go buy a <b>Malunggay</b> to a nearby store for our lunch later.",
                "Buy a Malunggay?"
            )));
    }

    public void QuestForIlocosRegion()
    {
        this.quests.Add(new Quest("Gift of the North", "I can talk to Mikaela for Malacanang of the North.", "Talk to <b>Mikaela Fudolig</b>", 25, ILOCOS_REGION, 1,
        new ShowPhotoAlbumGoal("Mikaela Fudolig",
        new Item[] { new Item("None", 0, false, "Malacanang of the North", "Delivery Ink Informations/Malacanang of the North", "PCP/Malacanang of the North") })));

        this.quests.Add(new Quest("Beautiful Places", "I can reach Zardo Domenios about Pagudpud Beach.", "Talk to <b>Zardo Domenios</b>", 25, ILOCOS_REGION, 1,
        new ShowPhotoAlbumGoal("Zardo Domenios",
        new Item[]
            {
                        new Item("None", 0, false, "Pagudpud Beach", "Delivery Ink Informations/Pagudpud Beach", "PCP/Pagudpud Beach"),
                        new Item("None", 0, false, "Hundred Islands", "Delivery Ink Informations/Hundred Islands", "PCP/Hundred Islands")
            })));

        this.quests.Add(new Quest("Ilocos' Notable Heroes!", "He is a man with <b>long hair</b> and <b>eyeglasses</b>.", "Go to Gregorio Zaide for National Heroes.", "Talk to someone who is an expert on our National Heroes.",
            15, ILOCOS_REGION, 1, new TalkGoal("Gregorio Zaide")));

        this.quests.Add(new Quest("A Bloody Lunch?!", "I can recap the info about Puto Festival to Ivan Henares.", "Help Ivan Henares to get his lunch at the karinderya.", 25, ILOCOS_REGION, 1,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Karinderya", "Buy a <b>Puto</b> and <b>Dinuguan</b>?", new List<Item> {
                        new Item("Puto", 1, false, "Puto Festival", "Delivery Ink Informations/Puto Festival", "PCP/Puto Festival"),
                        new Item("Dinuguan", 1, false, "", "", "")
                    })
                },
                "Ivan Henares",
                "Hi! Can you I ask you a favor to buy me a <b>Puto</b> and <b>Dinuguan</b> at the <b>Karinderya</b> for my lunch?",
                "Buy <b>Puto</b> and <b>Dinuguan</b>?"
            )));

        this.quests.Add(new Quest("Chef's Request!", "You can buy the recipe to a nearby store with a <b>BLUE</b> roof.", "Reach Mang Rolando about the info about his recipes.", "Help Mang Rolando to gather his recipes.", 25, ILOCOS_REGION, 1,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Store in Market", "Buy Empanada and Longganisa?", new List<Item> {
                        new Item("Empanada", 1, false, "Empanada Festival", "Delivery Ink Informations/Empanada Festival", "PCP/Empanada Festival"),
                        new Item("Longganisa", 1, false, "Longganisa Festival", "Delivery Ink Informations/Longganisa Festival", "PCP/Longganisa Festival")
                    })
                },
                "Rolando Laudico",
                "Hello, do you mind helping me to get my recipe to a market nearby? The recipe that I needed is <b>Empanada</b> and <b>Longganisa</b>.",
                "Buy Empanada and Longganisa?"
            )));

        this.quests.Add(new Quest("A Forgotten Costume", 
            "You can find <b>Joseph</b> at <b>School</b>.",
            "I can go to Joseph to know about Diego Silang.", "Help Joseph to get his costume.", 25, ILOCOS_REGION, 1,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Gregorio Zaide",
                    "Hi. Thank you so much for helping me to give this to my son. After you gave this, you will get an info about to the character that he is role playing.",
                    new List<Item> {new Item("Salakot", 1, false, "Diego Silang", "Delivery Ink Informations/Diego Silang", "PCP/Diego Silang")})
                },
                "Joseph",
                "Hello! I have a favor for you. Can you get the salakot from my father Gregorio Zaide?",
                "Hi. Thank you so much for helping me to give this to my son. After you gave this, you will get an info about to the character that he is role playing."
            )));
    }

    public void QuestForCagayanValley()
    {
        this.quests.Add(new Quest("A Beauty in Darkness", "I can go and talk to Ivan Henares about Aglipay and Callao caves.", "Go and talk to Ivan Henares.", 25, CAGAYAN_VALLEY, 2,
         new ShowPhotoAlbumGoal("Ivan Henares",
         new Item[] { 
             new Item("None", 0, false, "Aglipay Cave Quirino", "Delivery Ink Informations/Aglipay Cave Quirino", "PCP/Aglipay Cave Quirino") ,
             new Item("None", 0, false, "Callao Cave", "Delivery Ink Informations/Callao Cave", "PCP/Callao Cave")
         })));

        this.quests.Add(new Quest("Looks like a Cigarette", "I can talk to Dale Abenjobar about this Marlboro Country", "Go and talk to Dale Abenjobar.", 25, CAGAYAN_VALLEY, 2,
         new ShowPhotoAlbumGoal("Dale Abenjobar",
         new Item[] { new Item("None", 0, false, "Marlboro Country", "Delivery Ink Informations/Marlboro Country", "PCP/Marlboro Country") })));

        this.quests.Add(new Quest("Strength of a Great Typhoon!", "Ivatan House is located at school.", "Read the info about Ivatan House at <b>School</b>.",
          15, CAGAYAN_VALLEY, 2, new TalkGoal("Ivatan House")));
    }

    public void QuestForCAR()
    {

        this.quests.Add(new Quest("Flowers In Powers", "I can approach BJ Pascual about Panagbenga Festival.",
            "Help BJ Pascual to get the flowers from a nearby flower shop.", 10, CAR, 3,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Flower Shop", "Buy a Flower?", new List<Item> {
                        new Item("Flower", 1, false, "Panagbenga Festival", "Delivery Ink Informations/Panagbenga Festival", "PCP/Panagbenga Festival")
                    })
                },
                "BJ Pascual",
                "Hi! I'm planning to give my friend a present when she got home. Do you mind if you get me some flowers at nearby flower shop?",
                "Buy a flower?"
            )));

        this.quests.Add(new Quest("Jam of Sweetness", "I can approach Chef Rolando for the info about La Trinidad Strawberry Farms.",
            "Help Mang Rolando to get a strawberry jam from a nearby store.", 10, CAR, 3,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Strawberry Jam Shop", "Buy Strawberry Jam?", new List<Item> {
                        new Item("Strawberry Jam", 7, false, "La Trinidad Strawberry Farm", "Delivery Ink Informations/La Trinidad Strawberry Farm", 
                        "PCP/La Trinidad Strawberry Farm")
                    })
                },
                "Rolando Laudico",
                "Hi! I'm planning to bake a unque bread for the miryenda menu of the karinderya. Do you mind to buy me a strawberry jam at the local strawberry jam store?",
                "Buy a strawberry jam?"
            )));

        this.quests.Add(new Quest("A Home Full of Colors", "I can go to school to know the info about Sto Bosa.",
            "Find a photo stand about colorful houses at school.", 15, CAR, 3, new TalkGoal("Colors of Stobosa")));

        this.quests.Add(new Quest("A Stairs of Rice", "Picture of Banaue Rice Terraces at School.",
            "Find a photo stand of Banaue Rice Terraces at school.", 15, CAR, 3, new TalkGoal("Banaue Rice Terraces")));

        this.quests.Add(new Quest("Mines View Park", "I can talk to Dale Abenjobar about Mines View Park", "Help Ivan Henares to give a rope to Dale Abenjobar.",
        25, CAR, 3,
        new DeliveryGoal("Ivan Henares", "Dale Abenjobar", "Do you mind to give this rope to Dale?",
        THANKYOU_PATH + "Mines View Park", THANKYOU_PATH + "Mines View Park 2",
        "Text", new Item[] { new Item("Rope", 1, false, "", "", "") })));

        this.quests.Add(new Quest("Blessed With a Best View!", "Rix the Seminarian has a knowledge about the Our Lady of Lourdes.", 
            "Talk to Rix the Seminarian.", 15, CAR, 3, new TalkGoal("Rix the Seminarian")));
    }

    public void QuestForCentralLuzon()
    {
        this.quests.Add(new Quest("Looks like a Parol!", "Go to museum and look for a parol",
            "Go to <b>Museum</b> and see the info about an object that looks like a lantern.",
            15, CENTRAL_LUZON, 4, new TalkGoal("Giant Lantern")));

        this.quests.Add(new Quest("Ashes' Catastrophe!", "Mae Jardaleza has a knowledge about Mt. Pinatubo.",
            "Talk to someone who is good at Volcanoes.",
            15, CENTRAL_LUZON, 4, new TalkGoal("Mae Jardaleza")));

        this.quests.Add(new Quest("Balloons and Carabaos!", "I can reach BJ Pascual about Hot Air Balloon and Carabao Festival.", 
            "Talk to someone who takes pictures about festivals.", 15, CENTRAL_LUZON, 4,
            new ShowPhotoAlbumGoal("BJ Pascual",
            new Item[] {
                new Item("None", 0, false, "Hot Air Balloon Festival", INFO_PATH + "/Hot Air Balloon Festival", "PCP/Hot Air Balloon Festival"),
                new Item("None", 0, false, "Carabao Festival", INFO_PATH + "/Carabao Festival", "PCP/Carabao Festival")
            })));

        this.quests.Add(new Quest("The Del Pilars", "I can ask Gregorio Zaide to know about Del Pilars.", "Help Ramon Villegas to give the antique sword to Gregorio Zaide.", 
            25, CENTRAL_LUZON, 4,
            new DeliveryGoal("Ramon Villegas", "Gregorio Zaide", "Hello! I have a favor. Do you mind if you give this to Gregorio Zaide?",
            INFO_PATH + "/Del Pilar and Trecson", INFO_PATH + "/Del Pilar and Trecson 2",
            "Text", new Item[] { new Item("Sword", 1, false, "", "", "") })));

        this.quests.Add(new Quest("Dinamulag Festival", "I can ask Ramon Villegas about Dinamulag Festival.", 
            "Help Rolando to give 3 pcs of Mango to Ramon Villegas.",
          25, CENTRAL_LUZON, 4,
          new DeliveryGoal("Rolando Laudico", "Ramon Villegas", "Hello! Do you mind to give this to Ramon Villegas?",
          THANKYOU_PATH + "Dinamulag Festival", THANKYOU_PATH + "Dinamulag Festival 2",
          "Not Text", new Item[] { new Item("Mango", 3, false, "", "", "") },
          new Item[] {
                    new Item("Dinamulag Festival", 0, false, "Dinamulag Festival", INFO_PATH + "/Dinamulag Festival", "PCP/Dinamulag Festival")
          })));
    }

    public void QuestForNCR()
    {
        // WALA PANG DIALOGUE ITO
        this.quests.Add(new Quest("A Store and a Shield", "Go to Gregorio Zaide for National Heroes.", "Talk to someone who is an expert on our National Heroes.",
           15, NCR, 5, new TalkGoal("Gregorio Zaide")));

        this.quests.Add(new Quest("Black Nazarene", "Black Nazarene is located at Church.", "Go to Church to know about Black Nazarene.",
           15, NCR, 5, new TalkGoal("Black Nazarene")));

        this.quests.Add(new Quest("Facing a New Chapter", "Find a statue of a dragon.", "I can go to school to know the info about Chinese New Year.", "Find and know the info of Chinese New Year.",
           15, NCR, 5, new TalkGoal("Chinese New Year")));

        this.quests.Add(new Quest("A Walled City", "A <b>Photo stand</b> can be located beside of a building with a pink roof.", "I can go to school to know the info about Intramuros.", "Find a photostand of an Intramuros at the school event.",
           15, NCR, 5, new TalkGoal("Intramuros")));

        this.quests.Add(new Quest("Bagumbayan", "It is located near of the <b>Luzon booth</b>.", "I can go to school to know the info about Luneta Park.", "Find a Rizal monument at the school event.",
           15, NCR, 5, new TalkGoal("Rizal Statue")));

        this.quests.Add(new Quest("A Storage of the Past", "You can find it at the front of the Museum.", "I can go to the front of the Museum to know the info about it.", "Go and check the front of the museum to know the info about National Museum.",
           15, NCR, 5, new TalkGoal("Museum Info")));
    }

    public void QuestForCALABARZON()
    {
        this.quests.Add(new Quest("He Don't Need A Sword.", "I can go to Jose Rizal's bust inside the museum.",
        "Go to <b>Museum</b> and see the info about Jose Rizal.",
        15, CALABARZON, 6, new TalkGoal("Jose Rizal")));

        this.quests.Add(new Quest("The Revolutionaries", "I can go to Gregorio to know about the revolutionaries.",
        "Talk to Gregorio Zaide.",
        15, CALABARZON, 6, new TalkGoal("Gregorio Zaide")));

        this.quests.Add(new Quest("Colorful Leaves!", "I can go to BJ Pascual to know what is Pahiyas Festival.", 
            "Buy an unusual leaves at a nearby store for BJ Pascual and get a sampaguita to Gregorio Zaide.", 25, CALABARZON, 6,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Store in Market", "Buy Kiping?", new List<Item> {
                        new Item("Kiping", 5, false, "Pahiyas Festival", "Delivery Ink Informations/Pahiyas Festival", "PCP/Pahiyas Festival"),
                         new Item("None", 5, false, "Higantes Festival", "Delivery Ink Informations/Higantes Festival", "PCP/Higantes Festival")
                    }),
                    new ItemGiver("Gregorio Zaide", "Get the Sampaguita?", new List<Item> {
                        new Item("Sampaguita", 5, false, "Sampaguita Festival", "Delivery Ink Informations/Sampaguita Festival", "PCP/Sampaguita Festival")
                    })
                },
                "BJ Pascual",
                "Hello, do you mind to get me a kiping at a nearby market?",
                "Buy a Kiping leaves?"
            )));

        this.quests.Add(new Quest("A Great Caldera", "I can go to school to know about the Taal Volcano.",
            "Find the Taal Volcano info.",
        15, CALABARZON, 6, new TalkGoal("Taal Volcano")));

        this.quests.Add(new Quest("Holy Tall", "I can go to school to know about the Kamay ni Jesus.",
            "Find the inf about Kamay ni Jesus.",
        15, CALABARZON, 6, new TalkGoal("Kamay ni Jesus")));

        this.quests.Add(new Quest("Giant's Celebration", "I can go to school to know about the Higantes Festival.",
            "Find the info about Higantes Festival.",
        15, CALABARZON, 6, new TalkGoal("Higantes Festival")));

        this.quests.Add(new Quest("Aguinaldo Shrine", "I can go to school to know about the Aguinaldo Shrine.",
            "Go to school and search for Aguinaldo Shrine.",
        15, CALABARZON, 6, new TalkGoal("Aguinaldo Shrine")));
    }

    // REGION 4B (MIMAROPA)
    public void AddQuestToMIMAROPA()
    {
        this.quests.Add(new Quest("Unusual Jar", "I can go to museum to know about the Manunggul Jar.",
            "Find a Jar that you can get an info.",
        15, MIMAROPA, 7, new TalkGoal("Manunggul Jar")));

        this.quests.Add(new Quest("Palawan Pearls", "I can go to museum to know about the Palawan Pearl.",
            "Find a Pearl inside the museum.",
        15, MIMAROPA, 7, new TalkGoal("Palawan Pearl")));

        this.quests.Add(new Quest("Chef's Request!", "You can buy the recipe to a nearby store with a <b>BLUE</b> roof.", "I can go to Mang Rolando for Banana Festival info.", "Help Mang Rolando to get a Banana at the nearby store.", 25, MIMAROPA, 7,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Store in Market", "Buy 5pcs of Banana?", new List<Item> {
                        new Item("Banana", 5, false, "Banana Festival", INFO_PATH + "/Banana Festival", "PCP/Banana Festival")
                    })
                },
                "Rolando Laudico",
                "Hello, do you mind helping me to get my recipe to a market nearby? The recipe that I needed is <b>Empanada</b> and <b>Longganisa</b>.",
                "Buy Empanada and Longganisa?"
            )));

        this.quests.Add(new Quest("Moriones Festival", "I can go to school to know about the Moriones Festival.",
            "Find 4 manequin soldiers at <b>School</b>.",
        15, MIMAROPA, 7, new TalkGoal("Moriones Festival")));

        this.quests.Add(new Quest("Underground and Reefs", "I can ask Zardo to know about Underground and Reefs.",
            "Help Rolando to give a banana to Zardo Domenios.",
          25, MIMAROPA, 7,
          new DeliveryGoal("Rolando Laudico", "Zardo Domenios", "Hey! Do you mind giving this to Zardo?",
          THANKYOU_PATH + "Underground River and Reef", THANKYOU_PATH + "Underground River and Reef 2",
          "Not Text", new Item[] { new Item("Banana", 3, false, "", "", "") },
          new Item[] {
                    new Item("Puerto Princessa Underground River", 0, false, "Puerto Princessa Underground River", 
                    INFO_PATH + "/Puerto Princessa Underground River", "PCP/Puerto Princessa Underground River"),

                    new Item("Tubbataha Reef Natural Park", 0, false, "Tubbataha Reef Natural ParK",
                    INFO_PATH + "/Tubbataha Reef Natural Park", "PCP/Tubbataha Reef Natural Park")
          })));
    }

    public void QuestForBicolRegion()
    {
        this.quests.Add(new Quest("Cowboy Yarn?", "I can to BJ Pascual to know about Rodeo Festival",
        "BJ Pascual is requesting for the costume of her friend.", 25, BICOL_REGION, 8,
        new RequestGoal
        (
            new ItemGiver[]
            {
                new ItemGiver("Ramon Villegas", "Get the Hat?", new List<Item> {
                    new Item("Cowboy Hat", 1, false, "Rodeo Festival", INFO_PATH + "/Rodeo Festival", "PCP/Rodeo Festival")
                })
            },
            "BJ Pascual",
            "Do you mind if you get the <b>cowboy hat</b> to Ramon Villegas? My friend will use that as a costume at her school.",
            ""
        )));

        this.quests.Add(new  Quest("Cone of an Ice Cream", "I can get the info of Mayon Volcano at School",
        "Learn about Mayon Volcano",
        15, BICOL_REGION, 8, new TalkGoal("Mayon Volcano")));

        this.quests.Add(new Quest("Butanding", "I can go to school to know the info of Whale Shark",
        "Look for an object that looks like a fish.",
        15, BICOL_REGION, 8, new TalkGoal("Sorsogon's Whale Shark")));

        this.quests.Add(new Quest("Surfer Yarn?", "I can to Zardo to know about CamSur Watersports Complex",
       "Zardo Domenios want to surf. Help him to get the surfboard to Dale Abenjobar.", 25, BICOL_REGION, 8,
       new RequestGoal
       (
           new ItemGiver[]
           {
                new ItemGiver("Dale Abenjobar", "Get the Surf board?", new List<Item> {
                    new Item("Surfboard", 1, false, "Rodeo Festival", INFO_PATH + "/Camsur Watersports", "PCP/Camsur Watersports")
                })
           },
           "Zardo Domenios",
           "Do you mind if you get the surfboard to Dale Abenjobar?",
           ""
       )));
    }

    // VISAYAS REGION
    // Region 6
    public void QuestForWesternVisayas()
    {
        this.quests.Add(new Quest("BREAKING NEWS ALERT!", "I can go to Gregorio to know about Graciano Lopez Jaena.",
        "Go to Gregorio Zaide and know what's his request.", 20, WESTERN_VISAYAS, 9,
        new RequestGoal
        (
            new ItemGiver[]
            {
                new ItemGiver("Ramon Villegas", "Hello! Gregorio is asking for this. Thank you for helping me to give this to him.", 
                new List<Item> {
                    new Item("Newspaper", 5, false, "Graciano Lopez Jaena", INFO_PATH + "/Graciano Lopez Jaena", "PCP/Graciano Lopez Jaena"),
                    new Item("None", 0, false, "Teresa Magbanua", INFO_PATH + "/Teresa Magbanua", "PCP/Teresa Magbanua")
                })
            },
            "Gregorio Zaide",
            "Do you mind if you get the newspaper to Ramon Villegas?",
            ""
        )));

        // DONE
        this.quests.Add(new Quest("Black is Beautiful!", "I can talk to BJ Pascual for festivals that has a mask.", "Talk to <b>BJ Pascual</b>", 25, WESTERN_VISAYAS, 9,
        new ShowPhotoAlbumGoal("BJ Pascual",
        new Item[] 
        { 
            new Item("None", 0, false, "Ati-Atihan", "Delivery Ink Informations/Ati-Atihan", "PCP/Ati-Atihan"),
            new Item("None", 0, false, "Dinagyang Festival", "Delivery Ink Informations/Dinagyang Festival", "PCP/Dinagyang Festival")
        })));

        this.quests.Add(new Quest("Don't Hide the Smile", "I can go to museum to look at Mask for info about Masskara Festival.",
         "Find a mask to know the info about Masskara Festival.", 15, WESTERN_VISAYAS, 9, new TalkGoal("Masskara Festival")));

        // NEED DIALOGUE KAY IVAN HENARES (done)
        this.quests.Add(new Quest("Pure White!", "I can talk to Ivan Henares for the info about Boracay.", "Talk to <b>Ivan Henares</b>", 25, WESTERN_VISAYAS, 9,
        new ShowPhotoAlbumGoal("Ivan Henares",
        new Item[]
        {
            new Item("None", 0, false, "Boracay", "Delivery Ink Informations/Boracay", "PCP/Boracay")
        })));

        // NEED DIALOGUE KAY MIKAELA FUDOLIG (DONE)
        this.quests.Add(new Quest("Ancestral House", "I can go to School to know the info about Balay Negrese.",
        "Ask to a person who is good to architecture.", 15, WESTERN_VISAYAS, 9, new TalkGoal("Mikaela Fudolig")));
    }

    public void QuestForCentralVisayas()
    {
        this.quests.Add(new Quest("The First Battle", "I can go to School to know the info about Lapu-Lapu.",
       "Go to Lapu-Lapu statue.", 15, CENTRAL_VISAYAS, 10, new TalkGoal("Lapu-Lapu")));

        // NEED DIALOGUE KAY ENCARNACION ALZONA (DONE)
        this.quests.Add(new Quest("Another Younges Captain?!", "I can go to School to know the info about Francisco Dagohoy.",
       "Talk to someone who loves history specificaly about National Heroes.", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Encarnacion Alzona")));

        this.quests.Add(new Quest("A Cup of Blood", "Five men and two men are sitting.", "I can go to School to know the info about Sandugo.",
"Find a statue about Sandugo", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Sandugo")));

        this.quests.Add(new Quest("Magellan's Cross", "I can go to School to know the info about Magellan's Cross.",
"Find a cross inside the school event.", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Magellan's Cross")));

        // NEED DIALOGUE KAY RIX THE SEMINARIAN (DONE)
        this.quests.Add(new Quest("Mother of all Churches", "I can go to museum to know the info about The Basílica Menor del Santo Niño de Cebú.",
"Talk to someone who is know about churches.", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Rix the Seminarian")));

        this.quests.Add(new Quest("Chocolate Hills", "I can go to School to know the info about Chocolate Hills.",
"Learn the info about Chocolate Hills.", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Chocolate Hills")));

        this.quests.Add(new Quest("Celebration of Gratitude", "I can go to School to know the info about Sinulog Festival.",
"Learn the info about Sinulog Festival at school.", 5, CENTRAL_VISAYAS, 10, new TalkGoal("Sinulog Festival")));
    }

    public void QuestForEasternVisayas()
    {
        this.quests.Add(new Quest("Connect with Others", "I can go to Museum to know the info about San Juanico Bridge.",
        "Learn about San Juanico Bridge", 15, EASTERN_VISAYAS, 11, new TalkGoal("San Juanico Bridge")));

        this.quests.Add(new Quest("I Shall Return", "I can go to Museum to know the info about McArthur and McArthur's Landing.",
        "Learn about McArthur", 15, EASTERN_VISAYAS, 11, new TalkGoal("McArthurs Landing")));
    }

    // MINDANAO REGION
    // Region 9
    public void QuestForZamboangaPeninsula()
    {
        this.quests.Add(new Quest("Boat Racing", "It is a boat.", "I can go to Museum to know the info about Regatta De Zamboanga.",
        "Learn the info about Regatta De Zamboanga.", 15, ZAMBOANGA_PENINSULA, 12, new TalkGoal("Vinta")));

        this.quests.Add(new Quest("Eeew! Is it cockroach?!", "I can go to Museum to know the info about Curacha Crab.",
        "Find something that looks like a crab.", 15, ZAMBOANGA_PENINSULA, 12, new TalkGoal("Curacha Crab")));

        this.quests.Add(new Quest("Got Exiled", "It is one of the photo stand displayed in school.", "I can go to school to know the info about Dapitan Shrine.",
        "Learn the info about Dapitan Shrine.", 15, ZAMBOANGA_PENINSULA, 12, new TalkGoal("Dapitan Shrine")));

        this.quests.Add(new Quest("A Fortress", "It is one of the photo stand displayed in school.", "I can go to school to know the info about Fort Pillar.",
    "Learn the info about Fort Pillar.", 15, ZAMBOANGA_PENINSULA, 12, new TalkGoal("Fort Pillar")));
    }

    // 10
    public void QuestForNorthernMindanao()
    {
            this.quests.Add(new Quest("Christmas is near!", "I can go to school to know the info about Christmas Symbols.",
        "Learn about Christmas Symbols Festival", 15, NORTHERN_MINDANAO, 13, new TalkGoal("Christmas Symbols Festival")));

            this.quests.Add(new Quest("Rafting!", "I can go to school to know the info about White Water Rafting.",
        "Learn about White Water Rafting", 15, NORTHERN_MINDANAO, 13, new TalkGoal("White Water Rafting")));

            this.quests.Add(new Quest("The Vanished!", "A cross that sits to looks like a tomb.", "I can go to school to know the info about Sunken Cemetery.",
        "Find a cross that looks like sits at the ocean.", 15, NORTHERN_MINDANAO, 13, new TalkGoal("Sunken Cemetery")));

            this.quests.Add(new Quest("Like a Stairway to Heaven!", "I can go to school to know the info about Divine Mercy Shrine.",
        "Find a statue that has a <b>two rays</b> and the color of two rays is <b>Blue and Red</b>.", 15, NORTHERN_MINDANAO, 13, new TalkGoal("Divine Mercy Shrine")));

            this.quests.Add(new Quest("Twins!", "I can go to school to know the info about Maria Christina Falls.",
        "Find a photo booth that is about waterfall", 15, NORTHERN_MINDANAO, 13, new TalkGoal("Maria Christina Falls")));
    }

    // 11 (14)
    public void QuestForDavaoRegion()
    {
        // done
        this.quests.Add(new Quest("Highest Peak", "I can talk to Dale Abenjobar about Mount Apo National Park.",
        "Talk to a person who loves to hike.", 15, DAVAO_REGION, 14, new TalkGoal("Dale Abenjobar")));

        // done
        this.quests.Add(new Quest("A Great Eagle", "It is surrounded by transparent glass.", "I can go to museum about Philippine Eagle.",
        "Find an eagle.", 15, DAVAO_REGION, 14, new TalkGoal("Philippine Eagle")));

        this.quests.Add(new Quest("All About Gratitude", "I can know the info about Kadayawan Festival to BJ Pascual.",
        "Talk to a person who loves festivals.", 15, DAVAO_REGION, 14, new TalkGoal("BJ Pascual")));

        this.quests.Add(new Quest("Wild Beasts", "I can know the info about Crocodile's Park from museum.",
        "Find a crocodile replica to know the info about Davao's Crododile Park.", 15, DAVAO_REGION, 14, new TalkGoal("Davao Crocodile Park")));
    }

    // 12 (15)
    public void QuestForSOCCSKSARGEN()
    {
        // DONE
        this.quests.Add(new Quest("Century Tuna", "I can recap the info about Tuna Fesival to Chef Rolando.", "Help Mang Rolando to get his Tuna to a market nearby.", 25, SOCCSKSARGEN, 15,
            new RequestGoal
            (
                new ItemGiver[]
                {
                    new ItemGiver("Store in Market", "Buy a Tuna?", new List<Item> {
                        new Item("Tuna", 1, false, "Tuna Festival", "Delivery Ink Informations/Tuna Festival", "PCP/Tuna Festival")
                    })
                },
                "Rolando Laudico",
                "Hi! Can you I ask you a favor to get me a tuna at the market nearby?",
                "Buy <b>Puto</b> and <b>Dinuguan</b>?"
            )));

        // DONE
        this.quests.Add(new Quest("Lake Sebu", "I can talk to Mae Jardaleza to know about Lake Sebu", 
            "Zardo Domenios has a lotus plant, help him to give it to Mae Jardaleza.",
        25, SOCCSKSARGEN, 15,
        new DeliveryGoal("Zardo Domenios", "Mae Jardaleza", 
        "Hello. I have a lotus here and Mae Jardaleza is requesting to have one. Do you mind to give it to her?",
        THANKYOU_PATH + "Lake Sebu", THANKYOU_PATH + "Lake Sebu 2",
        "Text", new Item[] { new Item("Lotus", 1, false, "", "", "") })));

        // DONE
        this.quests.Add(new Quest("Grand Mosque", "I can know the info about Grand Mosque to Mikaela Fudolig.",
        "Talk to Mikaela Fudolig.", 15, SOCCSKSARGEN, 15, new TalkGoal("Mikaela Fudolig")));

        // DONE
        this.quests.Add(new Quest("Sky Is The Limit", "I can know the info about Bird Sanctuary to Elly.",
        "Talk to a person who loves birds to know the info about a bird sanctuary here in the Philippines.", 15, SOCCSKSARGEN, 15, new TalkGoal("Encarnacion Alzona")));

        this.quests.Add(new Quest("Sultan of Swing", "I can know the info about Sultan Kudarat to Gregorio Zaide.",
       "Talk to a person who is good at National Heroes.", 15, SOCCSKSARGEN, 15, new TalkGoal("Gregorio Zaide")));
    }

    // 13 (16)
    public void QuestForCARAGARegion()
    {
        this.quests.Add(new Quest("A Wildlife Protector", "I can talk to Encarnacion Alzona about Agusan Marsh Wildlife Sanctuary.", "Help Ramon Villegas to give a <b>DUCK Figurine</b> to someone who loves birds.",
        25, CARAGA_REGION, 16,
        new DeliveryGoal("Ramon Villegas", "Encarnacion Alzona", "Hey! Encarnacion or Elly loves birds. Do you mind to give this Duck Figurine to her?",
        THANKYOU_PATH + "Agusan Marsh", THANKYOU_PATH + "Agusan Marsh 2",
        "Text", new Item[] { new Item("Duck Figurine", 1, false, "", "", "") })));

        this.quests.Add(new Quest("Enchanted to Meet You", "I can talk to Zardo Domenios for Enchanted River and Tinuy-an Falls.", "Talk to someone who loves oceans or water sceneries.", 25, CARAGA_REGION, 16,
        new ShowPhotoAlbumGoal("Zardo Domenios",
        new Item[] { 
            new Item("None", 0, false, "Enchanted River", INFO_PATH + "/Enchanted River", "PCP/Enchanted River"),
            new Item("None", 0, false, "Tinuy-an Falls", INFO_PATH + "/Tinuy-an Falls", "PCP/Tinuy-an Falls")})));
    }

    public void QuestForBARMM()
    {
        this.quests.Add(new Quest("Unknown Depth", "I can know the info about Margues Blue Lagoon from Zardo Domenios.",
            "Talk to someone who loves to surf.", 15, BARMM, 17, new TalkGoal("Zardo Domenios")));

        this.quests.Add(new Quest("Holy Pink", "I can know the info about Pink Mosque to Mikaela Fudolig.",
            "Talk to someone who is good at architecture.", 15, BARMM, 17, new TalkGoal("Mikaela Fudolig")));
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
                "Ilocos is a region in the Philippines, encompassing the northwestern coast of Luzon island. It�s " +
                "known for its historic sites, beaches and the well-preserved Spanish colonial city of Vigan. " +
                "Dating from the 16th century, Vigan�s Mestizo district is characterized by cobblestone streets and " +
                "mansions with wrought-iron balconies. Farther north, Laoag City is a jumping-off point for the huge La Paz Sand Dunes.",
                new Category[3] { new Category(HEROES), new Category(FESTIVALS), new Category(TOURIST_ATTRACTIONS) }));

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
                "Central Luzon, designated as Region III, is an administrative region in the Philippines, primarily " +
                "serving to organize the 7 provinces of the vast central plains of the island of Luzon, for administrative convenience.",
                new Category[2] { new Category(FESTIVALS), new Category(GENERAL_KNOWLEDGE) }));

        this.regionsData.Add(new RegionData(
                5,
                false,
                NCR,
                "Metropolitan Manila, officially the National Capital Region, is the seat of " +
                "government and one of three defined metropolitan areas in the Philippines.",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                6,
                false,
                CALABARZON,
                "Calabarzon, formally known as the Southern Tagalog Mainland, is an administrative region in the Philippines, designated as Region IV-A. " +
                "The region comprises five provinces: Batangas, Cavite, Laguna, Quezon, and Rizal; and one highly urbanized city, Lucena.",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                7,
                false,
                MIMAROPA,
                "Mimaropa, formerly known as the Southwestern Tagalog Region, is an administrative region in the Philippines. " +
                "It was also formerly designated as Region IV-B until 2016. " +
                "It is one of two regions in the country having no land border with another region.",
                new Category[2] { new Category(FESTIVALS), new Category(TOURIST_ATTRACTIONS) }));

        this.regionsData.Add(new RegionData(
                8,
                false,
                BICOL_REGION,
                "Bicol is a region in the Philippines encompassing the southern part of Luzon Island and nearby island provinces. Caramoan, a peninsula in the east, is dotted with caves, limestone cliffs and white-sand beaches. " +
                "Nearby, Catanduanes Island has mountains, waterfalls and coral reefs. Donsol, in the west, is home to whale sharks. The region’s active volcanoes include Bulusan Volcano and Mayon Volcano.",
                new Category[1] { new Category(HEROES) }));


    }

    void AddRegionsForVisayas()
    {
        // It should be 9, 10, 11, for testing purposes only.
        this.regionsData.Add(new RegionData(
            9, 
            false, 
            WESTERN_VISAYAS, 
            "Western Visayas is an administrative region in the Philippines, numerically designated as Region VI. It consists of six provinces and two highly urbanized cities. The regional center is Iloilo City. " +
            "The region is dominated by the native speakers of four Visayan languages: Hiligaynon, Kinaray-a, Aklanon and Capiznon.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            10, 
            false, 
            CENTRAL_VISAYAS, 
            "Central Visayas is an administrative region in the Philippines, numerically designated as Region VII. " +
            "It consists of four provinces: and three highly urbanized cities: Cebu City, Lapu-Lapu, and Mandaue.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            11, 
            false, 
            EASTERN_VISAYAS,
            "Eastern Visayas is an administrative region in the Philippines, designated as Region VIII. " +
            "It consists of three main islands, Samar, Leyte and Biliran.", 
            new Category[] { new Category(HEROES) }));
    }

    void AddRegionsForMindanao()
    {
        this.regionsData.Add(new RegionData(
            12, 
            false, 
            ZAMBOANGA_PENINSULA,
            "Zamboanga Peninsula is an administrative region in the Philippines, designated as Region IX. " +
            "It consists of three provinces including four cities and the highly urbanized Zamboanga City." +
            " The region was previously known as Western Mindanao before the signing of Executive Order No. 36 of 2001.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            13, 
            false, 
            NORTHERN_MINDANAO,
            "Northern Mindanao is an administrative region in the Philippines, designated as Region X. " +
            "It comprises five provinces: Bukidnon, Camiguin, Misamis Occidental, Misamis Oriental, and Lanao del Norte, and two cities classified as highly urbanized, " +
            "all occupying the north-central part of Mindanao island, and the island-province of Camiguin.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            14, 
            false, 
            DAVAO_REGION,
            "Davao Region, formerly called Southern Mindanao, is an administrative region in the Philippines, designated as Region XI. " +
            "It is situated at the southeastern portion of Mindanao and comprises five provinces: Davao de Oro, Davao del Norte, Davao del Sur," +
            " Davao Oriental and Davao Occidental.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            15,
            false, 
            SOCCSKSARGEN, 
            "Soccsksargen, formerly known as Central Mindanao, is an administrative region of the Philippines, designated as Region XII. " +
            "Located in south-central Mindanao, its name is an acronym that stands for the region's four provinces and one highly urbanized city.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            16, 
            false, 
            CARAGA_REGION,
            "Caraga, officially the Caraga Administrative Region and designated as Region XIII, is an administrative region in the Philippines occupying the northeastern section of Mindanao. " +
            "The region was created through Republic Act No. 7901 on February 23, 1995.", 
            new Category[] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
            17, 
            false, 
            BARMM,
            "Bangsamoro, officially the Bangsamoro Autonomous Region in Muslim Mindanao, is an autonomous region located in the southern Philippines.", 
            new Category[] { new Category(HEROES) }));
    }

    void AddNPCsInfo()
    {
        this.npcInfos.Add(new NPC_INFO("Gregorio Zaide", ""));
        this.npcInfos.Add(new NPC_INFO("Mikaela Fudolig", ""));
        this.npcInfos.Add(new NPC_INFO("Ivan Henares", ""));
        this.npcInfos.Add(new NPC_INFO("Ramon Villegas", ""));
        this.npcInfos.Add(new NPC_INFO("Mae Jardaleza", ""));
        this.npcInfos.Add(new NPC_INFO("Zardo Domenios", ""));
        this.npcInfos.Add(new NPC_INFO("Rolando Laudico", ""));
        this.npcInfos.Add(new NPC_INFO("Jeremiah Villaroman", ""));
        this.npcInfos.Add(new NPC_INFO("BJ Pascual", ""));
        this.npcInfos.Add(new NPC_INFO("Encarnacion Alzona", ""));
        this.npcInfos.Add(new NPC_INFO("Dale Abenjobar", ""));
        this.npcInfos.Add(new NPC_INFO("Rix the Seminarian", ""));
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
        const string HEROES = "National Heroes";
        const string FESTIVALS = "National Festivals";
        const string TOURIST_ATTRACTIONS = "Tourist Attractions";
        const string GENERAL_KNOWLEDGE = "General Knowledge";

        return this.NumberOfCollectedCollectiblesFor(HEROES) + this.NumberOfCollectedCollectiblesFor(FESTIVALS)
            + this.NumberOfCollectedCollectiblesFor(TOURIST_ATTRACTIONS) + this.NumberOfCollectedCollectiblesFor(GENERAL_KNOWLEDGE);
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

    public void CopyTo(PlayerData toCopy, PlayerData newPlayerData)
    {
        newPlayerData.isGameCompleted = toCopy.isGameCompleted;
        newPlayerData.isNewlyCreated = toCopy.isNewlyCreated;
        newPlayerData.isTutorialDone = toCopy.isTutorialDone;
        newPlayerData.isIntroductionDone = toCopy.isIntroductionDone;
        newPlayerData.isPreQuestIntroductionDone = toCopy.isPreQuestIntroductionDone;
        newPlayerData.isNPCIntroductionPanelDone = toCopy.isNPCIntroductionPanelDone;
        newPlayerData.isSleepPanelNotShown = toCopy.isSleepPanelNotShown;
        newPlayerData.isFirstTimeGoingToSchool = toCopy.isFirstTimeGoingToSchool;
        newPlayerData.isQuestReset = toCopy.isQuestReset;
        newPlayerData.hasNewOpenRegion = toCopy.hasNewOpenRegion;

        newPlayerData.questNewIcon = toCopy.questNewIcon;
        newPlayerData.achievementsNewIcon = toCopy.achievementsNewIcon;
        newPlayerData.notesNewIcon = toCopy.notesNewIcon;

        newPlayerData.id = toCopy.id;
        newPlayerData.name = toCopy.name;
        newPlayerData.gender = toCopy.gender;
        newPlayerData.dunongPoints = toCopy.dunongPoints;
        newPlayerData.requiredDunongPointsToPlay = toCopy.requiredDunongPointsToPlay;
        newPlayerData.remainingTime = toCopy.remainingTime;
        newPlayerData.sceneToLoad = toCopy.sceneToLoad;
        newPlayerData.xPos = toCopy.xPos;
        newPlayerData.yPos = toCopy.yPos;

        newPlayerData.regionsData = new List<RegionData>();
        foreach (RegionData rd in toCopy.regionsData)
        {
            newPlayerData.regionsData.Add(rd);
        }

        newPlayerData.notebook = toCopy.notebook;
        newPlayerData.quests = toCopy.quests;
        newPlayerData.currentQuests = toCopy.currentQuests;
        newPlayerData.completedQuests = toCopy.completedQuests;
        newPlayerData.npcInfos = toCopy.npcInfos;
        newPlayerData.notesInfos = toCopy.notesInfos;
        newPlayerData.inventory = toCopy.inventory;
        newPlayerData.playerTime = toCopy.playerTime;
       
    }

    public PlayerData Restart()
    {
        PlayerData playerData = new PlayerData();

        playerData.id = this.id;
        playerData.isNewlyCreated = false;
        playerData.isTutorialDone = true;
        playerData.isIntroductionDone = true;
        playerData.isPreQuestIntroductionDone = true;
        playerData.isNPCIntroductionPanelDone = true;
        playerData.isFromSleeping = false;
        playerData.isSleepPanelNotShown = true;
        playerData.isFirstTimeGoingToSchool = false;

        playerData.name = this.name;
        playerData.gender = this.gender;
        playerData.dunongPoints = 0;
        playerData.requiredDunongPointsToPlay = 0;
        playerData.sceneToLoad = "Outside";
        playerData.xPos = -0.039f;
        playerData.yPos = -0.85f;

        playerData.regionsData = new List<RegionData>();
        playerData.notebook = new Notebook();
        playerData.quests = new List<Quest>();
        playerData.currentQuests = new List<Quest>();
        playerData.completedQuests = new List<Quest>();
        playerData.npcInfos = new List<NPC_INFO>();
        playerData.notesInfos = new List<Quest>();
        playerData.inventory = new Inventory();
        playerData.playerTime = new PlayerTime();

        playerData.AddRegionsForLuzon();
        playerData.AddRegionsForVisayas();
        playerData.AddRegionsForMindanao();

        playerData.QuestForIlocosRegion();
        playerData.QuestForCagayanValley();
        playerData.QuestForCAR();
        playerData.QuestForCentralLuzon();
        playerData.QuestForNCR();
        playerData.QuestForCALABARZON();
        playerData.AddQuestToMIMAROPA();
        playerData.QuestForBicolRegion();
        playerData.QuestForWesternVisayas();
        playerData.QuestForCentralVisayas();
        playerData.QuestForEasternVisayas();
        playerData.QuestForZamboangaPeninsula();
        playerData.QuestForNorthernMindanao();
        playerData.QuestForDavaoRegion();
        playerData.QuestForSOCCSKSARGEN();
        playerData.QuestForCARAGARegion();
        playerData.QuestForBARMM();

        playerData.AddNPCsInfo();
        playerData.InitializeRequiredDunongPointsToPlay(1);

        foreach(NPC_INFO info in playerData.npcInfos)
        {
            info.isMet = true;
        }

        playerData.playerTime.m_GameTimeHour = this.playerTime.m_GameTimeHour;
        playerData.playerTime.m_GameTimeMinute = this.playerTime.m_GameTimeMinute;
        playerData.playerTime.m_GameTimeSeconds = this.playerTime.m_GameTimeSeconds;
        return playerData;
    }

    public PlayerData RestartToPrevious()
    {
        PlayerData playerData = new PlayerData();

        playerData.id = this.id;
        playerData.isNewlyCreated = false;
        playerData.isTutorialDone = true;
        playerData.isIntroductionDone = true;
        playerData.isPreQuestIntroductionDone = true;
        playerData.isNPCIntroductionPanelDone = true;
        playerData.isFromSleeping = false;
        playerData.isSleepPanelNotShown = true;
        playerData.isFirstTimeGoingToSchool = false;
        playerData.name = this.name;
        playerData.gender = this.gender;
        playerData.dunongPoints = 0;
        playerData.requiredDunongPointsToPlay = 0;
        playerData.sceneToLoad = "Outside";
        playerData.xPos = 0f;
        playerData.yPos = 0f;

        playerData.regionsData = new List<RegionData>();

        playerData.notebook = this.notebook;
        playerData.quests = new List<Quest>();
        playerData.currentQuests = new List<Quest>();
        playerData.completedQuests = new List<Quest>();
        playerData.npcInfos = new List<NPC_INFO>();
        playerData.notesInfos = new List<Quest>();
        playerData.inventory = new Inventory();
        playerData.playerTime = new PlayerTime();

        int currentOpenRegion = CurrentOpenRegion();
        string currentRegionName = CurrentOpenRegionName();

        foreach(RegionData rd in this.regionsData)
        {
            playerData.regionsData.Add(rd.Copy());
        }

        ResetAllQuests(this.currentQuests, currentRegionName);
        ResetAllQuests(this.completedQuests, currentRegionName);


        foreach (Quest quest in this.quests)
        {
            Quest questFound = playerData.quests.Find(questToFind => questToFind.questID == quest.questID);

            quest.isCompleted = false;

            if (questFound == null)
                playerData.quests.Add(quest);
        }

        foreach (Quest quest in this.currentQuests)
        {
            Quest questFound = playerData.quests.Find(questToFind => questToFind.questID == quest.questID);

            quest.isCompleted = false;

            if (questFound == null && quest.regionNum == currentOpenRegion)
                playerData.quests.Add(quest);
        }

        foreach (Quest quest in this.completedQuests)
        {
            Quest questFound = playerData.quests.Find(questToFind => questToFind.questID == quest.questID);

            quest.isCompleted = false;

            if (questFound == null && quest.regionNum == currentOpenRegion)
                playerData.quests.Add(quest);
        }

        playerData.AddNPCsInfo();
        playerData.InitializeRequiredDunongPointsToPlay(currentOpenRegion);

        playerData.playerTime.m_GameTimeHour = this.playerTime.m_GameTimeHour;
        playerData.playerTime.m_GameTimeMinute = this.playerTime.m_GameTimeMinute;
        playerData.playerTime.m_GameTimeSeconds = this.playerTime.m_GameTimeSeconds;
        playerData.playerTime.m_DayEvent = 4;
        foreach (NPC_INFO info in playerData.npcInfos)
        {
            info.isMet = true;
        }

        return playerData;
    }

    /**<summary>
     *  Finds what is the current open region of this PlayerData instance.
     * </summary> */
    public int CurrentOpenRegion()
    {
        int openRegion = 1;

        foreach(RegionData rd in this.regionsData)
        {
            if (rd.isOpen)
            {
                openRegion = rd.regionNumber;
            }
        }

        return openRegion;
    }

    public string CurrentOpenRegionName()
    {
        string regionName = "Ilocos Region";

        foreach (RegionData rd in this.regionsData)
        {
            if (rd.isOpen)
            {
                regionName = rd.regionName;
            }
        }

        return regionName;
    }

    public void ResetAllQuests(List<Quest> list, string regionName)
    {
        foreach (Quest quest in list)
        {
            if (quest.region.ToUpper() == regionName.ToUpper())
            {
                quest.isCompleted = false;

                // IF THE Quest Type is Delivery
                if (quest.questType == Quest.QUEST_TYPE.DELIVERY)
                {
                    quest.deliveryGoal.isFinished = false;
                    quest.deliveryGoal.itemReceivedFromGiver = false;
                }
                // IF the Quest
                else if (quest.questType == Quest.QUEST_TYPE.REQUEST)
                {
                    quest.requestGoal.isRequestFromNPCGained = false;
                    quest.requestGoal.isItemReceivedOfNpc = false;

                    // Also reset the itemGiver object.
                    foreach (ItemGiver itemGiver in quest.requestGoal.itemGivers)
                    {
                        itemGiver.isItemsGiven = false;
                    }
                }
                else if (quest.questType == Quest.QUEST_TYPE.TALK)
                {
                }
                else if (quest.questType == Quest.QUEST_TYPE.SHOW_PHOTO_ALBUM)
                {
                }
            }
        }
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
    const string GENKNOW_FILEPATH = "Collectibles/General Knowledge/";

    public Notebook()
    {
        this.collectibles = new List<Collectible>();

        // =============================================== LUZON COLLECTIBLES =======================================================
        // Collectibles for REGION 1
        this.collectibles.Add(new Collectible("Diego Silang", HEROES_FILEPATH + "Diego Silang", HEROES, LUZON, REGION_1));
        this.collectibles.Add(new Collectible("Bangus Festival", FESTIVALS_FILEPATH + "Bangus Festival", FESTIVALS, LUZON, REGION_1));
        this.collectibles.Add(new Collectible("Cape Bojeador", TOURIST_ATT_FILEPATH + "Cape Bojeador", TOURIST_ATTRACTIONS, LUZON, REGION_1));

        // Collectibles for REGION 2
        this.collectibles.Add(new Collectible("Ivatan Houses", TOURIST_ATT_FILEPATH + "Ivatan Houses", TOURIST_ATTRACTIONS, LUZON, REGION_2));

        // Cordillera Administrative Region (CAR)
        this.collectibles.Add(new Collectible("Panagbenga Festival", FESTIVALS_FILEPATH + "Panagbenga Festival", FESTIVALS, LUZON, CAR));
        this.collectibles.Add(new Collectible("Diplomat Hotel", TOURIST_ATT_FILEPATH + "Diplomat Hotel", TOURIST_ATTRACTIONS, LUZON, CAR));

        // Collectibles for Central Luzon (REGION 3)
        this.collectibles.Add(new Collectible("Marcelo H. Del Pilar", HEROES_FILEPATH + "Marcelo H. Del Pilar", HEROES, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Gregorio Del Pilar", HEROES_FILEPATH + "Gregorio Del Pilar", HEROES, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Hot Air Balloon Festival", FESTIVALS_FILEPATH + "Hot Air Balloon Festival", FESTIVALS, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Kneeling Carabao Festival", FESTIVALS_FILEPATH + "Kneeling Carabao Festival", FESTIVALS, LUZON, REGION_3));
        this.collectibles.Add(new Collectible("Shrine of Valor", TOURIST_ATT_FILEPATH + "Shrine of Valor", TOURIST_ATTRACTIONS, LUZON, REGION_3));

        // Collectibles for CALABARZON (REGION 4A)
        this.collectibles.Add(new Collectible("Batingaw Festival", FESTIVALS_FILEPATH + "Batingaw Festival", FESTIVALS, LUZON, REGION_4A));
        this.collectibles.Add(new Collectible("Jose Rizal", HEROES_FILEPATH + "Jose Rizal", HEROES, LUZON, REGION_4A));
        this.collectibles.Add(new Collectible("Enchanted Kingdom", TOURIST_ATT_FILEPATH + "Enchanted Kingdom", TOURIST_ATTRACTIONS, LUZON, REGION_4A));

        // Collectibles for MIMAROPA (REGION 4B) 
        this.collectibles.Add(new Collectible("Moriones Festival", FESTIVALS_FILEPATH + "Moriones Festival", FESTIVALS, LUZON, MIMAROPA));

        // Bicol Region or Region 5
        this.collectibles.Add(new Collectible("Patintero", GENKNOW_FILEPATH + "Patintero", GENERAL_KNOWLEDGE, LUZON, REGION_5));

        // National Capital Region (NCR)
        this.collectibles.Add(new Collectible("Antonio Luna", HEROES_FILEPATH + "Antonio Luna", HEROES, LUZON, NCR));
        this.collectibles.Add(new Collectible("Emilio Jacinto", HEROES_FILEPATH + "Emilio Jacinto", HEROES, LUZON, NCR));
        this.collectibles.Add(new Collectible("Gregoria De Jesus", HEROES_FILEPATH + "Gregoria De Jesus", HEROES, LUZON, NCR));
        this.collectibles.Add(new Collectible("Chinese New Year", FESTIVALS_FILEPATH + "Chinese New Year", FESTIVALS, LUZON, NCR));
        this.collectibles.Add(new Collectible("Quezon Memorial Circle", TOURIST_ATT_FILEPATH + "Quezon Memorial Circle", TOURIST_ATTRACTIONS, LUZON, NCR));
        // ========================================= END OF LUZON ==================================================================

        // ================================================= VISAYAS ================================================================

        // WESTERN VISAYAS (REGION 6)
        this.collectibles.Add(new Collectible("Graciano Lopez Jaena", HEROES_FILEPATH + "Graciano Lopez Jaena", HEROES, VISAYAS, WESTERN_VISAYAS));
        this.collectibles.Add(new Collectible("Maskara Festival", FESTIVALS_FILEPATH + "Maskara Festival", FESTIVALS, VISAYAS, WESTERN_VISAYAS));
        this.collectibles.Add(new Collectible("Ledesma Ruins", TOURIST_ATT_FILEPATH + "Ledesma Ruins", TOURIST_ATTRACTIONS, VISAYAS, WESTERN_VISAYAS));

        // CENTRAL VISAYAS (REGION 7)
        this.collectibles.Add(new Collectible("Lapu-Lapu", HEROES_FILEPATH + "Lapu-Lapu", HEROES, VISAYAS, CENTRAL_VISAYAS));
        this.collectibles.Add(new Collectible("Magellan's Cross", TOURIST_ATT_FILEPATH + "Magellan Cross", TOURIST_ATTRACTIONS, VISAYAS, CENTRAL_VISAYAS));

        // EASTERN VISAYAS (REGION 8)
        this.collectibles.Add(new Collectible("Anahaw", GENKNOW_FILEPATH + "Anahaw", GENERAL_KNOWLEDGE, VISAYAS, EASTERN_VISAYAS));
        this.collectibles.Add(new Collectible("Duwende", GENKNOW_FILEPATH + "Duwende", GENERAL_KNOWLEDGE, VISAYAS, EASTERN_VISAYAS));
        // ========================================= END OF VISAYAS ==================================================================

        // ZAMBOANGA PENINSULA (Region 9)
        this.collectibles.Add(new Collectible("Kalabaw", GENKNOW_FILEPATH + "Kalabaw", GENERAL_KNOWLEDGE, MINDANAO, ZAMBOANGA_PENINSULA));
        this.collectibles.Add(new Collectible("Regatta de Zamboanga", FESTIVALS_FILEPATH + "Regatta de Zamboanga", FESTIVALS, MINDANAO, ZAMBOANGA_PENINSULA));
        this.collectibles.Add(new Collectible("Christmas Symbol", FESTIVALS_FILEPATH + "Christmas Symbol", FESTIVALS, MINDANAO, ZAMBOANGA_PENINSULA));

        // Northern Mindanao (Region 10)
        this.collectibles.Add(new Collectible("Sunken Cemetery", TOURIST_ATT_FILEPATH + "Sunken Cemetery", TOURIST_ATTRACTIONS, MINDANAO, NORTHERN_MINDANAO));

        // Davao Region (Region 11)
        this.collectibles.Add(new Collectible("Kapre", GENKNOW_FILEPATH + "Kapre", GENERAL_KNOWLEDGE, MINDANAO, DAVAO_REGION));

        // SOCCSARGEN (Region 12)
        this.collectibles.Add(new Collectible("Sultan Kudarat", HEROES_FILEPATH + "Sultan Kudarat", HEROES, MINDANAO, SOCCSKSARGEN));
        this.collectibles.Add(new Collectible("Manananggal", GENKNOW_FILEPATH + "Manananggal", GENERAL_KNOWLEDGE, MINDANAO, SOCCSKSARGEN));

        // CARAGA (Region 13)
        this.collectibles.Add(new Collectible("Mango", GENKNOW_FILEPATH + "Mango", GENERAL_KNOWLEDGE, MINDANAO, CARAGA_REGION));
        
        // BARMM (Region 14)
        this.collectibles.Add(new Collectible("Piko", GENKNOW_FILEPATH + "Piko", GENERAL_KNOWLEDGE, MINDANAO, BARMM));
        this.collectibles.Add(new Collectible("Tumbang Preso", GENKNOW_FILEPATH + "Tumbang Preso", GENERAL_KNOWLEDGE, MINDANAO, BARMM));
    }

    public Notebook Copy()
    {
        Notebook copy = new Notebook();

        foreach (Collectible collectible in this.collectibles)
        {
            copy.collectibles.Add(collectible.Copy());
        }

        return copy;
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

    public Collectible Copy()
    {
        Collectible copy = new Collectible(this.name, this.imagePath, this.categoryName, this.majorIsland, this.regionName);
        copy.isCollected = this.isCollected;

        return copy;
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

    public RegionData Copy()
    {
        RegionData copy = new RegionData(this.regionNumber, this.isOpen, this.regionName, this.information, this.categories.ToArray());

        copy.noOfStars = this.noOfStars;
        copy.highestScore = this.highestScore;

        return copy;
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
    public Category Copy()
    {
        Category copy = new Category(this.categoryName);

        copy.highestScore = this.highestScore;
        copy.noOfStars = this.noOfStars;

        return copy;
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

    public Inventory Copy()
    {
        Inventory copy = new Inventory();

        return copy;
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
        SECONDS_PER_MINUTE = 10f;
        this.m_NoOfSecondsPerTwoAndHalfMinutes = SECONDS_PER_HOUR;
        this.m_NoOfSecondsPerMinute = SECONDS_PER_MINUTE;

        // Game Time
        this.m_GameTimeHour = 0f;
        this.m_GameTimeMinute = 0f;
        this.m_GameTimeSeconds = 0f;
    }

    public PlayerTime Copy()
    {
        PlayerTime copy = new PlayerTime();

        copy.m_ActualHourInRealLife = this.m_ActualHourInRealLife;
        copy.m_ActualMinuteInRealLife = this.m_ActualMinuteInRealLife;
        copy.m_IsDaytime = this.m_IsDaytime;
        copy.m_IsAllEstablishmentsOpen = this.m_IsAllEstablishmentsOpen;
        copy.m_DayEvent = this.m_DayEvent;
        copy.m_NoOfSecondsPerTwoAndHalfMinutes = this.m_NoOfSecondsPerTwoAndHalfMinutes;
        copy.m_NoOfSecondsPerMinute = this.m_NoOfSecondsPerMinute;

        copy.m_GameTimeHour = this.m_GameTimeHour;
        copy.m_GameTimeMinute = this.m_GameTimeMinute;
        copy.m_GameTimeSeconds = this.m_GameTimeSeconds;

        return copy;
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

    public NPC_INFO Copy()
    {
        NPC_INFO copy = new NPC_INFO(this.name, this.info, this.isMet);
        copy.xPos = this.xPos;
        copy.yPos = this.yPos;

        return copy;
    }
}

[System.Serializable]
public class MemoryGameData
{
    public bool[] memoryGameProgress;

    public MemoryGameData()
    {
        this.memoryGameProgress = new bool[17];

        this.memoryGameProgress[0] = true;

        for(int i = 1; i < this.memoryGameProgress.Length; i++)
        {
            this.memoryGameProgress[i] = false;
        }
    }
}