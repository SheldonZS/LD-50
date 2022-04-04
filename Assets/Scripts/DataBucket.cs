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
    public bool debugMode, raolUnlocked, thobUnlocked, balUnlocked, jolieUnlocked;

    public static DataBucket instance = null;

    public GameData data;

    public int selection;

    public int tutorialMode;

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

public enum EndingCode { none, Raol, Bal, Thob, Jolie};