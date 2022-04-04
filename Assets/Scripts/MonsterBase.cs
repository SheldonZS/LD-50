using System.Collections;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public float moveSpeed;
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
    private Transform tracking;

    private Rigidbody2D rb;
    private Animator animator;
    private HealthBar healthBar;
    private RTSController RTSC;
    private AudioSource SFX;

    private float cooldown;
    private Color baseColor;
    private SpriteRenderer renderer;

    public void SetStats(Color color, float moveSpeed, int HP, int attack, float cooldown, int bones, MonsterMove moveType, MonsterAttack AttackType)
    {
        baseColor = color;
        GetComponent<SpriteRenderer>().color = color;
        this.moveSpeed = moveSpeed;
        health = HP;
        maxHealth = HP;
        attackDamage = attack;
        attackCooldown = cooldown;
        bonesDropped = bones;
        moveAI = moveType;
        attackAI = AttackType;

        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        waypoint = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x > 0)
            renderer.flipX = false;
        else if (rb.velocity.y < 0)
            renderer.flipX = true;
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position + Vector3.up * .5f, 1f);

        bool attacking = false;
        
        foreach (Collider2D target in inRange)
        {
            if (target.gameObject.tag == "Player" && (attackAI == MonsterAttack.attackPlayers || attackAI == MonsterAttack.attackAll))
            {
                attacking = true;
                target.gameObject.GetComponent<Hero>().TakeDamage(attackDamage);
                break;
            }

            if (target.gameObject.tag == "Tower" && (attackAI == MonsterAttack.attackTowers || attackAI == MonsterAttack.attackAll))
            {
                attacking = true;
                target.gameObject.GetComponent<TowerBase>().TakeDamage(attackDamage);
                break;
            }

            if (target.gameObject == RTSC.homeBase)
            {
                attacking = true;
                target.gameObject.GetComponent<TowerBase>().TakeDamage(attackDamage);
                break;
            }
        }

        if (attacking)
        {
            rb.velocity = Vector2.zero;
            animator.SetInteger("animation", (int)CharacterAnimation.action);
            cooldown = attackCooldown;
        }
        else
        {
            if (moveAI == MonsterMove.path && RTSC.base_intact == false)
                moveAI = MonsterMove.wander;
            animator.SetInteger("animation", (int)CharacterAnimation.move);
            switch (moveAI)
            {
                case MonsterMove.path:
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
                    break;
                case MonsterMove.moveToHome:
                    rb.velocity = (RTSC.homeBase.transform.localPosition - transform.localPosition).normalized * moveSpeed * bardMoveMultiplier;
                    break;
                case MonsterMove.moveToNearestTower:
                    if (tracking != null)
                    {
                        rb.velocity = (tracking.localPosition - transform.localPosition).normalized * moveSpeed * bardMoveMultiplier;
                    }
                    else
                    {
                        if (rb.velocity.magnitude >= moveSpeed *.9f * bardMoveMultiplier)
                            break;

                        float distance = float.PositiveInfinity;
                        TowerBase[] towers = transform.parent.GetComponentsInChildren<TowerBase>();

                        foreach (TowerBase tower in towers)
                        {
                            if (tower.gameObject != RTSC.homeBase)
                            {
                                float towerDistance = (tower.transform.localPosition - transform.localPosition).magnitude;
                                if (towerDistance < distance)
                                {
                                    tracking = tower.transform;
                                    distance = towerDistance;
                                }
                            }
                        }

                        if (tracking == null && RTSC.homeBase != null)
                            tracking = RTSC.homeBase.transform;

                        if (tracking != null)
                            rb.velocity = (tracking.localPosition - transform.localPosition).normalized * moveSpeed * bardMoveMultiplier;
                        else
                            rb.velocity = RandomDirection() * moveSpeed * bardMoveMultiplier;
                    }
                   
                    break;
                case MonsterMove.moveToNearestPlayer:
                    if (tracking != null)
                    {
                        rb.velocity = (tracking.localPosition - transform.localPosition).normalized * moveSpeed * bardMoveMultiplier;
                    }
                    else
                    {
                        float distance = float.PositiveInfinity;

                        foreach (GameObject hero in RTSC.heroes)
                        {
                            float targetDistance = (hero.transform.localPosition - transform.localPosition).magnitude;
                            if (targetDistance < distance)
                            {
                                distance = targetDistance;
                                tracking = hero.transform;
                            }
                        }

                        if (tracking != null)
                            rb.velocity = (tracking.localPosition - transform.localPosition).normalized * moveSpeed * bardMoveMultiplier;
                        else
                            rb.velocity = RandomDirection() * moveSpeed * bardMoveMultiplier;
                    }
                    break;
                case MonsterMove.wander:
                default:
                    if (rb.velocity.magnitude < moveSpeed * .9f * bardMoveMultiplier)
                        rb.velocity = RandomDirection() * moveSpeed * bardMoveMultiplier;
                    break;

            }
        }
    }

    public void SetSpawn(RTSController rtsc, SpawnPoint spawn)
    {
        RTSC = rtsc;
        transform.localPosition = new Vector3(spawn.location.x, spawn.location.y);
        path = (Vector2Int[]) spawn.path.Clone();

    }


    public void TakeDamage(int i)
    {
        health = Mathf.Max(health - i, 0);
        healthBar.UpdateHealth(health);

        if (health <= 0)
        {
            SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/#50_EnemyHit" + Random.Range(1,3)));

            Destroy(rb);
            Destroy(GetComponent<BoxCollider2D>());
            Die();
            enabled = false;
        }
        else
        {
            //animator.SetInteger("animation", (int)CharacterAnimation.hurt);
            SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/#50_Enemy Death"));
            Hurt();
        }

    }

    public void Hurt()
    {
        StartCoroutine(HurtAnimation());
    }

    public void MakeIdle()
    {
        animator.SetInteger("animation", (int)CharacterAnimation.idle);
    }

    public IEnumerator HurtAnimation()
    {
         GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);

        GetComponent<SpriteRenderer>().color = baseColor;
    }
    public void Die()
    {
        if (bonesDropped > 0)
        {
            GameObject bones = Instantiate(bonesPrefab, RTSController.instance.gridAnchor);
            bones.transform.localPosition = transform.localPosition + Vector3.forward * .01f;
            bones.GetComponent<BoneCollectible>().bones = bonesDropped;
        }

        animator.SetInteger("animation", (int)CharacterAnimation.dying);
    }

    public void DestroySelf()
    {

        Destroy(gameObject);
    }

    public Vector2 RandomDirection()
    {

        float theta = Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Sin(theta), Mathf.Cos(theta));
    }
}

public enum MonsterMove { path, moveToNearestTower, moveToNearestPlayer, moveToHome, wander}
public enum MonsterAttack { attackHomeOnly, attackTowers, attackPlayers, attackAll}
