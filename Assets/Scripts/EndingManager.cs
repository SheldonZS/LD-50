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
                endStory.Add("Reaper: Raol...you've prayed so long to Mother Moon.");
                endStory.Add("Reaper: Did she ever answer your howl?");
                endStory.Add("startEnding");
                endStory.Add("Reaper: If not, I'll put in a good word for you, and she owes me a favor.");
                endStory.Add("Raol: ...why are you helping me?");
                endStory.Add("Reaper: Look, I need a bit of help myself. That's why I summoned you all here, to ask you to embark on a new quest.");
                endStory.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us not unwarranted");
                endStory.Add("Reaper: It's a quest that can only be done in the afterlife. And only the four of you are qualified. Would you please consider it?");
                endStory.Add("Thob: It depends...");
                endStory.Add("Reaper: Yes, there is treasure.");
                endStory.Add("Thob: Then sign me up!");
                endStory.Add("Balthasar: Post-mortal job recruitment. Finally, an adventure you all will accept as having truly taken place.");
                endStory.Add("Reaper: What bout Jolie? Jolie?");
                endStory.Add("Jolie: Zzz...no...");
                endStory.Add("Reaper: She'll come around. Raol?");
                endStory.Add("Raol: First, you help me find Kol Kol.");
                endStory.Add("Reaper: Kol Kol the kenku necromancer, yes?");
                endStory.Add("Thob: So that's the Kol Kol you were going on about.");
                endStory.Add("Balthasar: Wait, the one you seek is a practitioner of black magic? Are you truly unequivocal in your desire to seek them?");
                endStory.Add("Raol: Kol Kol used to give me a lot of headaches, but they cleaned up my messes too, and...");
                endStory.Add("Thob: You can do it, Raol. Say the words. Kol Kol is a \"friend.\"");
                endStory.Add("Raol: ...Kol Kol was there for me.");
                endStory.Add("Thob: As a friend.");
                endStory.Add("Raol: ...");
                endStory.Add("Thob: Come now, surely you believe alpha males can have friends.");
                endStory.Add("Balthasar: Actually, the concept of alpha males is an erroneous concept created after observing wolves in unnatural--");
                endStory.Add("Raol: Let's go. We're done here.");
                endStory.Add("***");
                endStory.Add("Raol delayed seeking Kol Kol for " + db.data.wavesSurvived + " waves.");
                break;
            case EndingCode.Bal:
                endStory.Add("Reaper: Well well, Balthasar. It's been a while.");
                endStory.Add("Reaper: We have so much to catch up on.");
                endStory.Add("Reaper: The way you fought, I was starting to think you never wanted to see me again.");
                endStory.Add("Reaper: Is that the case?");
                endStory.Add("startEnding");
                endStory.Add("Balthasar: I beg to differ. I was merely still preoccupied with wrapping up the affairs of the living.");
                endStory.Add("Reaper: I'd forgotten how...loquacious you can be.");
                endStory.Add("Thob: Then you really haven't seen each other in a while.");
                endStory.Add("Jolie: Zzz...");
                endStory.Add("Balthasar: To matters at hand. You intend to offer us a new quest, correct?");
                endStory.Add("Reaper: How did you know?");
                endStory.Add("Balthasar: It was a logical conclusion to draw from the premise. Why else would you bring such considerable resources to bear to bring four adventurers to the afterlife at the same time, except to satisfy a need for an experienced adventuring party?");
                endStory.Add("Reaper: Yup. Yeah, that's about right.");
                endStory.Add("Balthasar: Then I accept. The afterlife too has its share of oddities and secrets I intend to unearth.");
                endStory.Add("Raol: Unless you explode them first. But yeah, I'll join you...I'm searching for something too.");
                endStory.Add("Thob: And I as well! Afterlife treasure must be even shinier, right?");
                endStory.Add("Balthasar: Your premonitions, while baseless, are serendipitously correct.");
                endStory.Add("Thob: What about Jolie?");
                endStory.Add("Balthasar: She is likely disinclined. If we let her slumber for the nonce, she may be more likely to accept the offer when she wakes, however reluctantly.");
                endStory.Add("Reaper: Thanks, Balthasar. I don't know what I'd do without you.");
                endStory.Add("Balthasar: The pleasure is mine, old friend.");
                endStory.Add("***");
                endStory.Add("Balthasar delayed his next adventure for " + db.data.wavesSurvived + " waves.");
                break;
            case EndingCode.Thob:
                endStory.Add("Reaper: Thob, you have spent your entire life grasping in greed.");
                endStory.Add("Reaper: For jewels. For drinks. For extra minutes before your final breath.");
                endStory.Add("Reaper: Now that I have you in my grasp, I can only ask...");
                endStory.Add("startEnding");
                endStory.Add("Reaper: Would you like to put that greed to use by working for me?");
                endStory.Add("Raol: What is this, a job offer?");
                endStory.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us was not unwarranted.");
                endStory.Add("Reaper: Yes. It's a quest that can only be done in the afterlife. And only the four of you are qualified. You have no idea how hard it is to find someone skilled in masonry, thievery, and musical composition who can also fit into tiny spaces.");
                endStory.Add("Jolie: Zzz...");
                endStory.Add("Thob: That reminds me...I wanted to return some things that I stole...");
                endStory.Add("Reaper: Your inventory remains with your physical body.");
                endStory.Add("Balthasar: Fret not, Thob. I choose to perceive this offer as an opportunity to obtain new oddities...which you may then immediately use to satisfy your kleptomaniac tendencies.");
                endStory.Add("Thob: Thanks!");
                endStory.Add("Raol: I will not be joining you. There's someone I need to find.");
                endStory.Add("Thob: Why not tell us who it is? We can help you search. You don't need to do everything alone.");
                endStory.Add("Raol: ...I'll think about it.");
                endStory.Add("Jolie: Zzz...");
                endStory.Add("Thob: I don't have the heart to wake her up and ask if she wants to join us.");
                endStory.Add("Reaper: I nearly forgot. She may join you when she learns how important this quest is to you.");
                endStory.Add("Thob: Me, personally?");
                endStory.Add("Reaper: Let me tell you about the kinds of trees that grow in hell...");
                endStory.Add("***");
                endStory.Add("Thob delayed fighting trees that actually need to die for " + db.data.wavesSurvived + " waves.");


                break;
            case EndingCode.Jolie:
                endStory.Add("Reaper: Jolie...you fought me for so long. I came as soon as I heard. Yet even as your comrades fell one by one, you persisted.");
                endStory.Add("Reaper: Why?");
                endStory.Add("Reaper: Jolie? Jolie, are you listening?");
                endStory.Add("startEnding");
                endStory.Add("Jolie: Zzzz...");
                endStory.Add("Thob: She gets like that sometimes.");
                endStory.Add("Raol: More like all the time. Anyway, what did you want us for?");
                endStory.Add("Reaper: I needed you all here so I could ask you to embark on a new quest.");
                endStory.Add("Balthasar: It appears my cognition that my old associate merely wished to speak to us was not unwarranted.");
                endStory.Add("Raol: Could have talked to us without killing us.");
                endStory.Add("Reaper: It's a quest that can only be done in the afterlife. And only the four of you are qualified. Would you consider it?");
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
