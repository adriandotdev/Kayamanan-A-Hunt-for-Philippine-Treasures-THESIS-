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
        if (gameObject.name != "1")
        {
            if (DataPersistenceManager.instance.playerData.memoryGameData.memoryGameProgress[int.Parse(gameObject.name) - 1])
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    public void StartMemoryGame()
    {
        if (DataPersistenceManager.instance != null)
        {
            if (DataPersistenceManager.instance.playerData.memoryGameData.memoryGameProgress[int.Parse(gameObject.name) - 1])
            {
                MemoryGameManager.instance.indexOfNextMemoryGameToBeOpen = int.Parse(gameObject.name);
                MemoryGameManager.instance?.StartMemoryGame(this.buttonImages, this.maxTimeForShowingCards, this.maxTimerValue);
                TransitionLoader.instance?.StartAnimation("Memory Game");
            }
        }
    }
}
