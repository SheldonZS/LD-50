using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour
{
    public static RTSController instance { get; private set; }
    public Camera camera { get; private set; }
    //[HideInInspector] 
    public GameObject selected = null;
    public List<GameObject> heroes;

    public GameObject[] monsters;

    public SpawnPoint[] spawnPoints;

    private Transform gridAnchor;

    public Commands command { get; private set; }
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        selected = null;
        camera = Camera.main;

        gridAnchor = GameObject.Find("Grid Anchor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Clicked at world position: " + clickWorldPosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(clickWorldPosition, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Player")
                    selected = hit.collider.gameObject;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            selected = null;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = heroes.IndexOf(selected);

            selected = heroes[(index + 1) % heroes.Count];
            command = Commands.idle;
        }
    }

    public Vector2 MouseToGrid()
    {
        return gridAnchor.InverseTransformPoint(camera.ScreenToWorldPoint(Input.mousePosition));
    }

}

[System.Serializable]
public struct SpawnPoint
{
    public Vector2Int location;
    public Vector2Int[] path;
}