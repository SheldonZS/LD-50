using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    private DataBucket db;
    private DialogueBox diaBox;
    private Image endingImage;
    private AudioSource bgm;

    List<string> endStory;

    // Start is called before the first frame update
    void Awake()
    {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        endingImage = GameObject.Find("Background").GetComponent<Image>();
        bgm = GameObject.Find("BGM").GetComponent<AudioSource>();

    }

    private void Start()
    {
        bgm.Stop();

        List<string> endStory = new List<string>();

        db.data.ending = EndingCode.Jolie;
        switch (db.data.ending)
        {
            case EndingCode.Raol:
                break;
            case EndingCode.Bal:
                break;
            case EndingCode.Thob:
                break;
            case EndingCode.Jolie:
                endStory.Add("Reaper: Jolie...you fought me for so long. I came as soon as I heard. Yet even after your comrades fell, you persisted.");
                endStory.Add("Reaper: Why?");
                endStory.Add("Reaper: Jolie? Jolie, are you listening?");
                endStory.Add("startEnding");
                endStory.Add("Jolie: Zzzz...");
                endStory.Add("Thob: She gets like that sometimes.");
                endStory.Add("Raol: More like all the time. Anyway, what did you want us for?");
                endStory.Add("Reaper: I needed you all here so I could ask you to embark on a new quest.");
                endStory.Add("Balthasar: It appears my cognition that my old associate just wanted to talk to us was correct.");
                endStory.Add("Raol: Could have talked to us without killing us.");
                endStory.Add("Reaper: It's a quest that can only be done in the afterlife. And only the four of you are qualified. Would you please consider it?");
                endStory.Add("Thob: It depends...");
                endStory.Add("Reaper: Yes, there is treasure.");
                endStory.Add("Thob: Then sign me up!");
                endStory.Add("Balthasar: Post-mortal job recruitment. Finally, an adventure you all will accept as having truly taken place.");
                endStory.Add("Raol: Actually, there's someone I need to find first.");
                endStory.Add("Reaper: If you help me, I'll help you find them.");
                endStory.Add("Raol: Fine.");
                endStory.Add("Reaper: Then that just leaves...Jolie? Jolie?");
                endStory.Add("Balthasar: Jolie, your consciousness is requested.");
                endStory.Add("Jolie: Zz--what? What? Where am...are we seriously in a tavern? Again?");
                endStory.Add("Thob: The Reaper has a quest for us. There's riches and--");
                endStory.Add("Jolie: Nope. Good bye. Zzzzzz...");
                endStory.Add("Reaper: Well. I suppose we'll just...let her sleep. She's earned it.");
                endStory.Add("***");
                endStory.Add("Somehow, Jolie delayed eternal sleep for " + db.data.wavesSurvived + " waves.");
                break;
            default:
                break;
        }
        StartCoroutine(diaBox.PlayText(endStory, TextMode.imm));   
 
    }

    public IEnumerator RevealEnding()
    {
        //bgm.PlayOneShot("Resources/BGM/");
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            endingImage.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }


}
