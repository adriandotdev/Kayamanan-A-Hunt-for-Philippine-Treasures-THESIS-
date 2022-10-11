using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCardController : MonoBehaviour
{
    public GameObject[] photoCards;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject photocard in this.photoCards)
        {
            photocard.GetComponent<Button>().onClick.AddListener(() =>
            {
                LeanTween.moveLocalX(photocard, -Screen.width, .2f);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
