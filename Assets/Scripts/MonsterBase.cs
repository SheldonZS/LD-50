using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public int moveSpeed;
    public int maxHealth;
    public int health;
    public int attackDamage;
    public float attackCooldown;

    //[NonSerialized]
    public float bardMoveMultiplier = 1f;
    //[NonSerialized]
    public float bardCooldownMultiplier = 1f;

    public int bonesDropped;
    public GameObject bonesPrefab;

    public MonsterMove moveAI;
    public MonsterAttack attackAI;

    public Vector2Int[] path;

    private int waypoint;
    private Rigidbody2D rb;
    private Animator animator;
    private HealthBar healthBar;

    private float cooldown;



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

            if (mag <= moveSpeed * Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) * bardMoveMultiplier || Vector2.Angle(rb.velocity, movement) > 90)
            {
                transform.localPosition = new Vector3(path[waypoint].x, path[waypoint].y);
                rb.velocity = Vector2.zero;
                waypoint++;
            }
            else
            {
                rb.velocity = movement.normalized * moveSpeed * bardMoveMultiplier;
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
        if (bonesDropped > 0)
        {
            GameObject bones = Instantiate(bonesPrefab, RTSController.instance.gridAnchor);
            bones.transform.localPosition = transform.localPosition + Vector3.forward * .01f;
            bones.GetComponent<BoneCollectible>().bones = bonesDropped;
        }

        Color color = GetComponent<SpriteRenderer>().color;

        for (color.a = 1; color.a > 0; color.a -= Time.deltaTime * 2)
        {
            GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }

        Destroy(gameObject);
       
    }

}

public enum MonsterMove { path, moveToNearestTower, moveToNearestPlayer, moveToHome, wander}
public enum MonsterAttack { attackHomeOnly, attackTowers, attackPlayers, attackAll}
