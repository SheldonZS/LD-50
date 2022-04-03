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
            operational = true;
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

        Debug.Log("Firing at " + target.gameObject.name);

        ProjectileController arrow = Instantiate(builder.attackPrefabs[0], RTSController.instance.gridAnchor).GetComponent<ProjectileController>();
        arrow.transform.localPosition = transform.localPosition;

        Vector2 direction = target.transform.localPosition - transform.localPosition;

        float atan = 0;
        if(direction.x != 0)
        {
            atan = Mathf.Atan(direction.y / direction.x);
            if (direction.y < 0)
                atan += Mathf.PI;
        }
        else
        {
            if (direction.y < 0)
                atan = Mathf.PI;
        }

        arrow.transform.eulerAngles = Vector3.back * (atan * 180 / Mathf.PI);

        cooldown = attackCooldown;

        arrow.SetProjectile(transform.localPosition, direction, 5f, range, damage, upgraded ? true : false);
    }
    //Wizard Towers
    public void BalthasarTower()
    {

    }

    //Bard Towers
    public void ThobTower()
    {

    }
    //Fighters Towers
    public void JolieTower()
    {

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