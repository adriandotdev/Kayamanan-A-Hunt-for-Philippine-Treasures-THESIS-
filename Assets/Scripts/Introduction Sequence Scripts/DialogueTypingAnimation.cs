using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTypingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DisplayLine(textMeshPro.text));
    }

    IEnumerator DisplayLine(string line)
    {
        this.textMeshPro.text = line;
        this.textMeshPro.maxVisibleCharacters = 0;

        foreach (char c in line.ToCharArray())
        {
            this.textMeshPro.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
