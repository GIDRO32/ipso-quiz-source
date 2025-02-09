using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingGame : MonoBehaviour
{
    public Text targetTextUI;  // UI element to display Target Text
    public Text titleTextUI;   // UI element to display the title
    public InputField playerInputField; // Player's Input Field
    public Text timerText;  // Countdown Timer UI
    public AudioSource SFX;
    public AudioClip misclick;
    public AudioClip YesSentence;

    private List<string> sentences = new List<string>();
    private List<string> titles = new List<string>();
    private List<int> sentenceOrder = new List<int>(); // Stores shuffled indices
    private int currentSentenceIndex = 0;
    private float gameTime = 180f; // 3-minute timer
    private bool gameActive = false;
    private string currentTargetSentence;
    private int difficultyMode = 0; // 0 = Normal, 1 = Hard, 2 = Extreme
    private int inkSplashCount = 0; // Number of ink splashes
public GameObject perfectSprite; // UI element for "Perfect!" animation
private int mistakeCount = 0;


public Text scoreText; // UI to display score
public Text multiplierText; // UI to display multiplier
public Text comboText; // UI to display combo streak
public Text comboTimer;

private int score = 0;
private int comboStreak = 0;
private int pointMultiplier = 1;
private float phraseStartTime;
private float comboTime = 180f;

    void Start()
    {
        // Initialize sentence list
        sentences.Add("The capital of Ukraine is Kyiv.");
        sentences.Add("The Moon orbits the Earth.");
        sentences.Add("The internet was invented by Tim Berners-Lee.");
        sentences.Add("Water boils at 100 Celsius degrees under standard conditions.");
        sentences.Add("Mount Everest is the tallest mountain on Earth.");
        sentences.Add("The Pacific Ocean is the largest ocean on Earth.");
        sentences.Add("A leap year has 366 days.");
        sentences.Add("The Great Wall of China is visible from space.");

        titles.Add("Type the phrase");
        titles.Add("Correct this sentence");
        titles.Add("Fact-check the statement");
        titles.Add("Validate the scientific claim");
        titles.Add("Analyze this information");
        titles.Add("Identify any inaccuracies");
        titles.Add("Rewrite the sentence correctly");
        titles.Add("Distinguish fact from fiction");

        // Disable input field until game starts
        playerInputField.interactable = false;
    }
    void Update()
    {
        comboTimer.text = comboTime.ToString();
        UpdateScoreUI();
    }
    void StartGame()
    {
        gameActive = true;
        gameTime = 180f; // Reset timer
        playerInputField.interactable = true;
        playerInputField.text = ""; // Clear input
        ShuffleSentences(); // Randomize order
        currentSentenceIndex = 0;
        UpdateTargetText();
        StartCoroutine(GameTimer());
    }

    void ShuffleSentences()
    {
        sentenceOrder.Clear();
        for (int i = 0; i < sentences.Count; i++)
        {
            sentenceOrder.Add(i);
        }
        for (int i = 0; i < sentenceOrder.Count; i++)
        {
            int temp = sentenceOrder[i];
            int randomIndex = Random.Range(i, sentenceOrder.Count);
            sentenceOrder[i] = sentenceOrder[randomIndex];
            sentenceOrder[randomIndex] = temp;
        }
    }

    void UpdateTargetText()
    {
        if (sentenceOrder.Count == 0)
        {
            ShuffleSentences(); // Reshuffle when all sentences are used
        }

        int sentenceIndex = sentenceOrder[currentSentenceIndex % sentences.Count];
        titleTextUI.text = titles[sentenceIndex % titles.Count];  
        currentTargetSentence = sentences[sentenceIndex];

        if (difficultyMode == 2) // Extreme Mode - Hide words
        {
            ApplyInkSplashes();
        }
        else
        {
            targetTextUI.text = currentTargetSentence;
        }
        phraseStartTime = Time.time; // Start phrase timer
    }
    void UpdateScoreUI()
{
    scoreText.text = "Score: " + score;
    multiplierText.text = "Multiplier: x" + pointMultiplier;
    comboText.text = "Combo: " + comboStreak;
}

public void OnPlayerInputChanged()
{
    if (!gameActive) return;

    string playerText = playerInputField.text;

    if (difficultyMode == 0) // Normal Mode
    {
        if (!currentTargetSentence.StartsWith(playerText))
        {
            playerInputField.text = playerText.Substring(0, playerText.Length - 1);
            playerInputField.caretPosition = playerInputField.text.Length;
            mistakeCount++; // Count mistake
            SFX.PlayOneShot(misclick);
        }
    }
    else if (difficultyMode == 1 || difficultyMode == 2) // Hard & Extreme Mode
    {
        if (!currentTargetSentence.StartsWith(playerText))
        {
            playerInputField.text = ""; 
            mistakeCount++; // Count mistake
            SFX.PlayOneShot(misclick);
        }
    }

    if (playerText == currentTargetSentence) // Phrase completed
    {
        if (mistakeCount == 0) // No mistakes - trigger "Perfect!"
        {
            ShowPerfectAnimation();
            gameTime += 5f; // Add 5 seconds
            SFX.PlayOneShot(YesSentence);
        }

        NextSentence();
    }
}


void NextSentence()
{
    comboTime = Time.time - phraseStartTime;
    
    if (comboTime < 10f) // Combo condition
    {
        comboStreak++;
        if (comboStreak % 5 == 0) // Every 5 combos, double multiplier
        {
            pointMultiplier *= 2;
        }
    }
    else
    {
        comboStreak = 0; // Reset combo
        pointMultiplier = 1;
    }

    score += 1 * pointMultiplier; // Apply multiplier
    UpdateScoreUI();

    currentSentenceIndex = (currentSentenceIndex + 1) % sentences.Count;
    playerInputField.text = ""; 
    mistakeCount = 0; // Reset mistakes
    UpdateTargetText();
}

    void ShowPerfectAnimation()
{
    perfectSprite.SetActive(true);
    StartCoroutine(HidePerfectAnimation());
}

IEnumerator HidePerfectAnimation()
{
    yield return new WaitForSeconds(0.5f); // Show for 1 second
    perfectSprite.SetActive(false);
}

    void EndGame()
    {
        gameActive = false;
        targetTextUI.text = "Time is up! You got " + score.ToString() + " points";
        playerInputField.interactable = false;
    }

    IEnumerator GameTimer()
    {
        while (gameTime > 0 && gameActive)
        {
            gameTime -= 1f;
            timerText.text = "Time Left: " + Mathf.FloorToInt(gameTime) + "s";
            yield return new WaitForSeconds(1f);
        }

        EndGame();
    }

    public void SetNormalMode()
    {
        difficultyMode = 0;
        inkSplashCount = 0;
        StartGame();
    }

    public void SetHardMode()
    {
        difficultyMode = 1;
        inkSplashCount = 0;
        StartGame();
    }

    public void SetExtremeMode()
    {
        difficultyMode = 2;
        inkSplashCount = 1;
        StartGame();
    }

    void ApplyInkSplashes()
    {
        string[] words = currentTargetSentence.Split(' ');
        List<int> hiddenIndices = new List<int>();

        while (hiddenIndices.Count < Mathf.Min(inkSplashCount, words.Length))
        {
            int randIndex = Random.Range(0, words.Length);
            if (!hiddenIndices.Contains(randIndex))
            {
                hiddenIndices.Add(randIndex);
            }
        }

        string obscuredText = "";
        for (int i = 0; i < words.Length; i++)
        {
            if (hiddenIndices.Contains(i))
            {
                obscuredText += "███ "; // Blackout effect
            }
            else
            {
                obscuredText += words[i] + " ";
            }
        }

        targetTextUI.text = obscuredText.Trim();
    }
}
