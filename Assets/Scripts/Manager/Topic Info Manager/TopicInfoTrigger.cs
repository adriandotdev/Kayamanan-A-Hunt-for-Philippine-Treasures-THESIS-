using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopicInfoTrigger : MonoBehaviour
{
    [SerializeField] private Sprite topicImage;
    [SerializeField] private string topic;
    [SerializeField] [TextArea] private string info;
    [SerializeField] public float timerValue;

    private Button viewInfoBtn;

    void Start()
    {
        this.viewInfoBtn = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        this.viewInfoBtn.onClick.AddListener(() =>
        {
            if (TopicInfoManager.Instance != null)
            {
                TopicInfoManager.Instance.TopicImage = topicImage;
                TopicInfoManager.Instance.Topic = topic;
                TopicInfoManager.Instance.Info = info;
                TopicInfoManager.Instance.TimerValue = timerValue;
                SceneManager.LoadSceneAsync("Topic Info Scene", LoadSceneMode.Additive);
            }
        });
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            this.viewInfoBtn.gameObject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            this.viewInfoBtn.gameObject.SetActive(false);
    }
}
