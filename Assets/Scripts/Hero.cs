using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private int health;
    public int maxHealth;

    public float moveSpeed = 200f;
    public float buildSpeedMultiplier = 1f;

    public GameObject[] towers;
    public GameObject[] attackPrefabs;

    private HealthBar healthBar;
    private List<HeroCommand> commands;
    private Animator animator;
    private Animator highlight;
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private RTSController RTSC;

    private float repairDecimal;
    private TowerBase repairTower;

    public bool upgrading;
    private float upgradeStartTime;

    public bool building;

    private int bonesCarried = 0;

    private DataBucket db;
    private DialogueBox diaBox;
    private DialogueManager DM;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        RTSC = GetComponentInParent<RTSController>();

        commands = new List<HeroCommand>();
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

        healthBar.UpdateHealth(health, maxHealth);

        highlight = transform.GetChild(1).GetComponent<Animator>();
        highlight.SetBool("Selected", false);

        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        DM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //

   

        if ((db.tutorialMode >=2 && this.name == "Raol") || db.tutorialMode >= 4)
        {

            if (commands.Count == 0) commands.Add(new HeroCommand(Commands.idle));
            HeroCommand currentCommand = commands[0];

            if (RTSC.selected != gameObject)
            {
                highlight.SetBool("Selected", false);

                ExecuteCommand();



                return;
            }

            if (collider.IsTouching(RTSC.homeBase))
            {
                RTSC.bones += bonesCarried;
                bonesCarried = 0;
            }

            highlight.SetBool("Selected", true);
            bool manualMove = false;
            if ((currentCommand.command == Commands.idle || currentCommand.command == Commands.move) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                if (!building)
                {
                    if (db.tutorialMode == 2)
                    {
                        ShowText(DM.tutorial2);
                        db.tutorialMode++;
                    }
                    commands.Clear();
                    commands.Add(new HeroCommand(Commands.idle));

                    Vector2 direction = Vector2.zero;

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) direction.y += 1;
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction.x -= 1;
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) direction.y -= 1;
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction.x += 1;

                    rb.velocity = direction.normalized * moveSpeed;
                    manualMove = true;
                }

                
            }
            else if (Input.GetMouseButtonDown(1) && !building)
            {
                if (db.tutorialMode == 2)
                {
                    ShowText(DM.tutorial2);
                    db.tutorialMode++;
                }
                Vector2 clickPos = RTSC.MouseToGrid();
                //RTSC.Say(this, "Right Clicked at world position " + clickPos);

                if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
                {
                    if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                    {
                        commands.Clear();
                        rb.velocity = Vector2.zero;
                    }

                    commands.Add(new HeroCommand(Commands.move, clickPos));
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPos = RTSC.MouseToGrid();

                if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
                {

                    switch (RTSC.command)
                    {
                        case Commands.build:
                            if (RTSC.GridContains(RTSC.MouseToGrid(), "Monster"))
                                RTSC.Say(this, "There are monsters there");
                            if (RTSC.GridContains(RTSC.MouseToGrid(), "Tower"))
                                RTSC.Say(this, "There's already a tower there");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.build, RTSC.RoundToGrid(RTSC.MouseToGrid()), towers[0]));
                            }
                            break;
                        case Commands.repair:
                            TowerBase targetTower = RTSC.GetTowerAt(clickPos);
                            if (targetTower == null)
                                RTSC.Say(this, "No tower here");
                            else if (targetTower.builder != this)
                                RTSC.Say(this, "This is not my tower");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.repair, clickPos, targetTower.gameObject));
                            }
                            break;
                        case Commands.upgrade:
                            targetTower = RTSC.GetTowerAt(clickPos);
                            if (targetTower == null)
                                RTSC.Say(this, "No tower here");
                            else if (targetTower.builder != this)
                                RTSC.Say(this, "This is not my tower");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.upgrade, clickPos, targetTower.gameObject));
                            }
                            break;
                        case Commands.move:
                        default:
                            /*
                            Vector3 mouseWorldPosition = RTSC.camera.ScreenToWorldPoint(Input.mousePosition);

                            //RTSC.Say(this, "Clicked at world position: " + mouseWorldPosition);

                            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

                            bool canMove = true;
                            foreach (RaycastHit2D hit in hits)
                            {
                                if (hit.collider.gameObject.tag == "Player")
                                {
                                    if (hit.collider.gameObject != gameObject)
                                    {
                                        canMove = false;
                                    }
                                }
                            }

                            if (canMove)
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                {
                                    commands.Clear();
                                    rb.velocity = Vector2.zero;
                                }

                                commands.Add(new HeroCommand(Commands.move, clickPos));
                            }
                            */
                            break;
                    }
                }
            }

            if (!manualMove)
                ExecuteCommand();
        }
        

    }

    public void PickUpBones(int x)
    {
        bonesCarried += x;
    }

    private void ExecuteCommand()
    {
        if (commands.Count == 0)
            commands.Add(new HeroCommand(Commands.idle));

        if (executeCommand(commands[0]))
            commands.RemoveAt(0);
    }
    private bool executeCommand(HeroCommand command)
    {
        switch (command.command)
        {
            case Commands.move:
                Vector2 movement = command.location - (Vector2)transform.localPosition;
                float mag = movement.magnitude;

                if (mag <= moveSpeed * Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) || Vector2.Angle(rb.velocity, movement) > 90)
                {
                    transform.localPosition = command.location;
                    rb.velocity = Vector2.zero;
                    return true;
                }
                else
                {
                    rb.velocity = movement.normalized * moveSpeed;
                    return false;
                }

            case Commands.build:
                //check if player is touching the tower build location
                rb.velocity = Vector2.zero;
                Collider2D[] hits = Physics2D.OverlapBoxAll(RTSC.gridAnchor.TransformPoint(RTSC.RoundToGrid(command.location, 0.5f)), Vector2.one * 0.9f, 0);

                bool inPosition = false;
                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject == gameObject)
                    {
                        inPosition = true;
                        break;
                    }
                }

                if (inPosition)
                {
                    if (RTSC.GridContains(command.location, "Monster"))
                    {
                        RTSC.Say(this, "Monsters Here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Tower"))
                    {
                        RTSC.Say(this, "Already a tower here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Path"))
                    {
                        RTSC.Say(this, "Path Here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Obstacle"))
                    {
                        RTSC.Say(this, "Obstacle Here");
                        return true;
                    }

                    building = true;
                    TowerBase newTower = Instantiate(towers[0], RTSC.gridAnchor).GetComponent<TowerBase>();
                    newTower.transform.localPosition = command.location;
                    newTower.builder = this;
                    newTower.SetHealth(1);

                    commands[0] = new HeroCommand(Commands.buildToMax, command.location, newTower.gameObject);

                    RTSC.Reset();
                    return false;
                }
                else
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed;
                    return false;
                }

            case Commands.buildToMax:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                    RTSC.Say(this, "No tower here");
                    repairDecimal = 0;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    float repairAmount = (repairTower.maxHealth / repairTower.buildTime) * buildSpeedMultiplier * Time.deltaTime + repairDecimal;
                    int repairInt = Mathf.FloorToInt(repairAmount);
                    repairTower.Repair(repairInt);

                    if (repairTower.health >= repairTower.maxHealth)
                    {
                        
                        repairDecimal = 0;
                        building = false;
                        if (db.tutorialMode == 3)
                        {
                            ShowText(DM.tutorial3);
                            db.tutorialMode++;
                        }
                        return true;
                    }

                    repairDecimal = repairAmount - repairInt;
                }
                else if (!building)
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed;
                }
                return false;

            case Commands.repair:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                    RTSC.Say(this, "No tower here");
                    repairDecimal = 0;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    float repairAmount = (repairTower.maxHealth / repairTower.buildTime) * buildSpeedMultiplier * Time.deltaTime + repairDecimal;
                    int repairInt = Mathf.FloorToInt(repairAmount);
                    repairTower.Repair(repairInt);

                    if (repairTower.health >= repairTower.maxHealth)
                    {
                        RTSC.Say(this, "Job's done!");
                        repairDecimal = 0;
                        return true;
                    }

                    repairDecimal = repairAmount - repairInt;
                }
                else
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed;
                }
                return false;

            case Commands.upgrade:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                    RTSC.Say(this, "No tower here");
                    upgrading = false;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    if (upgrading == false)
                    {

                        if (repairTower.health < repairTower.maxHealth * 0.5f)
                        {
                            RTSC.Say(this, "This tower is too badly damaged to upgrade");
                            return true;
                        }
                        
                        if (repairTower.operational == false)
                        {
                            RTSC.Say(this, "Finish building this before upgrading");
                            return true;
                        }

                        upgrading = true;
                        upgradeStartTime = Time.time;

                        RTSC.Say(this, "Starting upgrade...");
                        return false;
                    }
                    else
                    {
                        if (Time.time >= upgradeStartTime + repairTower.upgradeTime)
                        {
                            repairTower.Upgrade();
                            RTSC.Say(this, "Job's done!");
                            upgrading = false;
                            return true;
                        }
                    }

                }
                else
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed;
                }
                return false;
            case Commands.idle:
            default:
                rb.velocity = Vector2.zero;
                return false;
        }

    }

    void ShowText(List<string> storyText)
    {
        StartCoroutine(diaBox.PlayText(storyText, true));
    }
}

[System.Serializable]
public class HeroCommand
{
    public Commands command = Commands.idle;
    public Vector2 location;
    public GameObject tower;

    public HeroCommand(Commands com)
    {
        command = com;
        tower = null;
    }
    public HeroCommand(Commands com, Vector2 dest, GameObject target = null)
    {
        command = com;
        location = dest;
        tower = target;
    }
}

public enum Commands { idle, move, build, repair, upgrade, buildToMax}