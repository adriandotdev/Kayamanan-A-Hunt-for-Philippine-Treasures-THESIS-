using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameTrigger : MonoBehaviour
{
    public Sprite[] buttonImages;
    public float maxTimeForShowingCards;
    public float maxTimerValue;

    private void Start()
    {
            
    }

    public void StartMemoryGame()
    {
        MemoryGameManager.instance?.StartMemoryGame(this.buttonImages, this.maxTimeForShowingCards, this.maxTimerValue);
        SceneManager.LoadScene("Memory Game");
    }
}
