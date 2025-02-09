using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameToggle : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText;          // Assign Question text here
    public Text answerOptionsText;     // Assign Answer Options text here
    public RectTransform buttonA;      // Assign Button A here
    public RectTransform buttonB;      // Assign Button B here
    public CanvasGroup gamePanel;
        public RectTransform slideBlock1; // Assign SlideBlock1 here
    public RectTransform slideBlock2;
    public Vector2 targetPosition1;  // Target position for SlideBlock1
    public Vector2 targetPosition2;  // Target position for SlideBlock2
    public Vector2 targetPosition3;  // Target position for SlideBlock1
    public Vector2 targetPosition4;  // Target position for SlideBlock2

    [Header("Animation Settings")]
    public float textSpeed = 0.05f;    // Speed for letter-by-letter display
    public float slideSpeed = 500f;    // Speed for button sliding
    public float fadeSpeed = 1.5f;       // Speed for fading
    public Vector2 buttonSlideLeftA;   // Target position for Button A (left slide)
    public Vector2 buttonSlideRightB;  // Target position for Button B (right slide)
    public Vector2 buttonSlideRightA;  // Target position for Button A (right slide)
    public Vector2 buttonSlideLeftB;   // Target position for Button B (left slide)
    private bool isAnimating = false;
    public GameFlow flow;
    public AudioSource musicPlayer;
    public AudioClip titleTheme;
    public AudioClip gameTheme;
    public AudioClip secretTune;
    public Sprite KethlandFlag;

    private Coroutine currentCoroutine; // For managing ongoing animations
    public void Start()
    {
        gamePanel.gameObject.SetActive(false);
        PlayMusic(titleTheme);
    }
    public void StartGameToggle()
    {
        PlayMusic(gameTheme);
        if (!isAnimating)
        {
            StartCoroutine(SmoothToggle());
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        if (musicPlayer != null && clip != null)
        {
            musicPlayer.clip = clip;  // Set the audio clip
            musicPlayer.Play();       // Play the audio
        }
    }
private IEnumerator SmoothToggle()
{
    isAnimating = true;

    // Activate the GamePanel and set initial alpha to 0
    gamePanel.gameObject.SetActive(true);
    gamePanel.alpha = 0;

    // Slide blocks first
    yield return SlideBlocksSequentially();

    // Then fade in the game panel
    yield return FadeInPanel();

    isAnimating = false;
    flow.StartGameFlow();
}

private IEnumerator SlideBlocksSequentially()
{
    bool block1Done = false;
    bool block2Done = false;

    while (!block1Done || !block2Done)
    {
        block1Done = SlideBlock(slideBlock1, targetPosition1);
        block2Done = SlideBlock(slideBlock2, targetPosition2);
        yield return null;
    }
}
public void BackToMenu()
{
    StartCoroutine(ToggleMenu());
    PlayMusic(titleTheme);
}
private IEnumerator ToggleMenu()
{
    bool block1Done = false;
    bool block2Done = false;

    while (!block1Done || !block2Done)
    {
        block1Done = SlideBlock(slideBlock1, targetPosition3);
        block2Done = SlideBlock(slideBlock2, targetPosition4);
        yield return null;
    }
}
private IEnumerator FadeInPanel()
{
    while (Mathf.Abs(gamePanel.alpha - 1f) > 0.01f)
    {
        gamePanel.alpha = Mathf.MoveTowards(gamePanel.alpha, 1f, fadeSpeed * Time.deltaTime);
        yield return null;
    }

    gamePanel.alpha = 1f;
}

private bool SlideBlock(RectTransform block, Vector2 targetPosition)
{
    block.anchoredPosition = Vector2.MoveTowards(
        block.anchoredPosition,
        targetPosition,
        slideSpeed * Time.deltaTime
    );

    return Vector2.Distance(block.anchoredPosition, targetPosition) < 0.01f;
}


    public void DisplayQuestionLetterByLetter(string question)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(TypeText(question));
    }

    public void HideQuestionLetterByLetter()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(DeleteText());
    }

    public void SlideButtonsLeftRight()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        StartCoroutine(SlideButton(buttonA, buttonSlideLeftA));
        StartCoroutine(SlideButton(buttonB, buttonSlideRightB));
    }

    public void SlideButtonsRightLeft()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        StartCoroutine(SlideButton(buttonA, buttonSlideRightA));
        StartCoroutine(SlideButton(buttonB, buttonSlideLeftB));
    }

    public void FadeInAnswerOptions()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeText(answerOptionsText, 1f));
    }

    public void FadeOutAnswerOptions()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeText(answerOptionsText, 0f));
    }

    private IEnumerator TypeText(string text)
    {
        questionText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            questionText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private IEnumerator DeleteText()
    {
        while (questionText.text.Length > 0)
        {
            questionText.text = questionText.text.Substring(0, questionText.text.Length - 1);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private IEnumerator SlideButton(RectTransform button, Vector2 targetPosition)
    {
        while (Vector2.Distance(button.anchoredPosition, targetPosition) > 0.01f)
        {
            button.anchoredPosition = Vector2.MoveTowards(
                button.anchoredPosition,
                targetPosition,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    private IEnumerator FadeText(Text text, float targetAlpha)
    {
        Color color = text.color;
        while (Mathf.Abs(color.a - targetAlpha) > 0.01f)
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            text.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        text.color = color;
    }
    public void PanelFadeIn(GameObject panel)
    {
        StartCoroutine(FadePanel(panel, 1f)); // Fade to alpha = 1 (fully visible)
    }

    // Function to fade out a UI panel (or any GameObject with a CanvasGroup)
    public void PanelFadeOut(GameObject panel)
    {
        StartCoroutine(FadePanel(panel, 0f)); // Fade to alpha = 0 (fully invisible)
    }

    // Coroutine to handle fading
    private IEnumerator FadePanel(GameObject panel, float targetAlpha)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("GameObject does not have a CanvasGroup component!");
            yield break;
        }

        // Enable the panel if fading in
        if (targetAlpha > 0)
        {
            panel.SetActive(true);
        }

        // Gradually adjust alpha
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(
                canvasGroup.alpha,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Disable the panel if fading out
        if (targetAlpha == 0)
        {
            panel.SetActive(false);
        }
    }

    // Function to quit the game and go to desktop
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
public void ChangeImage(GameObject target)
    {
        Image imageComponent = target.GetComponent<Image>();
        
        if (imageComponent != null)
        {
            imageComponent.sprite = KethlandFlag; // Change the sprite
            PlayMusic(secretTune);
        }
        else
        {
            Debug.LogError("GameObject does not have an Image component!");
        }
    }
}
