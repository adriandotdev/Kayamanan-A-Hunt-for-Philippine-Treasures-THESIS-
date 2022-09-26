using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TopicInfoManager : MonoBehaviour
{
    public static TopicInfoManager Instance { get; private set; }
    public Sprite TopicImage { set; get; }
    public string Topic { set; get; }

    public TextAsset Info { set; get; }

    public float TimerValue { set; get; }
    
    public enum PANEL_NUMBER { ONE, TWO, THREE, FOUR }
    public PANEL_NUMBER panelNumber;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
