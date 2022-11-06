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
    public Sprite sprite;

    public string title;
    public string info;

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
                => SceneManager.LoadScene(DataPersistenceManager.instance.playerData.sceneToLoad));
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

            back.onClick.AddListener(() => SceneManager.LoadScene("Achievements"));
        }
    }

    public void OnPlayerScenesLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Outside" || scene.name == "House" || 
            scene.name == "School" || scene.name == "Museum" || scene.name == "Church")
        {
            Button achievementsButton = GameObject.Find("Achievements BTN").GetComponent<Button>();

            achievementsButton.onClick.AddListener(() => {

                SceneManager.LoadScene("Achievements");
                DataPersistenceManager.instance.playerData.achievementsNewIcon = false;
            });
        }
    }
}
