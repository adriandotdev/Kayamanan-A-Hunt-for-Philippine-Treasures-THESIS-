using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GettingNameInEndingCutscene : MonoBehaviour
{
    public TextMeshProUGUI certificateNme;

    // Start is called before the first frame update
    void Start()
    {
        this.certificateNme = GetComponent<TextMeshProUGUI>();

        if (DataPersistenceManager.instance != null)
        {
            certificateNme.text = DataPersistenceManager.instance.playerData.name;
        }
    }
}
