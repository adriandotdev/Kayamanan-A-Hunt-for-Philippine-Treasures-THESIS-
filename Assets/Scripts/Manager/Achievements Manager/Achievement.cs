using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Achievement : MonoBehaviour
{
    [SerializeField] 
    private Image achievementImage;

    [SerializeField] private string achievementName;

    [SerializeField] private string info;

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();

        if (IsCollected())
        {
            achievementImage.color = Color.white;
            btn.onClick.AddListener(() =>
            {
                SoundManager.instance?.PlaySound("Button Click 1");
                AchievementsManager.instance.sprite = achievementImage.sprite;
                AchievementsManager.instance.title = this.achievementName;
                AchievementsManager.instance.info = this.info;
                SceneManager.LoadScene("Achievement Info");
            });
        }
        else
        {
            achievementImage.color = Color.black;
        }

    }

    private bool IsCollected()
    {
        foreach (Collectible collectible in DataPersistenceManager.instance.playerData.notebook.collectibles)
        {
            if (collectible.name.ToUpper() == this.achievementName.ToUpper())
            {
                if (collectible.isCollected) return true;
            }
        }
        return false;
    }
}
