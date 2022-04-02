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

        if (RTSController.instance.selected != gameObject)
        {
            highlight.SetBool("Selected", false);

            ExecuteCommand();

            return;
        }

        highlight.SetBool("Selected", true);
        bool manualMove = false;

        if ((currentCommand.command == Commands.idle || currentCommand.command == Commands.move) &&
            (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.A) ||Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) ||Input.GetKey(KeyCode.UpArrow) ||Input.GetKey(KeyCode.RightArrow) ||Input.GetKey(KeyCode.DownArrow)))
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
        else if(Input.GetMouseButtonDown(1))
        {
            Vector2 clickPos = RTSController.instance.MouseToGrid();
            Debug.Log("Right Clicked at world position " + clickPos);

            if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
            {
                if (currentCommand.command == Commands.idle || !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                    commands.Clear();

                commands.Enqueue(new HeroCommand(Commands.move,clickPos));
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

                if (mag <= moveSpeed * Time.deltaTime)
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

            case Commands.build:    return false;
            case Commands.repair: return false;
            case Commands.upgrade: return false;


            case Commands.idle:
            default:
                rb.velocity = Vector2.zero;
                return false;
        }

    }

}


    public class HeroCommand
{
    public Commands command = Commands.idle;
    public Vector2 location;
    public TowerBase tower;

    public HeroCommand(Commands com)
    {
        command = com;
        tower = null;
    }
    public HeroCommand(Commands com, Vector2 dest, TowerBase target = null)
    {
        command = com;
        location = dest;
        tower = target;
    }
}

public enum Commands { idle, move, build, repair, upgrade}