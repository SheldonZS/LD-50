using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private int health;
    private int maxHealth;

    public SpriteRenderer barFront;

    public void UpdateHealth(int current, int max = -1)
    {
        if (max > 0) maxHealth = max;

        health = current;

        float percent = ((float)health) / maxHealth;

        if (percent >= .95f)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            barFront.size = new Vector2(percent, 1f);
        }
    }
}
