using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JukeboxController : MonoBehaviour
{
    public JukeboxSong[] tracks;
    private MusicLooper bgm;
    private DataBucket db;

    private Text title;
    private Text description;

    // Start is called before the first frame update
    void Awake()
    {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        bgm = GameObject.Find("BGM").GetComponent<MusicLooper>();
        title = GameObject.Find("SongTitle").GetComponent<Text>();
        description = GameObject.Find("Description").GetComponent<Text>();
    }

    private void Start()
    {
        title.text = "Now Playing: \n" + tracks[db.selection].title;
        description.text = tracks[db.selection].description;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickPrev()
    {
        if (db.selection <= 0)
            db.selection = tracks.Length;

        db.selection--;
        onClickPlay();

    }

    public void onClickNext()
    {
        db.selection = (db.selection + 1) % tracks.Length;

        onClickPlay();
    }

    public void onClickPlay()
    {
        if (tracks[db.selection].loop)
            bgm.NewCustomLoop(tracks[db.selection].song, tracks[db.selection].loopStart, tracks[db.selection].loopEnd);
        else
            bgm.FadeAndPlay(tracks[db.selection].song);

        title.text = "Now Playing: \n" + tracks[db.selection].title;
        description.text = tracks[db.selection].description;
    }

}
 [System.Serializable]
 public struct JukeboxSong
{
    public AudioClip song;
    public string title;
    [TextArea(10, 10)]
    public string description;
    public bool loop;
    public float loopStart;
    public float loopEnd;
}