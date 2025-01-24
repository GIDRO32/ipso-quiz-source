using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText;          // Question text UI
    public Text answerOptionsText;     // Answer options text UI
    public RectTransform buttonA;      // Button A RectTransform
    public RectTransform buttonB;      // Button B RectTransform
    public GameObject popupPanel;      // Popup panel for correct/incorrect feedback
    public Text popupText;             // Popup text
    public Text countResult;
    public Text explanation;
    public Text judgement;
    public GameObject resultPopup;     // Result popup for final results
    public GameObject ExtrasButton;
    public GameObject gameUI;
    public AudioSource effects;
    public AudioClip correctDing;
    public AudioClip incorrectDing;

    [Header("Game Settings")]
    public string[] questions;         // Array of questions
    public string[] answers;           // Array of answer options
    public string[] correctAnswers;    // Array of correct answers
    public string[] explanationArray;    
    public float fadeSpeed = 1f;       // Speed for fading in/out elements
    public float slideSpeed = 500f;    // Speed for sliding buttons
    public Vector2 buttonStartA;       // Start position for Button A
    public Vector2 buttonStartB;       // Start position for Button B
    public Vector2 buttonEndA;         // End position for Button A
    public Vector2 buttonEndB;         // End position for Button B
    private int correctAnswersCount = 0;

    private int currentQuestionIndex = 0;
    private bool isAnimating = false;
    public GameToggle toggler;
    

    private void Start()
    {
        ExtrasButton.SetActive(false);
        popupPanel.SetActive(false);
        resultPopup.SetActive(false);
        gameUI.SetActive(false);
        SetupQuestions();
    }

    public void StartGameFlow()
    {
        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        gameUI.SetActive(true);
        StartCoroutine(GameSequence());
    }
    public void SetupQuestions()
    {
        questions[0] = "Question 1: Which title is manipulative?";
        questions[1] = "Question 2: Which title tries to cause panic and fear?";
        questions[2] = "Question 3: Which title is fake and is not confirmed by sources?";
        questions[3] = "Question 4: Which title is potentially fake?";
        questions[4] = "Question 5: Which title might be IPSO?";
        questions[5] = "Question 6: Which title is demoralizing the population?";
        questions[6] = "Question 7: Which title attempts to draw attention with sensation?";
        questions[7] = "Question 8: Which title needs source verification due to exaggerations?";

        answers[0] = "A: A village school in Odessa region received a new computer class thanks to international assist \n\nB: A village school in Odessa region barely received one computer from international assist";
        answers[1] = "A: The wave of dangerous deceases will cover Kyiv. Doctors are forceless \n\nB: Kyiv has fixed an increase of seasonal ilnesses";
        answers[2] = "A: The government of European Union decided to fully cancel financial assist for Ukraine. \n\nB: Ukraine received 1 billion Euros of assist from EU";
        answers[3] = "A: An unknown virus was found in waters of Cherkasy, all swimming activities are restricted. \n\nB: Experts are warning about dangerous virus, that could be a threat to all ukrainins";
        answers[4] = "A: Ukrainian military received a new uniform from volunteers. \n\nB: Ukrainian government couldn't even provide basic uniform for military. Everything is on volunteers";
        answers[5] = "A: Ukrainian defenders are losing positions, but they are preparing the counter-offense. \n\nB: Frontline Disasters: Ukrainian defenders are losing positions, the situation is critical";
        answers[6] = "A: Scientists are shocking: new flu medicine causes irreversible effects \n\nB: Researchers created new cure for flu, which is passing final tests.";
        answers[7] = "A: A building, which is over 300 years old, was found in Kyiv \n\nB: A treasure, which overweights all known discoveries, was found in Kyiv.";

        explanationArray[0] = "Correct answer: B\nThe word \"barely\" is used in the title to cause negative mood";
        explanationArray[1] = "Correct answer: A\nExaggeration and Dramatization to cause panic";
        explanationArray[2] = "Correct answer: A\nExtreme claims without verification are often fake";
        explanationArray[3] = "Correct answer: B\n \"Threat to all\" is typical claim in fake news";
        explanationArray[4] = "Correct answer: B\n This title is used to break trust in the government.";
        explanationArray[5] = "Correct answer: B\n Mostly contains of negative context for ruining population's spirit.";
        explanationArray[6] = "Correct answer: A\n Words\"Shocking\" and \"Irreversible effects\" are the signs of fake news";
        explanationArray[7] = "Correct answer: B\n A huge claim without facts provided";
    }
    private IEnumerator GameSequence()
    {
        while (currentQuestionIndex < questions.Length)
        {
            // Step 1: Show the question
            yield return DisplayQuestionLetterByLetter(questions[currentQuestionIndex]);

            // Step 2: Show the answer options
            yield return FadeText(answerOptionsText, answers[currentQuestionIndex], 1f);

            // Step 3: Slide in buttons
            yield return SlideButtonsIn();

            // Step 4: Wait for player input
            yield return WaitForPlayerInput();

            yield return new WaitForSeconds(0.5f);

            // Step 5: Show popup feedback
            bool isCorrect = CheckAnswer(); // Check if the selected answer is correct
            if(isCorrect)
            {
                popupText.text = "Correct!";
                correctAnswersCount++;
                Debug.Log(correctAnswersCount + "/" + questions.Length);
                effects.PlayOneShot(correctDing);
            }
            else
            {
                Debug.Log(correctAnswersCount + "/" + questions.Length);
                popupText.text = "Incorrect!";
                effects.PlayOneShot(incorrectDing);
            }
            popupPanel.SetActive(true);
            explanation.text = explanationArray[currentQuestionIndex];
            yield return new WaitUntil(() => !popupPanel.activeSelf);

            // Step 6: Prepare for the next question
            currentQuestionIndex++;
        }

        // Step 8: Show the result popup
        CheckResults();
        resultPopup.SetActive(true);
    }
    private string selectedAnswer; // Stores the player's selected answer
    public void NextQuestion()
    {
        popupPanel.SetActive(false);
    }
    public void CloseGame()
    {
        resultPopup.SetActive(false);
        gameUI.SetActive(false);
    }
// Function called when a button is pressed
public void SendAnswer(string answer)
{
    selectedAnswer = answer; // Set the selected answer
}
public void CheckResults()
{
    countResult.text = "You gave " + correctAnswersCount.ToString() + "/" + questions.Length.ToString() + " Correct answers";
    if(correctAnswersCount == questions.Length)
    {
        ExtrasButton.SetActive(true);
        judgement.text = "PERFECT!";
        effects.PlayOneShot(correctDing);
    }
    else
    {
        judgement.text = "Try again!";
        effects.PlayOneShot(incorrectDing);
    }
}
    private IEnumerator DisplayQuestionLetterByLetter(string question)
    {
        questionText.text = "";
        foreach (char letter in question.ToCharArray())
        {
            questionText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator FadeText(Text text, string content, float targetAlpha)
    {
        text.text = content;
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

    private IEnumerator SlideButtonsIn()
    {
        isAnimating = true;

        // Reset buttons to start positions
        buttonA.anchoredPosition = buttonStartA;
        buttonB.anchoredPosition = buttonStartB;

        bool buttonADone = false;
        bool buttonBDone = false;

        while (!buttonADone || !buttonBDone)
        {
            buttonADone = SlideButton(buttonA, buttonEndA);
            buttonBDone = SlideButton(buttonB, buttonEndB);
            yield return null;
        }

        isAnimating = false;
    }

    private bool SlideButton(RectTransform button, Vector2 targetPosition)
    {
        button.anchoredPosition = Vector2.MoveTowards(
            button.anchoredPosition,
            targetPosition,
            slideSpeed * Time.deltaTime
        );
        return Vector2.Distance(button.anchoredPosition, targetPosition) < 0.01f;
    }

    private IEnumerator WaitForPlayerInput()
    {
        bool inputReceived = false;

        // Assuming buttons have listeners calling these methods:
        buttonA.GetComponent<Button>().onClick.AddListener(() => inputReceived = true);
        buttonB.GetComponent<Button>().onClick.AddListener(() => inputReceived = true);

        yield return new WaitUntil(() => inputReceived);

        // Clear listeners for the next question
        buttonA.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonB.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private bool CheckAnswer()
    {
        return selectedAnswer == correctAnswers[currentQuestionIndex];
    }
}

