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

    List<string> enemiesOnPath = new List<string>();
    List<string> howBonesWork = new List<string>();
    List<string> cannotRepairHome = new List<string>();


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
        testWords.Add("Raol: Stop");
        testWords.Add("Balthasar: Yeah no");
        testWords.Add("Jolie: Zzzz and let's just add a lot of text to fill up one or three or even nine lines and yeah, we can keep going all day, if that's what's required.");
        testWords.Add("Thob: Thought!");
        testWords.Add("Jolie: What? Huh?");
    }
}
