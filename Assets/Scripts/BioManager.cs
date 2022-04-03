using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioManager : MonoBehaviour
{
    private Button raolButton, balthasarButton, thobButton, jolieButton, tarshaButton, jadenButton;
    private Text bioText;
    private DataBucket db;


    private void Awake()
    {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        bioText = GameObject.Find("bioText").GetComponent<Text>();
        raolButton = GameObject.Find("raolButton").GetComponent<Button>();
        balthasarButton = GameObject.Find("balthasarButton").GetComponent<Button>();
        thobButton = GameObject.Find("thobButton").GetComponent<Button>();
        jolieButton = GameObject.Find("jolieButton").GetComponent<Button>();
        tarshaButton = GameObject.Find("tarshaButton").GetComponent<Button>();
        jadenButton = GameObject.Find("jadenButton").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //db.data.ending = EndingCode.Bal;

        if (db.data.ending == EndingCode.none)
        {
            tarshaButton.enabled = false;
            tarshaButton.GetComponentInChildren<Text>().text = "Locked";
            jadenButton.enabled = false;
            jadenButton.GetComponentInChildren<Text>().text = "Locked";

        }
        else
        {
            tarshaButton.enabled = true;
            tarshaButton.GetComponentInChildren<Text>().text = "";
            jadenButton.enabled = true;
            jadenButton.GetComponentInChildren<Text>().text = "";

        }
        bioText.color = Color.white;
        bioText.text = "Select a character portrait from the right to read their bio.";
    }

    public void CharButtonDown(string character)
    {
        switch (character)
        {
            case "raol":
                bioText.color = new Color(219f / 255f, 133f / 255f, 30f / 255f, 255f);
                bioText.text = "Name: Raol Eon" +
                    "\nAge: 92" +
                    "\nRace/Class: Shifter Ranger" +
                    "\nAlignment: Chaotic Neutral" +
                    "\n\nBackstory: Grumpy and misanthropic, Raol was hoping to retire to a quiet room in the Home for Aged Adventurers and live out the rest of his days gazing at Mother Moon in blissful isolation. Alas, the fight goes on, mostly with nurses trying to convince him that learning to type would be \"good for his fingers.\" \n\nWith his dulled senses of smell and hearing, his hunting days are far behind him. But when the Reaper comes calling, Raol gears up again to evade death, as he has many times before." +
                    "\n\nPlayer: Henna";
                if (db.data.ending == EndingCode.none)
                    bioText.text += "\n\nInevitable Fate: ???";
                else
                    bioText.text += "\n\nInevitable Fate: Attempting to rescue his feathery friend, but needing rescue himself in the process.";
                break;
            case "balthasar":
                bioText.color = new Color(105f / 255f, 165f / 255f, 209f / 255f, 255f);
                bioText.text = "Name: Balthasar Wildstone" +
                    "\nAge: 193" +
                    "\nRace/Class: Half-Elf Mage" +
                    "\nAlignment: Chaotic Good" +
                    "\n\nBackstory: Balthasar doesn't mean any harm, but that doesn't mean he's harmless. His magic is wildly unpredictable and has led to explosive mishaps that he recounts with gusto to disbelieving listeners. \n\nHe insists he once died 13 times in the span of six months, making the Reaper an old acquaintance of his by now. Perhaps there is some credence to this tale, for when the Reaper enters the retirement community and sees Balthasar, his hooded face takes on a look of exasperated recognition." +
                    "\n\nPlayer: Marppuli";
                if (db.data.ending == EndingCode.none)
                    bioText.text += "\n\nInevitable Fate: ???";
                else
                    bioText.text += "\n\nInevitable Fate: Joined the Reaper's quest when threatened with boredom, then immediately forgot its grand purpose.";
                break;
            case "thob":
                bioText.color = new Color(148f / 255f, 73f / 255f, 191f / 255f, 255f);
                bioText.text = "Name: Thob Thunartilat" +
                    "\nAge: 344" +
                    "\nRace/Class: Dwarf Bard" +
                    "\nAlignment: True Neutral" +
                    "\n\nBackstory: Even after stealing enough to afford a comfortable retirement, Thob can't stop himself from chasing the shinies. When the other retirees catch him rifling through their pockets, he apologies immediately, often so politely that they can't help but forgive him. He strums a good tune and makes a good drinking buddy, and everyone around here understands that some things never change. \n\nEspecially Thob's hatred of trees. A tree almost killed him once, which is more than the Reaper's ever done, that's for sure." +
                    "\n\nPlayer: ";
                if (db.data.ending == EndingCode.none)
                    bioText.text += "\n\nInevitable Fate: ???";
                else
                    bioText.text += "\n\nInevitable Fate: Selling the interplanar rights to his 21-movement flute sonata, assuming he can contact a living publisher.";
                break;
            case "jolie":
                bioText.color = new Color(179f / 255f, 40f / 255f, 40f / 255f, 255f);
                bioText.text = "Name: Jolie Cooper" +
                   "\nAge: 86" +
                   "\nRace/Class: Human Fighter" +
                   "\nAlignment: Neutral Tired" +
                   "\n\nBackstory: Jolie is ready to die. She's prepared her will and put her things in order. Now it's time for the long sleep. Why hasn't the Reaper come for her yet? All the years in the army, and she couldn't even get a Near-Death Experience? Maybe the Reaper thinks she's joking when she says she wants to go. But this is her serious face. (It looks exactly like her sarcastic face.) \n\nWell, if the Reaper isn't ready yet, perhaps she'll just nap while she waits. Her wheelchair is pretty comfortable..." +
                   "\n\nPlayer: Weeping Jester";
                if (db.data.ending == EndingCode.none)
                    bioText.text += "\n\nInevitable Fate: ???";
                else
                    bioText.text += "\n\nInevitable Fate: Turning down the Reaper's quest to nap and dream of reincarnating as a housecat.";
                break;
            case "tarsha":
                bioText.color = Color.yellow;
                bioText.text = "Name: Tarsha Motheater" +
                    "\nAge: 88" +
                    "\nRace/Class: Goliath Paladin" +
                    "\nAlignment: Lawful Good" +
                    "\n\nBackstory: Tarsha believes she was born to share the teachings of her God with the world. The Holy Book is wise and says many things, like \"Follow me in worship and you shall receive friendship and stuff,\" and \"Do not eat honey, lo, for it will embalden you.\" (Tarsha has never actually read the Holy Book, but she's confident those passages are in there somewhere.)\n\nThe afterlife is surprisingly full of people to convert, and she hopes to someday increase her conversion count to one." +
                    "\n\nPlayer: Meredith";
                bioText.text += "\n\nInevitable Fate: Spreading boredom to those too polite to cut her off.";
                break;
            case "jaden":
                bioText.color = new Color(136f / 255f, 197f / 255f, 68f / 255f, 128f);
                bioText.text = "Name: Jaden Highhill" +
                    "\nAge:" +
                    "\nRace/Class: Halfling Arcane Trickster" +
                    "\nAlignment: " +
                    "\n\nBackstory: " +
                    "\n\nPlayer: Sheldon";
                bioText.text += "\n\nInevitable Fate: ";
                break;
            default:
                Debug.Log("not a character");
                break;
        }
            

    }

}
