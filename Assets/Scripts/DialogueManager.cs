using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    private DialogueBox diaBox;
    private DataBucket db;
    private RTSController RTSC;

    List<string> testWords = new List<string>();

    //one time


    public List<string> intro = new List<string>();
    public List<string> tutorial1 = new List<string>();
    public List<string> tutorial2 = new List<string>();
    public List<string> tutorial3 = new List<string>();
    public List<string> firstRuin_R = new List<string>();
    public List<string> firstRuin_B = new List<string>();
    public List<string> firstRuin_T = new List<string>();
    public List<string> firstRuin_J = new List<string>();
    public List<string> firstTower_R = new List<string>();
    public List<string> firstTower_B = new List<string>();
    public List<string> firstTower_T = new List<string>();
    public List<string> firstTower_J = new List<string>();
    public List<string> firstUpgrade_R = new List<string>();
    public List<string> firstUpgrade_B = new List<string>();
    public List<string> firstUpgrade_T = new List<string>();
    public List<string> firstUpgrade_J = new List<string>();

    public List<string> firstTowerEnemy = new List<string>();
    public List<string> firstCharEnemy = new List<string>();

    public List<string> enemiesOnPath = new List<string>();
    public List<string> howBonesWork = new List<string>();
    public List<string> howBonesWork2 = new List<string>();
    public List<string> cannotRepairHome = new List<string>();

    public List<string> firstHurt_R = new List<string>();
    public List<string> firstHurt_B = new List<string>();
    public List<string> firstHurt_T = new List<string>();
    public List<string> firstHurt_J = new List<string>();

    public List<string> firstUpgradeFail_R = new List<string>();
    public List<string> firstUpgradeFail_B = new List<string>();
    public List<string> firstUpgradeFail_T = new List<string>();
    public List<string> firstUpgradeFail_J = new List<string>();

    public List<string> home75 = new List<string>();
    public List<string> home50 = new List<string>();
    public List<string> home25 = new List<string>();
    public List<string> homeRuined = new List<string>();

    //deaths
    public List<string> firstDeath_R = new List<string>();
    public List<string> firstDeath_B = new List<string>();
    public List<string> firstDeath_T = new List<string>();
    public List<string> firstDeath_J = new List<string>();

    public List<string> secondDeath_R = new List<string>();
    public List<string> secondDeath_B = new List<string>();
    public List<string> secondDeath_T = new List<string>();
    public List<string> secondDeath_J = new List<string>();

    public List<string> thirdDeath_R = new List<string>();
    public List<string> thirdDeath_B = new List<string>();
    public List<string> thirdDeath_T = new List<string>();
    public List<string> thirdDeath_J = new List<string>();

    public List<string> lastDeath_R = new List<string>();
    public List<string> lastDeath_B = new List<string>();
    public List<string> lastDeath_T = new List<string>();
    public List<string> lastDeath_J = new List<string>();

    //randomize
    public List<string> bonePickUp_R = new List<string>();
    public List<string> bonePickUp_B = new List<string>();
    public List<string> bonePickUp_T = new List<string>();
    public List<string> bonePickUp_J = new List<string>();

    public List<string> towerBuilt_R = new List<string>();
    public List<string> towerBuilt_B = new List<string>();
    public List<string> towerBuilt_T = new List<string>();
    public List<string> towerBuilt_J = new List<string>();

    public List<string> towerUpgrade_R = new List<string>();
    public List<string> towerUpgrade_B = new List<string>();
    public List<string> towerUpgrade_T = new List<string>();
    public List<string> towerUpgrade_J = new List<string>();

    public List<string> towerRepair_J = new List<string>();
    public List<string> towerRepair_R = new List<string>();
    public List<string> towerRepair_B = new List<string>();
    public List<string> towerRepair_T = new List<string>();

    public List<string> towerFullRepair_J = new List<string>();
    public List<string> towerFullRepair_R = new List<string>();
    public List<string> towerFullRepair_B = new List<string>();
    public List<string> towerFullRepair_T = new List<string>();

    public List<string> enemyDestroyed = new List<string>();
    public List<string> waveStarted = new List<string>();
    public List<string> waveDefeated = new List<string>();

    public List<string> returnBase_R = new List<string>();
    public List<string> returnBase_B = new List<string>();
    public List<string> returnBase_T = new List<string>();
    public List<string> returnBase_J = new List<string>();

    public List<string> departBase_R = new List<string>();
    public List<string> departBase_B = new List<string>();
    public List<string> departBase_T = new List<string>();
    public List<string> departBase_J = new List<string>();

    public List<string> enterAura_R = new List<string>();
    public List<string> enterAura_B = new List<string>();
    public List<string> enterAura_T = new List<string>();
    public List<string> enterAura_J = new List<string>();

    public List<string> exitAura_R = new List<string>();
    public List<string> exitAura_B = new List<string>();
    public List<string> exitAura_T = new List<string>();
    public List<string> exitAura_J = new List<string>();

    public List<string> levelUp_R = new List<string>();
    public List<string> levelUp_B = new List<string>();
    public List<string> levelUp_T = new List<string>();
    public List<string> levelUp_J = new List<string>();

    //randomize once
    public List<string> filler_R = new List<string>();
    public List<string> filler_B = new List<string>();
    public List<string> filler_T = new List<string>();
    public List<string> filler_J = new List<string>();

    void Awake()
    {
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        RTSC = GameObject.Find("RTS Controller").GetComponent<RTSController>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if(db.debugMode) db.tutorialMode = 5;
        else db.tutorialMode = 0;

        InitializeAllStories();
        StartCoroutine(diaBox.PlayText(intro, TextMode.imm));

        GameObject.Find("BGM").GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("BGM/#50_GameLoop_Combined");
        GameObject.Find("BGM").GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //play randomized text
    public void PlayRandom (List<string> possibleStories)
    {
        int chosenIndex = Random.Range(0, possibleStories.Count - 1);
        List<string> chosenStory = new List<string>();
        chosenStory.Add(possibleStories[chosenIndex]);
        StartCoroutine(diaBox.PlayText(chosenStory, TextMode.ifFree));
    }

    public void PlayRandomAlive(List<string> possibleStories, TextMode mode)
    {
        int chosenIndex = Random.Range(0, possibleStories.Count - 1);

        bool storyFound = false;

        for (int j = 0; j < possibleStories.Count; j++)
        {
            switch (possibleStories[chosenIndex].Split(':')[0])
            {
                case "Raol":
                    if (RTSC.raol_alive)
                        storyFound = true;
                    break;
                case "Balthasar":
                    if (RTSC.bal_alive)
                        storyFound = true;

                    break;
                case "Thob":
                    if (RTSC.thob_alive)
                        storyFound = true;

                    break;
                case "Jolie":
                    if (RTSC.jolie_alive)
                        storyFound = true;

                    break;
                default:
                    break;
            }

            if (storyFound)
                break;

            if (chosenIndex > possibleStories.Count - 1)
            {
                chosenIndex = 0;
            }
            else
            {
                chosenIndex++;

            }
        }
        List<string> chosenStory = new List<string>();

        chosenStory.Add(possibleStories[chosenIndex]);
        StartCoroutine(diaBox.PlayText(chosenStory, mode));


    }

    //play a random text and remove it so it is only played once per game
    public void PlayThenRemove(List<string> possibleStories)
    {
        int chosenIndex = Random.Range(0, possibleStories.Count-1);
        List <string> chosenStory = new List<string>();
        chosenStory.Add(possibleStories[chosenIndex]);
        possibleStories.RemoveAt(chosenIndex);
        StartCoroutine(diaBox.PlayText(chosenStory, TextMode.ifFree));

    }

    void InitializeAllStories()
    {
        //these first conversations are assumed to only play when all characters are alive near the beginning of the game
        
        
        intro.Add("Thob: Ah, the joys of retirement. Another day to work on my twenty-one movement lute sonata. Surrounded by friends.");
        intro.Add("Jolie: Zzz...");
        intro.Add("Thob: On the other hand, a nap does sound tempting...");
        intro.Add("Raol: There's someone coming. Down the road.");
        intro.Add("Balthasar: A visitor? Wonderful! No one ever visits us old people.");
        intro.Add("Raol: They're cloaked...like they're trying to hide their face. And masked. Holding some sort of long stick. A scythe?");
        intro.Add("Thob: A scythe! Are you certain?");
        intro.Add("Raol: My ears may be shot, but my eyes are as good as ever.");
        intro.Add("Thob: May I respectfully point out you are wearing an eyepatch?");
        intro.Add("Raol: It's aesthetic.");
        intro.Add("Thob: Right. Then assuming your vision is perfect, the Reaper has finally come.");
        intro.Add("Balthasar: Marvelous!");
        intro.Add("Thob: No, Balthasar, that's bad. We must do something about this. Jolie? Jolie, wake up!");
        intro.Add("Jolie: Zzz...to defend...just...add towers...zzz...");
        intro.Add("Tutorial: Select Raol, the hero in the top left corner of the retirement home.");
        intro.Add("incrementTutorial");



        tutorial1.Add("Use WASD, the arrow keys, or the mouse right-click to move the selected hero.");

        tutorial2.Add("Select the \"Build\" button in lower right corner. Click somewhere on the map in a valid (green) location to build Raol's first tower.");

        tutorial3.Add("Raol: There we are. The first of many dedications to Mother Moon.");
        tutorial3.Add("Thob: Do you think one tower is enough? Jolie, wake up. We need your military expertise.");
        tutorial3.Add("Jolie: Nnnh...fine, fine, I'm awake. What do you want?");
        tutorial3.Add("Thob: We're about to be attacked by the Reaper. We built one tower, but is it enough?");
        tutorial3.Add("Jolie: Why not just let the Reaper come? I could use some forever sleep.");
        tutorial3.Add("Balthasar: The Reaper is really quite amicable. We go way back.");
        tutorial3.Add("Thob: I can't die yet, I've only written fifteen movements of my sonata!");
        tutorial3.Add("Raol: It's too late. They're coming.");
        tutorial3.Add("Jolie: Really? Multiple Reapers?");
        tutorial3.Add("endTutorial");

        //end of first wave
        howBonesWork.Add("Balthasar: These freshly fallen osseus materials are precisely what we need to manufacture additional defensive structures.");
        howBonesWork.Add("Thob: But we still need to put them in that giant processing machine in the retirement home to transform them into actual building supplies.");
        howBonesWork.Add("Jolie: We'll have to retrieve bones and bring them back to the retirement home before we can use them to build...even the thought of this is exhausting.");
        howBonesWork.Add("Raol: Is that why you were hoarding building bones even before today, Balthasar?");
        howBonesWork.Add("Balthasar: Veritably, I prefer the lustrous incandescence they take on after application of beeswax.");
        howBonesWork.Add("Tutorial: Heroes collect bones by walking over them. Take the bones back to the retirement home to make them available for building.");
        howBonesWork.Add("startWave");

        //end of second wave
        howBonesWork2.Add("Jolie: How do the building bones get so quickly from the home to the building site?");
        howBonesWork2.Add("Balthasar: The enigma surrounding the retirement home's bone-processing mechanism is tantalizingly impenetrable.");
        howBonesWork2.Add("Thob: All hail the Mighty Bone Processor of Confusingly Convenient Teleportation!");
        howBonesWork2.Add("Shortcuts: Press Tab to cycle through heroes. While hero is selected, press B to Build, R to Repair, or U to Upgrade. May the heroes live forever. Good luck.");
        howBonesWork2.Add("startWave");

        //end of third wave
        enemiesOnPath.Add("Thob: How come the reapers don't stray from the path?");
        enemiesOnPath.Add("Balthasar: I hypothesize the reapers lack sufficient intelligence to maximize efficient target prioritization.");
        enemiesOnPath.Add("Jolie: The reapers are like me. They prefer a smooth surface to travel.");
        enemiesOnPath.Add("Raol: Don't let the reapers hear us speculate why they stay on the path. They might start to adapt.");
        enemiesOnPath.Add("startWave");

        //end of fourth wave
        cannotRepairHome.Add("Thob: If only we could repair our retirement home too. If it falls, we won't be able to process new bones.");
        cannotRepairHome.Add("Jolie: Does anyone know how to repair the retirement home?");
        cannotRepairHome.Add("Balthasar: Much to my consternation, my vast stores of knowledge contain no instruction manual regarding the reparation of bone processing contraptions.");
        cannotRepairHome.Add("Raol: So, we can't repair the retirement home because we only know how to repair what we ourselves built.");
        cannotRepairHome.Add("startWave");

        //start of fourth wave
        firstTowerEnemy.Add("Raol: That reaper looks different from the others we've seen.");
        firstTowerEnemy.Add("Thob: The purple reaper is going for the towers!");
        firstTowerEnemy.Add("Balthasar: Fret not, we shall simply repair the damaged towers with our excess bone collection.");
        firstTowerEnemy.Add("Jolie: We need to repair towers before the reapers ruin them completely, or we'll use up even more bones rebuilding from scratch.");

        //start of fifth wave
        firstCharEnemy.Add("Balthasar: That crimson reaper possesses aerial capabilities!");
        firstCharEnemy.Add("Raol: It's an enemy smart enough to leave the path to chase its prey.");
        firstCharEnemy.Add("Thob: It's coming straight for us! Hide in the retirement home!");
        firstCharEnemy.Add("Jolie: Hello, new enemy. Please fly over here and strike me down.");

        //the following conversations need to make sense even if any or all of the speakers (except for the initial speaker) are dead

        firstTower_B.Add("Balthasar: This creation of mine is more majestic than the ancient yellow dragon I turned into a cheese wheel.");
        firstTower_B.Add("Jolie: Like that ever happened.");
        firstTower_B.Add("Raol: Where's the proof?");
        firstTower_B.Add("Balthasar: Ludvig might've eaten the cheese wheel. Did you know owls enjoy eating cheese?");
        firstTower_B.Add("Raol: I did, actually. Didn't realize familiars could eat, though.");
        
        firstTower_T.Add("Thob: Behold. As sturdy and steadfast as all dwarvenfolk. And not a plank of wood in sight.");
        firstTower_T.Add("Jolie: What do you have against wood anyway? Isn't a dead tree a good tree?");
        firstTower_T.Add("Thob: Trees should have never existed in the first place.");
        firstTower_T.Add("Jolie: Then we wouldn't have paper either.");
        firstTower_T.Add("Thob: Announcement: I've just decided I hate paper.");

        firstTower_J.Add("Jolie: Neat, tidy, and deadly.");
        firstTower_J.Add("Thob: Get back to the home! You'll get hurt!");
        firstTower_J.Add("Jolie: Maybe I'll stay out here. It's not so bad.");

        //first time character's tower is ruined
        firstRuin_R.Add("Raol: Figures. The harder you work on something, the sooner it all falls apart.");
        firstRuin_R.Add("Thob: That's what happens when you build from wood.");

        firstRuin_B.Add("Balthasar: Ah, the ruination of my cherished tower. It brings me almost as much as despair as the day I found an ant colony in my portable sofa.");
        firstRuin_B.Add("Raol: Sofas are for betas with back problems.");

        firstRuin_T.Add("Thob: Dust and ruin! My beautiful tower! Whatever shall I do?");
        firstRuin_T.Add("Jolie: Easy. Just go build a new one.");

        firstRuin_J.Add("Jolie: Eh, that tower was starting to be an eyesore anyway. Now we can look at something else.");
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
        firstHurt_B.Add("Balthasar: Ow! Ludvig, we must fly posthaste to the retirement home at once if we wish to avoid further assaults upon our person.");
        firstHurt_T.Add("Thob: Oof! Let the tower shoot you so I can pick up your loot.");
        firstHurt_J.Add("Jolie: Ouch! Stop it, I'm trying to sleep here.");

        //first upgrade fail
        firstUpgradeFail_R.Add("Raol: I need more building practice before I can create the ultimate shrine to Mother Moon.");
        firstUpgradeFail_B.Add("Balthasar: It pains me to concede that more experience is necessary before embarking upon renovation.");
        firstUpgradeFail_T.Add("Thob: Better to build a few more practice towers before I construct the magnum opus.");
        firstUpgradeFail_J.Add("Jolie: I'm a fighter, not an fortification engineer. I need more experience building the basics first.");

        
        //the remaining conversations need to make sense even if any number of speakers are already dead



        home75.Add("Raol: We can't keep letting these reapers slip through to damage our home.");
        home75.Add("Jolie: The bone processing machine in the home is making weird whirring noises. It's too loud.");
        home75.Add("Balthasar: The machine will continue to manufacture building bones for the nonce, but we must exert ourselves to protect it.");

        home50.Add("Jolie: Our retirement home is already half destroyed. This is fine.");
        home50.Add("Raol: Thankfully all the nurses in the retirement home evacuated safely.");
        home50.Add("Thob: The home doesn't feel safe anymore. I've started carrying my sonata on my person.");

        home25.Add("Thob: The retirement home won't last much longer, but we need it if we want to process any bones.");
        home25.Add("Raol: Perhaps we could lug the bone machine out before it's destroyed along with the home? No, it's too heavy.");
        home25.Add("Jolie: I say, let the retirement home crumble. It will help everyone give up sooner.");

        homeRuined.Add("Raol: The retirement home is gone, along with our hopes of surviving.");
        homeRuined.Add("Balthasar: Our bone processing machine may be obliterated, but our machinations live on as long as we persist!");
        homeRuined.Add("Thob: Now that the home is fully destroyed, I've been meaning to check out the gated community down the road.");
        homeRuined.Add("Jolie: Good riddance to that retirement home. I won't miss those stiff mattresses.");

        //first deaths (all characters present)
        firstDeath_R.Add("Raol: Kol Kol...don't worry, I'm coming to find you.");
        firstDeath_R.Add("Balthasar: Who or what is a Kol Kol?");
        firstDeath_R.Add("Thob: It sounded like someone he cared about deeply. A lover, perhaps?");
        firstDeath_R.Add("Balthasar: A familiar?");
        firstDeath_R.Add("Thob: I wish we had gotten to know him better. Even if he was pretty prickly.");
        firstDeath_R.Add("Balthasar: It would have been a laborious process to befriend someone so misanthropic.");
        firstDeath_R.Add("Jolie: More importantly, I can't believe I didn't die first. What am I doing? I wish I were Raol now. I bet he doesn't have an awful headache.");
        firstDeath_R.Add("unpause:raol");

        firstDeath_B.Add("Balthasar: It seems that I will perish forthwith. Yet I do not sorrow, for I have died a plethora of times before--");
        firstDeath_B.Add("Jolie: He exploded.");
        firstDeath_B.Add("Thob: I could not imagine a more befitting end. He was a colorful sort. A bit talkative, perhaps, but ever unpredictable.");
        firstDeath_B.Add("Raol: Are we going to stand around and die just to hold a funeral? Let's move it.");
        firstDeath_B.Add("Jolie: Why can't I spontaneously explode like that?");
        firstDeath_B.Add("unpause:bal");

        firstDeath_T.Add("Thob: Och, I don't think I can recover from this wound...when I die...please loot my body. Apologies...I was going to return them...");
        firstDeath_T.Add("Jolie: Loot his body ? Does he mean he dropped treasure?");
        firstDeath_T.Add("Balthasar: Now that you mention it, the item protruding from his backpack does look suspiciously like my Wabbajack!");
        firstDeath_T.Add("Raol: Your Wabba-what now? That better not be some modern humbug.");
        firstDeath_T.Add("Balthasar: My Wabbajack. It used to be a legendary magic wand that could polymorph even the most gargantuan of creatures into innocuous food items.");
        firstDeath_T.Add("Jolie: Used to. Yeah right. And you can't use the \"Wabbajack\" anymore because--?");
        firstDeath_T.Add("Balthasar: It was transformed into a sniper rifle. Also I no longer possess the requisite ammunition.");
        firstDeath_T.Add("Jolie: Also I possess a headache.");
        firstDeath_T.Add("unpause:thob");

        firstDeath_J.Add("Jolie: Finally! I can sleep forever! See you all...never...");
        firstDeath_J.Add("Thob: All things considered, she seems pretty happy to go.");
        firstDeath_J.Add("Raol: But abandoning the rest of us to die as well. We can't repair her towers.");
        firstDeath_J.Add("Balthasar: Death is not as abominable as one might think. I myself once died thirteen times in the span of six months.");
        firstDeath_J.Add("Thob: Now that Jolie is no longer here to express disbelief at your tall tales, I need to take up her mantle. Because really? Thirteen times?");
        firstDeath_J.Add("Balthasar: As you can see, I am no worse for wear.");
        firstDeath_J.Add("Raol: This actually explains a lot.");
        firstDeath_J.Add("unpause:jolie");

        //the following death scenes should still work even if any single one of the speakers (in addition to the dying) is missing
        secondDeath_R.Add("Raol: Mother Moon! I...I cannot continue. Kol Kol...don't worry, I'm coming to find you.");
        secondDeath_R.Add("Balthasar: Who is Kol Kol? A lover?");
        secondDeath_R.Add("Thob: Is he talking about a friend?");
        secondDeath_R.Add("Balthasar: I find it difficult to imagine someone with such historically antagonistic behavior in an intimate scenario.");
        secondDeath_R.Add("Jolie: I once heard Raol ask the nurses if they would \"come back later for a price.\" It turned out he just wanted to pay them to leave him alone.");
        secondDeath_R.Add("Thob: Raol, you shall be missed. Even if you were pretty prickly.");
        secondDeath_R.Add("unpause:raol");

        secondDeath_B.Add("Balthasar: Hello Reaper, my old acquaintance...");
        secondDeath_B.Add("Thob: That was nearly a song I recognized.");
        secondDeath_B.Add("Jolie: Do you think the Reaper will actually greet him like an old friend?");
        secondDeath_B.Add("Raol: The Reaper might be more annoyed that he's been revived so many times.");
        secondDeath_B.Add("Thob: Whatever the Reaper thinks, getting Balthasar wasn't enough. And we won't be able to repair Balthasar's towers.");
        secondDeath_B.Add("Jolie: I'd like to greet the Reaper soon. If only to demand he explain why it's taken him so long put an end to my headaches.");
        secondDeath_B.Add("unpause:bal");

        secondDeath_T.Add("Thob: Och, I don't think I can recover from this wound...when I die...please loot my body. Apologies...I was going to return them...");
        secondDeath_T.Add("Jolie: Loot his body? Does he mean he's going to drop treasure?");
        secondDeath_T.Add("Balthasar: The kleptomaniac appears to have spirited my Icicle of Unmelting Ice into his own backpack.");
        secondDeath_T.Add("Jolie: He stole my sword scabbard! I wondered where that had gotten off to.");
        secondDeath_T.Add("Raol: I can't approach that body, it has too much silver on it. But you'll probably find my moon ring in his pouch.");
        secondDeath_T.Add("Jolie: Yup, found Raol's ring here too.");
        secondDeath_T.Add("Raol: What a scoundrel.");
        secondDeath_T.Add("Balthasar: Indeed, but his final words were so regretful, I cannot help but forgive him. Who among us has never succumbed to our baser instincts?");
        secondDeath_T.Add("unpause:thob");

        secondDeath_J.Add("Jolie: At last! I can be at peace!");
        secondDeath_J.Add("Thob: I've often wished I could be more accepting of death, but perhaps not that accepting.");
        secondDeath_J.Add("Balthasar: You know, if you would like to be more cavalier towards death, you can easily do so by simply undergoing multiple deaths and revivals, as I have.");
        secondDeath_J.Add("Raol: I've evaded death many times and I intend to continue doing so. We don't need Jolie's towers to survive this.");
        secondDeath_J.Add("Thob: Onwards! Let Jolie enjoy her eternal rest as we work to never join her!");
        secondDeath_J.Add("unpause:jolie");

        //the following death scenes should be read as though only one of the speakers is present (aside from the dying speaker)
        thirdDeath_R.Add("Raol: Mother Moon, the pain is too much...Kol Kol...I'm coming to find you.");
        thirdDeath_R.Add("Balthasar: How perplexing. You have never divulged this name \"Kol Kol\" to me before. To be fair, you did not speak of many things. Mostly I expatiated on my own adventurers, and you may or may not have listened. Who shall bear witness to my adventurers now? Aside from Ludvig, that is?");
        thirdDeath_R.Add("Thob: Ah Raol, though I barely knew you, I shall continue to sing of your glory to the end. What rhymes with Raol? Cowl? Drawl? Wait, how do you pronounce your name? Have you been scowling at me this whole time because I was saying it wrong? Hmm, scowl and Raol go together pretty well."); 
        thirdDeath_R.Add("Jolie: Raol, who will repair your towers now? Wait, what am I saying? I don't need to repair your towers or my towers or anyone's towers. I don't need to keep anyone else alive. I can finally rest. Right? Right? Come, Death! You shall not escape me! Rid me of my bad hips!");
        thirdDeath_R.Add("unpause:raol");

        thirdDeath_B.Add("Balthasar: Ah, Reaper, fancy meeting you here...");
        thirdDeath_B.Add("Raol: Sounds like you really did know the Reaper after all. He's probably not too pleased with you after we murdered so many of his minions. I don't even feel bad about it. They're endless. Like new technology. Just when you've learned to dial the phone, they tell you now you have to do the email instead...");
        thirdDeath_B.Add("Thob: No! Balthasar! Do you mind if I...check your pockets for anything shiny? Just to comfort me in my last moments, you know? I might have stolen some of it already. But I was planning to return it, honest. Ooh, this looks pretty...");
        thirdDeath_B.Add("Jolie: You sound delighted to meet the Reaper. Well, when you do, ask him why he hasn't come for me. I've been very patient. Never mind, I'm going to go ask him myself.");
        thirdDeath_B.Add("unpause:bal");

        thirdDeath_T.Add("Thob: When I die, check my pockets...I'm sorry...I couldn't help myself...");
        thirdDeath_T.Add("Raol: I can't loot your body, stupid. You've got too much silver on you. I guess you can't hear me now. Without everyone else's towers, I won't be able to last much longer against these never-ending waves. As endless as new technology. Just when you've learned to dial the phone, they tell you now you have to learn to do the email too... ");
        thirdDeath_T.Add("Balthasar: Check your pockets? Ah, now I comprehend. You have successfully relieved me of my Icicle of Melting Ice, my legendary wand that is now an ammunition-less sniper rifle, and...even my killer sheep skulls? And I had all these items on my person but an hour ago. You stole them over the course of this battle? I commend you for your skill.");
        thirdDeath_T.Add("Jolie: You stole something, didn't you? Let me see...my scabbard! You scoundrel! To be fair, I didn't notice it was missing. Well done. Not that I'll be needing this scabbard much longer, anyway. Everyone else is dead, so I don't need to fight anymore. Take me with you!");
        thirdDeath_T.Add("unpause:thob");

        thirdDeath_J.Add("Jolie: Ah, sweet release! Let me slumber!");
        thirdDeath_J.Add("Raol: Jolie? Jolie? Gone...but at least you seem much happier now. Guess it's just me. Me and the never-ending waves. As endless as new technology. Just when you've learned to dial the phone, they tell you now you have to learn the email too. It seems inevitable to fight against progress. But I fight on.");
        thirdDeath_J.Add("Balthasar: Never have I known someone so eager to rendezvous with death, yet so unable to do so in an expeditious manner. It speaks volumes that you acquiesced, however begrudgingly, to protect your compatriots for so long.");
        thirdDeath_J.Add("Thob: No! Jolie! I never got to tell you...I stole your earrings that one time. I also returned them when you didn't catch me. You were asleep the whole time, so...but I still feel bad about it. I guess it's just me now. I'll memorialize all of you in song. Perhaps if it's even good enough, the Reaper will let me go.");
        thirdDeath_J.Add("unpause:jolie");

        //the following death scenes only have one speaker, so yay!

        lastDeath_R.Add("Raol: Kol Kol...it's been so long. Have you gotten yourself into trouble again? Well, look at the trouble I'm in now. Mother Moon, I shall still howl to you...even in death...");
        lastDeath_R.Add("endgame");

        lastDeath_B.Add("Balthasar: Ludvig, my flying feathered familiar, you are the only witness to my final moments.");
        lastDeath_B.Add("Ludvig: Hoot!");
        lastDeath_B.Add("Balthasar: I have left the mortal coil countless times before. My heart clenches with the premonition that I will not return this time.");
        lastDeath_B.Add("Ludvig: Hoot hoot...");
        lastDeath_B.Add("Balthasar: Still, I admit curiosity in seeing what death has to offer...");
        lastDeath_B.Add("endgame");

        lastDeath_T.Add("Thob: Well, this is embarrassing...I meant to return the treasures I stole from everyone else, but they died before they could catch me...I really did mean to bring it up...I suppose it doesn't matter now...at least I'll die with an apology on my lips...and the dignity of knowing that I was never felled by a tree. Wait. These scythes are made from wood...no...");
        lastDeath_T.Add("endgame");

        lastDeath_J.Add("Jolie: Ow! But...worth it...I bet if I close my eyes now, I can die in my sleep...like I've always wanted...though really, any type of death would have been fine...zzz...");
        lastDeath_J.Add("endgame");

        //randomize
        bonePickUp_R.Add("Raol: Those bones must smell terrible, but I've lost my sense of smell.");
        bonePickUp_R.Add("Raol: Better take these to get processed.");
        bonePickUp_R.Add("Raol: These bones won't process themselves.");
        bonePickUp_R.Add("Raol: I�ll be sure to put aside some of these for you, Kol Kol.");
        bonePickUp_R.Add("Raol: I wish I had all those bones I buried in my backyard now.");
        bonePickUp_R.Add("Raol: Mother Moon, a humble offering to you...");
        

        bonePickUp_B.Add("Balthasar: Curious that our enemies would dissipate, only to drop precisely what we need to defeat them.");
        bonePickUp_B.Add("Balthasar: These raw materials must be processed in the retirement home's serendipitously placed bone-processing machine.");
        bonePickUp_B.Add("Balthasar: I confess myself astonished that these bones don't explode when I pick them up. Most things do.");
        bonePickUp_B.Add("Balthasar: Bones are made up of a mineral phase, hydroxyapatite, an organic phase and water. How curious!");
        bonePickUp_B.Add("Balthasar: This reminds me of that time I was transmogrified into a skeleton myself.");
        bonePickUp_B.Add("Balthasar: It is most vexing how precious these osseous remains are to us, considering that calcium is not even a precious mineral.");

        bonePickUp_T.Add("Thob: I wonder if reapers have a wishing bone. I could really use one right now!");
        bonePickUp_T.Add("Thob: There are many bones like this, but this one is mine.");
        bonePickUp_T.Add("Thob: I bet one could make a fine instrument out of these bones.");
        bonePickUp_T.Add("Thob: Loot!");
        bonePickUp_T.Add("Thob: Treasure!");
        bonePickUp_T.Add("Thob: Shiny! Okay, not that shiny, but still precious!");

        bonePickUp_J.Add("Jolie: Great. Another heavy bone to lug around.");
        bonePickUp_J.Add("Jolie: How am I supposed to carry these and move my wheelchair at the same time?");
        bonePickUp_J.Add("Jolie: Transporting bones reminds me of my army days when I carried around firewood and people yelled at me.");
        bonePickUp_J.Add("Jolie: I wish these bones were me. But here I am, collecting bones.");
        bonePickUp_J.Add("Jolie: Hey reaper, I have a bone to pick with you. Ha ha.");
        bonePickUp_J.Add("Jolie: Funny how I wish those were my bones. Funny bone. Ha ha.");

        towerBuilt_R.Add("Raol: Mother Moon, I dedicate this shrine to you.");
        towerBuilt_R.Add("Raol: You can never have enough shrines to Mother Moon.");
        towerBuilt_R.Add("Raol: Every arrow shot comes closer to lunar perfection.");
        towerBuilt_R.Add("Raol: Fly arrow, straight and true... Just like in my youth.");
        towerBuilt_R.Add("Raol: A real alpha male knows how to build a tower all by himself.");
        towerBuilt_R.Add("Raol: A real alpha male doesn't need to read the assembly instructions.");

        towerBuilt_B.Add("Balthasar: Behold this architectural splendor!");
        towerBuilt_B.Add("Balthasar: Ludvig, feel free to roost at the top.");
        towerBuilt_B.Add("Balthasar: Be not perturbed, it's perfectly safe. I think.");
        towerBuilt_B.Add("Balthasar: Sometimes I wonder if my perception of sound architecture is fundamentally flawed. Oh well!");
        towerBuilt_B.Add("Balthasar: Behold the nigh non-Euclidean architecture of my tower!");

        towerBuilt_T.Add("Thob: Perfect.");
        towerBuilt_T.Add("Thob: Dwarven-forged.");
        towerBuilt_T.Add("Thob: And they said I was only good with a lute.");
        towerBuilt_T.Add("Thob: A prelude to an architectural masterpiece, I dare say.");
        towerBuilt_T.Add("Thob: All this building makes a dwarf thirsty. I wonder if we have anymore ale back inside...");

        towerBuilt_J.Add("Jolie: Can I stop building yet?");
        towerBuilt_J.Add("Jolie: Did we really need another one of these?");
        towerBuilt_J.Add("Jolie: I'd like to go to bed now.");
        towerBuilt_J.Add("Jolie: I thought coming to the retirement home meant no more work.");
        towerBuilt_J.Add("Jolie: Why yes, I am a Die-IY person. It�s a pun on DIY person, I came up with it by myself.");

        towerUpgrade_R.Add("Raol: A moon as full as my heart.");
        towerUpgrade_R.Add("Raol: Let's see what the gods think about this.");
        towerUpgrade_R.Add("Raol: And I did all that without needing to \"look it up inside the internet.\"");
        towerUpgrade_R.Add("Raol: Mother Moon, watch over us!");
        towerUpgrade_R.Add("Raol: I may still wane, but Mother Moon waxes!");

        towerUpgrade_B.Add("Balthasar: I added natural roosting spaces that should attract nocturnal creatures.");
        towerUpgrade_B.Add("Balthasar: Finally, somewhere worthy to cache my tank ammunition, since I seem to have misplaced my tank.");
        towerUpgrade_B.Add("Balthasar: The exterior may seem overly convoluted, but I promise it is architecturally sound.");
        towerUpgrade_B.Add("Balthasar: Unlimited POWER!");
        towerUpgrade_B.Add("Balthasar: How did I do it? It�s the power of the arcane arts. There is no necessity for further elucidation.");

        towerUpgrade_T.Add("Thob: Such shiny gems!");
        towerUpgrade_T.Add("Thob: Another dwarven masterpiece.");
        towerUpgrade_T.Add("Thob: Who knew I had inherited my grandfather's mastery of stonecraft?");
        towerUpgrade_T.Add("Thob: Dwarven architecture is the greatest architecture in the world!");
        towerUpgrade_T.Add("Thob: Now this is a mighty tower worthy of a song!");

        towerUpgrade_J.Add("Jolie: Surely it's good enough now.");
        towerUpgrade_J.Add("Jolie: Now it can swing in a wider arc. Happy?");
        towerUpgrade_J.Add("Jolie: Zzz...did I build something?");
        towerUpgrade_J.Add("Jolie: It's no longer a...bare-bones fortification. Gods, even my sense of humor died before me.");
        towerUpgrade_J.Add("Jolie: Hauling these dumb stones takes a lot. Almost like...tomb stones. Ha ha.");

        towerRepair_J.Add("Jolie: Am I done repairing yet?");
        towerRepair_J.Add("Jolie: Figure. They always forget to allocate resources for maintenance.");
        towerRepair_J.Add("Jolie: At least in the army, I was working for a reason. Or was I?");
        towerRepair_J.Add("Jolie: Finally I lay my sword to rest... Only to pick up a hammer.");
        towerRepair_J.Add("Jolie: To be honest, I�m more used to breaking things with a hammer.");

        towerRepair_R.Add("Raol: I...will...repair...until...the...moon...shines!");
        towerRepair_R.Add("Raol: I wonder what Kol Kol would think of me now.");
        towerRepair_R.Add("Raol: There are two ways to do this: my way and the wrong way.");
        towerRepair_R.Add("Raol: If these parts were loose enough to fall out, they�ll also go back in if I hammer hard enough!");
        towerRepair_R.Add("Raol: I made it myself, so ain�t nobody gonna tell me how to fix it!");


        towerRepair_B.Add("Balthasar: With a wave of my finger and a flick of my wand, I hope it won�t explode, and make the harm undone!");
        towerRepair_B.Add("Balthasar:The structural integrity of this construction must be reinforced post-haste!");
        towerRepair_B.Add("Preventative maintenance is always superior to starting from nothing. I ought to know that by now.");
        towerRepair_B.Add(" Arise, my hammer! Descend, my hammer! Arise! This is much more fun when I speak the movements out loud.");
        towerRepair_B.Add("If I install a bone-processing machine in my tower, might it be able to automate its repairs?");

        towerRepair_T.Add("Thob: Hi ho, hi ho, it�s off to work we go!");
        towerRepair_T.Add("Thob: This would be much easier if there were six more dwarves. They could work while I sing for them!");
        towerRepair_T.Add("Thob: No one will notice that the gem I embed here belongs to them, right?");
        towerRepair_T.Add("Thob: If a tree falls alone in the forest, I'm sad I didn't get to see it go.");
        towerRepair_T.Add("Thob: All you got to do is hammer to a rhythm.");

        towerFullRepair_J.Add("Jolie: Zzz...oh, is it done?");
        towerFullRepair_J.Add("Jolie: Why did I do this?");
        towerFullRepair_J.Add("Jolie: Huzzah. It is finished.");
        towerFullRepair_J.Add("Jolie: Ugh. I�m so done with this.");
        towerFullRepair_J.Add("Jolie: Mrs. Cooper reporting in, repairs completed. Finally.");

        towerFullRepair_R.Add("Raol: And keep your filthy hands off this shrine.");
        towerFullRepair_R.Add("Raol: There. That should keep them busy for a bit.");
        towerFullRepair_R.Add("Raol: My people know how to shove arrows where the moon don't shine.");
        towerFullRepair_R.Add("Raol: See? Better than new. Alpha males don't need instruction booklets.");
        towerFullRepair_R.Add("Raol: All done! Couldn�t have done it better myself.");

        towerFullRepair_B.Add("Balthasar: As chaotic as new!");
        towerFullRepair_B.Add("Balthasar: More complex than its previous iteration!");
        towerFullRepair_B.Add("Balthasar: Excellent craftmanship, Balthasar. Why, my deepest gratitude for your compliments, Balthasar.");
        towerFullRepair_B.Add("Balthasar: I wonder how tall I could go with the next addition...");
        towerFullRepair_B.Add("Balthasar: I could do better, but alas, I am constrained by the technological advancements of our era.");

        towerFullRepair_T.Add("Thob: Now I'm ready for some ale!");
        towerFullRepair_T.Add("Thob: I've lived three centuries. May this tower last even longer!");
        towerFullRepair_T.Add("Thob: It's got that brand new shiny gleam again.");
        towerFullRepair_T.Add("Thob: This reminds me of a song that says broken things made whole again are all the more beautiful.");
        towerFullRepair_T.Add("Thob: You know how they say a piece of art is never truly finished? It can always be improved upon--especially with more gems!");

        waveStarted.Add("Raol: Mother Moon! They just keep coming!");
        waveStarted.Add("Balthasar: Our enemies are inexhaustible.");
        waveStarted.Add("Thob: I was hoping for an opportunity to catch my breath and compose a verse.");
        waveStarted.Add("Jolie: Zzz...what, there's more already?");
        waveStarted.Add("Raol: The dark moon hangs over us... Mother Moon preserve us.");
        waveStarted.Add("Raol: Old age has caught up with me. The hunter has become the hunted.");
        waveStarted.Add("Balthasar: The tide of our foes seems unrelenting, just like telemarketing!");
        waveStarted.Add("Balthasar: Maybe this will convince the head nurse of my proposal to install new safety measures to the retirement home.");
        waveStarted.Add("Thob: A one, and a two, and a one, two, three, four!");
        waveStarted.Add("Thob: Last time I had a crowd that wanted to get a hold of me this bad was over a century ago! Sadly.");
        waveStarted.Add("Jolie: I remember fighting an endless horde once. Back then, I didn�t even need to draw my sword.");
        waveStarted.Add("Jolie: They seem endless. Just like my days waiting for the sweet embrace of death.");

        waveDefeated.Add("Raol: That's the last of them for now.");
        waveDefeated.Add("Jolie: Let's take advantage of the time between waves to nap.");
        waveDefeated.Add("Balthasar: Indubitably, the Reaper will send forth more formidable creatures in the next wave.");
        waveDefeated.Add("Thob: Drinks, anyone?");
        waveDefeated.Add("Raol: If only I had the time to track down where our enemies come from...");
        waveDefeated.Add("Raol: Mother Moon still watches over us, stay strong.");
        waveDefeated.Add("Jolie: Don�t bother waking me up for the next round.");
        waveDefeated.Add("Jolie: Even the army had more meaningful work to offer me than this nonsense.");
        waveDefeated.Add("Balthasar: Marvelous! This means we have time for me to regale you with another recollection of mine, and I know just the one for this occasion...");

        returnBase_R.Add("Raol: Mother Moon, I cannot run as fast as I used to.");
        returnBase_B.Add("Balthasar: I shall endeavor to examine bone-processing contraption while I am present.");
        returnBase_T.Add("Thob: I could use an ale before I head out again.");
        returnBase_J.Add("Jolie: Finally, I need a nap.");

        departBase_R.Add("Mother Moon, lend me your courage.");
        departBase_B.Add("Balthasar: Perhaps this time I'll find out where I left my bag of time sand.");
        departBase_T.Add("Thob: I'm getting better at running and strumming.");
        departBase_J.Add("Jolie: Zzz...really? I just fell asleep too.");

        enterAura_R.Add("Raol: How does the music make me stronger when I can barely hear it without hearing aids?");
        enterAura_B.Add("Balthasar: An observation: The strengthening aura of this tower permits us to leave it as quickly as possible.");
        enterAura_T.Add("Thob: Ah, the songs of home.");
        enterAura_J.Add("Jolie: This music isn't half bad.");
        
        levelUp_R.Add("Raol: I've got it. The tower will be stronger with a full moon!");
        levelUp_B.Add("Balthasar: I now consider myself qualified to construct a more efficacious explosive structure.");
        levelUp_T.Add("Thob: I think I know how to improve on my tower design.");
        levelUp_J.Add("Jolie: I suppose I can upgrade my tower now, if I really want to.");

    }
}
