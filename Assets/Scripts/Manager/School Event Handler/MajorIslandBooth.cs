using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MajorIslandBooth : MonoBehaviour
{
    public RectTransform alertBox;
    public Button playButton;

    private void Start()
    {
        this.playButton = transform.GetChild(1).GetChild(0).GetComponent<Button>();

        this.playButton.onClick.AddListener(() => {

            if (gameObject.name.ToUpper() == "VISAYAS" && this.IsRegionWithNumberOpen(9))
                SceneManager.LoadScene(gameObject.name);
            else if (gameObject.name.ToUpper() == "MINDANAO" && this.IsRegionWithNumberOpen(12))
                SceneManager.LoadScene(gameObject.name);
            else if (gameObject.name.ToUpper() == "LUZON")
            {
                SceneManager.LoadScene(gameObject.name);
            }
            else
            {
                print(gameObject.name + " island is not yet open.");
            }
        });

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
}
