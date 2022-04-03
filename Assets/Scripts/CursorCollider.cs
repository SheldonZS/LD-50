using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    public Sprite square;
    public Color green;
    public Color red;

    private RTSController RTSC;
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Awake()
    {
        RTSC = GetComponentInParent<RTSController>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseGrid = RTSC.MouseToGrid();

        if (mouseGrid.x >= -.5 && mouseGrid.x < 17.5 && mouseGrid.y >= 0 && mouseGrid.y < 10 && RTSC.selected != null)
        {
            renderer.enabled = true;

            switch (RTSC.command)
            {
                case Commands.build:
                    renderer.sprite = RTSC.selected.GetComponent<Hero>().towers[0].GetComponent<SpriteRenderer>().sprite;
                    mouseGrid = RTSC.RoundToGrid(mouseGrid, 0);
                    mouseGrid.z = transform.localPosition.z;
                    transform.localPosition = mouseGrid;

                    renderer.color = green;

                    Collider2D[] hits = Physics2D.OverlapBoxAll(RTSC.gridAnchor.TransformPoint(RTSC.RoundToGrid(mouseGrid, 0.5f)), Vector2.one * 0.9f, 0);
                    foreach (Collider2D hit in hits)
                    {
                        if (hit.gameObject.tag == "Tower" || hit.gameObject.tag == "Monster" || hit.gameObject.tag == "Path" || hit.gameObject.tag == "Obstacle")
                        {
                            renderer.color = red;
                            break;
                        }
                    }
                    break;
                case Commands.repair:
                    renderer.sprite = square;
                    mouseGrid = RTSC.RoundToGrid(mouseGrid, 0.5f);
                    mouseGrid.z = transform.localPosition.z;
                    transform.localPosition = mouseGrid;

                    TowerBase tower = RTSC.GetTowerAt(mouseGrid);

                    if (tower == null || tower.builder.gameObject != RTSC.selected || tower.health <= 0 || tower.health >= tower.maxHealth)
                        renderer.color = red;
                    else renderer.color = green;
                    break;

                case Commands.upgrade:
                    renderer.sprite = square;
                    mouseGrid = RTSC.RoundToGrid(mouseGrid, 0.5f);
                    mouseGrid.z = transform.localPosition.z;
                    transform.localPosition = mouseGrid;

                    tower = RTSC.GetTowerAt(mouseGrid);

                    if (tower == null || tower.builder.gameObject != RTSC.selected || tower.health < tower.maxHealth / 2 || tower.operational == false)
                        renderer.color = red;
                    else renderer.color = green;
                    break;

                case Commands.move:
                default:
                    renderer.enabled = false;
                    /*renderer.sprite = square;
                    mouseGrid = RTSC.RoundToGrid(mouseGrid, 0.5f);
                    mouseGrid.z = transform.localPosition.z;
                    transform.localPosition = mouseGrid;

                    renderer.color = green;*/
                    break;
            }


        }
        else
            renderer.enabled = false;        
    }

}
