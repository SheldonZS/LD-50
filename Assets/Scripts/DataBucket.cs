using System;
using UnityEngine;

[Serializable]
public struct GameData
{
    public EndingCode ending;
}

public class DataBucket : MonoBehaviour
{
    public bool debugMode;

    public static DataBucket instance = null;

    public GameData data;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        data.ending = EndingCode.none;
    }
}

public enum EndingCode { none, bad, good, joke};