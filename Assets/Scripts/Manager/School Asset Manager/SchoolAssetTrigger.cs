using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchoolAssetTrigger : MonoBehaviour
{
    public TextAsset textToRead;
    public Button infoButton;
    public string nameOfInfo;

    private void Start()
    {
        infoButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        nameOfInfo = transform.parent.gameObject.name;

        infoButton.onClick.AddListener(ReadInfo);
        infoButton.gameObject.SetActive(false);
    }

    public void ReadInfo()
    {
        SchoolAssetManager.instance.StartShowingInfo(textToRead);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            infoButton.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            infoButton.gameObject.SetActive(false);
        }
    }
}
