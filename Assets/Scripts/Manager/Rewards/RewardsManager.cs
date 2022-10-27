using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    public GameObject BG;
    private CanvasGroup canvasGroup;
    private Button collectBTN;
    private string regionName;

    public RectTransform collectiblesPanel;

    public GameObject newRegionOpenPrefab;
    public GameObject collectiblePrefab;

    private bool[] IsAllUICollected = { false, false, false };
    private int IsAllUICollectedIndex = 1;

    private void OnEnable()
    {
        /** Since the scene is additive, we disabled the all of the UI to avoid a certain
         * click events. */

        // COMMENT THIS FOR TESTING.
        this.canvasGroup = GameObject.Find("Safe Area of Game").GetComponent<CanvasGroup>();
        this.canvasGroup.interactable = false;

        this.regionName = AssessmentManager.instance.regionName;
        // COMMENT ABOVE FOR TESTING

        LeanTween.scaleX(this.BG.gameObject, 1f, .3f)
            .setOnComplete(DisplayCollectibles);

        this.collectBTN = GameObject.Find("Collect BTN").GetComponent<Button>();
        this.collectBTN.onClick.AddListener(TestCollect);
    }

    public void RemoveAllRewards()
    {
        foreach (RectTransform reward in this.collectiblesPanel)
        {
            Destroy(reward.gameObject);
        }
    }

    private void DisplayCollectibles()
    {
        SoundManager.instance?.PlaySound("Unlock Item");

        try
        {
            foreach (Collectible collectible in DataPersistenceManager.instance.playerData.notebook.collectibles)
            {
                if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
                {
                    GameObject collectibleObj = Instantiate(collectiblePrefab, collectiblesPanel.transform);

                    collectibleObj.transform.localScale = Vector2.zero;

                    collectibleObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(collectible.imagePath);
                    //collectibleObj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = collectible.name;

                    LeanTween.scale(collectibleObj, Vector2.one, .5f)
                        .setEaseSpring();
                }
            }
        }
        catch (System.Exception e) {
            print(e.Message);
        }
    }

    private void DisplayNewOpenRegion()
    {
        GameObject newRegion = Instantiate(newRegionOpenPrefab, collectiblesPanel.transform);

        newRegion.transform.localScale = Vector2.zero;

        LeanTween.scale(newRegion, new Vector2(2.5291f, 2.5291f), .5f)
            .setEaseSpring();
    }

    public void TestCollect()
    {
        this.RemoveAllRewards();

        switch(this.IsAllUICollectedIndex)
        {
            /** Show the new open region. */
            case 1:
           
                SoundManager.instance?.PlaySound("Unlock Item");
             
                this.collectBTN.gameObject.SetActive(false);
                this.BG.gameObject.transform.localScale = new Vector2(0, 1);

                LeanTween.scaleX(this.BG.gameObject, 1f, .3f)
                    .setOnComplete(() => {
                        this.collectBTN.gameObject.SetActive(true);
                        this.DisplayNewOpenRegion();
                    });
                this.IsAllUICollectedIndex++;
                break;
            case 2:
                this.Close();
                break;
        }
    }

    public void Close()
    {
        this.canvasGroup.interactable = true;
        SceneManager.UnloadSceneAsync("Collectibles", UnloadSceneOptions.None);
    }
}
