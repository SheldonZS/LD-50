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
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        commands = new Queue<HeroCommand>();
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

        healthBar.UpdateHealth(health, maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;

        if (commands.Count == 0) commands.Enqueue(new HeroCommand(Commands.idle));
        HeroCommand currentCommand = commands.Peek();
        if (RTSController.instance.selected != gameObject)
        {
            animator.SetBool("Selected", false);

            if (commands.Count == 0)
                commands.Enqueue(new HeroCommand(Commands.idle));

            if (executeCommand(commands.Peek()))
                commands.Dequeue();

            return;
        }

        animator.SetBool("Selected", true);

        if (currentCommand.command == Commands.idle || currentCommand.command == Commands.move)
        {
            commands.Clear();
            commands.Enqueue(new HeroCommand(Commands.idle));

            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.A) ||Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.LeftArrow) ||Input.GetKey(KeyCode.UpArrow) ||Input.GetKey(KeyCode.RightArrow) ||Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) direction.y += 1;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction.x -= 1;
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) direction.y -= 1;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction.x += 1;
            }

            rb.velocity = (Vector3)direction.normalized * moveSpeed;
        }
        
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
                    return true;
                }
                else
                {
                    transform.localPosition += (Vector3) movement * moveSpeed * Time.deltaTime / mag;
                    return false;
                }

            case Commands.build:    return false;
            case Commands.repair: return false;
            case Commands.upgrade: return false;


            case Commands.idle:
            default:
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