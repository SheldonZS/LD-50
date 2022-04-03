using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public int moveSpeed;
    public int maxHealth;
    public int health;
    public MonsterMove moveAI;
    public MonsterAttack attackAI;

    public Vector2Int[] path;

    private int waypoint;
    private Rigidbody2D rb;
    private Animator animator;
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        waypoint = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoint < path.Length)
        {
            Vector2 movement = path[waypoint] - (Vector2)transform.localPosition;
            float mag = movement.magnitude;

            if (mag <= moveSpeed * Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) * 2)
            {
                transform.localPosition = new Vector3(path[waypoint].x, path[waypoint].y);
                rb.velocity = Vector2.zero;
                waypoint++;
            }
            else
            {
                rb.velocity = movement.normalized * moveSpeed;
            }
        }
    }

    public void SetSpawn(SpawnPoint spawn)
    {
        transform.localPosition = new Vector3(spawn.location.x, spawn.location.y);
        path = (Vector2Int[]) spawn.path.Clone();
        health = maxHealth;

        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);
    }


    public void TakeDamage(int i)
    {
        health -= i;
        healthBar.UpdateHealth(health);
        if (health <= 0)
        {
            Destroy(rb);
            Destroy(GetComponent<BoxCollider2D>());
            StartCoroutine(Die());
            enabled = false;
        }
        else
        {
            StartCoroutine(Hurt());
        }

    }

    public IEnumerator Hurt()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return null;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public IEnumerator Die()
    {
        Color color = GetComponent<SpriteRenderer>().color;

        for (color.a = 1; color.a > 0; color.a -= .1f)
        {
            GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }

        Destroy(gameObject);
       
    }

}

public enum MonsterMove { path, moveToNearestTower, moveToNearestPlayer, moveToHome, wander}
public enum MonsterAttack { attackHomeOnly, attackTowers, attackPlayers, attackAll}
