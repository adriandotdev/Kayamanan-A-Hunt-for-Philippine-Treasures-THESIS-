using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager instance;

    public Image achievementImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public Scrollbar vertScrollBar;

    public Sprite sprite;

    public string title;
    public string info;
    public float vertValue = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAchievementsSceneLoaded;
        SceneManager.sceneLoaded += OnPlayerScenesLoaded;
        SceneManager.sceneLoaded += OnAchievementInfoLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAchievementsSceneLoaded;
        SceneManager.sceneLoaded -= OnPlayerScenesLoaded;
        SceneManager.sceneLoaded -= OnAchievementInfoLoaded;
    }

    public void OnAchievementsSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Achievements")
        {
            Button backButton = GameObject.Find("Back").GetComponent<Button>();

            backButton.onClick.AddListener(() 
                => {
                    SoundManager.instance?.PlaySound("Click Close");
                    TransitionLoader.instance?.StartAnimation(DataPersistenceManager.instance.playerData.sceneToLoad);
                    //SceneManager.LoadScene(DataPersistenceManager.instance.playerData.sceneToLoad);
                });

            vertScrollBar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        }
    }

    public void OnAchievementInfoLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Achievement Info")
        {

            Button back = GameObject.Find("Back").GetComponent<Button>();
            achievementImage = GameObject.Find("Achievement Image").GetComponent<Image>();
            titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
            infoText = GameObject.Find("Information").GetComponent<TextMeshProUGUI>();

            achievementImage.sprite = sprite;
            titleText.text = title;
            infoText.text = info;

            back.onClick.AddListener(() => {
                SoundManager.instance?.PlaySound("Click Close");
                TransitionLoader.instance?.StartAnimation("Achievements");
                //SceneManager.LoadScene("Achievements");
            });
        }
    }

    public void ScrollbarChanged(float value)
    {
        vertValue = value;
    }

    public void OnPlayerScenesLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Outside" || scene.name == "House" || 
            scene.name == "School" || scene.name == "Museum" || scene.name == "Church"
            || scene.name == "Vacation Scene")
        {
            Button achievementsButton = GameObject.Find("Achievements BTN").GetComponent<Button>();

            achievementsButton.onClick.AddListener(() => {

                //SceneManager.LoadScene("Achievements");
                SoundManager.instance?.PlaySound("Button Click 1");
                TransitionLoader.instance?.StartAnimation("Achievements");
                DataPersistenceManager.instance.playerData.achievementsNewIcon = false;
            });
        }
    }
}
