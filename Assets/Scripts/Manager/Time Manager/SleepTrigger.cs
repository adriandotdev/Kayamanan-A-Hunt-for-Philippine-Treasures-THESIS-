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
        TimeManager.OnTimeToSleep += TriggerSleepAnimation; // Register to the event at TimeManager.

        this.panel = GameObject.Find("Go To Sleep Warning Panel");
        this.splashEffect = GameObject.Find("Splash Effect");
        this.clock = GameObject.Find("Sleep Clock");

        this.splashEffect.transform.localScale = Vector2.zero;
        this.clock.transform.localScale = Vector2.zero;

        this.panel.SetActive(false);
    }

    private void OnDisable()
    {
        TimeManager.OnTimeToSleep -= TriggerSleepAnimation; // Unregister.
    }

    void TriggerSleepAnimation()
    {
        this.panel.SetActive(true);

        // Scale the splash effect image
        LeanTween.scale(this.splashEffect, new Vector2(89.14472f, 89.14472f), .2f)
        // when complete
        .setOnComplete(() => 
        {
            // scale the clock
            LeanTween.scale(this.clock, new Vector2(89.14472f, 89.14472f), .2f)
            .setDelay(.2f)
            .setEaseSpring()

            // when the clock stops scaling
            .setOnComplete(() => {

                // Scale the splash effect 2 times with a loop type of ping pong. meaning, it is scaling up and down.
                LeanTween.scale(this.splashEffect, new Vector2(85.20542f, 85.20542f), .5f)
                    .setLoopPingPong()
                    .setLoopType(LeanTweenType.pingPong);

                // Rotate the clock 2 times and in a ping pong style.
                LeanTween.rotateZ(this.clock, 25f, .5f)
                .setLoopPingPong(3)
                .setOnComplete(() => {

                    DataPersistenceManager.instance.playerData.sceneToLoad = "House";
                    SceneManager.LoadScene("Sleep Cutscene");
                });
            });
        });
    }
}
