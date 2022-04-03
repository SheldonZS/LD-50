using System;
using UnityEngine;

[Serializable]
public struct GameData
{
    public EndingCode ending;
    public int wavesSurvived;
}

public class DataBucket : MonoBehaviour
{
    public bool debugMode;

    public static DataBucket instance = null;

    public GameData data;

    public int selection;
    public bool raolUnlocked, balUnlocked, thobUnlocked, jolieUnlocked;

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

        //raolUnlocked = true;
        //balUnlocked = true;
        //thobUnlocked = true;
        //jolieUnlocked = true;
    }
}

public enum EndingCode { none, Raol, Bal, Thob, Jolie};