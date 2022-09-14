using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepTrigger : MonoBehaviour
{
    private GameObject panel;
    private GameObject splashEffect;
    private GameObject clock;

    private void OnEnable()
    {
        TimeManager.OnTimeToSleep += TriggerSleepAnimation;

        this.panel = GameObject.Find("Go To Sleep Warning Panel");
        this.splashEffect = GameObject.Find("Splash Effect");
        this.clock = GameObject.Find("Sleep Clock");

        this.splashEffect.transform.localScale = Vector2.zero;
        this.clock.transform.localScale = Vector2.zero;

        this.panel.SetActive(false);
    }

    private void OnDisable()
    {
        TimeManager.OnTimeToSleep -= TriggerSleepAnimation;
    }

    void TriggerSleepAnimation()
    {
        this.panel.SetActive(true);

        LeanTween.scale(this.splashEffect, new Vector2(89.14472f, 89.14472f), .2f)
        .setEaseSpring()
        .setOnComplete(() => 
        {
            LeanTween.scale(this.clock, new Vector2(89.14472f, 89.14472f), .2f)
            .setEaseSpring()
            .setOnComplete(() => {

                LeanTween.scale(this.splashEffect, new Vector2(85.20542f, 85.20542f), .5f)
                    .setLoopPingPong()
                    .setLoopCount(2)
                    .setLoopType(LeanTweenType.pingPong);

                LeanTween.rotateZ(this.clock, 25f, .5f)
                .setLoopPingPong()
                .setLoopCount(2)
                .setLoopType(LeanTweenType.pingPong)
                .setOnComplete(() => {

                    DataPersistenceManager.instance.playerData.sceneToLoad = "House";
                    StartCoroutine(this.LoadScene());
                });
            });
        });
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);

        print("Load Sleep Cutscene scene");

        SceneManager.LoadScene("Sleep Cutscene");
    }
}
