using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private Image background;
    private Image textMask;
    private Text testText;

    public Color normalText, choiceText, highlightedChoiceText, selectedChoiceText, disabledChoiceText;
    public int margins = 5;
    private float maxWidth;
    private float minY;
    private float lineHeight;
    private float spaceWidth;

    private GameObject FFButton;
    private GameObject slowButton;

    //private ChoiceItem selection;

    public float charactersPerSecond = 50;
    public float scrollTime = .2f;
    public float autoPauseatLineEndTime = 1f;

    private List<Text> textBoxes;

    public bool displayingText;


    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        textMask = GameObject.Find("TextMask").GetComponent<Image>();
        testText = GameObject.Find("TestText").GetComponent<Text>();
        textBoxes = new List<Text>();

        FFButton = GameObject.Find("Fast Forward");
        slowButton = GameObject.Find("Slow");

        testText.color = normalText;
        testText.text = " ";
        lineHeight = testText.preferredHeight;
        spaceWidth = testText.preferredWidth;

        maxWidth = GetComponent<RectTransform>().sizeDelta.x - margins * 2;
        //minY = -transform.parent.GetComponent<RectTransform>().sizeDelta.y - GetComponent<RectTransform>().sizeDelta.y + margins + lineHeight;
        minY = -GetComponent<RectTransform>().sizeDelta.y + margins + lineHeight;
        Debug.Log("Min Y: " + minY);

        background.enabled = false;
        textMask.enabled = false;



    }

    public IEnumerator OpenWindow()
    {
        while (background == null)
        {
            yield return null;
        }

        background.enabled = true;
        textMask.enabled = true;
        FFButton.SetActive(true);
        slowButton.SetActive(true);

        yield return null;
    }

    public IEnumerator CloseWindow()
    {
        if (background.enabled == false)
            yield break;

        while (!Input.GetMouseButtonDown(0))
            yield return null;

        foreach (Text go in textBoxes)
            Destroy(go.gameObject);

        textBoxes.Clear();
        background.enabled = false;
        textMask.enabled = false;
        FFButton.SetActive(false);
        slowButton.SetActive(false);

        yield return null;
    }

    public IEnumerator PlayText(List<string> story, bool auto)
    {
        string[] words;

        for (int i = 0; i < story.Count; i++)
        {
            words = story[i].Split(' ');

            displayingText = true;

            if (words.Length <= 0 || words[0].TrimStart(null) == "")
                yield break;

            yield return OpenWindow();

            float currentLineWidth = margins;
            float currentLineY = -margins;
            Text currentTextField = null;
            int cumulativeCharacters = 0;
            int index = 0;
            FontStyle currentStyle = FontStyle.Normal;

            if (textBoxes.Count > 0)
                currentLineY = textBoxes[textBoxes.Count - 1].GetComponent<RectTransform>().localPosition.y - lineHeight;

            Debug.Log("is " + currentLineY + " less than " + minY);

            if (currentLineY < minY)
            {
                yield return ScrollText();
                currentLineY += lineHeight;
            }

            float startTime = Time.time;

            testText.text = "";
            currentTextField = Instantiate(testText.gameObject, testText.transform.parent).GetComponent<Text>();
            textBoxes.Add(currentTextField);
            currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);



            while (index < words.Length)
            {
                //change color depending on speaker
                if (words[index].Contains(":"))
                {
                    switch (words[index].Split(':')[0])
                    {
                        case "Raol":
                            currentTextField.color = new Color(219f / 255f, 133f / 255f, 30f / 255f, 255f);
                            break;
                        case "Balthasar":
                            currentTextField.color = new Color(105f / 255f, 165f / 255f, 209f / 255f, 255f);
                            break;
                        case "Thob":
                            currentTextField.color = new Color(148f / 255f, 73f / 255f, 191f / 255f, 255f);
                            break;
                        case "Jolie":
                            currentTextField.color = new Color(179f / 255f, 40f / 255f, 40f / 255f, 255f);
                            break;
                        default:
                            break;
                    }
                    /*
                    currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);
                    textBoxes.Add(currentTextField);
                    
                    currentLineWidth = currentTextField.preferredWidth + margins;
                    currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);

                    currentTextField = null;
                    currentStyle = FontStyle.Normal;
                    index++;
                    */
                }

                if (words[index] == "/i" || words[index] == "/n")
                {
                    if (words[index] == "/i") currentStyle = FontStyle.Italic;
                    else currentStyle = FontStyle.Normal;

                    if (currentTextField != null)
                    {
                        currentLineWidth += currentTextField.rectTransform.sizeDelta.x;
                        cumulativeCharacters += currentTextField.text.Length;
                        currentTextField = null;
                    }
                }
                else
                {
                    if (currentTextField == null)
                    {
                        testText.text = "";
                        currentTextField = Instantiate(testText.gameObject, testText.transform.parent).GetComponent<Text>();
                        currentTextField.fontStyle = currentStyle;
                        textBoxes.Add(currentTextField);

                        if (currentLineWidth == margins)
                        {
                            currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);
                            currentTextField.text = words[index];
                            currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);
                        }
                        else
                        {
                            testText.text = " " + words[index];
                            if (currentLineWidth + testText.preferredWidth <= maxWidth)
                            {
                                currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);
                                currentTextField.text += " " + words[index];
                                currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);
                            }
                            else
                            {
                                currentLineY -= lineHeight;
                                currentLineWidth = margins;
                                cumulativeCharacters += currentTextField.text.Length;

                                if (currentLineY < minY)
                                {
                                    yield return ScrollText();
                                    currentLineY += lineHeight;
                                }

                                startTime = Time.time;
                                cumulativeCharacters = 0;

                                currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);
                                currentTextField.text = words[index];
                                currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);
                            }
                        }
                    }
                    else
                    {
                        testText.text = currentTextField.text + " " + words[index];
                        if (currentTextField.text == "")
                        {
                            currentTextField.text = words[index];
                            currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);

                        }
                        else if (currentLineWidth + testText.preferredWidth <= maxWidth)
                        {
                            currentTextField.text += " " + words[index];
                            currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);
                        }
                        //in instance that text is longer than the textbox length, automatically creates new line with the remainder of the text
                        else
                        {
                            currentLineY -= lineHeight;
                            currentLineWidth = margins;
                            cumulativeCharacters += currentTextField.text.Length;

                            if (currentLineY < minY)
                            {
                                Debug.Log("Scroll text played");
                                yield return ScrollText();
                                currentLineY += lineHeight;
                            }

                            startTime = Time.time;
                            cumulativeCharacters = 0;

                            testText.text = "";

                            Color colorStash = currentTextField.color;

                            currentTextField = Instantiate(testText.gameObject, testText.transform.parent).GetComponent<Text>();
                            currentTextField.fontStyle = currentStyle;
                            currentTextField.color = colorStash;
                            textBoxes.Add(currentTextField);

                            currentTextField.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);
                            currentTextField.text = words[index];
                            currentTextField.rectTransform.sizeDelta = new Vector2(currentTextField.preferredWidth, lineHeight);
                        }
                    }

                    string fullText = currentTextField.text;
                    int shownCharacters = (int)Mathf.Clamp(((Time.time - startTime) * charactersPerSecond) - cumulativeCharacters, 0, fullText.Length);

                    while (shownCharacters < fullText.Length && Input.GetMouseButtonDown(0) == false)
                    {
                        currentTextField.text = fullText.Substring(0, shownCharacters);
                        yield return null;
                        shownCharacters = (int)Mathf.Clamp(((Time.time - startTime) * charactersPerSecond) - cumulativeCharacters, 0, fullText.Length);
                    }
                    currentTextField.text = fullText;
                }
                index++;
            }

            //at the end of a line of dialogue
            if (auto)
            {
                yield return new WaitForSeconds(autoPauseatLineEndTime);
            }

        }//displaying multiple lines of dialogue
        
        displayingText = false;

        yield return null;

    }

    public IEnumerator ScrollText()
    {
        float startTime = Time.time;
        float totalScrolled = 0;
        float scrollAmount;

        while (Time.time < startTime + scrollTime && Input.GetMouseButtonDown(0) == false)
        {
            scrollAmount = lineHeight * Time.deltaTime / scrollTime;
            totalScrolled += scrollAmount;

            foreach (Text box in textBoxes)
            {
                Vector3 temp = box.rectTransform.localPosition;
                if (box.fontStyle == FontStyle.Bold)
                    temp.y += scrollAmount/2;
                else temp.y += scrollAmount;
                box.rectTransform.localPosition = temp;
            }
            yield return null;
        }

        scrollAmount = lineHeight - totalScrolled;

        foreach (Text box in textBoxes)
        {
            Vector3 temp = box.rectTransform.localPosition;
            if (box.fontStyle == FontStyle.Bold)
                temp.y += scrollAmount / 2;
            else temp.y += scrollAmount;
            box.rectTransform.localPosition = temp;
        }
    }
    /*
    public IEnumerator WaitForChoice(List<ChoiceItem> choices)
    {
        testText.rectTransform.sizeDelta = new Vector2(maxWidth, lineHeight);

        float currentLineY;

        if (textBoxes.Count > 0)
            currentLineY = textBoxes[textBoxes.Count - 1].GetComponent<RectTransform>().localPosition.y - lineHeight;
        else currentLineY = -margins;

        for (int i = 0;i < choices.Count;i++)
        {
            testText.text = choices[i].prompt;

            Text choiceWindow = Instantiate(testText, testText.transform.parent).GetComponent<Text>();
            textBoxes.Add(choiceWindow);
            Button b = choiceWindow.gameObject.AddComponent<Button>();

            switch (i)
            {
                case 0: b.onClick.AddListener(() => OnClick(choices[0])); break;
                case 1: b.onClick.AddListener(() => OnClick(choices[1])); break;
                case 2: b.onClick.AddListener(() => OnClick(choices[2])); break;
                case 3: b.onClick.AddListener(() => OnClick(choices[2])); break;
            }

            b.interactable = choices[i].valid;
            choiceWindow.color = Color.white;

            ColorBlock colors = b.colors;
            colors.normalColor = choiceText;
            colors.highlightedColor = highlightedChoiceText;
            colors.selectedColor = selectedChoiceText;
            colors.disabledColor = disabledChoiceText;
            b.colors = colors;

            choiceWindow.rectTransform.sizeDelta = new Vector2(maxWidth, choiceWindow.preferredHeight);
            choiceWindow.rectTransform.localPosition = new Vector3(margins, currentLineY, 0);


            while (currentLineY - choiceWindow.preferredHeight  + lineHeight < minY)
            {
                yield return ScrollText();
                currentLineY += lineHeight;
            }
            currentLineY -= choiceWindow.preferredHeight;
        }

        ChoiceItem waiting = new ChoiceItem("waiting", "waiting", false);
        selection = waiting;
        while (selection.prompt == "waiting")
        {
            yield return null;
        }
        //Debug.Log("selected: " + selection);

        while(textBoxes.Count > 0 && textBoxes[textBoxes.Count - 1].GetComponent<Button>() != null)
        {
            Text temp = textBoxes[textBoxes.Count - 1];
            textBoxes.Remove(temp);
            Destroy(temp);
        }
    }

    public ChoiceItem GetChoice()
    {
        return selection;
    }

    public void OnClick(ChoiceItem value)
    {
        selection = value;
    }
    */
    public void speedUp()
    {
        charactersPerSecond += 10;
        scrollTime = 5f / charactersPerSecond;
    }

    public void speedDown()
    {
        charactersPerSecond -= 10;
        if (charactersPerSecond < 10)
            charactersPerSecond = 10;
        scrollTime = 5f / charactersPerSecond;
    }

}