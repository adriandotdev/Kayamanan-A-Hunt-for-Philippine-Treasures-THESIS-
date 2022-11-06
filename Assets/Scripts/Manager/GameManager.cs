using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    //[Header("Save Panel")]
    //[SerializeField] private RectTransform saveSlotsPanel;

    [Header("Settings UI")]
    [SerializeField] private RectTransform optionsPanel;

    [Header("Profile Confirmation Panel")]
    [SerializeField] private RectTransform profileConfirmationPanel;

    [Header("Volume UI")]
    [SerializeField] public Button soundButton;
    [SerializeField] public Button quitButton;
    [SerializeField] public RectTransform volumePanel;

    [SerializeField] public Button closeNotesPanel;
    [SerializeField] public RectTransform notesPanel;
    [SerializeField] public Button notesButton;

    [Header("Canvas Groups")]
    public CanvasGroup menuSceneCanvasGroup;
    public CanvasGroup homeCanvasGroup;
    public CanvasGroup characterCreationGroup;

    public TMPro.TextMeshProUGUI dunongPointsValue;

    [Header("Player Data")]
    public PlayerData playerData;

    public string sceneToLoadFromPhilippineMap;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnMenuSceneLoaded;
        SceneManager.sceneLoaded += OnCharacterAndLoadSceneLoaded;
        SceneManager.sceneLoaded += OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded += OnAssessmentAndWordGamesSceneLoaded;
        SceneManager.sceneLoaded += OnMajorIslandsSceneLoaded;
        SceneManager.sceneLoaded += OnSleepCutsceneLoaded;
        SceneManager.sceneLoaded += OnSceneRequiresDataLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMenuSceneLoaded;
        SceneManager.sceneLoaded -= OnCharacterAndLoadSceneLoaded;
        SceneManager.sceneLoaded -= OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded -= OnAssessmentAndWordGamesSceneLoaded;
        SceneManager.sceneLoaded -= OnMajorIslandsSceneLoaded;
        SceneManager.sceneLoaded -= OnSleepCutsceneLoaded;
        SceneManager.sceneLoaded -= OnSceneRequiresDataLoaded;
    }

    // For Menu Scene
    public void OnMenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
                // GET ALL THE NECESSARY COMPONENTS.
                this.menuSceneCanvasGroup = GameObject.Find("Menu Canvas Group").GetComponent<CanvasGroup>();
                this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
                this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
                this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
                this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();

                Button optionsPanelCloseBtn = GameObject.Find("Options Panel Close Button").GetComponent<Button>();
                Button playButton = GameObject.Find("Play Button").GetComponent<Button>();
                Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
                Button leaderboardsButton = GameObject.Find("Leaderboards Button").GetComponent<Button>();

                this.soundButton.onClick.AddListener(() =>
                {
                    this.ShowVolumeUI();
                    SoundManager.instance.PlaySound("Button Click 2");
                });

                this.quitButton.onClick.AddListener(() => { 
                    
                    this.Quit();
                    SoundManager.instance.PlaySound("Button Click 2");
                });

                optionsPanelCloseBtn.onClick.AddListener(() =>
                {
                    this.CloseOptionPanel();
                    SoundManager.instance.PlaySound("Button Click 1");
                });

                playButton.onClick.AddListener(() => {

                    this.LoadScene("CharacterAndLoad");
                    SoundManager.instance.PlaySound("Button Click 1");
                    //TransitionLoader.instance.StartAnimation("CharacterAndLoad");
                });

                leaderboardsButton.onClick.AddListener(() =>
                {
                    this.LoadScene("Leaderboards");
                    SoundManager.instance.PlaySound("Button Click 1");
                });

                optionsButton.onClick.AddListener(() => {
                    this.ShowOptionsPanel();
                    SoundManager.instance.PlaySound("Button Click 1");
                });
                // Hide the optionsPanel at first render
                this.optionsPanel.gameObject.SetActive(false);
                this.volumePanel.gameObject.SetActive(false);
        
        }
    }

    // For CharacterAndLoad Scene
    public void OnCharacterAndLoadSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CharacterAndLoad"))
        {
            // Get the 'Back' button and Add Event to it.
            Button backButton = GameObject.Find("Back").GetComponent<Button>();
            backButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Menu");
                SoundManager.instance.PlaySound("Button Click 2");
            });
            //backButton.onClick.AddListener(() => TransitionLoader.instance.StartAnimation("Menu"));
        }
    }

    public void OnSceneRequiresDataLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Museum")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Church"))
        {
            if (scene.name == "House")
            {
                this.sceneToLoadFromPhilippineMap = "House";
                this.playerData.sceneToLoad = "House";
            }
            else if (scene.name == "Outside")
            {
                this.sceneToLoadFromPhilippineMap = "Outside";
                this.playerData.sceneToLoad = "Outside";
            }
            else if (scene.name == "Museum")
            {
                this.sceneToLoadFromPhilippineMap = "Museum";
                this.playerData.sceneToLoad = "Museum";
            }
            else if (scene.name == "School")
            {
                this.sceneToLoadFromPhilippineMap = "School";
                this.playerData.sceneToLoad = "School";
            }
            else if (scene.name == "Church")
            {
                this.sceneToLoadFromPhilippineMap = "Church";
                this.playerData.sceneToLoad = "Church";
            }

            this.SetUpHouseOrOutsideSceneButtons();
        }
    }

    // For Philippine Map Scene
    public void OnPhilippineMapSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Philippine Map"))
        {
            // Get the Go To House button and add an event to it.
            Button goToHouseButton = GameObject.Find("Go to House").GetComponent<Button>();

            // Three Major Islands Button
            Button luzonBTN = GameObject.Find("Luzon").GetComponent<Button>();
            Button visayasBTN = GameObject.Find("Visayas").GetComponent<Button>();
            Button mindanaoBTN = GameObject.Find("Mindanao").GetComponent<Button>();

            goToHouseButton.onClick.AddListener(() => {

                SoundManager.instance?.PlaySound("Button Click 1");
                this.LoadScene(this.sceneToLoadFromPhilippineMap);
            });

            luzonBTN.onClick.AddListener(() => SceneManager.LoadScene("Luzon"));
            visayasBTN.onClick.AddListener(() => SceneManager.LoadScene("Visayas"));
            mindanaoBTN.onClick.AddListener(() => SceneManager.LoadScene("Mindanao"));

            // Set the value of dunong points of a current player.
            GameObject.Find("DP Value").GetComponent<TMPro.TextMeshProUGUI>().text = this.playerData.dunongPoints.ToString();
        }
    }

    public void OnMajorIslandsSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Luzon")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Visayas")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Mindanao"))
        {
            Button backButton = GameObject.Find("Back").GetComponent<Button>();

            backButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySound("Button Click 1");
                /*SceneManager.LoadScene("Philippine Map");*/ // FOR TESTING ONLY. It should be sceneToLoad property.
                SceneManager.LoadScene(this.playerData.sceneToLoad);
            });
        }
    }

    /**
     * <summary>
     *  This is the registered function for Assessment and Word Games scene.
     *  
     *  Since ang dalawang scene na iyon ay may parehas na replay and exit button.
     *  Pinagsama ko na lang sila sa isang function na ito.
     *  
     *  Also, to know kung anong scene ang iloload, magbabase ang iloload na scene
     *  sa current na active na scene.
     * </summary>
     */
    public void OnAssessmentAndWordGamesSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Word Games"))
        {
            Button replayButton = GameObject.Find("Replay Button").GetComponent<Button>();
            Button exit = GameObject.Find("Exit Button").GetComponent<Button>();
            Button close = GameObject.Find("Close").GetComponent<Button>();

            replayButton.onClick.AddListener(() =>
            {
                if (DataPersistenceManager.instance.playerData.dunongPoints >= 5)
                {
                    this.playerData.dunongPoints -= 5;

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment"))
                    {
                        this.LoadScene("Assessment");
                        return;
                    }
                    this.LoadScene("Word Games");
                }
            });

            exit.onClick.AddListener(() => this.LoadScene(AssessmentManager.instance.previousSceneToLoad));
            close .onClick.AddListener(() => this.LoadScene(AssessmentManager.instance.previousSceneToLoad));
        }
    }

    public void OnSleepCutsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Sleep Cutscene")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Fourth Sequence"))
        {
            if (this.playerData.gender.ToUpper() == "MALE")
            {
                try
                {
                    Transform toFollow = GameObject.Find("Male").transform;
                    GameObject.Find("Timeline for Player Female").SetActive(false);

                    GameObject.Find("Sequence Follow Cam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = toFollow;
                    GameObject.Find("Female").SetActive(false);

                }
                catch (System.Exception e) { }
            }
            else
            {
                try {
                    Transform toFollow = GameObject.Find("Female").transform;

                    GameObject.Find("Timeline for Player Male").SetActive(false);
                    GameObject.Find("Male").SetActive(false);

                    GameObject.Find("Sequence Follow Cam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = toFollow;

                }
                catch (System.Exception e) { }
            }
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Fifth Sequence")
            || scene.name == "E Second Sequence" || scene.name == "E Third Sequence")
        {
            if (this.playerData.gender.ToUpper() == "FEMALE")
            {
                GameObject.Find("Male").SetActive(false);
            }
            else
            {
                GameObject.Find("Female").SetActive(false);
            }
        }
    }

    /**
     * <summary>
     *  Ang function na ito ay isesetup ang common UI
     *  sa house and outside scene.
     *  
     *  Example ay ang mga optionsPanel, soundButton, volumePanel etc.
     * </summary>
     */
    public void SetUpHouseOrOutsideSceneButtons()
    {
        // Commented for Unit Testing ONLY!
        //if (!this.playerData.isTutorialDone)
        //{
        //    return;
        //}

        try
        {
            // GET ALL THE NECESSARY COMPONENTS.
            GameObject.Find("Character Name").GetComponent<TMPro.TextMeshProUGUI>().text = DataPersistenceManager.instance.playerData.name;
            this.homeCanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
            this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
            this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
            this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
            this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();
            
            this.dunongPointsValue = GameObject.Find("Dunong Points").transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            this.dunongPointsValue.text = DataPersistenceManager.instance.playerData.dunongPoints.ToString();

            Button closeButton = GameObject.Find("Close Button").GetComponent<Button>();

            this.soundButton.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 2");
                this.ShowVolumeUI();
            });

            closeButton.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                this.CloseOptionPanel();
            });

            this.quitButton.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 2");
                this.LoadScene("Menu");
            });

            Button showMapButton = GameObject.Find("Show Map").GetComponent<Button>();
            showMapButton.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                this.LoadScene("Philippine Map");
            });

            Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
            optionsButton.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                this.ShowOptionsPanel();

                if (TimeManager.instance != null)
                    TimeManager.instance.m_IsPaused = true;
            });

            this.optionsPanel.gameObject.SetActive(false);
            this.volumePanel.gameObject.SetActive(false);
        }
        catch (System.Exception e) { print(e.Message); }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void EnableCanvasGroupWhenOptionPanelIsClosed()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuSceneCanvasGroup.interactable = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.homeCanvasGroup.interactable = true;
            this.homeCanvasGroup.blocksRaycasts = true;
        }
    }


    public void DisableCanvasGroupWhenOptionPanelIsOpen()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuSceneCanvasGroup.interactable = false;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.homeCanvasGroup.interactable = false;
            this.homeCanvasGroup.blocksRaycasts = false;
        }
    }

    
    public void ShowOptionsPanel()
    {
        this.optionsPanel.gameObject.SetActive(true);
        this.DisableCanvasGroupWhenOptionPanelIsOpen();
        LeanTween.scale(optionsPanel.gameObject, new Vector3(1.586f, 1.586f, 1.586f), .2f)
            .setEase(LeanTweenType.easeInCubic);
    }

    // Function for closing the options panel.
    public void CloseOptionPanel()
    {
        if (soundButton.gameObject.activeInHierarchy)
        {
            LeanTween.scale(optionsPanel.gameObject, new Vector3(0, 0, 0), .2f)
            .setEaseSpring()
            .setOnComplete(() => {
                optionsPanel.gameObject.SetActive(false);
                this.EnableCanvasGroupWhenOptionPanelIsClosed();

                if (TimeManager.instance != null)
                    TimeManager.instance.m_IsPaused = false;
            });
        }
        else
        {
            CloseVolumeUI();
        }
    }

    // Function for showing the volume.
    public void ShowVolumeUI()
    {
        soundButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        volumePanel.gameObject.SetActive(true);
    }

    public void CloseVolumeUI()
    {
        soundButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        volumePanel.gameObject.SetActive(false);
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene("Philippine Map");
    }

 
    public void Quit()
    {
        Application.Quit();
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
