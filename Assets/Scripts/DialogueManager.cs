using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    private DialogueBox diaBox;

    List<string> testWords = new List<string>();

    //one time


    List<string> intro = new List<string>();
    List<string> tutorial = new List<string>();
    List<string> firstRuin_R = new List<string>();
    List<string> firstRuin_B = new List<string>();
    List<string> firstRuin_T = new List<string>();
    List<string> firstRuin_J = new List<string>();
    List<string> firstTower_R = new List<string>();
    List<string> firstTower_B = new List<string>();
    List<string> firstTower_T = new List<string>();
    List<string> firstTower_J = new List<string>();
    List<string> firstUpgrade_R = new List<string>();
    List<string> firstUpgrade_B = new List<string>();
    List<string> firstUpgrade_T = new List<string>();
    List<string> firstUpgrade_J = new List<string>();

    List<string> firstTowerEnemy = new List<string>();
    List<string> firstCharEnemy = new List<string>();

    List<string> enemiesOnPath = new List<string>();
    List<string> howBonesWork = new List<string>();
    List<string> howBonesWork2 = new List<string>();
    List<string> cannotRepairHome = new List<string>();

    List<string> firstHurt_R = new List<string>();
    List<string> firstHurt_B = new List<string>();
    List<string> firstHurt_T = new List<string>();
    List<string> firstHurt_J = new List<string>();

    List<string> firstUpgradeFail_R = new List<string>();
    List<string> firstUpgradeFail_B = new List<string>();
    List<string> firstUpgradeFail_T = new List<string>();
    List<string> firstUpgradeFail_J = new List<string>();

    //deaths
    List<string> firstDeath_R = new List<string>();
    List<string> firstDeath_B = new List<string>();
    List<string> firstDeath_T = new List<string>();
    List<string> firstDeath_J = new List<string>();

    List<string> secondDeath_R = new List<string>();
    List<string> secondDeath_B = new List<string>();
    List<string> secondDeath_T = new List<string>();
    List<string> secondDeath_J = new List<string>();

    List<string> thirdDeath_R = new List<string>();
    List<string> thirdDeath_B = new List<string>();
    List<string> thirdDeath_T = new List<string>();
    List<string> thirdDeath_J = new List<string>();

    List<string> lastDeath_R = new List<string>();
    List<string> lastDeath_B = new List<string>();
    List<string> lastDeath_T = new List<string>();
    List<string> lastDeath_J = new List<string>();

    //randomize
    List<string> bonePickUp_R = new List<string>();
    List<string> bonePickUp_B = new List<string>();
    List<string> bonePickUp_T = new List<string>();
    List<string> bonePickUp_J = new List<string>();

    List<string> towerBuilt_R = new List<string>();
    List<string> towerBuilt_B = new List<string>();
    List<string> towerBuilt_T = new List<string>();
    List<string> towerBuilt_J = new List<string>();

    List<string> towerUpgrade_R = new List<string>();
    List<string> towerUpgrade_B = new List<string>();
    List<string> towerUpgrade_T = new List<string>();
    List<string> towerUpgrade_J = new List<string>();

    List<string> towerRepair_J = new List<string>();
    List<string> towerRepair_R = new List<string>();
    List<string> towerRepair_B = new List<string>();
    List<string> towerRepair_T = new List<string>();

    List<string> towerFullRepair_J = new List<string>();
    List<string> towerFullRepair_R = new List<string>();
    List<string> towerFullRepair_B = new List<string>();
    List<string> towerFullRepair_T = new List<string>();

    List<string> enemyDestroyed = new List<string>();
    List<string> waveStarted = new List<string>();
    List<string> waveDefeated = new List<string>();

    List<string> returnBase_R = new List<string>();
    List<string> returnBase_B = new List<string>();
    List<string> returnBase_T = new List<string>();
    List<string> returnBase_J = new List<string>();

    List<string> departBase_R = new List<string>();
    List<string> departBase_B = new List<string>();
    List<string> departBase_T = new List<string>();
    List<string> departBase_J = new List<string>();

    List<string> enterAura_R = new List<string>();
    List<string> enterAura_B = new List<string>();
    List<string> enterAura_T = new List<string>();
    List<string> enterAura_J = new List<string>();

    List<string> exitAura_R = new List<string>();
    List<string> exitAura_B = new List<string>();
    List<string> exitAura_T = new List<string>();
    List<string> exitAura_J = new List<string>();

    List<string> levelUp_R = new List<string>();
    List<string> levelUp_B = new List<string>();
    List<string> levelUp_T = new List<string>();
    List<string> levelUp_J = new List<string>();

    //randomize once
    List<string> filler_R = new List<string>();
    List<string> filler_B = new List<string>();
    List<string> filler_T = new List<string>();
    List<string> filler_J = new List<string>();

    void Awake()
    {
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeAllStories();
        StartCoroutine(diaBox.PlayText(testWords, true));
        StartCoroutine(diaBox.PlayText(intro, true));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //play randomized text
    void PlayRandom (List<string> possibleStories)
    {
        int chosenIndex = Random.Range(0, possibleStories.Count - 1);
        List<string> chosenStory = new List<string>();
        chosenStory.Add(possibleStories[chosenIndex]);
        StartCoroutine(diaBox.PlayText(chosenStory, true));
    }

    //play a random text and remove it so it is only played once per game
    void PlayThenRemove(List<string> possibleStories)
    {
        int chosenIndex = Random.Range(0, possibleStories.Count-1);
        List <string> chosenStory = new List<string>();
        chosenStory.Add(possibleStories[chosenIndex]);
        possibleStories.RemoveAt(chosenIndex);
        StartCoroutine(diaBox.PlayText(chosenStory, true));

    }

    void InitializeAllStories()
    {
        //these first conversations are assumed to only play when all characters are alive near the beginning of the game

        intro.Add("Thob: Ah, the joys of retirement. Another day to work on my twenty-one movement lute sonata. Surrounded by friends.");
        intro.Add("Jolie: Zzz...");
        intro.Add("Thob: Or a nap does sound tempting...");
        intro.Add("Raol: There's someone coming. Down the road.");
        intro.Add("Balthasar: A visitor? Wonderful! No one ever visits us old people.");
        intro.Add("Raol: They're cloaked...like they're trying to hide their face. And masked. Holding some sort of long stick. A scythe?");
        intro.Add("Thob: A scythe! Are you certain?");
        intro.Add("Raol: My ears may be shot, but my eyes are as good as ever.");
        intro.Add("Thob: May I respectfully point out you are wearing an eyepatch?");
        intro.Add("Raol: It's aesthetic.");
        intro.Add("Thob: Right. Ahem. So the Reaper has finally come.");
        intro.Add("Balthasar: Marvelous!");
        intro.Add("Thob: No, Balthasar, that's bad. We must do something about this. Jolie? Jolie, wake up!");
        intro.Add("Jolie: Zzz...to defend...just...add towers...zzz...");

        tutorial.Add("Thob: Do you think one tower is enough? Jolie, wake up. We need your military expertise.");
        tutorial.Add("Jolie: Nnnh...fine, fine, I'm awake. What do you want?");
        tutorial.Add("Thob: We're about to be attacked by the Reaper. We built one tower, but is it enough?");
        tutorial.Add("Jolie: Why not just let the Reaper come? I could use some forever sleep.");
        tutorial.Add("Balthasar: The Reaper is really quite amicable. We go way back.");
        tutorial.Add("Thob: I can't die yet, I've only finished fifteen movements!");
        tutorial.Add("Raol: It's too late. They're coming.");
        tutorial.Add("Jolie: Really? Multiple Reapers?");

        howBonesWork.Add("Balthasar: These freshly fallen osseus materials are precisely what we need to manufacture additional defensive structures.");
        howBonesWork.Add("Thob: But we still need to put them in that giant processing machine in the retirement home to transform them into actual building supplies.");
        howBonesWork.Add("Jolie: We'll have to retrieve bones and bring them back to the retirement home before we can use them to build...even the thought of this is exhausting.");
        howBonesWork.Add("Raol: Is that why you were hoarding building bones even before today, Balthasar?");
        howBonesWork.Add("Balthasar: Veritably, I prefer the lustrous incandescence they take on after application of beeswax.");

        howBonesWork2.Add("Jolie: How do the building bones get so quickly from the home to the building site?");
        howBonesWork2.Add("Balthasar: The enigma surrounding the retirement home's bone-processing mechanism is tantalizingly impenetrable.");
        howBonesWork2.Add("Thob: All hail the Mighty Bone Processor of Confusingly Convenient Teleportation!");
        
        //first towers
        firstTower_R.Add("Raol: There we are. The first of many dedications to Mother Moon.");
        firstTower_R.Add("Thob: Who is this Mother Moon you keep going on about?");
        firstTower_R.Add("Balthasar: Might I suggest you simply gaze in an aerial direction? That is, after the crepuscular hours?");

        //the following conversations need to make sense even if any or all of the speakers (except for the initial speaker) are dead

        firstTower_B.Add("Balthasar: This creation of mine is more majestic than the ancient yellow dragon I turned into a cheese wheel.");
        firstTower_B.Add("Jolie: Like that ever happened.");
        firstTower_B.Add("Raol: Where's the proof?");
        firstTower_B.Add("Balthasar: Ludwig might've eaten the cheese wheel. Did you know owls enjoy eating cheese?");
        firstTower_B.Add("Raol: I did, actually. Didn't realize familiars could eat, though.");
        
        firstTower_T.Add("Thob: Behold. As sturdy and steadfast as all dwarvenfolk. And not a plank of wood in sight.");
        firstTower_T.Add("Jolie: What do you have against wood anyway? Isn't a dead tree a good tree?");
        firstTower_T.Add("Thob: Trees should have never existed in the first place.");
        firstTower_T.Add("Jolie: Then we wouldn't have paper either.");
        firstTower_T.Add("Thob: Announcement: I've just decided I hate paper.");

        firstTower_J.Add("Jolie: Not bad. Neat, tidy, and deadly.");
        firstTower_J.Add("Thob: Get back to the home! You'll get hurt!");
        firstTower_J.Add("Jolie: Maybe I'll stay out here. It's not so bad.");

        //first time character's tower is ruined
        firstRuin_R.Add("Raol: Figures. The harder you work on something, the sooner it all falls apart.");
        firstRuin_R.Add("Thob: That's what happens when you build from wood.");

        firstRuin_B.Add("Balthasar: Ah, the ruination of my cherished tower. It brings me almost as much as despair as the day I found an ant colony in my portable sofa.");
        firstRuin_B.Add("Raol: Sofas are for betas with back problems.");

        firstRuin_T.Add("Thob: Dust and ruin! My beautiful tower! Whatever shall I do?");
        firstRuin_T.Add("Jolie: Easy. Just go build a new one.");

        firstRuin_J.Add("Jolie: Eh, that tower was getting to be an eyesore anyway. Build something else there.");
        firstRuin_J.Add("Balthasar: Might I suggest one of my patented incendiary structures?");
        
        //first time character successfully upgrades
        firstUpgrade_R.Add("Raol: O Mother Moon, I submit to thee. Smite down thine enemies and bathe me in your holy light!");
        firstUpgrade_R.Add("Thob: Is it me, or does that tower look a little bit furry?");
        firstUpgrade_R.Add("Jolie: He's obviously a werewolf.");

        firstUpgrade_B.Add("Balthasar: Perfect. If anyone else is in need of assistance, I and my wide breadth of architectural knowledge are at your service.");
        firstUpgrade_B.Add("Raol: It's so crooked even a human could have built it.");
        firstUpgrade_B.Add("Thob: It looks even more unstable after upgrades.");
        firstUpgrade_B.Add("Balthasar: Anyone?");
        firstUpgrade_B.Add("Jolie: Zzz...I mean, no thanks.");

        firstUpgrade_T.Add("Thob: It's so beautiful. I almost want to...pry the gems from it.");
        firstUpgrade_T.Add("Jolie: Is it possible to steal from yourself?");
        firstUpgrade_T.Add("Thob: All I have to do is forget it was mine to begin with.");
        firstUpgrade_T.Add("Raol: Shouldn't be hard, with your memory.");

        firstUpgrade_J.Add("Jolie: Not bad. Now it's time for a nap...");
        firstUpgrade_J.Add("Thob: No, don't sit there! It's dangerous!");
        firstUpgrade_J.Add("Jolie: It's not dangerous if I sleep through it.");

        //first time character is hurt
        firstHurt_R.Add("Raol: Yow! Stop swinging that stupid stick!");
        firstHurt_B.Add("Balthasar: Ow! Ludwig, we must fly posthaste to the retirement home at once if we wish to avoid further assaults upon our person.");
        firstHurt_T.Add("Thob: Oof! Let the tower shoot you so I can pick up your loot.");
        firstHurt_J.Add("Jolie: Ouch! Stop it, I'm trying to sleep here.");

        //first upgrade fail
        firstUpgradeFail_R.Add("Raol: I need more building practice before I can create the ultimate shrine to Mother Moon.");
        firstUpgradeFail_B.Add("Balthasar: It pains me to concede that more experience is necessary before embarking upon renovation.");
        firstUpgradeFail_T.Add("Thob: Better to build a few more practice towers before I construct the magnum opus.");
        firstUpgradeFail_J.Add("Jolie: I need experience building basic towers before I can promote them.");

        
        //the remaining conversations need to make sense even if any number of speakers are already dead

        firstTowerEnemy.Add("Raol: That reaper looks different from the others we've seen.");
        firstTowerEnemy.Add("Thob: The blue reaper is going for the towers!");
        firstTowerEnemy.Add("Balthasar: Fret not, we shall simply repair the damaged towers with our excess bone collection.");
        firstTowerEnemy.Add("Jolie: We need to repair towers before the reapers ruin them completely, or we'll use up even more bones rebuilding from scratch.");

        firstCharEnemy.Add("Balthasar: That crimson reaper possesses aerial capabilities!");
        firstCharEnemy.Add("Raol: It's an enemy smart enough to leave the path to chase its prey.");
        firstCharEnemy.Add("Thob: It's coming straight for us! Hide in the retirement home!");
        firstCharEnemy.Add("Jolie: Hello, new enemy. Please fly over here and strike me down.");

        cannotRepairHome.Add("Thob: If only we could repairing our retirement home too. If it falls, we won't be able to process new bones.");
        cannotRepairHome.Add("Jolie: Does anyone know how to repair the retirement home?");
        cannotRepairHome.Add("Balthasar: Much to my consternation, my vast stores of knowledge contain no instruction manual regarding the reparation of bone processing contraptions.");
        cannotRepairHome.Add("Raol: So we can't repair the retirement home because we only know how to repair what we ourselves built.");

        enemiesOnPath.Add("Thob: How come the reapers don't stray from the path?");
        enemiesOnPath.Add("Balthasar: I hypothesize the reapers lack sufficient intelligence to maximize efficient target prioritization.");
        enemiesOnPath.Add("Jolie: The reapers are like me. They prefer a smooth surface to travel.");
        enemiesOnPath.Add("Raol: Don't let the reapers hear us speculate why they stay on the path. They might start to adapt.");

        //first deaths (all characters present)
        firstDeath_R.Add("Raol: Kol Kol...don't worry, I'm coming to find you.");
        firstDeath_R.Add("Balthasar: Who or what is a Kol Kol?");
        firstDeath_R.Add("Thob: It sounded like someone he cared about deeply. A lover, perhaps?");
        firstDeath_R.Add("Balthasar: A familiar?");
        firstDeath_R.Add("Thob: I wish we had gotten to know him better. Even if he was pretty prickly.");
        firstDeath_R.Add("Balthasar: It would have been a laborious process to befriend someone so misanthropic.");
        firstDeath_R.Add("Jolie: More importantly, I can't believe I didn't die first. What am I doing? I wish I were Raol now. I bet he doesn't have an awful headache.");

        firstDeath_B.Add("Balthasar: It seems that I will perish forthwith. Yet I do not sorrow, for I have died a plethora of times before--");
        firstDeath_B.Add("Jolie: He exploded.");
        firstDeath_B.Add("Thob: I could not imagine a more befitting end. He was a colorful sort. A bit talkative, perhaps, but ever unpredictable.");
        firstDeath_B.Add("Raol: Are we going to stand around and die just to hold a funeral? Let's move it.");
        firstDeath_B.Add("Jolie: Why can't I spontaneously explode like that?");

        firstDeath_T.Add("Thob: Och, I don't think I can recover from this wound...when I die...please loot my body. Apologies...I was going to return them...");
        firstDeath_T.Add("Jolie: Loot his body ? Does he mean he dropped treasure?");
        firstDeath_T.Add("Balthasar: Now that you mention it, the item protruding from his backpack does look suspiciously like my Wabbajack!");
        firstDeath_T.Add("Raol: Your Wabba-what now?");
        firstDeath_T.Add("Balthasar: My Wabbajack. It used to be a legendary magic wand that could polymorph even the most gargantuan of creatures into innocuous food items.");
        firstDeath_T.Add("Jolie: Used to. Yeah right. And you can't use the \"Wabbajack\" anymore because--?");
        firstDeath_T.Add("Balthasar: It was transfromed into a sniper rifle. Also I no longer possess bullets.");
        firstDeath_T.Add("Jolie: I possess a headache.");

        firstDeath_J.Add("Jolie: Finally! I can sleep forever! See you all...never...");
        firstDeath_J.Add("Thob: All things considered, she seems pretty happy to go.");
        firstDeath_J.Add("Raol: But abandoning the rest of us to die as well. We can't repair her towers.");
        firstDeath_J.Add("Balthasar: Death is not as abominable as one might think. I myself once died thirteen times in the span of six months.");
        firstDeath_J.Add("Thob: Now that Jolie is no longer here to express disbelief at your tall tales, I need to take up her mantle. Because really? Thirteen times?)";
        firstDeath_J.Add("Balthasar: As you can see, I am no worse for wear.");
        firstDeath_J.Add("Raol: This actually explains a lot.");

        //the following death scenes should still work even if any single one of the speakers (in addition to the dying) is missing
        secondDeath_R.Add("Raol: Mother Moon! I...I cannot continue. Kol Kol...don't worry, I'm coming to find you.");
        secondDeath_R.Add("Balthasar: Who is Kol Kol?");
        secondDeath_R.Add("Thob: Is Kol Kol someone he cared about deeply? A lover?");
        secondDeath_R.Add("Balthasar: I find it difficult to imagine someone with such historically antagonistic behavior in an intimate scenario.");
        secondDeath_R.Add("Jolie: I once heard Raol ask the nurses if they would \"come back later for a price.\" It turned out he just wanted to pay them to leave him alone.");
        secondDeath_R.Add("Thob: Raol, you shall be missed. Even if you were pretty prickly.");

        secondDeath_B.Add("");

        secondDeath_T.Add("Thob: Och, I don't think I can recover from this wound...when I die...please loot my body. Apologies...I was going to return them...");
        secondDeath_T.Add("Jolie: Loot his body? Does he mean he's going to drop treasure?");
        secondDeath_T.Add("Balthasar: The kleptomaniac appears to have spirited my Icicle of Unmelting Ice into his own backpack.");
        secondDeath_T.Add("Jolie: He stole my sword scabbard! I wondered where that had gotten off to.");
        secondDeath_T.Add("Raol: I can't approach that body, it has too much silver on it. But you'll probably found my moon ring in his pouch.");
        secondDeath_T.Add("Jolie: Yup, found Raol's ring here too.");
        secondDeath_T.Add("Raol: What a scoundrel.");
        secondDeath_T.Add("Balthasar: Indeed, but his final words were so regretful, I cannot help but forgive him. Who among us has never succumbed to our basest intincts?");

        secondDeath_J.Add("Jolie: At last! I can be at peace!");
        secondDeath_J.Add("Thob: I've often wished I could be more accepting of death, but perhaps not that accepting.");
        secondDeath_J.Add("Balthasar: You know, if you would like to be more cavalier towards death, you can easily do so by simply undergoing multiple deaths and revivals, as I have.");
        secondDeath_J.Add("Raol: I've evaded death many times and I intend to continue doing so. We don't need Julie's towers to survive this.");
        secondDeath_J.Add("Thob: Onwards! Let Julie enjoy her eternal rest as we work to never join her!");

        //the following death scenes should be read as though only one of the speakers is present (aside from the dying speaker)
        thirdDeath_R.Add("Raol: Mother Moon, the pain is too much...Kol Kol...I'm coming to find you.");
        thirdDeath_R.Add("Balthasar: How perplexing. You have never divulged this name \"Kol Kol\" to me before. To be fair, you did not speak of many things. Mostly I expatiated on my own adventurers, and you may or may not have listened. Who shall bear witness to my adventurers now? Aside from Ludwig, that is?");
        thirdDeath_R.Add("Thob: Ah Raol, though I barely knew you, I shall continue to sing of your glory to the end. What rhymes with Raol? Cowl? Drawl? Wait, how do you pronounce your name? Have you been scowling at me this whole time because I was saying it wrong? Hmm, scowl and Raol go together pretty well."); 
        thirdDeath_R.Add("Jolie: But who will repair your towers now? Wait, what am I saying? I don't need to repair your towers or my towers or anyone's towers. I don't need to keep anyone else alive. I can finally rest. Right? Right? Come, Death! You shall not escape me! Rid me of my bad hips!");
        
        thirdDeath_B.Add("");
        thirdDeath_T.Add("");
        thirdDeath_J.Add("");

        //the following death scenes only have one speaker, so yay!

        lastDeath_R.Add("Raol: Kol Kol...it's been so long. Have you gotten yourself into trouble again? Well, look at the trouble I'm in now. Mother Moon, I shall still howl to you...even in death...");

        lastDeath_B.Add("Balthasar: I suppose ");

        lastDeath_T.Add("Thob: Well, this is embarrassing...I meant to return the treasures I stole from everyone else, but they died before they could catch me...I really did mean to bring it up...I suppose it doesn't matter now...at least I'll die with an apology on my lips...and the dignity of knowing that I was never felled by a tree. Wait. These scythes are made from wood...");
        lastDeath_J.Add("");

        //randomize
        bonePickUp_R.Add("");
        bonePickUp_B.Add("");
        bonePickUp_T.Add("");
        bonePickUp_J.Add("");

        towerBuilt_R.Add("");
        towerBuilt_B.Add("");
        towerBuilt_T.Add("");
        towerBuilt_J.Add("");

        towerUpgrade_R.Add("");
        towerUpgrade_B.Add("");
        towerUpgrade_T.Add("");
        towerUpgrade_J.Add("");

        towerRepair_J.Add("");
        towerRepair_R.Add("");
        towerRepair_B.Add("");
        towerRepair_T.Add("");

        towerFullRepair_J.Add("");
        towerFullRepair_R.Add("");
        towerFullRepair_B.Add("");
        towerFullRepair_T.Add("");

        enemyDestroyed.Add("");
        waveStarted.Add("");
        waveDefeated.Add("");

        returnBase_R.Add("");
        returnBase_B.Add("");
        returnBase_T.Add("");
        returnBase_J.Add("");

        departBase_R.Add("");
        departBase_B.Add("");
        departBase_T.Add("");
        departBase_J.Add("");

        enterAura_R.Add("");
        enterAura_B.Add("");
        enterAura_T.Add("");
        enterAura_J.Add("");

        exitAura_R.Add("");
        exitAura_B.Add("");
        exitAura_T.Add("");
        exitAura_J.Add("");

        levelUp_R.Add("");
        levelUp_B.Add("");
        levelUp_T.Add("");
        levelUp_J.Add("");

        //randomize once
        filler_R.Add("");
        filler_B.Add("");
        filler_T.Add(""); //compares ages
        filler_J.Add(""); 
    }
}
