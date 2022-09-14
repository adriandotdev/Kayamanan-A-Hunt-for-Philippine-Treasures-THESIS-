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

    public string Info { set; get; }

    public float TimerValue { set; get; }
    /// UI
 

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
