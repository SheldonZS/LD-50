using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCollectible : MonoBehaviour
{
    public int bones;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Hero>().PickUpBones(bones);
            bones = 0;
            Destroy(gameObject);
        }
    }
}
