using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour
{
    public static RTSController instance { get; private set; }
    [HideInInspector] public GameObject selected = null;
    public List<GameObject> heroes;
    // Start is called before the first frame update

    void Start()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Input.mousePosition, Vector3.forward);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag == "Hero")
                    selected = hit.collider.gameObject;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            selected = null;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = heroes.IndexOf(selected);

            selected = heroes[(index + 1) % heroes.Count];
        }
    }
}
