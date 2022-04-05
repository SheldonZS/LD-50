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
    private Text instructions;

    List<string> endStory_R;
    List<string> endStory_B;
    List<string> endStory_T;
    List<string> endStory_J;

    // Start is called before the first frame update
    void Awake()
    {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        endingImage = GameObject.Find("Background").GetComponent<Image>();
        bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        instructions = GameObject.Find("EndingInstructions").GetComponent<Text>();

    }

    private void Start()
    {
        bgm.Stop();

        endStory_R = new List<string>();
        endStory_B = new List<string>();
        endStory_T = new List<string>();
        endStory_J = new List<string>();

        InitializeEndings();


        PlayEnding();
    }

    void PlayEnding()
    {
        switch (db.data.ending)
        {
            case EndingCode.Raol:
                instructions.text = "";
                StartCoroutine(diaBox.PlayText(endStory_R, TextMode.imm));
                break;
            case EndingCode.Bal:
                instructions.text = "";
                StartCoroutine(diaBox.PlayText(endStory_B, TextMode.imm));

                break;
            case EndingCode.Thob:
                instructions.text = "";
                StartCoroutine(diaBox.PlayText(endStory_T, TextMode.imm));
                break;
            case EndingCode.Jolie:
                instructions.text = "";
                StartCoroutine(diaBox.PlayText(endStory_J, TextMode.imm));

                break;
            default:
                instructions.text = "Select a character to view their ending.";

                break;
        }
    }

    public void OnClickRaol()
    {
        foreach (Text go in diaBox.textBoxes)
            Destroy(go.gameObject);
        diaBox.ClearAllStories();
        diaBox.textBoxes.Clear();

        if (db.raolUnlocked)
        {
            db.data.ending = EndingCode.Raol;
            foreach (Text go in diaBox.textBoxes)
                Destroy(go.gameObject);
            diaBox.ClearAllStories();
            diaBox.textBoxes.Clear();
            PlayEnding();
        }
        else
        {
            instructions.text = "To unlock this ending, Raol must be the last hero to delay the inevitable.";
        }
    }

    public void OnClickThob()
    {

        foreach (Text go in diaBox.textBoxes)
            Destroy(go.gameObject);
        diaBox.ClearAllStories();
        diaBox.textBoxes.Clear();

        if (db.thobUnlocked)
        {
            db.data.ending = EndingCode.Thob;
            foreach (Text go in diaBox.textBoxes)
                Destroy(go.gameObject);
            diaBox.ClearAllStories();
            diaBox.textBoxes.Clear();
            PlayEnding();
        }
        else
        {
            instructions.text = "To unlock this ending, Thob must be the last hero to delay the inevitable.";
        }
    }

    public void OnClickBalthasar()
    {
        foreach (Text go in diaBox.textBoxes)
            Destroy(go.gameObject);
        diaBox.ClearAllStories();
        diaBox.textBoxes.Clear();

        if (db.balUnlocked)
        {
            db.data.ending = EndingCode.Bal;
            foreach (Text go in diaBox.textBoxes)
                Destroy(go.gameObject);
            diaBox.ClearAllStories();
            diaBox.textBoxes.Clear();
            PlayEnding();
        }
        else
        {
            instructions.text = "To unlock this ending, Balthasar must be the last hero to delay the inevitable.";
        }
    }

    public void OnClickJolie()
    {
        foreach (Text go in diaBox.textBoxes)
            Destroy(go.gameObject);
        diaBox.ClearAllStories();
        diaBox.textBoxes.Clear();

        if (db.jolieUnlocked)
        {
            db.data.ending = EndingCode.Jolie;
            foreach (Text go in diaBox.textBoxes)
                Destroy(go.gameObject);
            diaBox.ClearAllStories();
            diaBox.textBoxes.Clear();
            PlayEnding();
        }
        else
        {
            instructions.text = "To unlock this ending, Jolie must be the last hero to delay the inevitable.";
        }
    }
    public IEnumerator RevealEnding()
    {
        bgm.PlayOneShot(Resources.Load<AudioClip>("BGM/#50_GameOver"));
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            endingImage.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    void InitializeEndings()
    {
        endStory_R.Add("Reaper: Raol...you've prayed so long to Mother Moon.");
        endStory_R.Add("Reaper: Did she ever answer your howl?");
        endStory_R.Add("startEnding");
        endStory_R.Add("Reaper: If not, I'll put in a good word for you, and she owes me a favor.");
        endStory_R.Add("Raol: ...why are you helping me?");
        endStory_R.Add("Reaper: Look, I need a bit of help myself. That's why I summoned you all here, to ask you to embark on a new quest.");
        endStory_R.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us was not unwarranted.");
        endStory_R.Add("Reaper: It's a quest that can only be done in the afterlife. And only the four of you are qualified. Would you please consider it?");
        endStory_R.Add("Thob: It depends...");
        endStory_R.Add("Reaper: Yes, there is treasure.");
        endStory_R.Add("Thob: Then sign me up!");
        endStory_R.Add("Balthasar: Post-mortal job recruitment. Finally, an adventure you all will accept as having truly taken place.");
        endStory_R.Add("Reaper: What bout Jolie? Jolie?");
        endStory_R.Add("Jolie: Zzz...no...");
        endStory_R.Add("Reaper: She'll come around. Raol?");
        endStory_R.Add("Raol: First, you help me find Kol Kol.");
        endStory_R.Add("Reaper: Kol Kol the kenku necromancer, yes?");
        endStory_R.Add("Thob: So that's the Kol Kol you were going on about.");
        endStory_R.Add("Balthasar: Wait, the one you seek is a practitioner of black magic? Are you truly unequivocal in your desire to seek them?");
        endStory_R.Add("Raol: Kol Kol used to give me a lot of headaches, but they cleaned up my messes too, and...");
        endStory_R.Add("Thob: You can do it, Raol. Say the words. Kol Kol is a \"friend.\"");
        endStory_R.Add("Raol: ...Kol Kol was there for me.");
        endStory_R.Add("Thob: As a friend.");
        endStory_R.Add("Raol: ...");
        endStory_R.Add("Thob: Come now, surely you believe alpha males can have friends.");
        endStory_R.Add("Balthasar: Actually, the concept of alpha males is an erroneous concept created after observing wolves in unnatural--");
        endStory_R.Add("Raol: Let's go. We're done here.");
        endStory_R.Add("***");
        endStory_R.Add("Raol delayed seeking Kol Kol for " + db.data.wavesSurvived + " waves.");

        endStory_B.Add("Reaper: Well well, Balthasar. It's been a while.");
        endStory_B.Add("Reaper: We have so much to catch up on.");
        endStory_B.Add("Reaper: The way you fought, I was starting to think you never wanted to see me again.");
        endStory_B.Add("Reaper: Is that the case?");
        endStory_B.Add("startEnding");
        endStory_B.Add("Balthasar: I beg to differ. I was merely still preoccupied with wrapping up the affairs of the living.");
        endStory_B.Add("Reaper: I'd forgotten how...loquacious you can be.");
        endStory_B.Add("Thob: Then you really haven't seen each other in a while.");
        endStory_B.Add("Jolie: Zzz...");
        endStory_B.Add("Balthasar: To matters at hand. You intend to offer us a new quest, correct?");
        endStory_B.Add("Reaper: How did you know?");
        endStory_B.Add("Balthasar: It was a logical conclusion to draw from the premise. Why else would you bring such considerable resources to bear to bring four adventurers to the afterlife at the same time, except to satisfy a need for an experienced adventuring party?");
        endStory_B.Add("Reaper: Yup. Yeah, that's about right.");
        endStory_B.Add("Balthasar: Then I accept. The afterlife too has its share of oddities and secrets I intend to unearth.");
        endStory_B.Add("Raol: Unless you explode them first. But yeah, I'll join you...I'm searching for something too.");
        endStory_B.Add("Thob: And I as well! Afterlife treasure must be even shinier, right?");
        endStory_B.Add("Balthasar: Your premonitions, while baseless, are serendipitously correct.");
        endStory_B.Add("Thob: What about Jolie?");
        endStory_B.Add("Balthasar: She is likely disinclined. If we let her slumber for the nonce, she may be more likely to accept the offer when she wakes, however reluctantly.");
        endStory_B.Add("Reaper: Thanks, Balthasar. I don't know what I'd do without you.");
        endStory_B.Add("Balthasar: The pleasure is mine, old friend.");
        endStory_B.Add("***");
        endStory_B.Add("Balthasar delayed his next adventure for " + db.data.wavesSurvived + " waves.");

        endStory_T.Add("Reaper: Thob, you have spent your entire life grasping in greed.");
        endStory_T.Add("Reaper: For jewels. For drinks. For extra minutes before your final breath.");
        endStory_T.Add("Reaper: Now that I have you in my grasp, I can only ask...");
        endStory_T.Add("startEnding");
        endStory_T.Add("Reaper: Would you like to put that greed to use by working for me?");
        endStory_T.Add("Thob: What? Is this a job offer?");
        endStory_T.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us was not unwarranted.");
        endStory_T.Add("Reaper: Yes. It's a quest that can only be done in the afterlife. And only the four of you are qualified. You have no idea how hard it is to find someone skilled in masonry, thievery, and musical composition who can also fit into tiny spaces.");
        endStory_T.Add("Jolie: Zzz...");
        endStory_T.Add("Thob: That reminds me...I wanted to return some things that I stole...");
        endStory_T.Add("Reaper: Your inventory remains with your physical body.");
        endStory_T.Add("Balthasar: Fret not, Thob. I choose to perceive this offer as an opportunity to obtain new oddities...which you may then immediately use to satisfy your kleptomaniac tendencies.");
        endStory_T.Add("Thob: Thanks!");
        endStory_T.Add("Raol: I will not be joining you. There's someone I need to find.");
        endStory_T.Add("Thob: Why not tell us who it is? We can help you search. You don't need to do everything alone.");
        endStory_T.Add("Raol: ...I'll think about it.");
        endStory_T.Add("Jolie: Zzz...");
        endStory_T.Add("Thob: I don't have the heart to wake her up and ask if she wants to join us.");
        endStory_T.Add("Reaper: I nearly forgot. She may join you when she learns how important this quest is to you.");
        endStory_T.Add("Thob: Me, personally?");
        endStory_T.Add("Reaper: Let me tell you about the kinds of trees that grow in hell...");
        endStory_T.Add("***");
        endStory_T.Add("Thob delayed fighting trees that actually need to die for " + db.data.wavesSurvived + " waves.");


        endStory_J.Add("Reaper: Jolie...you fought me for so long. I came as soon as I heard. Yet even as your comrades fell one by one, you persisted.");
        endStory_J.Add("Reaper: Why?");
        endStory_J.Add("Reaper: Jolie? Jolie, are you listening?");
        endStory_J.Add("startEnding");
        endStory_J.Add("Jolie: Zzzz...");
        endStory_J.Add("Thob: She gets like that sometimes.");
        endStory_J.Add("Raol: More like all the time. Anyway, what did you want us for?");
        endStory_J.Add("Reaper: I needed you all here so I could ask you to embark on a new quest.");
        endStory_J.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us was not unwarranted.");
        endStory_J.Add("Raol: Could have talked to us without killing us.");
        endStory_J.Add("Reaper: It's a quest that can only be done in the afterlife. And only the four of you are qualified. Would you consider it?");
        endStory_J.Add("Thob: It depends...");
        endStory_J.Add("Reaper: Yes, there is treasure.");
        endStory_J.Add("Thob: Then sign me up!");
        endStory_J.Add("Balthasar: Post-mortal job recruitment. Finally, an adventure you all will accept as having truly taken place.");
        endStory_J.Add("Raol: Actually, there's someone I need to find first.");
        endStory_J.Add("Reaper: If you help me, I'll help you find them.");
        endStory_J.Add("Raol: Fine.");
        endStory_J.Add("Reaper: Then that just leaves...Jolie? Jolie?");
        endStory_J.Add("Balthasar: Jolie, your consciousness is requested.");
        endStory_J.Add("Jolie: Zz--what? What? Where am...are we seriously in a tavern? Again?");
        endStory_J.Add("Thob: The Reaper has a quest for us. There's riches and--");
        endStory_J.Add("Jolie: Nope. Good bye. Zzzzzz...");
        endStory_J.Add("Reaper: Well. I can find someone else. I suppose we'll just...let her sleep. She's earned it.");
        endStory_J.Add("***");
        endStory_J.Add("Somehow, Jolie delayed eternal sleep for " + db.data.wavesSurvived + " waves.");
    } 


}
