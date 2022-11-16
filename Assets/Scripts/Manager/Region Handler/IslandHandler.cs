using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IslandHandler : MonoBehaviour, IDataPersistence
{
    public List<Button> m_RegionButtons;
    public RectTransform m_IslandInformationPanel;
    public RectTransform m_RegionInformationPanel;
    public RectTransform m_RegionInformationPanelContent;
    public PlayerData playerData;

    Dictionary<string, Image> m_Images = new Dictionary<string, Image>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnIslandSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnIslandSceneLoaded;
    }

    void OnIslandSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Luzon" || scene.name == "Visayas" || scene.name == "Mindanao")
        {
            /** FOR TESTING PURPOSES */
            //this.playerData = new PlayerData();

            Button majorIslandInformationBTN = null;
            Button closeRegionInformationPanelBTN = null;
            Button closeIslandInformationPanelBTN = null;

            // Set the scene to be loaded after assessment is Luzon.
            if (scene.name == "Luzon")
            {
                AssessmentManager.instance.previousSceneToLoad = "Luzon";
                WordManager.instance.previousSceneToLoad = "Luzon";
            }
            else if (scene.name == "Visayas")
            {
                AssessmentManager.instance.previousSceneToLoad = "Visayas";
                WordManager.instance.previousSceneToLoad = "Visayas";
            }
            else
            {
                AssessmentManager.instance.previousSceneToLoad = "Mindanao";
                WordManager.instance.previousSceneToLoad = "Mindanao";
            }

            this.m_IslandInformationPanel = GameObject.Find("Island Information Panel").GetComponent<RectTransform>();
            this.m_RegionInformationPanel = GameObject.Find("Region Information Panel").GetComponent<RectTransform>();

            this.m_RegionInformationPanelContent = this.m_RegionInformationPanel.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();

            // COLLECTIBLES VALUE
            GameObject.Find("Heroes Value").GetComponent<TextMeshProUGUI>().text = GetNumberOfCollectiblesCollected(scene.name, "National Heroes").ToString() 
                + "/" + GetTotalOfCollectiblesCollected(scene.name, "National Heroes");
            GameObject.Find("Festivals Value").GetComponent<TextMeshProUGUI>().text = GetNumberOfCollectiblesCollected(scene.name, "National Festivals").ToString()
                + "/" + GetTotalOfCollectiblesCollected(scene.name, "National Festivals");
            GameObject.Find("Tourist Attractions Value").GetComponent<TextMeshProUGUI>().text = GetNumberOfCollectiblesCollected(scene.name, "Tourist Attractions").ToString()
                + "/" + GetTotalOfCollectiblesCollected(scene.name, "Tourist Attractions");
            GameObject.Find("General Knowledge Value").GetComponent<TextMeshProUGUI>().text = GetNumberOfCollectiblesCollected(scene.name, "General Knowledge").ToString()
                + "/" + GetTotalOfCollectiblesCollected(scene.name, "General Knowledge");

            majorIslandInformationBTN = GameObject.Find("Information BTN").GetComponent<Button>();
            closeRegionInformationPanelBTN = this.m_RegionInformationPanel.gameObject.transform.GetChild(0).GetComponent<Button>();
            closeIslandInformationPanelBTN = this.m_IslandInformationPanel.gameObject.transform.GetChild(0).GetComponent<Button>();

            GameObject.Find("DP Value").GetComponent<TextMeshProUGUI>().text = this.playerData.dunongPoints.ToString(); // Get the text for setting up the DP Value.

            majorIslandInformationBTN.onClick.AddListener(() =>
            {

                LeanTween.scale(this.m_IslandInformationPanel.gameObject, Vector2.one, .2f)
                .setEaseSpring();
            });

            closeRegionInformationPanelBTN.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Click Close");
                LeanTween.scale(this.m_RegionInformationPanel.gameObject, Vector2.zero, .2f)
                .setEaseSpring();
            });

            closeIslandInformationPanelBTN.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                LeanTween.scale(this.m_IslandInformationPanel.gameObject, Vector2.zero, .2f)
                .setEaseSpring();
            });

            this.m_IslandInformationPanel.localScale = Vector2.zero;
            this.m_RegionInformationPanel.localScale = Vector2.zero;

            this.GetAllRegionImages(scene.name);
            this.GetAllRegionButtons(scene.name);

            if (QuestManager.instance != null)
                QuestManager.instance.GetListOfQuests();
        }
    }

    int GetNumberOfCollectiblesCollected(string sceneName, string categoryName)
    {
        Notebook nb = DataPersistenceManager.instance.playerData.notebook;
        int count = 0; 

        foreach (Collectible collectible in nb.collectibles)
        {
            if (collectible.majorIsland.ToUpper() == sceneName.ToUpper() && collectible.categoryName == categoryName && collectible.isCollected)
            {
                count++;
            }
        }

        return count;
    }

    int GetTotalOfCollectiblesCollected(string sceneName, string categoryName)
    {

        Notebook nb = DataPersistenceManager.instance.playerData.notebook;
        int count = 0;

        foreach (Collectible collectible in nb.collectibles)
        {
            if (collectible.majorIsland.ToUpper() == sceneName.ToUpper() && collectible.categoryName == categoryName)
            {
                count++;
            }
        }

        return count;
    }

    public void GetAllRegionImages(string islandName)
    {
        Transform transform = GameObject.Find(islandName).transform.GetChild(0);

        foreach (Transform image in transform)
        {
            if (!m_Images.ContainsKey(image.name))
                m_Images.Add(image.name, image.GetComponent<Image>());
        }
    }

    public void GetAllRegionButtons(string islandName)
    {
        Transform locations = GameObject.Find(islandName).transform.GetChild(1);

        foreach (Transform button in locations)
        {
            RegionData foundRegionData = this.FindRegionDataWithName(button.name);

            

            if (foundRegionData != null)
            {
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SoundManager.instance?.PlaySound("Button Click 1");
                    LeanTween.scale(this.m_RegionInformationPanel.gameObject, Vector2.one, .2f)
                    .setEaseSpring();

                    // Set the regionName and regionNum properties for AssessmentManager and WordManager
                    AssessmentManager.instance.regionName = button.name;
                    WordManager.instance.regionName = button.name;
                    AssessmentManager.instance.regionNum = foundRegionData.regionNumber;
                    WordManager.instance.regionNum = foundRegionData.regionNumber;

                    AssessmentSetup typeOfAssessment = button.GetComponent<AssessmentSetup>();
                    WordSetup typeOfWord = button.GetComponent<WordSetup>();

                    Transform regionPanelInfoTransform = this.m_RegionInformationPanel.transform;

                    regionPanelInfoTransform.GetChild(1).GetComponent<Image>().sprite = this.m_Images[button.name].sprite;
                    regionPanelInfoTransform.GetChild(1).GetComponent<Image>().color = this.m_Images[button.name].color;

                    m_RegionInformationPanelContent.GetChild(0).GetComponent<TextMeshProUGUI>().text = foundRegionData.regionName;
                    m_RegionInformationPanelContent.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = foundRegionData.information;

                    // Play Button
                    regionPanelInfoTransform.GetChild(regionPanelInfoTransform.childCount - 1).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (!this.IsAllQuestCompleted() || this.playerData.dunongPoints < this.playerData.requiredDunongPointsToPlay)
                        {
                            SoundManager.instance?.PlaySound("Click Error");
                        }
                        else
                        {
                            SoundManager.instance?.PlaySound("Button Click 1");
                        }

                        // Check if all the quest is completed AND Check if the dunong points is valid.
                        if (this.IsAllQuestCompleted() && this.playerData.dunongPoints == this.playerData.requiredDunongPointsToPlay)
                        {
                            //this.playerData.dunongPoints -= this.playerData.requiredDunongPointsToPlay;

                            if (typeOfAssessment != null)
                            {
                                AssessmentManager.instance.StartAssessments(typeOfAssessment.assessments);
                                TransitionLoader.instance?.StartAnimation(typeOfAssessment.sceneToLoad);
                                //SceneManager.LoadScene(typeOfAssessment.sceneToLoad);
                            }
                            else if (typeOfWord != null)
                            {
                                WordManager.instance.StartWordGames(typeOfWord.words);
                                TransitionLoader.instance?.StartAnimation(typeOfWord.sceneToLoad);
                                //SceneManager.LoadScene(typeOfWord.sceneToLoad);
                            }
                            return;
                        }
                    });

                });
            }

            // Change the color of the region image if the found RegionData is open.
            Color greenColor;
            ColorUtility.TryParseHtmlString("#B0FF96", out greenColor);

            if (foundRegionData != null && foundRegionData.isOpen && foundRegionData.noOfStars == 3)
            {
                button.gameObject.SetActive(false);
                this.m_Images[button.name].color = this.m_Images[button.name].color;
            }
            else if (foundRegionData != null && foundRegionData.isOpen)
            {
                button.gameObject.SetActive(true);
                this.m_Images[button.name].color = this.m_Images[button.name].color;
            }
            else
            {
                this.m_Images[button.name].color = Color.black;
            }
        }
    }

    // Find Region by name.
    public RegionData FindRegionDataWithName(string regionName)
    {
        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == regionName.ToUpper())
            {
                return regionData;
            }
        }

        return null;
    }

    private bool IsAllQuestCompleted()
    {
        return this.playerData.currentQuests.Count == 0;
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }

    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }
}
