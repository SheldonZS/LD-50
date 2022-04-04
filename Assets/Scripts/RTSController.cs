using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RTSController : MonoBehaviour
{
    public static RTSController instance { get; private set; }
    public int bones = 0;

    public Sprite[] build;
    public Sprite[] repair;
    public Sprite[] upgrade;
    private Image buildButtonImage;
    private Image repairButtonImage;
    private Image upgradeButtonImage;
    private Text bonesText;
    public Text repairErrorText, repairCostText, buildErrorText, buildCostText, upgradeErrorText, upgradeCostText;
    private Button buildButton, repairButton, upgradeButton;

    public Camera camera { get; private set; }
    //[HideInInspector] 
    public GameObject selected = null;
    public List<GameObject> heroes;

    public Collider2D homeBase;
    public Transform gridAnchor { get; private set; }
    public Commands command { get; private set; }

    public DialogueBox dialogueBox { get; private set; }
    public DialogueManager DM { get; private set; }
    public DataBucket db { get; private set; }
    //public CursorCollider mouseOver { get; private set; }
    public WaveManager wave;

    public bool raol_alive;
    public bool bal_alive;
    public bool thob_alive;
    public bool jolie_alive;

    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        selected = null;
        camera = Camera.main;

        gridAnchor = GameObject.Find("Grid Anchor").transform;
        buildButtonImage = GameObject.Find("Build").GetComponent<Image>();
        repairButtonImage = GameObject.Find("Repair").GetComponent<Image>();
        upgradeButtonImage = GameObject.Find("Upgrade").GetComponent<Image>();
        dialogueBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        DM = GameObject.Find("DialogueManager").GetComponent <DialogueManager>();
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        bonesText = GameObject.Find("BonesText").GetComponent<Text>();
        repairErrorText = GameObject.Find("repairErrorText").GetComponent<Text>();
        repairCostText = GameObject.Find("repairCostText").GetComponent<Text>();
        buildErrorText = GameObject.Find("buildErrorText").GetComponent<Text>();
        buildCostText = GameObject.Find("buildCostText").GetComponent<Text>();
        upgradeErrorText = GameObject.Find("upgradeErrorText").GetComponent<Text>();
        upgradeCostText = GameObject.Find("upgradeCostText").GetComponent<Text>();
        buildButton = GameObject.Find("Build").GetComponent<Button>();
        repairButton = GameObject.Find("Repair").GetComponent<Button>();
        upgradeButton = GameObject.Find("Upgrade").GetComponent<Button>();
        wave = GetComponent<WaveManager>();
        //mouseOver = GameObject.Find("MouseOver").GetComponent<CursorCollider>();
    }

    private void Start()
    {
        raol_alive = true;
        bal_alive = true;
        thob_alive = true;
        jolie_alive = true;

        bones = 200;
        ResetButtons();

        TowerBase home = homeBase.GetComponent<TowerBase>();
        home.SetHealth(home.maxHealth);

    }


    public void WaveDefeated()
    {

    }

    public bool GridContains(Vector2 location, string tag)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(gridAnchor.TransformPoint(RoundToGrid(location, 0.5f)), Vector2.one * 0.9f, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == tag)
                return true;
        }

        return false;
    }

    public TowerBase GetTowerAt(Vector2 location)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(gridAnchor.TransformPoint(RoundToGrid(location, 0.5f)), Vector2.one * 0.1f, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "Tower")
                return hit.gameObject.GetComponent<TowerBase>();
        }

        return null;
    }

    public Vector2 RoundToGrid(Vector2 loc, float Yoffset = 0f)
    {
        loc.x = Mathf.RoundToInt(loc.x);
        loc.y = Mathf.FloorToInt(loc.y) + Yoffset;

        return loc;
    }

    // Update is called once per frame
    void Update()
    {
        bonesText.text = bones.ToString();

        if (selected == null)
        {
            Reset();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log("Clicked at world position: " + mouseWorldPosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (hit.collider.gameObject != selected)
                    {
                        selected = hit.collider.gameObject;

                        UpdateButtons();

                        if (db.tutorialMode == 1 && selected.name == "Raol")
                        {
                            StartCoroutine(dialogueBox.PlayText(DM.tutorial1, TextMode.imm));
                            db.tutorialMode++;
                        }
                        Reset();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selected = null;
            Reset();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = heroes.IndexOf(selected);

            selected = heroes[(index + 1) % heroes.Count];

            Reset();
        }

        if (Input.GetKeyDown(KeyCode.B))
            BuildButton();
        if (Input.GetKeyDown(KeyCode.R))
            RepairButton();
        if (Input.GetKeyDown(KeyCode.U))
            UpgradeButton();
    }

    public Vector2 MouseToGrid()
    {
        return gridAnchor.InverseTransformPoint(camera.ScreenToWorldPoint(Input.mousePosition));
    }

    public void Reset()
    {
        command = Commands.move;
        buildButtonImage.overrideSprite = build[0];
        repairButtonImage.overrideSprite = repair[0];
        upgradeButtonImage.overrideSprite = upgrade[0];
    }
    public void BuildButton()
    {
        command = Commands.build;

        buildButtonImage.overrideSprite = build[1];
        repairButtonImage.overrideSprite = repair[0];
        upgradeButtonImage.overrideSprite = upgrade[0];
    }

    public void RepairButton()
    {
        command = Commands.repair;

        buildButtonImage.overrideSprite = build[0];
        repairButtonImage.overrideSprite = repair[1];
        upgradeButtonImage.overrideSprite = upgrade[0];
    }


    public void UpgradeButton()
    {
        command = Commands.upgrade;

        buildButtonImage.overrideSprite = build[0];
        repairButtonImage.overrideSprite = repair[0];
        upgradeButtonImage.overrideSprite = upgrade[1];
    }

    public void Say(string text)
    {
        if (dialogueBox != null)
        StartCoroutine(AnimateText(text));
    }
    
    public void Say(Hero hero, string text)
    {
        //if (dialogueBox != null) ;
        //StartCoroutine(AnimateText(hero.gameObject.name + ": " + text));
    }

    public IEnumerator AnimateText(string text)
    {
        yield return dialogueBox.PlayText(new List<string>() { text }, TextMode.queue);
        yield return dialogueBox.CloseWindow();
    }

    public void UpdateButtons()
    {
        ResetButtons();

        if (selected.tag != "Player") return;

        Hero hero = selected.GetComponent<Hero>();

        int heroBuildCost = hero.towers[0].GetComponent<TowerBase>().buildCost;
        buildCostText.text = "Build Cost: " + heroBuildCost;

        if (hero.building)
        {
            buildErrorText.text = "Building In Progress";
            buildButton.enabled = false;
        }
        else if (heroBuildCost <= bones)
        {
            if (selected.name == "Thob")
            {
                buildErrorText.text = "Aura Slows Monsters & Speeds Heroes";
            }

            buildButton.enabled = true;
        }
        else
        {
            buildErrorText.text = "Insufficient Bones";
            buildButton.enabled = false;

        }


        if (hero.repairing)
        {
            repairErrorText.text = "Repairs In Progress";
            repairButton.enabled = false;

        }
        else
        {
            repairErrorText.text = "Click to Repair";
            repairButton.enabled = true;

        }

        //repairCostText.text = "Repair Cost: " + (hero.towers[0].GetComponent<TowerBase>().repairCost);
        if (hero.level >= 2)
        {
            upgradeButton.enabled = true;


            int heroUpgradeCost = hero.towers[0].GetComponent<TowerBase>().upgradeCost;

            if (hero.upgrading)
            {
                upgradeErrorText.text = "Upgrade in Progress";
                upgradeButton.enabled = false;

            }
            else if (heroUpgradeCost <= bones)
            {
                upgradeErrorText.text = "Click to Upgrade";
                upgradeCostText.text = "Upgrade Cost: " + heroUpgradeCost;
                upgradeButton.enabled = true;
            }
            else
            {
                upgradeErrorText.text = "Insufficient Bones";
                upgradeButton.enabled = false;


            }

        }
        else
        {
            upgradeErrorText.text = "Hero Level Too Low";

        }
    }

    public void ResetButtons()
    {
        buildErrorText.text = "Select Hero";
        repairErrorText.text = "Select Hero";
        upgradeErrorText.text = "Select Hero";
        buildCostText.text = "Build";
        repairCostText.text = "Repair";
        upgradeCostText.text = "Upgrade";

        buildButton.enabled = false;
        repairButton.enabled = false;
        upgradeButton.enabled = false;
    }
}

[System.Serializable]
public struct SpawnPoint
{
    public Vector2Int location;
    public Vector2Int[] path;
}