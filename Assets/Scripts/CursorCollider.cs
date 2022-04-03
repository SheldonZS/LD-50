using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    public bool monsterPresent;
    public bool towerPresent;
    public bool heroPresent;

    private RTSController RTSC;
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Awake()
    {
        RTSC = GetComponentInParent<RTSController>();
        renderer = GetComponent<SpriteRenderer>();

        monsterPresent = false;
        towerPresent = false;
        heroPresent = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouseGrid = RTSC.MouseToGrid();
        monsterPresent = false;
        towerPresent = false;
        heroPresent = false;

        if (mouseGrid.x >= -.5 && mouseGrid.x < 17.5 && mouseGrid.y >= 0 && mouseGrid.y < 10)
        {
            renderer.enabled = true;

            mouseGrid = RTSC.RoundToGrid(mouseGrid, 0.5f);
            mouseGrid.z = transform.localPosition.z;
            transform.localPosition = mouseGrid;
        }
        else renderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            heroPresent = true;
        if (other.gameObject.tag == "Monster")
            monsterPresent = true;
        if (other.gameObject.tag == "Tower")
            towerPresent = true;

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            heroPresent = true;
        if (other.gameObject.tag == "Monster")
            monsterPresent = true;
        if (other.gameObject.tag == "Tower")
            towerPresent = true;
    }
}
