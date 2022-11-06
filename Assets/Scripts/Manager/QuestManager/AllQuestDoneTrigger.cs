using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AllQuestDoneTrigger : MonoBehaviour
{
    public RectTransform allQuestDonePanel;
    public GameObject allQuestDoneBase;
    public TextMeshProUGUI description;
    public Button gotItBtn;

    private void OnEnable()
    {
        QuestManager.OnAllQuestCompleted += ShowAllQuestDonePanel;

        this.allQuestDoneBase = GameObject.Find("All Quest Done Base");
        this.allQuestDonePanel = GameObject.Find("All Quest Done Panel").GetComponent<RectTransform>();
        this.description = this.allQuestDonePanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        this.gotItBtn = allQuestDonePanel.GetChild(2).GetComponent<Button>();

        this.gotItBtn.onClick.AddListener(Close);

        this.allQuestDoneBase.SetActive(false);
        this.allQuestDonePanel.localScale = Vector2.zero;
    }

    private void OnDisable()
    {
        QuestManager.OnAllQuestCompleted -= ShowAllQuestDonePanel;
    }

    public void ShowAllQuestDonePanel(string message)
    {
        this.allQuestDoneBase.SetActive(true);

        this.description.text = message;
        LeanTween.scale(this.allQuestDonePanel.gameObject, new Vector2(334.8216f, 334.8216f), .2f)
            .setEaseSpring();
    }

    public void Close()
    {
        LeanTween.scale(this.allQuestDonePanel.gameObject, Vector2.zero, .2f)
            .setEaseSpring();

        this.allQuestDoneBase.SetActive(false);
    }
}
