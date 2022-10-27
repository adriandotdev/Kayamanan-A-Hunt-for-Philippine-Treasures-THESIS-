using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoothInfoManager : MonoBehaviour
{
    public static BoothInfoManager instance;

    public GameObject boothInfoPanel;
    public Button closeButton;
    public TextMeshProUGUI titleUI;
    public TextMeshProUGUI infoUI;

    // Content
    public string title;
    public string information;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        this.boothInfoPanel = GameObject.Find("Booth Info Panel");
        this.closeButton = this.boothInfoPanel.transform.GetChild(2).GetComponent<Button>();

        this.titleUI = this.boothInfoPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        this.infoUI = this.boothInfoPanel.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();

        this.boothInfoPanel.transform.localScale = Vector2.zero;

        this.closeButton.onClick.AddListener(Close);
    }

    public void Open()
    {
        LeanTween.scale(this.boothInfoPanel, Vector2.one, .2f)
           .setEaseSpring();

        this.titleUI.text = this.title;
        this.infoUI.text = this.information;
    }

    public void Close()
    {
        LeanTween.scale(this.boothInfoPanel, Vector2.zero, .2f)
            .setEaseSpring();
    }
}
