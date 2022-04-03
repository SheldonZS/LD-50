using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSController : MonoBehaviour
{
    public static RTSController instance { get; private set; }

    public Sprite[] build;
    public Sprite[] repair;
    public Sprite[] upgrade;
    private Image buildButton;
    private Image repairButton;
    private Image upgradeButton;

    public Camera camera { get; private set; }
    //[HideInInspector] 
    public GameObject selected = null;
    public List<GameObject> heroes;

    public GameObject[] monsters;

    public SpawnPoint[] spawnPoints;

    public Transform gridAnchor { get; private set; }
    public Commands command { get; private set; }
    public CursorCollider mouseOver { get; private set; }

    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        selected = null;
        camera = Camera.main;

        gridAnchor = GameObject.Find("Grid Anchor").transform;
        buildButton = GameObject.Find("Build").GetComponent<Image>();
        repairButton = GameObject.Find("Repair").GetComponent<Image>();
        upgradeButton = GameObject.Find("Upgrade").GetComponent<Image>();
        mouseOver = GameObject.Find("MouseOver").GetComponent<CursorCollider>();
    }

    public bool GridContains(Vector2 location, string tag)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(RoundToGrid(location, 0.5f), Vector2.one * 0.9f, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == tag)
                return true;
        }

        return false;
    }

    public Vector2 RoundToGrid(Vector2 loc, float Yoffset = 0f)
    {
        loc.x = Mathf.RoundToInt(loc.x);
        loc.y = Mathf.FloorToInt(loc.y) + Yoffset;

        return loc;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log("Clicked at world position: " + mouseWorldPosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (hit.collider.gameObject != selected)
                    {
                        selected = hit.collider.gameObject;
                        command = Commands.move;
                        buildButton.overrideSprite = build[0];
                        repairButton.overrideSprite = repair[0];
                        upgradeButton.overrideSprite = upgrade[0];
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selected = null;
            command = Commands.move;
            buildButton.overrideSprite = build[0];
            repairButton.overrideSprite = repair[0];
            upgradeButton.overrideSprite = upgrade[0];
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = heroes.IndexOf(selected);

            selected = heroes[(index + 1) % heroes.Count];

            command = Commands.move;
            buildButton.overrideSprite = build[0];
            repairButton.overrideSprite = repair[0];
            upgradeButton.overrideSprite = upgrade[0];
        }

        if (Input.GetKeyDown(KeyCode.B))
            BuildButton();
        if (Input.GetKeyDown(KeyCode.R))
            RepairButton();
        if (Input.GetKeyDown(KeyCode.U))
            UpgradeButton();
    }

    public Vector2 MouseToGrid()
    {
        return gridAnchor.InverseTransformPoint(camera.ScreenToWorldPoint(Input.mousePosition));
    }

    public void BuildButton()
    {
        command = Commands.build;

        buildButton.overrideSprite = build[1];
        repairButton.overrideSprite = repair[0];
        upgradeButton.overrideSprite = upgrade[0];
    }

    public void RepairButton()
    {
        command = Commands.repair;

        buildButton.overrideSprite = build[0];
        repairButton.overrideSprite = repair[1];
        upgradeButton.overrideSprite = upgrade[0];
    }    
    
    
    public void UpgradeButton()
    {
        command = Commands.upgrade;

        buildButton.overrideSprite = build[0];
        repairButton.overrideSprite = repair[0];
        upgradeButton.overrideSprite = upgrade[1];
    }

}

[System.Serializable]
public struct SpawnPoint
{
    public Vector2Int location;
    public Vector2Int[] path;
}