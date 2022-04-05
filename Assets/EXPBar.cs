using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBar : MonoBehaviour
{
    private int exp;
    private int maxExp;

    public SpriteRenderer barFront;

    public void UpdateHealth(int current, int max = -1)
    {
        if (max > 0) maxExp = max;

        exp = current;

        float percent = ((float)exp) / maxExp;

        if (percent >= .95f)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            barFront.size = new Vector2(percent, 1f);
        }
    }
}
