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

    // Start is called before the first frame update
    void Start()
    {
        commands = new Queue<HeroCommand>();
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

        healthBar.UpdateHealth(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

public class HeroCommand
{
    public Commands command = Commands.idle;
    public object target;
}

public enum Commands { idle, move, build, repair}