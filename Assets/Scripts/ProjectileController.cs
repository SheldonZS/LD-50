using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Vector2 source;
    private bool piercing;
    private float maxRange;
    private int damage;

    private bool alreadyHit = false;
    private GameObject endEffect = null;

    public void SetEndEffect(GameObject end)
    {
        endEffect = end;
    }

    public void SetProjectile(Vector2 from, Vector2 direction, float speed, float range, int dam, bool pierce = false)
    {
        source = from;
        piercing = pierce;
        maxRange = range;
        damage = dam;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity = direction.normalized * speed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (((Vector2)transform.localPosition - source).magnitude >= maxRange)
        {
            Destroy(gameObject);

            if (endEffect != null)
            {
                Instantiate(endEffect, transform.position, Quaternion.identity, RTSController.instance.gridAnchor);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (piercing || alreadyHit == false)
        {
            if (collision.gameObject.tag == "Monster")
            {
                collision.gameObject.GetComponent<MonsterBase>().TakeDamage(damage);

                if (!piercing)
                {
                    Destroy(gameObject);
                    alreadyHit = true;
                }
            }
        }
    }
}
