using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public int health { get; private set; }
    public int maxHealth;

    public int buildTime;
    public int upgradeTime;

    public TowerDamage[] damageImages;
    private SpriteRenderer sr;
    private bool operational = false;
    private HealthBar healthBar;
    

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        operational = false;

        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void SetHealth(int x)
    {
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

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

public struct TowerDamage
{
    public int minHealth;
    public Sprite image;
}