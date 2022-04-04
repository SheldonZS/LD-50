using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public int health { get; private set; }
    public int maxHealth;
    public int buildCost;
    public int buildTime;
    public int range;
    public int damage;
    public float attackCooldown;

    [Space(10)]
    public int upgradeMaxHealth;
    public int upgradeCost;
    public int upgradeTime;
    public int upgradedRange;
    public int upgradedDamage;
    public float upgradedAttackCooldown;

    public bool upgraded { get; private set; }

    public Hero builder;

    public TowerDamage[] damageImages;
    private SpriteRenderer sr;
    public bool operational { get; private set; }
    private HealthBar healthBar;

    public float cooldown { get; private set; }
    public GameObject turret;



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        operational = false;

        healthBar = GetComponentInChildren<HealthBar>();
        cooldown = 0;
    }

    private void Update()
    {
        if (builder == null)
            return;

        if (cooldown <= 0 && operational)
            switch (builder.gameObject.name)
            {
                case "Raol":
                    RaolTower();
                    break;
                case "Balthasar":
                    BalthasarTower();
                    break;
                case "Thob":
                    ThobTower();
                    break;
                case "Jolie":
                    JolieTower();
                    break;
                default: break;
            }
        else
            cooldown -= Time.deltaTime;
    }

    public void SetHealth(int x)
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();
        health = x;
        healthBar.UpdateHealth(x, maxHealth);
    }

    public void Repair(int damage)
    {
        TakeDamage(-damage);

        if (health >= maxHealth * 0.95)
        {
            operational = true;

            if (builder.gameObject.name == "Thob")
            {
                GetComponentInChildren<Animator>().SetBool("operational", true);
                GetComponentInChildren<CircleCollider2D>().enabled = true;

            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        foreach(TowerDamage state in damageImages)
            if(health >= state.minHealth)
            {
                sr.sprite = state.image;
                break;
            }

        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.UpdateHealth(health);

        if (health <= 0)
        {
            builder.PlayRuinText();
        }
    }

    public void Upgrade()
    {
        RTSController.instance.bones -= upgradeCost;
        upgraded = true;

        float percent = ((float)health) / maxHealth;
        maxHealth = upgradeMaxHealth;
        health = Mathf.RoundToInt(percent * upgradeMaxHealth);

        healthBar.UpdateHealth(health, maxHealth);

        range = upgradedRange;
        damage = upgradedDamage;
        attackCooldown = upgradedAttackCooldown;

        if (builder.gameObject.name == "Thob")
            GetComponentInChildren<Animator>().SetBool("upgraded", true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    //Ranger Towers
    public void RaolTower()
    {
        GameObject target = GetClosestMonster(range);

        if (target == null)
            return;

        ProjectileController arrow = Instantiate(builder.attackPrefabs[0], RTSController.instance.gridAnchor).GetComponent<ProjectileController>();
        arrow.transform.localPosition = transform.localPosition + Vector3.up * 0.5f;

        Vector2 direction = target.transform.localPosition - transform.localPosition;

        arrow.transform.eulerAngles = Vector3.back * DirectionToAngle(direction);
        arrow.SetProjectile(transform.localPosition, direction, 10f, range, damage, upgraded ? true : false);

        cooldown = attackCooldown;

    }
    //Wizard Towers
    public void BalthasarTower()
    {
        GameObject target = GetClosestMonster(range);

        if (target == null)
            return;

        ProjectileController bolt = Instantiate(builder.attackPrefabs[0], RTSController.instance.gridAnchor).GetComponent<ProjectileController>();
        
        if(upgraded)
        {
            bolt.GetComponent<Collider2D>().enabled = false;
            bolt.SetEndEffect(builder.attackPrefabs[1]);
        }
        bolt.transform.localPosition = transform.localPosition + Vector3.up * 0.5f;

        Vector2 direction = target.transform.localPosition - transform.localPosition;

        bolt.transform.eulerAngles = Vector3.back * DirectionToAngle(direction);

        if(!upgraded)
            bolt.SetProjectile(transform.localPosition, direction, 5f, range, damage, true);
        else bolt.SetProjectile(transform.localPosition, direction, 5f, (target.transform.localPosition - transform.localPosition).magnitude, 0, true);

        cooldown = attackCooldown;
    }

    //Bard Towers
    public virtual void ThobTower()
    {

    }
    //Fighters Towers
    public void JolieTower()
    {
        RaolTower();
    }

    public float DirectionToAngle(Vector2 direction)
    {
        float atan = 0;
        if (direction.y != 0)
        {
            atan = Mathf.Atan(direction.x / direction.y) * 180 / Mathf.PI;
            if (direction.y < 0)
                atan += 180;
        }
        else
        {
            if (direction.x < 0)
                atan = -90;
            else atan = 90;
        }

        return atan;
    }
    public GameObject GetClosestMonster(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + Vector3.up * 0.5f, radius); //layer 11 is player projectiles

        if (hits.Length == 0)
            return null;

        GameObject closest = null;
        float distance = float.PositiveInfinity;

        foreach (Collider2D hit in hits)
        {

            if (hit.gameObject.tag == "Monster")
            {
                float newDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (newDistance < distance)
                {
                    closest = hit.gameObject;
                    distance = newDistance;
                }
            }
        }

        return closest;
    }
}

[System.Serializable]
public struct TowerDamage
{
    public int minHealth;
    public Sprite image;
}