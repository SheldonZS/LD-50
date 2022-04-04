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
    public Color raolColor, balthasarColor, thobColor, jolieColor;

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


        raolColor = new Color(219f / 255f, 133f / 255f, 30f / 255f, 255f);
        balthasarColor = new Color(105f / 255f, 165f / 255f, 209f / 255f, 255f);
        thobColor = new Color(148f / 255f, 73f / 255f, 191f / 255f, 255f);
        jolieColor = new Color(179f / 255f, 40f / 255f, 40f / 255f, 255f);
    }
}

public enum EndingCode { none, Raol, Bal, Thob, Jolie};