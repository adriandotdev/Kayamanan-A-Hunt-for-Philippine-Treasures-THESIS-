using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoothInfoTrigger : MonoBehaviour
{
    public string title;
    [TextArea]
    public string info;
    public Button button;

    void Start()
    {
        this.button = transform.GetChild(0).GetChild(0).GetComponent<Button>();

        this.button.onClick.AddListener(SetUpBoothInfo);
    }

    public void SetUpBoothInfo()
    {
        BoothInfoManager.instance.title = this.title;
        BoothInfoManager.instance.information = this.info;

        BoothInfoManager.instance.Open();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.button.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.button.gameObject.SetActive(false);
        }
    }
}
