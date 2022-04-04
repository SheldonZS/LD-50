using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCrew : MonoBehaviour
{
    public float verticalOffset;
    public float horizontalOffset;

    // Start is called before the first frame update
    void Start()
    {
        verticalOffset = -13;
        horizontalOffset = 82;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(transform.parent.parent.position);
        this.transform.position = new Vector3 (namePos.x + horizontalOffset, namePos.y + verticalOffset, namePos.z);
    }
}
