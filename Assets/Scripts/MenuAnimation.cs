using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuAnimation : MonoBehaviour
{
    public RectTransform image; // Assign your image UI element here
    public Text titleText;      // Assign the first text child here
    public Text dateText;       // Assign the second text child here
    public Text terminology;
    public string[] titleStrings; // Array of strings for the title
    public string[] paragraphs;
    public float speed = 100f;  // Speed of movement
    public float resetPositionY = 1200f;  // Y position where it resets
    public float teleportPositionY = -1100f;
    public float startPositionY = 100f; // Starting Y position
    private int titleIndex;
    private int paragraphIndex;

    private void Start()
    {
        SetupDecs();
        if (titleStrings.Length > 0)
        {
            UpdateTitleText();
            UpdateDateText();
        }
    }

    private void Update()
    {
        MoveImage();
    }
    public void SetupDecs()
    {
        paragraphs[0]=("Disinformation is misleading content deliberately spread to deceive people, or to secure economic or political gain and which may cause public harm.Disinformation is an orchestrated adversarial activity in which actors employ strategic deceptions and media manipulation tactics to advance political, military, or commercial goals. Disinformation is implemented through attacks that weaponize multiple rhetorical strategies and forms of knowing—including not only falsehoods but also truths, half-truths, and value judgements—to exploit and amplify culture wars and other identity-driven controversies.");
        paragraphs[1]=("The word \"fake\" refers to something that is not genuine or authentic, often created to deceive or mislead others intentionally. It can apply to physical objects, like counterfeit goods, or intangible concepts, such as fabricated stories, fake news, or false claims. In the context of media and communication, fake content is often used to distort reality, create confusion, or manipulate public perception. The rise of digital platforms has made it easier for fake information to spread rapidly, making it a powerful tool for misinformation and deceit. Combating fake content often requires vigilance, critical thinking, and verification of sources.");
        paragraphs[2]=("\"Manipulation\" refers to the deliberate and often covert influence of someone's thoughts, emotions, or actions for a specific purpose, typically benefiting the manipulator. It involves exploiting vulnerabilities or using deceptive techniques to achieve control or gain. In the context of communication and propaganda, manipulation often relies on emotional appeals, selective presentation of facts, or outright falsehoods to sway opinions or behaviors. While manipulation can be subtle, its effects can be profound, undermining trust, distorting reality, and influencing decisions without the target's awareness. Understanding manipulation is crucial to recognizing and resisting attempts to influence unfairly or unethically.");
        paragraphs[3]=("IPSO, or Informational-Psychological Operation, refers to the strategic use of information and psychological tactics to influence the perceptions, emotions, and decisions of a target audience. These operations aim to destabilize, manipulate, or control groups or individuals, often by exploiting existing fears, divisions, or biases. IPSO campaigns can include the dissemination of misleading or false information, psychological pressure, and the use of media to amplify narratives that serve specific goals. They are often employed in military, political, or sociocultural contexts to weaken opponents, gain support, or achieve strategic objectives. The psychological element distinguishes IPSO, focusing on shaping how people think and feel rather than just what they know.");
        paragraphs[4]=("The term \"negative\" in this context relates to harmful or adverse narratives that are designed to provoke fear, anger, or distrust. Negative messaging often focuses on highlighting problems, faults, or threats while downplaying or ignoring positive aspects. It is a common tactic in propaganda and misinformation campaigns, as negative emotions tend to be more impactful and memorable than positive ones. By emphasizing negativity, these efforts can erode trust in institutions, deepen divisions, or discredit individuals or groups. The focus on negative content underscores the psychological impact of fear and doubt in shaping public perception.");
        paragraphs[5]=("\"Propaganda\" is the systematic dissemination of information, often biased or misleading, aimed at promoting a particular political, social, or ideological agenda. It is designed to influence public opinion and behavior, frequently by appealing to emotions rather than facts. Historically, propaganda has been used by governments, organizations, and individuals to rally support, justify actions, or discredit opponents. While propaganda can sometimes have positive uses, such as public health campaigns, it is often associated with manipulation and control. Its effectiveness lies in its ability to shape narratives and persuade audiences on a large scale.");
        paragraphs[6]=("\"Propaganda\" is the systematic dissemination of information, often biased or misleading, aimed at promoting a particular political, social, or ideological agenda. It is designed to influence public opinion and behavior, frequently by appealing to emotions rather than facts. Historically, propaganda has been used by governments, organizations, and individuals to rally support, justify actions, or discredit opponents. While propaganda can sometimes have positive uses, such as public health campaigns, it is often associated with manipulation and control. Its effectiveness lies in its ability to shape narratives and persuade audiences on a large scale.");
    }
    private void MoveImage()
    {
        // Move the image upward
        image.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // Check if the image has reached the reset position
        if (image.anchoredPosition.y >= resetPositionY)
        {
            // Teleport to the starting position
            image.anchoredPosition = new Vector2(image.anchoredPosition.x, teleportPositionY);

            // Change the title text
            UpdateTitleText();
            UpdateDateText();
        }
    }
    private void UpdateTitleText()
    {
        titleIndex = Random.Range(0, titleStrings.Length);
        titleText.text = titleStrings[titleIndex];
        paragraphIndex = titleIndex;
        terminology.text = paragraphs[paragraphIndex];
    }
    public void Gao()
    {
        titleStrings[6] = "Who is Gao?";
        paragraphs[6] = "We don't really know what is this and where it came from. GAO is the word that this masked entity in white cape always growls with devilish distorted voice. That's all we know and that's why we call it GAO. If you have any additional information about origins or purpose of this guy, please contact us via this email: pambossable@gmail.com. We already had one letter, but all they have provided is some kind of flag with purple, black and orange colors and white bold icon. Yeah, Thank you for giving us even more mysteries, ot it was just a spam. For now, try to avoid people with similar appearance, because that might be him, or her, or it, nobody knows.";
    }
    private void UpdateDateText()
    {
        int year = Random.Range(2000, 3001); // Random year between 2000 and 3000
        int month = Random.Range(1, 13);    // Random month between 1 and 12

        // Determine the maximum number of days in the selected month
        int maxDays = 31;
        if (month == 2)
        {
            maxDays = 28;
            if (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0))
            {
                maxDays = 29; // Leap year
            }
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            maxDays = 30;
        }

        int day = Random.Range(1, maxDays + 1); // Random day based on maxDays

        // Format and assign the date
        dateText.text = $"{day:00}/{month:00}/{year}";
    }
}
