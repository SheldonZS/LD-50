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

    private HealthBar healthBar;
    private Queue<HeroCommand> commands;
    private Animator animator;
    private Animator highlight;
    private Rigidbody2D rb;
    private RTSController RTSC;

    private float repairDecimal;
    private TowerBase repairTower;
    private float upgradeStartTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        RTSC = GetComponentInParent<RTSController>();

        commands = new Queue<HeroCommand>();
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

        healthBar.UpdateHealth(health, maxHealth);

        highlight = transform.GetChild(1).GetComponent<Animator>();
        highlight.SetBool("Selected", false);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;

        if (commands.Count == 0) commands.Enqueue(new HeroCommand(Commands.idle));
        HeroCommand currentCommand = commands.Peek();

        if (RTSC.selected != gameObject)
        {
            highlight.SetBool("Selected", false);

            ExecuteCommand();

            return;
        }

        highlight.SetBool("Selected", true);
        bool manualMove = false;

        if ((currentCommand.command == Commands.idle || currentCommand.command == Commands.move) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            commands.Clear();
            commands.Enqueue(new HeroCommand(Commands.idle));

            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) direction.y += 1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction.x -= 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) direction.y -= 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction.x += 1;

            rb.velocity = direction.normalized * moveSpeed;
            manualMove = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2 clickPos = RTSC.MouseToGrid();
            Debug.Log("Right Clicked at world position " + clickPos);

            if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
            {
                if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                    commands.Clear();

                commands.Enqueue(new HeroCommand(Commands.move, clickPos));
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            switch(RTSC.command)
            {
                case Commands.build:
                    if (RTSC.GridContains(RTSC.MouseToGrid(), "Monster"))
                        Debug.Log("Monsters over there");
                    if (RTSC.GridContains(RTSC.MouseToGrid(), "Tower"))
                        Debug.Log("Already a tower there");
                    else
                    {
                        if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                            commands.Clear();
                        commands.Enqueue(new HeroCommand(Commands.move, RTSC.RoundToGrid(RTSC.MouseToGrid())));
                        commands.Enqueue(new HeroCommand(Commands.build, RTSC.RoundToGrid(RTSC.MouseToGrid()), towers[0]));
                    }
                    break;
                case Commands.repair:
                    if (RTSC.GridContains(RTSC.MouseToGrid(), "Tower"))
                    {
                        if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                            commands.Clear();
                        commands.Enqueue(new HeroCommand(Commands.move, RTSC.RoundToGrid(RTSC.MouseToGrid())));
                        commands.Enqueue(new HeroCommand(Commands.repair, RTSC.RoundToGrid(RTSC.MouseToGrid())));
                    }
                    else Debug.Log("No tower here");
                    break;
                case Commands.upgrade:
                    if (RTSC.GridContains(RTSC.MouseToGrid(), "Tower"))
                    {
                        if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                            commands.Clear();
                        commands.Enqueue(new HeroCommand(Commands.move, RTSC.RoundToGrid(RTSC.MouseToGrid())));
                        commands.Enqueue(new HeroCommand(Commands.upgrade, RTSC.RoundToGrid(RTSC.MouseToGrid())));
                    }
                    else Debug.Log("No tower here");
                    break;
                case Commands.move:
                default:
                    if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                        commands.Clear();
                    commands.Enqueue(new HeroCommand(Commands.move, RTSC.MouseToGrid()));
                    break;
            }
        }

        if  (!manualMove)
            ExecuteCommand();

    }

    private void ExecuteCommand()
    {
        if (commands.Count == 0)
            commands.Enqueue(new HeroCommand(Commands.idle));

        if (executeCommand(commands.Peek()))
            commands.Dequeue();
    }
    private bool executeCommand(HeroCommand command)
    {
        switch (command.command)
        {
            case Commands.move:
                Vector2 movement = command.location - (Vector2) transform.localPosition;
                float mag = movement.magnitude;

                if (mag <= moveSpeed * Time.deltaTime * 2)
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
                if (RTSC.GridContains(command.location, "Monster"))
                {
                    Debug.Log("Monsters Here");
                    return true;
                }
                else if (RTSC.GridContains(command.location, "Tower"))
                {
                    Debug.Log("Already a tower here");
                    return true;
                }
                TowerBase newTower = Instantiate(towers[0], RTSC.gridAnchor).GetComponent<TowerBase>();
                newTower.transform.localPosition = command.location;
                newTower.SetHealth(1);

                Debug.Log(commands.Peek().command);
                commands.Dequeue();
                commands.Enqueue(new HeroCommand(Commands.repair, command.location, newTower.gameObject));
                Debug.Log(commands.Peek().command);


                for (int i = 1; i < commands.Count; i++)
                    commands.Enqueue(commands.Dequeue());

                RTSC.Reset();
                return false;
            case Commands.repair:
                if (command.tower == null)
                {

                    Collider2D[] hits = Physics2D.OverlapBoxAll(RTSC.gridAnchor.TransformPoint(command.location + Vector2.up * 0.5f), Vector2.one * 0.1f, 0);

                    Debug.Log("Found " + hits.Length + " colliders around coordinates " + (command.location + Vector2.up * 0.5f));

                    foreach (Collider2D hit in hits)
                    {
                        Debug.Log("Collider Found: " + hit.gameObject.name);
                        if (hit.gameObject.tag == "Tower")
                        {
                            repairTower = hit.gameObject.GetComponent<TowerBase>();
                            break;
                        }
                    }
                }
                else repairTower = command.tower.GetComponent<TowerBase>();

                if (repairTower == null || repairTower.health <= 0)
                {
                    Debug.Log("No tower here");
                    repairDecimal = 0;
                    return true;
                }

                float repairAmount = (repairTower.maxHealth / repairTower.buildTime) * buildSpeedMultiplier * Time.deltaTime + repairDecimal;
                int repairInt = Mathf.FloorToInt(repairAmount);
                repairTower.Repair(repairInt);
                
                if(repairTower.health >= repairTower.maxHealth)
                {
                    Debug.Log("Job's done!");
                    repairDecimal = 0;
                    return true;
                }

                repairDecimal = repairAmount - repairInt;
                RTSC.Reset();
                return false;

            case Commands.upgrade:  Debug.Log("Upgrading..."); return false;
                if (command.tower == null)
                {

                    Collider2D[] hits = Physics2D.OverlapBoxAll(RTSC.gridAnchor.TransformPoint(command.location + Vector2.up * 0.5f), Vector2.one * 0.1f, 0);

                    foreach (Collider2D hit in hits)
                    {
                        if (hit.gameObject.tag == "Tower")
                        {
                            repairTower = hit.gameObject.GetComponent<TowerBase>();
                            break;
                        }
                    }


                    if (repairTower == null || repairTower.health <= 0)
                    {
                        Debug.Log("No tower here");
                        return true;
                    }

                    if (repairTower.health < repairTower.maxHealth * 0.5f)
                    {
                        Debug.Log("Too badly damaged to upgrade");
                        return true;
                    }

                    upgradeStartTime = Time.time;
                }

                RTSC.Reset();

                if (Time.time >= upgradeStartTime + repairTower.upgradeTime)
                {
                    Debug.Log("Job's done!");
                    return true;
                }

                return false;


            case Commands.idle:
            default:
                rb.velocity = Vector2.zero;
                return false;
        }

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

public enum Commands { idle, move, build, repair, upgrade}