using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MemoryGameManager : MonoBehaviour
{
    public static MemoryGameManager instance;
    public RectTransform puzzleContainer; // Container of the tile
    public GameObject puzzleButton; // Puzzle Prefab
    public Sprite notFlippedImage; // Image of the button when it is not clicked.
    public Sprite[] buttonImages; // Images that is going to be used for memory game.

    public bool firstGuess;
    public bool secondGuess;

    public string firstGuessName;
    public string secondGuessName;

    public Transform transform1;
    public Transform transform2;

    Coroutine firstSelectionCoroutine;
    Coroutine flipTwoCardsCoroutine;

    // Out of Time Panel UI
    private Button replayButton;
    private Button exitButton;

    // Timer UI
    public Image timerImage;
    public TMPro.TextMeshProUGUI timerText;
    public bool isShowingAllCards;
    public float maxTimeForShowingCards;
    public float maxTimerValue;
    private float timerValue; // need to set by memory trigger script.
    private float showingCardsTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnMemoryGameSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMemoryGameSceneLoaded;
    }

    private void Update()
    {
        this.UpdateTime();
    }

    /// <summary>
    /// A function that setups the buttonImages, maximum time of showing cards for the players able to memorize,
    /// and the maximum time or timer that a player can play.
    /// </summary>
    /// <seealso cref="MemoryGameTrigger"/>
    public void StartMemoryGame(Sprite[] buttonImages, float maxTimeForShowingCards, float maxTimerValue)
    {
        this.buttonImages = buttonImages;
        this.maxTimeForShowingCards = maxTimeForShowingCards;
        this.maxTimerValue = maxTimerValue;
        this.timerValue = this.maxTimerValue;
        this.showingCardsTimer = this.maxTimeForShowingCards;
    }

    /// <summary>
    /// Updates the time.
    /// 
    /// It is either showing the cards images for a certain amout of time
    /// or it is playing the actual memory game.
    /// </summary>
    private void UpdateTime()
    {
        /**- Check if the cards is not showing, which means it is the actual memory game.
         * - Check if the 'puzzleContainer' is not equal to null to avoid any errors. */
        if (isShowingAllCards == false && this.puzzleContainer != null)
        {
            this.timerValue -= (int)1 * Time.deltaTime;

            if (this.timerValue >= 0f && !this.IsAllFlipped())
            {
                // Update UI.
                this.timerImage.fillAmount = timerValue / maxTimerValue;
                this.timerText.text = ((int)timerValue).ToString();
            }

            /**If the player is run out of time, then we show the Out of time panel. */
            else if (this.timerValue <= 0f && !this.IsAllFlipped())
            {
                TweeningManager.instance?.ShowOutOfTimePanel();
                return;
            }
        }
        // This will run if the cards is still showing to be memorize by the player.
        else
        {
            if (this.puzzleContainer != null)
            {
                this.showingCardsTimer -= (int)1 * Time.deltaTime;
                this.timerImage.fillAmount = showingCardsTimer / maxTimerValue;
                this.timerText.text = ((int)showingCardsTimer).ToString();
            }
        }
    }

    void OnMemoryGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Memory Game"))
        {
            ///<summary>
            /// If the player is failed to finish the memory game in an amount of time.
            /// Then we need to reset some of the properties or variables.
            /// <see cref="Reset"/>
            /// </summary>
            this.Reset(); 

            this.puzzleContainer = GameObject.Find("Puzzle Container").GetComponent<RectTransform>();
            this.timerImage = GameObject.Find("Timer Image").GetComponent<Image>();
            this.timerText = GameObject.Find("Timer Text").GetComponent<TextMeshProUGUI>();
            this.replayButton = GameObject.Find("Replay Button").GetComponent<Button>();
            this.timerText.text = ((int)this.timerValue).ToString();

            this.AddAllButtons();
            /**
             * Add events to the button (Replay Button). 
             */
            this.replayButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Memory Game");
            });
        }
    }

    public void Reset()
    {
        this.isShowingAllCards = true;
        this.timerValue = this.maxTimerValue;
        this.showingCardsTimer = this.maxTimeForShowingCards;
        this.firstGuessName = "";
        this.secondGuessName = "";
        this.transform1 = null;
        this.transform2 = null;
    }

    public void AddAllButtons()
    {
        /**
         * <summary>
         *  Ang laman nito ay yung mga name ng buttons na para sa memory game.
         * </summary>
         **/
        int[] buttonNames = new int[this.buttonImages.Length * 2];

        FisherYates.Shuffle(ref this.buttonImages);

        /**
         * <summary>
         *  Instantiate the buttons and set the parent to 'buttonsContainer'.
         *  The number of buttons na iinstantiate ay double ng number of 'buttonImages' length.
         *  
         *  Example:
         *  4 = 4 * 2 = 8 
         *  Since matching lang naman ito.
         * </summary> 
         */
        GameObject memoryGameBTN = null;
        for (int i = 0; i < (4 * 2); i++)
        {
            memoryGameBTN = Instantiate(this.puzzleButton, this.puzzleContainer, false);
            memoryGameBTN.GetComponent<Image>().sprite = this.notFlippedImage;
            memoryGameBTN.GetComponent<FlippedScript>().IsFlipped = false;
        }

        int index = 0;
        int j = 0;
        foreach (Transform transform in this.puzzleContainer)
        {
            // We need to check if the index is equal to half of 'buttonsContainer' number of childs then we set it again to 0.
            if (index == this.puzzleContainer.childCount / 2 - 1)
            {
                index = 0;
            }
            else
            {
                index += 1;
            }
            buttonNames[j] = index;
            j++;
        }

        FisherYates.Shuffle(ref buttonNames);

        // ADD NAME TO ALL BUTTON LAYOUTS INSIDE THE CONTAINER
        int h = 0;

        foreach (Transform transform in this.puzzleContainer)
        {
            transform.gameObject.name = buttonNames[h].ToString();
            h++;
        }

        this.ShowAllMemoryGameImages(this.maxTimeForShowingCards);
    }

    /**<summary>
     *  Show All Images for Memorization.
     * </summary> */
    void ShowAllMemoryGameImages(float time)
    {
        foreach (Transform transform in this.puzzleContainer)
        {
            transform.GetComponent<Image>().sprite = this.buttonImages[int.Parse(transform.name)];
        }
        StartCoroutine(HideAllImages(time));
    }

    IEnumerator HideAllImages(float time)
    {
        yield return new WaitForSeconds(time);

        this.isShowingAllCards = false;

        foreach (Transform transform in this.puzzleContainer)
        {
            transform.GetComponent<Image>().sprite = this.notFlippedImage;

            // Add events to each buttons.
            transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                this.CheckImage(transform);
            });
        }
    }

    public bool IsAllFlipped()
    {
        foreach (Transform transform in this.puzzleContainer)
        {
            if (transform.GetComponent<FlippedScript>().IsFlipped == false) return false;
        }
        return true;
    }

    public void CheckImage(Transform transform)
    {
        // Check if the player has already flipped two cards, so we may wait for a certain seconds.
        if (this.flipTwoCardsCoroutine != null)
        {
            return;
        }

        // Disable the button of the selected image (NOTE: It has "Button" component).
        transform.GetComponent<Button>().enabled = false;

        // If the 'firstGuess' boolean is false.
        if (!firstGuess)
        {
            this.firstGuess = true;

            // Change the scale to 0.
            LeanTween.scale(transform.gameObject, new Vector3(0f, transform.localScale.y), 0.05f)
            .setOnComplete(() =>
            {
                // Change the scale to 1 after the first scaling completed to make it set back to normal
                LeanTween.scale(transform.gameObject, new Vector3(1f, transform.localScale.y), .05f)
                .setOnComplete(() =>
                {
                    // If it is completed, set the image of the clicked tile or image and set the 'firstGuessName' to the name of the sprite.
                    transform.GetComponent<Image>().sprite = this.buttonImages[int.Parse(transform.name)];
                    this.firstGuessName = transform.GetComponent<Image>().sprite.name;

                    // It is used to reference this when the player is flipped two cards that aren't match and we need to flipped it again in a specific amount of time.
                    this.transform1 = transform;

                    // If the player is not clicked the second card, we need to flip the card.
                    this.firstSelectionCoroutine = StartCoroutine(FlipCard(transform));
                });
            });
        }
        else if (!secondGuess)
        {
            this.secondGuess = true;

            LeanTween.scale(transform.gameObject, new Vector3(0f, transform.localScale.y), .05f)
            .setOnComplete(() =>
            {
                LeanTween.scale(transform.gameObject, new Vector3(1f, transform.localScale.y), .05f)
                .setOnComplete(() =>
                {
                    transform.GetComponent<Image>().sprite = this.buttonImages[int.Parse(transform.name)];
                    this.secondGuessName = transform.GetComponent<Image>().sprite.name;

                    this.transform2 = transform;

                    /** Check if it the card is the same. */
                    if (this.firstGuessName.ToUpper() == this.secondGuessName.ToUpper())
                    {
                        transform1.GetComponent<FlippedScript>().IsFlipped = true;
                        transform2.GetComponent<FlippedScript>().IsFlipped = true;

                        // If all the cards are flipped.
                        if (IsAllFlipped())
                        {
                            print("IS ALL FLIPPED");
                        }
                        this.firstGuess = false;
                        this.secondGuess = false;

                        StopAllCoroutines();
                    }
                    else
                    {
                        this.firstGuess = false;
                        this.secondGuess = false;

                        StopAllCoroutines();
                        this.flipTwoCardsCoroutine = StartCoroutine(FlippedTwoCards());
                    }
                });
            });
        }
    }

    IEnumerator FlipCard(Transform transform)
    {
        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(transform.gameObject, new Vector3(0f, transform.localScale.y), .05f)
        .setOnComplete(() =>
        {
            LeanTween.scale(transform.gameObject, new Vector3(1f, transform.localScale.y), .05f)
            .setOnComplete(() =>
            {
                transform.GetComponent<Image>().sprite = this.notFlippedImage;
                transform.GetComponent<Button>().enabled = true;

                this.firstGuess = false;
                this.secondGuess = false;

                this.firstGuessName = "";
                this.secondGuessName = "";
            });
        });
    }

    IEnumerator FlippedTwoCards()
    {
        yield return new WaitForSeconds(1);


        LeanTween.scale(transform1.gameObject, new Vector3(0f, transform1.localScale.y), .05f)
        .setOnComplete(() =>
        {
            LeanTween.scale(transform1.gameObject, new Vector3(1f, transform1.localScale.y), .05f)
            .setOnComplete(() =>
            {
                transform1.GetComponent<Image>().sprite = this.notFlippedImage;
                this.firstGuessName = "";
                transform1.GetComponent<Button>().enabled = true;
                transform1 = null;
            });
        });

        LeanTween.scale(transform2.gameObject, new Vector3(0f, transform2.localScale.y), .05f)
        .setOnComplete(() =>
        {
            LeanTween.scale(transform2.gameObject, new Vector3(1f, transform2.localScale.y), .05f)
            .setOnComplete(() =>
            {
                transform2.GetComponent<Image>().sprite = this.notFlippedImage;
                this.secondGuessName = "";
                transform2.GetComponent<Button>().enabled = true;
                transform2 = null;
                this.flipTwoCardsCoroutine = null;
            });
        });
    }
}
