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
    private Image buildButton;
    private Image repairButton;
    private Image upgradeButton;
    private Text bonesText;

    public Camera camera { get; private set; }
    //[HideInInspector] 
    public GameObject selected = null;
    public List<GameObject> heroes;

    public GameObject[] monsters;
    public SpawnPoint[] spawnPoints;

    public Collider2D homeBase;
    public Transform gridAnchor { get; private set; }
    public Commands command { get; private set; }

    public DialogueBox dialogueBox { get; private set; }
    public DialogueManager DM { get; private set; }
    public DataBucket db { get; private set; }
    //public CursorCollider mouseOver { get; private set; }

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
        buildButton = GameObject.Find("Build").GetComponent<Image>();
        repairButton = GameObject.Find("Repair").GetComponent<Image>();
        upgradeButton = GameObject.Find("Upgrade").GetComponent<Image>();
        dialogueBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        DM = GameObject.Find("DialogueManager").GetComponent <DialogueManager>();
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        bonesText = GameObject.Find("BonesText").GetComponent<Text>();

    //mouseOver = GameObject.Find("MouseOver").GetComponent<CursorCollider>();
}

private void Start()
    {
        raol_alive = true;
        bal_alive = true;
        thob_alive = true;
        jolie_alive = true;
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            int spawner = Random.Range(0, spawnPoints.Length);
            MonsterBase monster = Instantiate(monsters[0], gridAnchor).GetComponent<MonsterBase>();
            monster.SetSpawn(spawnPoints[spawner]);
        }
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

                        if (db.tutorialMode == 1 && selected.name == "Raol")
                        {
                            StartCoroutine(dialogueBox.PlayText(DM.tutorial1, true));
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
        buildButton.overrideSprite = build[0];
        repairButton.overrideSprite = repair[0];
        upgradeButton.overrideSprite = upgrade[0];
    }
    public void BuildButton()
    {
        command = Commands.build;

        buildButton.overrideSprite = build[1];
        repairButton.overrideSprite = repair[0];
        upgradeButton.overrideSprite = upgrade[0];
    }

    public void RepairButton()
    {
        command = Commands.repair;

        buildButton.overrideSprite = build[0];
        repairButton.overrideSprite = repair[1];
        upgradeButton.overrideSprite = upgrade[0];
    }


    public void UpgradeButton()
    {
        command = Commands.upgrade;

        buildButton.overrideSprite = build[0];
        repairButton.overrideSprite = repair[0];
        upgradeButton.overrideSprite = upgrade[1];
    }

    public void Say(string text)
    {
        if (dialogueBox != null)
        StartCoroutine(AnimateText(text));
    }
    
    public void Say(Hero hero, string text)
    {
        if (dialogueBox != null)
        StartCoroutine(AnimateText(hero.gameObject.name + ": " + text));
    }

    public IEnumerator AnimateText(string text)
    {
        yield return dialogueBox.PlayText(new List<string>() { text }, true);
        yield return dialogueBox.CloseWindow();
    }


}

[System.Serializable]
public struct SpawnPoint
{
    public Vector2Int location;
    public Vector2Int[] path;
}