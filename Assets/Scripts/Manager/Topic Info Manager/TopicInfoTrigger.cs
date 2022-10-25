using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopicInfoTrigger : MonoBehaviour
{
    [SerializeField] private Sprite topicImage;
    [SerializeField] private string topic;
    [SerializeField] private TextAsset info;
    [SerializeField] public float timerValue;
    
    public enum PANEL_NUMBER { ONE, TWO, THREE, FOUR }
    public PANEL_NUMBER panelNumber;

    private Button viewInfoBtn;

    void Start()
    {
        try
        {
            this.viewInfoBtn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        }
        catch (System.Exception e)
        {
            this.viewInfoBtn = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        }
        this.viewInfoBtn.onClick.AddListener(() =>
        {
            if (TopicInfoManager.Instance != null)
            {
                QuestManager.instance?.FindTalkQuestGoal(gameObject.name);
                TopicInfoManager.Instance.TopicImage = this.topicImage;
                TopicInfoManager.Instance.Topic = this.topic;
                TopicInfoManager.Instance.Info = this.info;
                TopicInfoManager.Instance.panelNumber = (TopicInfoManager.PANEL_NUMBER)this.panelNumber;
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
