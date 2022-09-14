using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopicInfoLoader : MonoBehaviour
{
    private Button closeButton;
    private Image topicPhoto;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI info;

    private float time;
    private float maxTime;

    // Timer UI
    private GameObject timerUIParentObj;
    private Image timerImage;
    private TMPro.TextMeshProUGUI timerText;

    // Info UI
    private RectTransform bubbleEffectImage;
    private TMPro.TextMeshProUGUI bubbleTitle;
    void Start()
    {
        this.bubbleEffectImage = GameObject.Find("Cloud Image").GetComponent<RectTransform>();
        this.bubbleTitle = GameObject.Find("Main Title").GetComponent<TMPro.TextMeshProUGUI>();

        this.bubbleEffectImage.transform.localScale = Vector2.zero;
        this.bubbleTitle.transform.localScale = Vector2.zero;

        TweeningManager.instance.ShowTopicInfoMainTitle(this.bubbleEffectImage.gameObject, this.bubbleTitle.gameObject);

        //this.closeButton = GameObject.Find("Close").GetComponent<Button>();
        //this.topicPhoto = GameObject.Find("Topic Photo").GetComponent<Image>();
        this.title = GameObject.Find("Main Title").GetComponent<TMPro.TextMeshProUGUI>();
        //this.info = GameObject.Find("Topic Info").GetComponent<TMPro.TextMeshProUGUI>();

        //// Get the Timer UI
        //this.timerUIParentObj = GameObject.Find("Timer UI");
        //this.timerImage = GameObject.Find("Timer Image").GetComponent<Image>();
        //this.timerText = GameObject.Find("Timer Text").GetComponent<TMPro.TextMeshProUGUI>();

        //this.topicPhoto.sprite = TopicInfoManager.Instance.TopicImage;
        this.title.text = TopicInfoManager.Instance.Topic;
        //this.info.text = TopicInfoManager.Instance.Info;
        //this.time = TopicInfoManager.Instance.TimerValue;
        //this.maxTime = this.time;

        //// Hide and wait the closeButton to show for a certain amount of time.
        //this.closeButton.gameObject.SetActive(false);

        //// Add event to the button.
        //this.closeButton.onClick.AddListener(() =>
        //{
        //    SceneManager.UnloadSceneAsync("Topic Info Scene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        //});
    }

    //private void Update()
    //{
    //    if (time > 0)
    //    {
    //        time -= 1 * Time.deltaTime;
    //        this.timerImage.fillAmount = (float)(time / maxTime);
    //        this.timerText.text = ((int)time).ToString();
    //    }
    //    else
    //    {
    //        if(!this.closeButton.gameObject.activeInHierarchy)
    //        {
    //            this.closeButton.gameObject.SetActive(true);
    //            this.timerUIParentObj.SetActive(false);
    //        }
    //    }
    //}

}
