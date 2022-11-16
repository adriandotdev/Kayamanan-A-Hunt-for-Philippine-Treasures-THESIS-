using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MajorIslandBooth : MonoBehaviour
{
    public RectTransform alertBox;
    public Button playButton;
    public RectTransform plainAlertBox;

    private void Start()
    {
        try
        {
            this.playButton = transform.GetChild(1).GetChild(0).GetComponent<Button>();
            this.plainAlertBox = GameObject.Find("Plain Alert Box").GetComponent<RectTransform>();

            this.playButton.onClick.AddListener(() =>
            {

                if (gameObject.name.ToUpper() == "VISAYAS" && this.IsRegionWithNumberOpen(9))
                    //SceneManager.LoadScene("Visayas");
                    TransitionLoader.instance?.StartAnimation("Visayas");
                else if (gameObject.name.ToUpper() == "MINDANAO" && this.IsRegionWithNumberOpen(12))
                    //SceneManager.LoadScene("Mindanao");
                    TransitionLoader.instance?.StartAnimation("Mindanao");
                else if (gameObject.name.ToUpper() == "LUZON")
                {
                    //SceneManager.LoadScene("Luzon");
                    TransitionLoader.instance?.StartAnimation("Luzon");
                }
                else
                {
                    this.plainAlertBox.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " is not yet open";
                    LeanTween.scale(this.plainAlertBox.gameObject, Vector2.one, .2f)
                    .setEaseSpring();
                    StartCoroutine(ClosePanel());
                }
            });
        }
        catch (System.Exception e) { }

        this.playButton.gameObject.SetActive(false);
    }

    public bool IsRegionWithNumberOpen(int regionNumber)
    {
        foreach (RegionData regionData in DataPersistenceManager.instance.playerData.regionsData)
        {
            if (regionNumber == regionData.regionNumber && regionData.isOpen)
            {
                return true;
            }
        }

        return false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.playButton.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.playButton.gameObject.SetActive(false);
        }
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(1);

        LeanTween.scale(this.plainAlertBox.gameObject, Vector2.zero, .2f)
                .setEaseSpring();
    }
}
