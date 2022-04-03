using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    private DialogueBox diaBox;

    List<string> testWords = new List<string>();

    void Awake()
    {
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeAllStory();
        StartCoroutine(diaBox.PlayText(testWords, true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void InitializeAllStory()
    {
        testWords.Add("Raol: Stop");
        testWords.Add("Balthasar: Yeah no");
        testWords.Add("Jolie: Zzzz and let's just add a lot of text to fill up one or three or even nine lines and yeah, we can keep going all day, if that's what's required.");
        testWords.Add("Thob: Thought!");
        testWords.Add("Jolie: What? Huh?");
    }
}
