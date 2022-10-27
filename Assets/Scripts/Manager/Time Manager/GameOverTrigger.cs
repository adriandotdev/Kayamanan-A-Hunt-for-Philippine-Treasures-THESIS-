using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject failMark;
    public Button restartBtn;
    public Button prevBtn;

    // Restarting Confirmation Panel
    public GameObject restartToRegion1Panel;
    public Button restartToReg1Btn;
    public Button cancelToReg1Btn;

    public GameObject restartToPreviousPanel;
    public Button restartToPrevBtn;
    public Button cancelToPrevBtn;

    public CanvasGroup restartButtonsCG; 

    private void OnEnable()
    {
        TimeManager.OnGameOver += OnGameOver;

        this.gameOverPanel = GameObject.Find("Game Over Panel");
        this.failMark = this.gameOverPanel.transform.GetChild(0).gameObject;
        this.restartBtn = this.gameOverPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        this.prevBtn =  this.gameOverPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        this.restartButtonsCG = this.gameOverPanel.transform.GetChild(1).GetComponent<CanvasGroup>();

        this.restartToRegion1Panel = gameOverPanel.transform.GetChild(2).gameObject;
        this.restartToReg1Btn = restartToRegion1Panel.transform.GetChild(3).GetChild(0).GetComponent<Button>();
        this.cancelToReg1Btn = restartToRegion1Panel.transform.GetChild(3).GetChild(1).GetComponent<Button>();

        this.restartToPreviousPanel = gameOverPanel.transform.GetChild(3).gameObject;
        this.restartToPrevBtn = restartToPreviousPanel.transform.GetChild(3).GetChild(0).GetComponent<Button>();
        this.cancelToPrevBtn = restartToPreviousPanel.transform.GetChild(3).GetChild(1).GetComponent<Button>();

        this.restartToReg1Btn.onClick.AddListener(RestartToRegion1);
        this.restartToPrevBtn.onClick.AddListener(RestartToPrevious);
        this.cancelToReg1Btn.onClick.AddListener(() => Close(this.restartToRegion1Panel));
        this.cancelToPrevBtn.onClick.AddListener(() => Close(this.restartToPreviousPanel));

        this.gameOverPanel.SetActive(false);
        this.restartToRegion1Panel.transform.localScale = Vector2.zero;
        this.restartToPreviousPanel.transform.localScale = Vector2.zero;
    }

    private void Start()
    {
        this.restartBtn.onClick.AddListener(() =>
        {
            LeanTween.scale(this.restartToRegion1Panel, new Vector2(1.53554f, 1.53554f), .2f)
            .setEaseSpring();
            this.restartButtonsCG.interactable = false;
        });

        this.prevBtn.onClick.AddListener(() =>
        {
            LeanTween.scale(this.restartToPreviousPanel, new Vector2(1.53554f, 1.53554f), .2f)
           .setEaseSpring();
            this.restartButtonsCG.interactable = false;
        });
    }

    private void RestartToRegion1()
    {
        print("RESTART BTN FROM GAME OVER TRIGGER");
        DataPersistenceManager.instance.playerData = DataPersistenceManager.instance.playerData.Restart();
        SceneManager.LoadScene("Outside");
    }

    private void RestartToPrevious()
    {
        print("RESTART PREVIOUS DATA BTN FROM GAME OVER TRIGGER");
        DataPersistenceManager.instance.playerData = DataPersistenceManager.instance.playerData.RestartToPrevious();
        SceneManager.LoadScene("Outside");
    }

    private void Close(GameObject panel)
    {
        LeanTween.scale(panel, Vector2.zero, .2f)
           .setEaseSpring();

        this.restartButtonsCG.interactable = true;
    }

    private void OnDisable()
    {
        TimeManager.OnGameOver -= OnGameOver;
    }


    public void OnGameOver()
    {
        this.gameOverPanel.SetActive(true);
    }
}
