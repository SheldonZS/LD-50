using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardAura : MonoBehaviour
{
    public float slowMoveMultiplier = 0.8f;
    public float slowCooldownMultiplier = 0.8f;
    public float hasteMoveMultiplier = 1.2f;
    public float hasteBuildMultiplier = 1.2f;

    private TowerBase tower;

    private void Start()
    {
        tower = GetComponentInParent<TowerBase>();   
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name + " entered bard aura");

        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterBase>().bardCooldownMultiplier = slowCooldownMultiplier;
            other.GetComponent<MonsterBase>().bardMoveMultiplier = slowMoveMultiplier;
        }
        else if (other.gameObject.tag == "Player" && tower.upgraded == true)
        {
            //other.GetComponent<Hero>().bardBuildMultiplier = hasteBuildMultiplier;
            //other.GetComponent<Hero>().bardMoveMultiplier = hasteMoveMultiplier;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterBase>().bardCooldownMultiplier = slowCooldownMultiplier;
            other.GetComponent<MonsterBase>().bardMoveMultiplier = slowMoveMultiplier;
        }
        else if (other.gameObject.tag == "Player" && tower.upgraded == true)
        {
            //other.GetComponent<Hero>().bardBuildMultiplier = hasteBuildMultiplier;
            //other.GetComponent<Hero>().bardMoveMultiplier = hasteMoveMultiplier;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterBase>().bardCooldownMultiplier = 1f;
            other.GetComponent<MonsterBase>().bardMoveMultiplier = 1f;
        }
        else if (other.gameObject.tag == "Player" && tower.upgraded == true)
        {
            //other.GetComponent<Hero>().bardBuildMultiplier = 1f;
            //other.GetComponent<Hero>().bardMoveMultiplier = 1f;
        }
    }

    private void OnDestroy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2.5f);

        foreach(Collider2D other in hits)
        {
            if (other.gameObject.tag == "Monster")
            {
                other.GetComponent<MonsterBase>().bardCooldownMultiplier = 1f;
                other.GetComponent<MonsterBase>().bardMoveMultiplier = 1f;
            }
            else if (other.gameObject.tag == "Player" && tower.upgraded == true)
            {
                //other.GetComponent<Hero>().bardBuildMultiplier = 1f;
                //other.GetComponent<Hero>().bardMoveMultiplier = 1f;
            }
        }
    }
}
