using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDunongPoints : MonoBehaviour
{
    private Image dunongPointsImage;
    private TMPro.TextMeshProUGUI dunongPointsText;

    private void OnEnable()
    {
        this.dunongPointsImage = transform.GetChild(0).GetComponent<Image>();
        this.dunongPointsText = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();

        QuestManager.UpdateDPValueUI += UpdateUI;
    }

    private void OnDisable()
    {
        QuestManager.UpdateDPValueUI -= UpdateUI;
    }

    public void UpdateUI()
    {
        this.dunongPointsText.text = DataPersistenceManager.instance.playerData.dunongPoints.ToString();

        LeanTween.scale(this.dunongPointsImage.gameObject, new Vector2(0.5640947f, 0.5640947f), .4f)
            .setEaseSpring()
            .setLoopCount(3)
            .setOnComplete(() => 
            {
                this.dunongPointsImage.gameObject.transform.localScale = new Vector2(0.4458542f, 0.4458542f);
            });
    }
}
